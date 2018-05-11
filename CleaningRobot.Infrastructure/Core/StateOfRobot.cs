using CleaningRobot.Infrastructure.Core.Enums;

namespace CleaningRobot.Infrastructure.Core
{
    public class StateOfRobot
    {
        public Cell Cell { get; set; }
        public FacingEnum Faceing { get; set; }
    }
}
