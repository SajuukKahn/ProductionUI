using ProductionUI.Commands;
using ProductionUI.Models;
using ProductionUI.Tests;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ProductionUI.ViewModels
{
    public class ProductionCardDeckViewModel : INotifyPropertyChanged
    {
        public ProductionCardDeckModel ProductionDeck { get; private set; }
        private readonly ProductionCardDeckModel _productionDeck = new ProductionCardDeckModel();

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private MediaElement _processImage;
        public MediaElement ProcessImage
        {
            get
            {
                return _processImage;
            }
            set
            {
                _processImage = value;
                OnPropertyChanged("ProcessImage");
            }
        }

        private ImageSource _processDisplay;
        public ImageSource ProcessDisplay
        {
            get
            {
                return _processDisplay;
            }
            set
            {
                _processDisplay = value;
                OnPropertyChanged("ProcessDisplay");
            }
        }

        private int _currentIndex;
        public int CurrentIndex
        {
            get
            {
                return _currentIndex;
            }
            set
            {
                _currentIndex = NextCard(value);
                OnPropertyChanged("CurrentIndex");
            }
        }

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

        public event EventHandler PlayBackRunningChanged;

        protected virtual void OnPlayBackRunningChanged(EventArgs e)
        {
            EventHandler handler = PlayBackRunningChanged;
            handler?.Invoke(this, e);
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


        public int NextCard(int value, string StepDataLog = "no data", bool StepPassed = true, bool StepComplete = true)
        {
            if (CurrentIndex > -1 && CurrentIndex < _productionDeck.Count)
            {

                bool setCompleted = false;
                int i = value, ceiling = CurrentIndex;

                if (CurrentIndex < value)
                {
                    setCompleted = true;
                    i = CurrentIndex;
                    ceiling = value;
                }

                for (int j = i; j <= ceiling; j++)
                {
                    if (setCompleted)
                    {
                        _productionDeck[j].StepPassed = StepPassed;
                        _productionDeck[j].StepDataLog = StepDataLog;
                        _productionDeck[j].StepComplete = StepComplete;
                    }
                    else
                    {
                        _productionDeck[j].StepPassed = false;
                        _productionDeck[j].StepDataLog = null;
                        _productionDeck[j].StepComplete = false;
                    }
                    _productionDeck[j].IsActiveStep = false;
                 //   _productionDeck[j].StopCardTimer();
                }

                _productionDeck[value].IsActiveStep = true;
                _productionDeck[value].StepComplete = false;
              //  _productionDeck[value].StartCardTimer();
            }
            return value;
        }

        private void PausePressed()
        {
            PlayBackRunning = false;
        }

        private void PlayPressed()
        {
            PlayBackRunning = true;
        }

        public ProductionCardDeckViewModel()
        {
            RandomProgramGenerator randomProgramGenerator = new RandomProgramGenerator();
            ProductionDeck = randomProgramGenerator.GenerateRandom(_productionDeck);
            randomProgramGenerator.GenerateRandom(this);
            ProcessDisplay = new BitmapImage(new Uri("/Tests/TestImages/PCB" + new Random().Next(1, 9).ToString() + ".bmp", UriKind.Relative));
            PlayButton = new RelayCommand(o => PlayPressed());
            PauseButton = new RelayCommand(o => PausePressed());

            NextCard(0);

            Thread thread = new Thread(() => randomProgramGenerator.RunRandomProgram(this));
            thread.Start();
        }
    }
}

