using CleaningRobot.Infrastructure.Core;
using CleaningRobot.Infrastructure.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleaningRobot.Infrastructure
{
    public class OrderFromJson
    {
        public List<List<string>> Map { get; set; }
        public Start Start { get; set; }
        public string[] Commands { get; set; }
        public int Battery { get; set; }

        public Order ConvertToOrder()
        {
            var order = new Order
            {
                Battery = this.Battery
            };

            for (int i = 0; i < Map.Count; i++)
            {
                for (int j = 0; j < Map[i].Count; j++)
                {
                    var cell = new Cell {
                        Point = new System.Drawing.Point(j, i),
                        State = Map[i].ElementAt(j) == "S" ? CellStateEnum.StateS : Map[i].ElementAt(j) == "C" ? CellStateEnum.StateC : CellStateEnum.StateN
                    };
                }
            }

            return order;
        }
        
    }

    public class Start
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Facing { get; set; }
    }
}
