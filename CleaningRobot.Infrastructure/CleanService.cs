using CleaningRobot.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleaningRobot.Infrastructure
{
    public class CleanService
    {
        private readonly Order _order;

        public CleanService(Order order)
        {
            this._order = order;
        }
    }
}
