using CleaningRobot.Infrastructure.Core.Enums;
using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleaningRobot.Infrastructure.Core
{
    public class Cell
    {
        public Point Point { get; set; }
        public CellStateEnum State { get; set; }
        public bool IsVisited { get; set; }

        public bool IsVisitable
        {
            get
            {
                return State == CellStateEnum.StateS;
            }
        }
    }
}
