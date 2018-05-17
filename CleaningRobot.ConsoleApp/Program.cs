using CleaningRobot.Infrastructure;
using CleaningRobot.Infrastructure.Core;
using CleaningRobot.Infrastructure.Core.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleaningRobot.ConsoleApp
{
	class Program
	{
		static void Main(string[] args)
		{
			var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.ToString(), args[0]);
			using (StreamReader r = new StreamReader(path))
			{
				string json = r.ReadToEnd();
				dynamic array = JsonConvert.DeserializeObject(json);
				var orderFromJson = JsonConvert.DeserializeObject<OrderFromJson>(json);
				var order = orderFromJson.ConvertToOrder();

				CleanService cleanService = new CleanService(order);

				var result = cleanService.Start();

				var resultToJson = new ResultToJson
				{
					Battery = result.Battery,
					Cleaned = result.CleanedCells.Select(x => x.Point).ToList(),
					Visited = result.VisitedCells.Select(x => x.Point).ToList(),
					Final = new Start
					{
						Facing = Enum.GetName(typeof(FacingEnum), result.FinalState.Faceing).Substring(0, 1),
						X = result.FinalState.Cell.Point.X,
						Y = result.FinalState.Cell.Point.Y
					}
				};

				var resultJson = JsonConvert.SerializeObject(resultToJson, new PointJsonConverter());

				var resultPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.ToString(), args[1]);

				File.WriteAllText(resultPath, JToken.Parse(resultJson).ToString(Formatting.Indented));

				Console.WriteLine("Done");
			}
		}
	}
}
