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

        public static bool PlayBackIsRunning = true;
        
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
                PlayBackIsRunning = value;
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

        public ProcessContainerViewModel()
        {
            PlayButton = new RelayCommand(o => PlayPressed());
            PauseButton = new RelayCommand(o => PausePressed());

            RandomProgramGenerator randomProgramGenerator = new RandomProgramGenerator();
            randomProgramGenerator.GenerateRandom(this);
        }

        private void PausePressed()
        {
            PlayBackIsRunning ^= true;
        }

        private void PlayPressed()
        {
            PlayBackIsRunning ^= true;
        }
    }
}
