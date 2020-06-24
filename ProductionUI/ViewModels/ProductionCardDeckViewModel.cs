using Microsoft.VisualBasic.CompilerServices;
using ProductionUI.Commands;
using ProductionUI.Models;
using ProductionUI.Tests;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Schema;

namespace ProductionUI.ViewModels
{
    public class ProductionCardDeckViewModel : INotifyPropertyChanged
    {
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

        private DateTime _dateTime = new DateTime();

        private void SetCycleTimer()
        {
            _dateTime = DateTime.Now;
        }

        private string GetCycleTimer()
        {
            return new DateTimeConverter().ConvertToString(DateTime.Now - _dateTime);
        }

        private double _cycleTime;
        public double CycleTime
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
                if(_cycleTimeTask.Status != TaskStatus.Running)
                {
                    _cycleTimeTask.Start();
                }
            }
            PlayBackRunning = true;
        }

        public void LoadProductionDeck(bool reUseProgram = false)
        {
            PlayBackRunning = false;
            DeckCompleted = false;
            CycleTime = 0;
            LoadRandomProgram(reUseProgram);
            _timerCancellationTokenSource.Cancel();
            CycleTime = 0;
            PlayAvailable = true;
            PauseAvailable = false;
        }

        private bool _deckCompleted;
        public bool DeckCompleted
        {
            get
            {
                return _deckCompleted;
            }
            set
            {
                _deckCompleted = value;
                if(value)
                {
                    LoadProductionDeck(true);
                }
            }
        }


        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancelToken;


        private void LoadRandomProgram(bool reUseProgram = false)
        {
            if(!reUseProgram)
            { 
                RandomProgramGenerator randomProgramGenerator = new RandomProgramGenerator();
                ProcessDisplay = new BitmapImage(new Uri("/Tests/TestImages/PCB" + new Random().Next(1, 9).ToString() + ".bmp", UriKind.Relative));
                _productionDeck.Clear();
                ProductionDeck = randomProgramGenerator.GenerateRandom(_productionDeck);
                randomProgramGenerator.GenerateRandom(this);
            }
            if (_cancellationTokenSource == null)
            {
                _cancellationTokenSource = new CancellationTokenSource();
                _cancelToken = _cancellationTokenSource.Token;
            }
            else
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = new CancellationTokenSource();
                _cancelToken = _cancellationTokenSource.Token;
            }

            var task = Task.Run(async() =>
            {
                Debug.WriteLine(this.ProductionDeck.Count);
                this.CurrentIndex = 0;
                //for (int i = 1; i < this.ProductionDeck.Count; i++) ;

                while(this.CurrentIndex < (this.ProductionDeck.Count - 1))
                {
                    Debug.WriteLine("CurrentIndex: " + this.CurrentIndex + " count: " + this.ProductionDeck.Count);
                    while (!this.PlayBackRunning)
                    {
                        if (_cancelToken.IsCancellationRequested)
                        {
                            return;
                        }
                    }

                    if (!_cancelToken.IsCancellationRequested)
                    {
                        await Task.Delay((TimeSpan.FromSeconds(new Random().Next(1, 5) + new Random().NextDouble())));
                        while (!this.PlayBackRunning)
                        {
                            if (_cancelToken.IsCancellationRequested)
                            {
                                return;
                            }
                        }
                        this.CurrentIndex++;
                    }
                    else
                    {
                        return;
                    }
                    Debug.WriteLine(this.CurrentIndex < this.ProductionDeck.Count - 1);
                }
                Debug.WriteLine("EXIT CurrentIndex: " + this.CurrentIndex + " count: " + this.ProductionDeck.Count);
                await Task.Delay((TimeSpan.FromSeconds(new Random().Next(1, 5) + new Random().NextDouble())));
                this.DeckCompleted = true;
            },_cancelToken);

            NextCard(0);
            OnPropertyChanged(nameof(ProductionDeck));
        }

        private CancellationTokenSource _timerCancellationTokenSource;
        private CancellationToken _timerCancelToken;
        private Task _cycleTimeTask;

        public ProductionCardDeckViewModel()
        {
            PlayButton = new RelayCommand(o => PlayPressed());
            PauseButton = new RelayCommand(o => PausePressed());
            PlayAvailable = false;
            PauseAvailable = false;

            if (_timerCancellationTokenSource == null)
            {
                _timerCancellationTokenSource = new CancellationTokenSource();
                _timerCancelToken = _timerCancellationTokenSource.Token;
            }
            else
            {
                _timerCancellationTokenSource.Cancel();
                _timerCancellationTokenSource.Dispose();
                _timerCancellationTokenSource = new CancellationTokenSource();
                _timerCancelToken = _timerCancellationTokenSource.Token;
            }

            _cycleTimeTask = Task.Run(async () =>
            {
                while(true && !_timerCancelToken.IsCancellationRequested)
                {
                    while(!_timerCancelToken.IsCancellationRequested)
                    { 
                        await Task.Delay(TimeSpan.FromMilliseconds(10));
                        CycleTime += 0.01;
                    }
                }
                return;
            }, _timerCancelToken);

        }

        private void CycleTime_Tick()
        {

        }

        public event EventHandler PlayBackRunningChanged;
        protected virtual void OnPlayBackRunningChanged(EventArgs e)
        {
            EventHandler handler = PlayBackRunningChanged;
            handler?.Invoke(this, e);
        }

        public ICommand PlayButton { get; set; }
        public ICommand PauseButton { get; set; }
    }
}

