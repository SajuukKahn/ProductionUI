using ProductionUI.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace ProductionUI.Abstracts
{
    public abstract class AProductionCard : IProductionCard , INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string prop)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        public ICommand PlayButton { get; set; }
        public ICommand PauseButton { get; set; }

        private string _processLocation;

        public string ProcessLocation 
        {
            get
            {
                return _processLocation;
            }
            
            set
            {
                _processLocation = value;
                OnPropertyChanged("ProcessLocation");
            }
        
        }

        private string _programName;
        public string ProgramName
        {
            get
            {
                return _programName;
            }
            set 
            {
                _programName = value;
                OnPropertyChanged("ProgramName");
            }
        }

        private string _stepDataDisplay;
        public string StepDataDisplay
        {
            get
            {
                return _stepDataDisplay;
            }
            set
            {
                _stepDataDisplay = value;
                OnPropertyChanged("StepDataDisplay");
            }
        }


        private string _stepActiveTime;
        public string StepActiveTime
        {
            get
            {
                return _stepActiveTime;
            }
            set
            {
                _stepActiveTime = value;
                OnPropertyChanged("StepActiveTime");
            }
        }

        private string _stepStatus;
        public string StepStatus
        {
            get
            {
                return _stepStatus;
            }
            set
            {
                _stepStatus = value;
                OnPropertyChanged("StepStatus");
            }
        }
        private bool _stepComplete;
        public bool StepComplete
        {
            get
            {
                return _stepComplete;
            }
            set
            {
                _stepComplete = value;
                OnPropertyChanged("StepComplete");
            }
        }

        private int[,,,] _stepPointData;
        public int[,,,] StepPointData
        {
            get
            {
                return _stepPointData;
            }
            set
            {
                _stepPointData = value;
                OnPropertyChanged("StepPointData");
            }
        }
    }
}
