using ProductionUI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProductionUI.Interfaces
{
    interface IProductionCard
    {
        public string StepDataDisplay { get; set; }
        public string StepActiveTime { get; set; }
        public string StepStatus { get; set; }
        public string StepPointData { get; set; }
        public ImageSource StepImage { get; set; }
        public Visibility ImageVisible { get; set; }
        public bool StepComplete { get; set; }
        public bool IsActiveStep { get; set; }
        public bool StepPassed { get; set; }
        public bool BreakOnError { get; set; }
    }
}
