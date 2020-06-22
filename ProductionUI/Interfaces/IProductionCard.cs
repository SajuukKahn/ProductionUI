using System.Windows.Media;

namespace ProductionUI.Interfaces
{
    interface IProductionCard
    {
        public string StepTitle { get; set; }
        public string StepActiveTime { get; set; }
        public string StepStatus { get; set; }
        public ImageSource StepImage { get; set; }
        public bool StepRequiresUserInteraction { get; set; }
        public bool StepComplete { get; set; }
        public bool IsActiveStep { get; set; }
        public bool StepPassed { get; set; }
        public bool BreakOnError { get; set; }
    }
}
