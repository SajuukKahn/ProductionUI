using Microsoft.VisualBasic.CompilerServices;
using ProductionUI.Commands;
using ProductionUI.Models;
using ProductionUI.Tests;
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml.Schema;

namespace ProductionUI.ViewModels
{
    public class ProductionCardDeckViewModel : INotifyPropertyChanged
    {
        public ICommand PlayButton { get; set; }
        public ICommand PauseButton { get; set; }
        public ICommand ConfirmButton { get; set; }
        public ICommand CancelButton { get; set; }

        public ProductionCardDeckViewModel()
        {
            PlayButton = new RelayCommand(o => PlayPressed());
            PauseButton = new RelayCommand(o => PausePressed());
            ConfirmButton = new RelayCommand(o => ConfirmProgramChange());
            CancelButton = new RelayCommand(o => CancelProgramChange());
            InitializeCycleTimer();
            InitializeCardTimer();
            PlayAvailable = false;
            PauseAvailable = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private ProductionCardCollection _productionCards = new ProductionCardCollection();
        public ProductionCardCollection ProductionCards
        {
            get
            {
                return _productionCards;
            }
            set
            {
                _productionCards = value;
                OnPropertyChanged(nameof(ProductionCards));
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

        //private MediaElement _processImage;
        //public MediaElement ProcessImage
        //{
        //    get
        //    {
        //        return _processImage;
        //    }
        //    set
        //    {
        //        _processImage = value;
        //        OnPropertyChanged(nameof(ProcessImage));
        //    }
        //}

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

        private bool _programSelectionConfirmationRaised;
        public bool ProgramSelectionConfirmationRaised
        {
            get
            {
                return _programSelectionConfirmationRaised;
            }
            set
            {
                _programSelectionConfirmationRaised = value;
                OnPropertyChanged(nameof(ProgramSelectionConfirmationRaised));
            }
        }

        private int _currentSubStepIndex;
        public int CurrentSubStepIndex
        {
            get
            {
                return _currentSubStepIndex;
            }
            set
            {
                _currentSubStepIndex = value;
                OnPropertyChanged(nameof(CurrentSubStepIndex));
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

        private bool _allowProgramChange = true;
        public bool AllowProgramChange
        {
            get
            {
                return _allowProgramChange;
            }
            set
            {
                _allowProgramChange = value;
                OnPropertyChanged(nameof(AllowProgramChange));
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


        public int NextCard(int value, bool StepPassed = true, bool StepComplete = true)
        {
            if (value > -1 && CurrentIndex > -1 && CurrentIndex < _productionCards.Count)
            {

                bool setCompleted = false;
                int i = value, ceiling = CurrentIndex;

                // need to add in some stuff here... changing the "status" of the card, for instance

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
                        _productionCards[j].StepPassed = StepPassed;
                        _productionCards[j].StepComplete = StepComplete;
                    }
                    else
                    {
                        _productionCards[j].StepPassed = false;
                        _productionCards[j].StepComplete = false;
                    }
                    _productionCards[j].IsActiveStep = false;
                }

                _productionCards[value].IsActiveStep = true;
                _productionCards[value].StepComplete = false;
            }

            return value;
        }

        private int _newProgramSelection = -1;
        private int _currentProgram = -1;
        public int CurrentProgram
        {
            get
            {
                return _currentProgram;
            }
            set
            {
                if (_currentProgram != value)
                {
                    VerifyChange(value);
                }
            }
        }

        private string _oldProgramName;
        private string _programName;
        public string ProgramName
        {
            get
            {
                return _programName;
            }
            set
            {
                _oldProgramName = _programName;
                _programName = value;
                OnPropertyChanged(nameof(ProgramName));
            }
        }

        private void VerifyChange(int newProgramSelectionValue)
        {
            if (CycleStartTime.Equals(null) || DeckCompleted)
            {
                _currentProgram = newProgramSelectionValue;
                OnPropertyChanged(nameof(CurrentProgram));
                LoadProductionDeck();
                _newProgramSelection = -1;
                return;
            }
            PauseAvailable = false;
            PlayAvailable = false;
            AllowProgramChange = false;
            ProgramSelectionConfirmationRaised = true;
            _newProgramSelection = newProgramSelectionValue;
        }

        private void ConfirmProgramChange()
        {
            _currentProgram = _newProgramSelection;
            OnPropertyChanged(nameof(CurrentProgram));
            LoadProductionDeck();
            PlayAvailable = true;
            ProgramSelectionConfirmationRaised = false;
            _newProgramSelection = -1;
        }

        private void CancelProgramChange()
        {
            _newProgramSelection = -1;
            AllowProgramChange = true;
            CurrentProgram = _currentProgram;
            PlayAvailable = true;
            ProgramName = _oldProgramName;
            ProgramSelectionConfirmationRaised = false;
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
                AllowProgramChange = false;
                PlayAvailable = false;
                PauseAvailable = true;
                StartCycleTimer();
                StartCardTimer();
            }
            PlayBackRunning = true;
        }

        public void LoadProductionDeck(bool reUseProgram = false)
        {
            PlayBackRunning = false;
            AllowProgramChange = true;
            DeckCompleted = false;
            LoadRandomProgram(reUseProgram);
            ResetCycleTimer();
            ResetCardTimer();
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
                OnPropertyChanged(nameof(DeckCompleted));
                if(value)
                {
                    var task = Task.Run(async() =>
                    {
                        Debug.WriteLine("Beginning ~3s wait");
                        await Task.Delay(2750);
                        Debug.WriteLine("Async load of production deck?");
                        LoadProductionDeck(true);
                        Debug.WriteLine("Async load success");
                    });
                }
            }
        }

        #region MechanicalTurk

        private CancellationTokenSource _randomProgramCancellationTokenSource;
        private CancellationToken _randomProgramCancelToken;

        private void LoadRandomProgram(bool reUseProgram = false)
        {
            if(!reUseProgram)
            { 
                RandomProgramGenerator randomProgramGenerator = new RandomProgramGenerator();
                ProcessDisplay = new BitmapImage(new Uri("/Tests/TestImages/PCB" + new Random().Next(1, 9).ToString() + ".bmp", UriKind.Relative));
                _productionCards.Clear();
                ProductionCards = randomProgramGenerator.GenerateRandom(_productionCards);
                randomProgramGenerator.GenerateRandom(this);
            }
            if (_randomProgramCancellationTokenSource == null)
            {
                _randomProgramCancellationTokenSource = new CancellationTokenSource();
                _randomProgramCancelToken = _randomProgramCancellationTokenSource.Token;
            }
            else
            {
                _randomProgramCancellationTokenSource.Cancel();
                _randomProgramCancellationTokenSource.Dispose();
                _randomProgramCancellationTokenSource = new CancellationTokenSource();
                _randomProgramCancelToken = _randomProgramCancellationTokenSource.Token;
            }

            var task = Task.Run(async() =>
            {
                CurrentIndex = 0;

                while(CurrentIndex < (ProductionCards.Count - 1))
                {
                    while (!PlayBackRunning)
                    {
                        AllowProgramChange = true;
                        if (_randomProgramCancelToken.IsCancellationRequested)
                        {
                            return;
                        }
                    }
                    AllowProgramChange = false;
                    if (!_randomProgramCancelToken.IsCancellationRequested)
                    {
                        while (CurrentSubStepIndex < (_productionCards[CurrentIndex].CardSubSteps.Count - 1))
                        {
                            await Task.Delay((TimeSpan.FromSeconds(new Random().Next(1, 5) + new Random().NextDouble())));
                            while (!PlayBackRunning)
                            {
                                PauseCardTimer();
                                AllowProgramChange = true;
                                if (_randomProgramCancelToken.IsCancellationRequested)
                                {
                                    return;
                                }
                            }
                            StartCardTimer();
                            CurrentSubStepIndex++;
                        }
                        AllowProgramChange = false;
                        await Task.Delay(TimeSpan.FromSeconds(new Random().Next(1, 5) + new Random().NextDouble()));
                        while (!PlayBackRunning)
                        {
                            PauseCardTimer();
                            if (_randomProgramCancelToken.IsCancellationRequested)
                            {
                                return;
                            }
                        }
                        CurrentSubStepIndex = 0;
                        CurrentIndex++;
                        ResetCardTimer();
                        StartCardTimer();
                    }
                    else
                    {
                        return;
                    }
                }
                DeckCompleted = true;
            },_randomProgramCancelToken);

            NextCard(0);
            OnPropertyChanged(nameof(ProductionCards));
        }

        #endregion

        #region CycleTime handler

        private DispatcherTimer _cycleTimer = null;
        private readonly Stopwatch _cycleStopwatch = new Stopwatch();
        private TimeSpan _cycleDifference;

        public void ResetCycleTimer()
        {
            _cycleStopwatch.Reset();
            _cycleDifference = new TimeSpan(0);
            CycleTime = new TimeSpan(0).ToString("hh\\:mm\\:ss\\.ff");
        }

        public void PauseCycleTimer()
        {
            _cycleStopwatch.Stop();
        }

        public void StartCycleTimer()
        {
           _cycleStopwatch.Start();
            CycleStartTime = DateTime.Now;
        }

        public void InitializeCycleTimer()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                _cycleTimer = new DispatcherTimer();
                _cycleTimer.Tick += Cycle_Timer_Tick;
                _cycleTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);

            }, DispatcherPriority.Normal);
        }

