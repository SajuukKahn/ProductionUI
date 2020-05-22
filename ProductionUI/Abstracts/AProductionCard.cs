using ProductionUI.Interfaces;
using ProductionUI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ProductionUI.Abstracts
{
    public abstract class AProductionCard : IProductionCard , INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
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


        private bool _isActiveStep;
        public bool IsActiveStep
        {
            get
            {
                return _isActiveStep;
            }
            set
            {
                _isActiveStep = value;
                OnPropertyChanged("IsActiveStep");
            }
        }

        private StepCoordinatesModel _stepPointData;
        public StepCoordinatesModel StepPointData
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

        private readonly DispatcherTimer t;

        private DateTime _startTime;

        public AProductionCard()
        {

            DependencyObject dep = new DependencyObject();
            if (DesignerProperties.GetIsInDesignMode(dep) != true)
            {
                t = new DispatcherTimer
                {
                    IsEnabled = true,
                    Interval = TimeSpan.FromMilliseconds(1)
                };
                t.Tick += Tick;
                ResetRunTime();
                //t.Start();
            }
        }

        public void ResetRunTime()
        {
            _startTime = System.DateTime.Now;
        }

        private void Tick(object sender, EventArgs e)
        {
            StepActiveTime = Convert.ToString(DateTime.Now - _startTime);
        }

        ~AProductionCard()
        {
            t.Stop();
            t.IsEnabled = false;
        }
    }
}
