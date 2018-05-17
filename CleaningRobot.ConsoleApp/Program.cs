using CleaningRobot.Infrastructure;
using CleaningRobot.Infrastructure.Core;
using Newtonsoft.Json;
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
            using (StreamReader r = new StreamReader("test1.json"))
            {
                string json = r.ReadToEnd();
                dynamic array = JsonConvert.DeserializeObject(json);
                var test = JsonConvert.DeserializeObject<OrderFromJson>(json);
                foreach (var item in array)
                {
                    Console.WriteLine("{0} {1}", item.temp, item.vcc);
                }
            }

            Console.ReadKey();
        }
    }
}
