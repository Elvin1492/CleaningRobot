using CleaningRobot.Infrastructure.Core;
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
                        break;
                    case Core.Enums.CommandEnum.TL:
                        break;
                    case Core.Enums.CommandEnum.A:
                        Advance(_order.CurrentState);
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

        public void Advance(StateOfRobot stateOfRobot)
        {
            var nextPoint = new Point { X = stateOfRobot.Cell.Point.X, Y = stateOfRobot.Cell.Point.Y };
            _order.Battery -= 2;
            switch (stateOfRobot.Faceing)
            {
                case Core.Enums.FacingEnum.North:
                    nextPoint.Y--;
                    break;
                case Core.Enums.FacingEnum.East:
                    nextPoint.X++;
                    break;
                case Core.Enums.FacingEnum.South:
                    nextPoint.Y++;
                    break;
                case Core.Enums.FacingEnum.West:
                    nextPoint.X--;
                    break;
            }

            if (nextPoint.X > _order.Map.Count && nextPoint.Y > _order.Map.First().Count || _order.Battery < 2)
            {
                BackOffStrategy(stateOfRobot);
                return;
            }

            stateOfRobot.Cell.Point = nextPoint;
            
            _result.VisitedCells.Add(new Cell
            {
                Point = nextPoint
            });
            _result.FinalState = stateOfRobot;
        }

        public void TurnLeft(StateOfRobot stateOfRobot)
        {

        }

        public void BackOffStrategy(StateOfRobot stateOfRobot)
        {

        }
    }
}
