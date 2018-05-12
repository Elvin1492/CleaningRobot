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
            var nextPoint = IsNextCellAvaliable();
            _order.Battery -= 2;

            if (!HasEnoughBatteryCappacity(CommandEnum.TL) || !nextPoint.Item1)
            {
                if (IsBackOffStrategyEnabled) return;
                BackOffStrategy();
                return;
            }

            _order.CurrentState.Cell.Point = nextPoint.Item2;

            _result.VisitedCells.Add(new Cell
            {
                Point = nextPoint.Item2
            });
            _result.FinalState = _order.CurrentState;
            IsBackOffStrategyEnabled = false;
        }

        public void TurnLeft()
        {
            if (!HasEnoughBatteryCappacity(CommandEnum.TL))
            {
                if (IsBackOffStrategyEnabled) return;
                BackOffStrategy();
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
                if (IsBackOffStrategyEnabled) return;
                BackOffStrategy();
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

        public bool IsBackOffStrategyEnabled { get; set; }
        public void BackOffStrategy()
        {
            IsBackOffStrategyEnabled = true;
            TurnRight();
            Advance();
            if (IsBackOffStrategyEnabled)
            {
                TurnLeft();
                
            }
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

        public Tuple<bool, Point> IsNextCellAvaliable()
        {
            var nextPoint = new Point { X = _order.CurrentState.Cell.Point.X, Y = _order.CurrentState.Cell.Point.Y };

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

            if ((nextPoint.X > _order.Map.Count || nextPoint.Y > _order.Map.First().Count) || _order.Map[nextPoint.X][nextPoint.Y].State == CellStateEnum.StateN || _order.Map[nextPoint.X][nextPoint.Y].State == CellStateEnum.StateC)
            {
                return new Tuple<bool, Point>(false, new Point());
            }

            return new Tuple<bool, Point>(true, nextPoint);
        }
    }
}
