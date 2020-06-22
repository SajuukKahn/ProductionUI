using ProductionUI.Commands;
using ProductionUI.Models;
using ProductionUI.Tests;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Schema;

namespace ProductionUI.ViewModels
{
    public class ProductionCardDeckViewModel : INotifyPropertyChanged
    {
        private Thread _thread;

        private ProductionCardCollection _productionDeck = new ProductionCardCollection();
        public ProductionCardCollection ProductionDeck
        {
            get
            {
                return _productionDeck;
            }
            set
            {
                _productionDeck = value;
                OnPropertyChanged(nameof(ProductionDeck));
                OnPropertyChanged(nameof(CardCount));
            }
        }

        private ProductionProgramCollection _productionPrograms = new ProductionProgramCollection();
        public ProductionProgramCollection ProductionPrograms
        {
            get
            {
                return _productionPrograms;
            }
            set
            {
                _productionPrograms = value;
                OnPropertyChanged(nameof(ProductionPrograms));
            }
        }

        //public ProductionCardModel PauseCard { get; private set; }
        //private readonly ProductionCardModel _defaultPauseCard = new ProductionCardModel()
        //{
        //    StepTitle = "Paused.\nPress Play to Continue Playback",
        //    StepStatus = "",
        //    StepRequiresUserInteraction = true,
        //    StepActiveTime = null
        //};

        //public ProductionCardModel InitialCard { get; private set; }
        //private ProductionCardModel _defaultInitialCard = new ProductionCardModel()
        //{   
        //    StepTitle = "Press Play to Begin Playback",
        //    StepStatus = "",
        //    StepRequiresUserInteraction = true,
        //    StepActiveTime = null
        //};

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
                OnPropertyChanged(nameof(ProcessImage));
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
                OnPropertyChanged(nameof(ProcessDisplay));
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
                OnPropertyChanged(nameof(CurrentIndex));
            }
        }

        private int _currentProgram  = -1;
        public int CurrentProgram
        {
            get
            {
                return _currentProgram;
            }
            set
            {
                if(_currentProgram != value)
                {
                    _currentProgram = value;
                    LoadProductionDeck();
                    OnPropertyChanged(nameof(CurrentProgram));
                }
            }
        }

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
                OnPropertyChanged(nameof(PlayBackRunning));
            }
        }

        private bool _playAvailable;
        public bool PlayAvailable
        {
            get
            {
                return _playAvailable;
            }
            set
            {
                _playAvailable = value;
                OnPropertyChanged(nameof(PlayAvailable));
            }
        }

        private bool _pauseAvailable;
        public bool PauseAvailable
        {
            get
            {
                return _pauseAvailable;
            }
            set
            {
                _pauseAvailable = value;
                OnPropertyChanged(nameof(PauseAvailable));
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
                OnPropertyChanged(nameof(ProgramName));
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
                OnPropertyChanged(nameof(CycleTime));
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
                OnPropertyChanged(nameof(CycleCount));
            }
        }

        public int CardCount
        {
            get
            {
                return _productionDeck.Count;
            }
        }

        public int NextCard(int value, bool StepPassed = true, bool StepComplete = true)
        {
            if (value > -1 && CurrentIndex > -1 && CurrentIndex < _productionDeck.Count)
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
                        _productionDeck[j].StepComplete = StepComplete;
                    }
                    else
                    {
                        _productionDeck[j].StepPassed = false;
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
            PlayAvailable = true;
            PauseAvailable = false;
        }

        private void PlayPressed()
        {
            if(PlayBackRunning == false)
            {
                PlayAvailable = false;
                PauseAvailable = true;
            }
            PlayBackRunning = true;
        }

        public void LoadProductionDeck()
        {
            PlayBackRunning = false;
            LoadRandomProgram();
            PlayAvailable = true;
            PauseAvailable = false;
        }

        private void LoadRandomProgram()
        {

            RandomProgramGenerator randomProgramGenerator = new RandomProgramGenerator();
            ProcessDisplay = new BitmapImage(new Uri("/Tests/TestImages/PCB" + new Random().Next(1, 9).ToString() + ".bmp", UriKind.Relative));
            _productionDeck.Clear();
            ProductionDeck = randomProgramGenerator.GenerateRandom(_productionDeck);
            randomProgramGenerator.GenerateRandom(this);

            NextCard(0);

            if (_thread != null && _thread.IsAlive)
            {
                _thread.Abort();
            }
            _thread = new Thread(() => randomProgramGenerator.RunRandomProgram(this))
            {
                IsBackground = true
            };
            _thread.Start();
        }

        public ProductionCardDeckViewModel()
        {
            PlayButton = new RelayCommand(o => PlayPressed());
            PauseButton = new RelayCommand(o => PausePressed());
            _playAvailable = false;
            _pauseAvailable = false;
        }

        public event EventHandler PlayBackRunningChanged;
        protected virtual void OnPlayBackRunningChanged(EventArgs e)
        {
            EventHandler handler = PlayBackRunningChanged;
            handler?.Invoke(this, e);
        }

        public ICommand PlayButton { get; set; }
        public ICommand PauseButton { get; set; }

        ~ProductionCardDeckViewModel()
        {
            _thread.Abort();
        }
    }
}

