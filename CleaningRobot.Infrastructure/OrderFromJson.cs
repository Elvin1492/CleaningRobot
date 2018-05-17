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
				var cellList = new List<Cell>();
				for (int j = 0; j < Map[i].Count; j++)
				{
					var cell = new Cell
					{
						Point = new System.Drawing.Point(j, i),
						State = Map[i].ElementAt(j) == "S" ? CellStateEnum.StateS : Map[i].ElementAt(j) == "C" ? CellStateEnum.StateC : CellStateEnum.StateN
					};
					cellList.Add(cell);
				}
				order.Map.Add(cellList);
			}

			order.CurrentState = new StateOfRobot
			{
				Cell = new Cell
				{
					Point = new System.Drawing.Point(this.Start.X, this.Start.Y)
				},
				Faceing = Start.ConvertFace()
			};

			order.Commands = GetListOfCommands();

			return order;
		}

		private List<CommandEnum> GetListOfCommands()
		{
			var list = new List<CommandEnum>();
			foreach (var item in Commands)
			{
				switch (item.ToUpper())
				{
					case "TL":
						list.Add(CommandEnum.TL);
						break;
					case "TR":
						list.Add(CommandEnum.TR);
						break;
					case "A":
						list.Add(CommandEnum.A);
						break;
					case "B":
						list.Add(CommandEnum.B);
						break;
					case "C":
						list.Add(CommandEnum.C);
						break;
					default:
						throw new Exception();
				}
			}

			return list;
		}
	}

	public class Start
	{
		public int X { get; set; }
		public int Y { get; set; }
		public string Facing { get; set; }

		public FacingEnum ConvertFace()
		{
			switch (Facing.ToUpper())
			{
				case "N":
					return FacingEnum.North;
				case "S":
					return FacingEnum.South;
				case "W":
					return FacingEnum.West;
				case "E":
					return FacingEnum.East;
				default:
					throw new Exception();
			}
		}
	}

	public class ResultToJson
	{
		public List<Point> Visited { get; set; }
		public List<Point> Cleaned{ get; set; }
		public Start Final { get; set; }
		public int Battery { get; set; }
	}
}
