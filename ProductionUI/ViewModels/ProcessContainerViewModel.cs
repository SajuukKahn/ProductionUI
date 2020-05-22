using ProductionUI.Abstracts;
using ProductionUI.Commands;
using ProductionUI.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ProductionUI.ViewModels
{
    public class ProcessContainerViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
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

        public ProcessContainerViewModel()
        {
            PlayButton = new RelayCommand(o => PlayPressed());
            PauseButton = new RelayCommand(o => PausePressed());
        }

        private void PausePressed()
        {
            
        }

        private void PlayPressed()
        {
            
        }
    }
}
