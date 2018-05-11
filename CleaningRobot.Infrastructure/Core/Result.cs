﻿using System.Collections.Generic;

namespace CleaningRobot.Infrastructure.Core
{
    public class Result
    {
        public List<Cell> VisitedCells { get; set; }
        public List<Cell> CleanedCells { get; set; }
        public StateOfRobot FinalState { get; set; }
        public int Battery { get; set; }
    }
}
