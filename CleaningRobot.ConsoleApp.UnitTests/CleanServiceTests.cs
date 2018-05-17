using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        public void Start_Order1_ShouldReturnExpectedResult()
        {
            _order1.Commands = new List<CommandEnum> {
                CommandEnum.TL,
                CommandEnum.A,
                CommandEnum.C,
                CommandEnum.A,
                CommandEnum.C,
                CommandEnum.TR,
                CommandEnum.A,
                CommandEnum.C
            };
            _order1.CurrentState = new StateOfRobot
            {
                Faceing = FacingEnum.North,
                Cell = new Cell { Point = new Point(3, 0) }
            };
            var expectedVisitedCells = new List<Cell>
            {
                new Cell
                {
                     Point = new Point(3,0),
                    State =CellStateEnum.StateS,
                    IsVisited = false

                },
                 new Cell
                {
                    Point = new Point(2,0),
                    State =CellStateEnum.StateS,
                    IsVisited = false
                },
                  new Cell
                {
                     Point = new Point(1,0),
                    State =CellStateEnum.StateS,
                    IsVisited = false
                }
            };
            var expectedCleanedCells = new List<Cell>
            {
                new Cell
                {
                     Point = new Point(2,0),
                    State =CellStateEnum.StateS,
                    IsVisited = false
                },
                new Cell
                {
                     Point = new Point(1,0),
                    State =CellStateEnum.StateS,
                    IsVisited = false
                }
            };

            var cleanService = new CleanService(_order1);

            var result = cleanService.Start();

            Assert.That(54, Is.EqualTo(result.Battery));
            Assert.That(3, Is.EqualTo(result.VisitedCells.Count));
            CollectionAssert.AreEqual(expectedVisitedCells, result.VisitedCells);
            Assert.That(2, Is.EqualTo(result.CleanedCells.Count));
            CollectionAssert.AreEqual(expectedCleanedCells, result.CleanedCells);
            Assert.That(FacingEnum.East, Is.EqualTo(result.FinalState.Faceing));
        }

        [Test]
        public void Start_Order2_ShouldReturnExpectedResult()
        {
            _order1.Commands = new List<CommandEnum> {
                CommandEnum.TR,
                CommandEnum.A,
                CommandEnum.C,
                CommandEnum.A,
                CommandEnum.C,
                CommandEnum.TR,
                CommandEnum.A,
                CommandEnum.C
            };
            _order1.CurrentState = new StateOfRobot
            {
                Faceing = FacingEnum.South,
                Cell = new Cell { Point = new Point(3, 1) }
            };

            _order1.Battery = 1094;

            var expectedVisitedCells = new List<Cell>
            {
                new Cell
                {
                     Point = new Point(3,2),
                    State =CellStateEnum.StateS,
                    IsVisited = false

                },
                 new Cell
                {
                    Point = new Point(3,1),
                    State =CellStateEnum.StateS,
                    IsVisited = false
                },
                  new Cell
                {
                    Point = new Point(3,0),
                    State =CellStateEnum.StateS,
                    IsVisited = false
                },
                new Cell
                {
                    Point = new Point(2,2),
                    State =CellStateEnum.StateS,
                    IsVisited = false
                }
            };
            var expectedCleanedCells = new List<Cell>
            {
                new Cell
                {
                     Point = new Point(3,2),
                    State =CellStateEnum.StateS,
                    IsVisited = false
                },
                new Cell
                {
                     Point = new Point(3,0),
                    State =CellStateEnum.StateS,
                    IsVisited = false
                },
                new Cell
                {
                     Point = new Point(2,2),
                    State =CellStateEnum.StateS,
                    IsVisited = false
                }
            };

            var cleanService = new CleanService(_order1);

            var result = cleanService.Start();
			expectedVisitedCells = expectedVisitedCells.OrderBy(x => x.Point.X).ThenBy(x => x.Point.Y).ToList();
			result.VisitedCells = result.VisitedCells.OrderBy(x => x.Point.X).ThenBy(x => x.Point.Y).ToList();

			expectedCleanedCells = expectedCleanedCells.OrderBy(x => x.Point.X).ThenBy(x => x.Point.Y).ToList();
			result.CleanedCells = result.CleanedCells.OrderBy(x => x.Point.X).ThenBy(x => x.Point.Y).ToList();

			Assert.That(1040, Is.EqualTo(result.Battery));
            Assert.That(4, Is.EqualTo(result.VisitedCells.Count));
            CollectionAssert.AreEqual(expectedVisitedCells, result.VisitedCells);
            Assert.That(3, Is.EqualTo(result.CleanedCells.Count));
            CollectionAssert.AreEqual(expectedCleanedCells, result.CleanedCells);
            Assert.That(FacingEnum.East, Is.EqualTo(result.FinalState.Faceing));
        }

        [Test]
        [TestCase(0, 0, FacingEnum.East, 1, 0)]
        [TestCase(1, 1, FacingEnum.North, 1, 0)]
        [TestCase(1, 0, FacingEnum.South, 1, 1)]
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

        [Test]
        [TestCase(FacingEnum.North, FacingEnum.East)]
        [TestCase(FacingEnum.East, FacingEnum.South)]
        [TestCase(FacingEnum.South, FacingEnum.West)]
        [TestCase(FacingEnum.West, FacingEnum.North)]
        public void TurnRight_ForAllFacings_TurnTheRobotToRight(FacingEnum facing, FacingEnum expectedFacing)
        {
            _order1.CurrentState = new StateOfRobot { Cell = new Cell { Point = new Point(1, 1), State = CellStateEnum.StateS }, Faceing = facing };
            _order1.Commands = new List<CommandEnum> { CommandEnum.TR };

            var cleanService = new CleanService(_order1);

            var result = cleanService.Start();

            Assert.That(expectedFacing, Is.EqualTo(result.FinalState.Faceing));
        }

        [Test]
        [TestCase(CommandEnum.A, 2, true)]
        [TestCase(CommandEnum.A, 1, false)]
        [TestCase(CommandEnum.B, 3, true)]
        [TestCase(CommandEnum.B, 2, false)]
        [TestCase(CommandEnum.C, 5, true)]
        [TestCase(CommandEnum.C, 4, false)]
        [TestCase(CommandEnum.TL, 1, true)]
        [TestCase(CommandEnum.TL, 0, false)]
        [TestCase(CommandEnum.TR, 1, true)]
        [TestCase(CommandEnum.TR, 0, false)]
        public void HasEnoughBatteryCappacity_ForAllCommands(CommandEnum command, int battery, bool expectedResult)
        {
            _order1.CurrentState = new StateOfRobot { Cell = new Cell { Point = new Point(1, 1), State = CellStateEnum.StateS }, Faceing = FacingEnum.East };
            _order1.Commands = new List<CommandEnum> { command };
            _order1.Battery = battery;

            var cleanService = new CleanService(_order1);

            var result = cleanService.HasEnoughBatteryCappacity(command);

            Assert.That(expectedResult, Is.EqualTo(result));
        }
    }
}
