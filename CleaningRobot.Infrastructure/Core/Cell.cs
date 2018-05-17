using CleaningRobot.Infrastructure.Core.Enums;
using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleaningRobot.Infrastructure.Core
{
    public class Cell : IEquatable<Cell>, IComparable<Cell>
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

		public int CompareTo(Cell other)
		{
			if (this.Point.X == other.Point.X)
			{
				return this.Point.Y - other.Point.Y;
			}
			else
			{
				return this.Point.X - other.Point.X;
			}
		}

		public bool Equals(Cell other)
        {
            if (other == null)
            {
                return false;
            }

            if (this.Point.X == other.Point.X && this.Point.Y == other.Point.Y && this.State == other.State && this.IsVisited == other.IsVisited)
            {
                return true;
            }
            return false;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null)
                return false;

            Cell personObj = obj as Cell;

            if (personObj == null)
                return false;
            else
                return Equals(personObj);

        }

        public override int GetHashCode()

        {

            return this.IsVisited.GetHashCode();

        }
    }
}
