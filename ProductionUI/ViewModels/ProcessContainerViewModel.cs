using ProductionUI.Commands;
using ProductionUI.Interfaces;
using ProductionUI.Tests;
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

        public event EventHandler PlayBackRunningChanged;

        protected virtual void OnPlayBackRunningChanged(EventArgs e)
        {
            EventHandler handler = PlayBackRunningChanged;
            handler?.Invoke(this, e);
        }

        public static bool PlayBackIsRunning = false;
        
        public ICommand PlayButton { get; set; }
        public ICommand PauseButton { get; set; }

        private bool _playBackRunning;
        public bool PlayBackRunning
        {
            get
            {
                return _playBackRunning;
            }
            set
            {
                _playBackRunning = value;
                OnPropertyChanged("PlayBackRunning");
            }
        }


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

        private Timer _cycleTime;
        public Timer CycleTime
        {
            get 
            { 
                return _cycleTime; 
            }
            set 
            { 
                _cycleTime = value; 
                OnPropertyChanged("CycleTime");
            }
        }


        private long _cycleCount;
        public long CycleCount
        {
            get
            {
                return _cycleCount;
            }
            set
            {
                _cycleCount = value;
            }
        }

        public ProcessContainerViewModel()
        {
            PlayButton = new RelayCommand(o => PlayPressed());
            PauseButton = new RelayCommand(o => PausePressed());

            RandomProgramGenerator randomProgramGenerator = new RandomProgramGenerator();
            randomProgramGenerator.GenerateRandom(this);
        }

        private void PausePressed()
        {
            PlayBackIsRunning = false;
        }

        private void PlayPressed()
        {
            PlayBackIsRunning = true;
        }
    }
}
