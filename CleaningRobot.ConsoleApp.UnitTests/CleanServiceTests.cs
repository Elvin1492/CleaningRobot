﻿using System;
using System.Collections.Generic;
using System.Drawing;
using CleaningRobot.Infrastructure;
using CleaningRobot.Infrastructure.Core;
using CleaningRobot.Infrastructure.Core.Enums;
using NUnit.Framework;

namespace CleaningRobot.ConsoleApp.UnitTests
{
    [TestFixture]
    public class CleanServiceTests
    {
        private Order _order1;

        [SetUp]
        public void Setup()
        {
            _order1 = new Order
            {
                Battery = 80,
                Map = new List<List<Cell>>
                {
                    new List<Cell>{
                        new Cell { Point = new Point(0,0),State =CellStateEnum.StateS },
                        new Cell { Point = new Point(1,0),State =CellStateEnum.StateS },
                        new Cell { Point = new Point(2,0),State =CellStateEnum.StateS },
                        new Cell { Point = new Point(3,0),State =CellStateEnum.StateS }
                    },
                    new List<Cell>{
                        new Cell { Point = new Point(0,1),State =CellStateEnum.StateS },
                        new Cell { Point = new Point(1,1),State =CellStateEnum.StateS },
                        new Cell { Point = new Point(2,1),State =CellStateEnum.StateC },
                        new Cell { Point = new Point(3,1),State =CellStateEnum.StateS }
                    },
                    new List<Cell>{
                        new Cell { Point = new Point(0,2),State =CellStateEnum.StateS },
                        new Cell { Point = new Point(1,2),State =CellStateEnum.StateS },
                        new Cell { Point = new Point(2,2),State =CellStateEnum.StateS },
                        new Cell { Point = new Point(3,2),State =CellStateEnum.StateS }
                    },
                    new List<Cell>{
                        new Cell { Point = new Point(0,3),State =CellStateEnum.StateS },
                        new Cell { Point = new Point(1,3),State =CellStateEnum.StateN },
                        new Cell { Point = new Point(2,3),State =CellStateEnum.StateS },
                        new Cell { Point = new Point(3,3),State =CellStateEnum.StateS }
                    }
                }
            };
        }

        [Test]
        [TestCase(1, 1, FacingEnum.East, 2, 1)]
        [TestCase(1, 1, FacingEnum.North, 1, 0)]
        [TestCase(1, 1, FacingEnum.South, 1, 2)]
        [TestCase(1, 1, FacingEnum.West, 0, 1)]
        public void Advance_EverythingNormalForForward_RobotShouldGoNextPoint(int x, int y, FacingEnum facing, int expectedX, int expectedY)
        {

            _order1.CurrentState = new StateOfRobot { Cell = new Cell { Point = new Point(x, y), State = CellStateEnum.StateS }, Faceing = facing };
            _order1.Commands = new List<CommandEnum> { CommandEnum.A };

            var cleanService = new CleanService(_order1);

            var result = cleanService.Start();

            Assert.That(expectedX, Is.EqualTo(result.FinalState.Cell.Point.X));
            Assert.That(expectedY, Is.EqualTo(result.FinalState.Cell.Point.Y));
            Assert.That(_order1.Battery, Is.EqualTo(78));
        }

        [Test]
        [TestCase(FacingEnum.North, FacingEnum.West)]
        [TestCase(FacingEnum.East, FacingEnum.North)]
        [TestCase(FacingEnum.South, FacingEnum.East)]
        [TestCase(FacingEnum.West, FacingEnum.South)]
        public void TurnLeft_ForAllFacings_TurnTheRobotToLeft(FacingEnum facing, FacingEnum expectedFacing)
        {
            _order1.CurrentState = new StateOfRobot { Cell = new Cell { Point = new Point(1, 1), State = CellStateEnum.StateS }, Faceing = facing };
            _order1.Commands = new List<CommandEnum> { CommandEnum.TL };

            var cleanService = new CleanService(_order1);

            var result = cleanService.Start();

            Assert.That(expectedFacing, Is.EqualTo(result.FinalState.Faceing));
        }
    }
}