        private DateTime? _cycleStartTime;
        public DateTime? CycleStartTime
        {
            get { return _cycleStartTime; }
            set
            {
                //if (_cycleStartTime != value)
                //{
                    if (!_cycleStartTime.HasValue)
                    {
                        _cycleStartTime = value;
                        _cycleDifference = DateTime.Now.Subtract(CycleStartTime.Value);
                        System.Windows.Application.Current.Dispatcher.Invoke(() =>
                        {
                            _cycleStopwatch.Start();
                            _cycleTimer.Start();
                        }, DispatcherPriority.Normal);
                    }
                    else if (_cycleStartTime.HasValue && value == null)
                    {
                        _cycleStopwatch.Stop();
                        _cycleStartTime = value;
                    }
                    else if (_cycleStartTime.HasValue && value != null)
                    {
                        _cycleStartTime = value;
                        _cycleDifference = DateTime.Now.Subtract(CycleStartTime.Value);
                    }
                    OnPropertyChanged(nameof(CycleStartTime));
                //}
            }
        }

        private string _cycleTime;
        public string CycleTime
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

        void Cycle_Timer_Tick(object sender, EventArgs e)
        {
            if (_cycleStopwatch.IsRunning)
            {
                TimeSpan total = _cycleDifference + _cycleStopwatch.Elapsed;
                CycleTime = total.ToString("hh\\:mm\\:ss\\.ff");
                //TimeElapsed = String.Format("{0:0} {1:00}:{2:00}:{3:00}.{4:00}", total.Days, total.Hours, total.Minutes, total.Seconds, total.Milliseconds);
            }
        }

