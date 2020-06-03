using ProductionUI.Interfaces;
using ProductionUI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace ProductionUI.Abstracts
{
    public abstract class AProductionCard : IProductionCard, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            if (prop == "IsActiveStep")
            {
                if (IsActiveStep == false)
                {
                    ImageVisible = Visibility.Collapsed;
                    t.Stop();
                }
                else
                {
                    ImageVisible = Visibility.Visible;
                    ResetRunTime();
                    t.Start();
                }
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

        private string _stepDataLog;
        public string StepDataLog
        {
            get
            {
                return _stepDataLog;
            }
            set
            {
                _stepDataLog = value;
                OnPropertyChanged("StepDataLog");
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


        private ImageSource _stepImage;
        public ImageSource StepImage
        {
            get
            {
                return _stepImage;
            }
            set
            {
                _stepImage = value;
                OnPropertyChanged("StepImage");
            }
        }

        private Visibility _imageVisible;
        public Visibility ImageVisible
        {
            get
            {
                return _imageVisible;
            }
            set
            {
                _imageVisible = value;
                OnPropertyChanged("ImageVisible");
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

        private bool _stepPassed;
        public bool StepPassed
        {
            get
            {
                return _stepPassed;
            }
            set
            {
                _stepPassed = value;
                OnPropertyChanged("Step Passed");
            }
        }

        private bool _breakOnError;
        public bool BreakOnError
        {
            get
            {
                return _breakOnError;
            }
            set
            {
                _breakOnError = value;
                OnPropertyChanged("BreakOnError");
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
