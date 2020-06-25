using ProductionUI.Models;
using System.Collections.Generic;
using System.Windows.Media;

namespace ProductionUI.Interfaces
{
    interface IProductionCard
    {
        public string StepTitle { get; set; }
        public List<ProductionCardSubStepModel> CardSubSteps { get; set; }
        public ProductionCardStepStatus StepStatus { get; set; }
        public ImageSource StepImage { get; set; }
        public bool StepComplete { get; set; }
        public bool IsActiveStep { get; set; }
        public bool StepPassed { get; set; }
        public bool BreakOnError { get; set; }
    }
}