        #endregion

        #region CardTime handler

        private DispatcherTimer _cardTimer = null;
        private readonly Stopwatch _cardStopwatch = new Stopwatch();
        private TimeSpan _cardDifference;

        public void ResetCardTimer()
        {
            _cardStopwatch.Reset();
            _cardDifference = new TimeSpan(0);
            CardTime = new TimeSpan(0).ToString("hh\\:mm\\:ss\\.ff");
        }

        public void PauseCardTimer()
        {
            _cardStopwatch.Stop();
        }

        public void StartCardTimer()
        {
            _cardStopwatch.Start();
            CardStartTime = DateTime.Now;
        }

        public void InitializeCardTimer()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                _cardTimer = new DispatcherTimer();
                _cardTimer.Tick += Card_Timer_Tick;
                _cardTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            }, DispatcherPriority.Normal);
        }

        private DateTime? _cardStartTime;
        public DateTime? CardStartTime
        {
            get { return _cardStartTime; }
            set
            {
                //if (_cardStartTime != value)
                //{
                if (!_cardStartTime.HasValue)
                {
                    _cardStartTime = value;
                    _cardDifference = DateTime.Now.Subtract(CardStartTime.Value);
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        _cardStopwatch.Start();
                        _cardTimer.Start();
                    }, DispatcherPriority.Normal);
                }
                else if (_cardStartTime.HasValue && value == null)
                {
                    _cardStopwatch.Stop();
                    _cardStartTime = value;
                }
                else if (_cardStartTime.HasValue && value != null)
                {
                    _cardStartTime = value;
                    _cardDifference = DateTime.Now.Subtract(CardStartTime.Value);
                }
                OnPropertyChanged(nameof(CardStartTime));
                //}
            }
        }

        private string _cardTime;
        public string CardTime
        {
            get
            {
                return _cardTime;
            }
            set
            {
                _cardTime = value;
                OnPropertyChanged(nameof(CardTime));
            }
        }

        void Card_Timer_Tick(object sender, EventArgs e)
        {
            if (_cardStopwatch.IsRunning)
            {
                TimeSpan total = _cardDifference +_cardStopwatch.Elapsed;
                CardTime = total.ToString("hh\\:mm\\:ss\\.ff");
                //TimeElapsed = String.Format("{0:0} {1:00}:{2:00}:{3:00}.{4:00}", total.Days, total.Hours, total.Minutes, total.Seconds, total.Milliseconds);
            }
        }

        #endregion


    }
}

