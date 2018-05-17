using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing;

namespace CleaningRobot.ConsoleApp
{
	public class PointJsonConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return (objectType == typeof(Point));
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			Point point = (Point)value;
			JObject jo = new JObject();
			jo.Add("X", point.X);
			jo.Add("Y", point.Y);
			jo.WriteTo(writer);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JObject jo = JObject.Load(reader);
			return new Point((int)jo["X"], (int)jo["Y"]);
		}
	}
}
