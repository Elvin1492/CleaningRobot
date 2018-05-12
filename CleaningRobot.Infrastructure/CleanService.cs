using CleaningRobot.Infrastructure.Core;
using CleaningRobot.Infrastructure.Core.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleaningRobot.Infrastructure
{
    public class CleanService
    {
        private readonly Order _order;
        private Result _result;

        public CleanService(Order order)
        {
            this._order = order;
            this._result = new Result();
            _result.VisitedCells.Add(new Cell
            {
                Point = _order.CurrentState.Cell.Point
            });
        }

        public Result Start()
        {
            foreach (var command in _order.Commands)
            {
                switch (command)
                {
                    case Core.Enums.CommandEnum.TR:
                        TurnRight();
                        break;
                    case Core.Enums.CommandEnum.TL:
                        TurnLeft();
                        break;
                    case Core.Enums.CommandEnum.A:
                        Advance();
                        break;
                    case Core.Enums.CommandEnum.B:
                        break;
                    case Core.Enums.CommandEnum.C:
                        break;
                    default:
                        break;
                }
            }
            return _result;
        }

        public void Advance()
        {
            var nextPoint = new Point { X = _order.CurrentState.Cell.Point.X, Y = _order.CurrentState.Cell.Point.Y };

            if (!HasEnoughBatteryCappacity(CommandEnum.TL))
            {
                BackOffStrategy(_order.CurrentState);
                return;
            }

            _order.Battery -= 2;

            switch (_order.CurrentState.Faceing)
            {
                case FacingEnum.North:
                    nextPoint.Y--;
                    break;
                case FacingEnum.East:
                    nextPoint.X++;
                    break;
                case FacingEnum.South:
                    nextPoint.Y++;
                    break;
                case FacingEnum.West:
                    nextPoint.X--;
                    break;
            }

            if (nextPoint.X > _order.Map.Count && nextPoint.Y > _order.Map.First().Count)
            {
                BackOffStrategy(_order.CurrentState);
                return;
            }

            _order.CurrentState.Cell.Point = nextPoint;

            _result.VisitedCells.Add(new Cell
            {
                Point = nextPoint
            });
            _result.FinalState = _order.CurrentState;
        }

        public void TurnLeft()
        {
            if (!HasEnoughBatteryCappacity(CommandEnum.TL))
            {
                BackOffStrategy(_order.CurrentState);
                return;
            }

            _order.Battery -= 1;
            switch (_order.CurrentState.Faceing)
            {
                case FacingEnum.North:
                    _order.CurrentState.Faceing = FacingEnum.West;
                    break;
                case FacingEnum.East:
                    _order.CurrentState.Faceing = FacingEnum.North;
                    break;
                case FacingEnum.South:
                    _order.CurrentState.Faceing = FacingEnum.East;
                    break;
                case FacingEnum.West:
                    _order.CurrentState.Faceing = FacingEnum.South;
                    break;
            }
            _result.FinalState = _order.CurrentState;
        }

        public void TurnRight()
        {
            if (!HasEnoughBatteryCappacity(CommandEnum.TR))
            {
                BackOffStrategy(_order.CurrentState);
                return;
            }

            _order.Battery -= 1;
            switch (_order.CurrentState.Faceing)
            {
                case FacingEnum.North:
                    _order.CurrentState.Faceing = FacingEnum.East;
                    break;
                case FacingEnum.East:
                    _order.CurrentState.Faceing = FacingEnum.South;
                    break;
                case FacingEnum.South:
                    _order.CurrentState.Faceing = FacingEnum.West;
                    break;
                case FacingEnum.West:
                    _order.CurrentState.Faceing = FacingEnum.North;
                    break;
            }
            _result.FinalState = _order.CurrentState;
        }

        public void BackOffStrategy(StateOfRobot stateOfRobot)
        {

        }

        public bool HasEnoughBatteryCappacity(CommandEnum command)
        {
            switch (command)
            {
                case CommandEnum.TR:
                case CommandEnum.TL:
                    return _order.Battery > 0;
                case CommandEnum.A:
                    return _order.Battery > 1;
                case CommandEnum.B:
                    return _order.Battery > 2;
                case CommandEnum.C:
                    return _order.Battery > 4;
                default:
                    return false;
            }
        }
    }
}
