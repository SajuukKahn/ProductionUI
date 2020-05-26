using ProductionUI.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ProductionUI.Models
{
    public class ProductionCardModel : AProductionCard , INotifyPropertyChanged
    { 

        public ProductionCardModel(bool isActiveStep = false,
                                    bool stepComplete = false,
                                    string stepActiveTime = "0",
                                    string stepDataDisplay = "No Data",
                                    string stepPointData = "PolyLine ... ",
                                    string stepStatus = "Waiting...")
        {
            IsActiveStep = isActiveStep;
            StepComplete = stepComplete;
            StepActiveTime = stepActiveTime;
            StepDataDisplay = stepDataDisplay;
            StepStatus = stepStatus;
            StepPointData = stepPointData;
        }
    }
}
