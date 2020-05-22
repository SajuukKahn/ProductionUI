using ProductionUI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace ProductionUI.Interfaces
{
    interface IProductionCard
    {
        public string StepDataDisplay { get; set; }
        public string StepActiveTime { get; set; }
        public string StepStatus { get; set; }
        public bool StepComplete { get; set; }
        public bool IsActiveStep { get; set; }
        public StepCoordinatesModel StepPointData { get; set; }
    }
}
