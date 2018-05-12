using CleaningRobot.Infrastructure.Core.Enums;
using System.Collections.Generic;

namespace CleaningRobot.Infrastructure.Core
{
    public class Order
    {
        public List<List<Cell>> Map { get; set; }

        public StateOfRobot CurrentState { get; set; }

        public List<CommandEnum> Commands { get; set; }

        public int Battery { get; set; }
    }
}
