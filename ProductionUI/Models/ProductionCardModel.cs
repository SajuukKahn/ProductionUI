using ProductionUI.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProductionUI.Models
{
    public class ProductionCardModel : AProductionCard , INotifyPropertyChanged
    { 

        public ProductionCardModel( ImageSource imageSource = null,
                                    bool isActiveStep = false,
                                    bool stepComplete = false,
                                    string stepActiveTime = "0",
                                    string stepDataLog = "No Data",
                                    string stepPointData = "PolyLine ... ",
                                    string stepStatus = "Waiting...")
        {
            StepImage = imageSource;
            IsActiveStep = isActiveStep;
            StepComplete = stepComplete;
            StepActiveTime = stepActiveTime;
            StepDataDisplay = stepDataLog;
            StepStatus = stepStatus;
            StepDataLog = stepPointData;
        }
    }
}
