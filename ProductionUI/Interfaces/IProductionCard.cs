using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace ProductionUI.Interfaces
{
    interface IProductionCard
    {
        public string ProcessLocation { get; set; }
        public string ProgramName { get; set; }
        public string StepDataDisplay { get; set; }
        public string StepActiveTime { get; set; }
        public string StepStatus { get; set; }
        public bool StepComplete { get; set; }
        public int[,,,] StepPointData { get; set; }
    }
}
