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
using System.Xml.Schema;

namespace ProductionUI.ViewModels
{
    public class ProductionCardDeckViewModel : INotifyPropertyChanged
    {
        public ICommand PlayButton { get; set; }
        public ICommand PauseButton { get; set; }
        public ICommand ConfirmButton { get; set; }
        public ICommand CancelButton { get; set; }

        private CancellationTokenSource _cycleTimerCancellationTokenSource;
        private CancellationToken _cycleTimerCancelToken;
        private Task _cycleTimeTask;

        public ProductionCardDeckViewModel()
        {
            PlayButton = new RelayCommand(o => PlayPressed());
            PauseButton = new RelayCommand(o => PausePressed());
            ConfirmButton = new RelayCommand(o => ConfirmProgramChange());
            CancelButton = new RelayCommand(o => CancelProgramChange());
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


        private double _cardTime;
        public double CardTime
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

        private bool _playBackPaused;

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
            if (CycleTime == 0 || DeckCompleted)
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
            AllowProgramChange = true;
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
            AllowProgramChange = true;
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

        private void StartCycleTimer()
        {
            if (_cycleTimeTask?.Status != TaskStatus.Running || _cycleTimeTask == null)
            {

                if (_cycleTimerCancellationTokenSource == null)
                {
                    _cycleTimerCancellationTokenSource = new CancellationTokenSource();
                    _cycleTimerCancelToken = _cycleTimerCancellationTokenSource.Token;
                }
                else
                {
                    _cycleTimerCancellationTokenSource.Cancel();
                    while(!_cycleTimeTask.IsCompleted)
                    {
                    }
                    _cycleTimerCancellationTokenSource.Dispose();
                    _cycleTimerCancellationTokenSource = new CancellationTokenSource();
                    _cycleTimerCancelToken = _cycleTimerCancellationTokenSource.Token;
                }
                _cycleTimeTask = Task.Run(async () =>
                {
                    while (!_cycleTimerCancelToken.IsCancellationRequested && !DeckCompleted)
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(10));
                        if (!_cycleTimerCancelToken.IsCancellationRequested)
                        {
                            CycleTime += 0.01;
                        }
                    }
                    return;
                }, _cycleTimerCancelToken);
            }
        }

        private CancellationTokenSource _cardTimerCancellationTokenSource;
        private CancellationToken _cardTimerCancelToken;
        private Task _cardTimeTask;

        private void StartCardTimer()
        {
            if (_cycleTimeTask?.Status != TaskStatus.Running || _cycleTimeTask == null)
            {

                if (_cardTimerCancellationTokenSource == null)
                {
                    _cardTimerCancellationTokenSource = new CancellationTokenSource();
                    _cardTimerCancelToken = _cardTimerCancellationTokenSource.Token;
                }
                else
                {
                    _cardTimerCancellationTokenSource.Cancel();
                    while (!_cardTimeTask.IsCompleted)
                    {
                    }
                    _cardTimerCancellationTokenSource.Dispose();
                    _cardTimerCancellationTokenSource = new CancellationTokenSource();
                    _cardTimerCancelToken = _cardTimerCancellationTokenSource.Token;
                }
                _cardTimeTask = Task.Run(async () =>
                {
                    while (!_cardTimerCancelToken.IsCancellationRequested)
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(10));
                        if (!_cardTimerCancelToken.IsCancellationRequested && !_playBackPaused)
                        {
                            CardTime += 0.01;
                        }
                    }
                    return;
                }, _cardTimerCancelToken);
            }
        }

        public void LoadProductionDeck(bool reUseProgram = false)
        {
            _cycleTimerCancellationTokenSource?.Cancel();
            PlayBackRunning = false;
            AllowProgramChange = true;
            DeckCompleted = false;
            LoadRandomProgram(reUseProgram);
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
                        _playBackPaused = true;
                        if (_randomProgramCancelToken.IsCancellationRequested)
                        {
                            return;
                        }
                    }
                    _playBackPaused = false;
                    if (!_randomProgramCancelToken.IsCancellationRequested)
                    {
                        while (CurrentSubStepIndex < (_productionCards[CurrentIndex].CardSubSteps.Count - 1))
                        {
                            await Task.Delay((TimeSpan.FromSeconds(new Random().Next(1, 5) + new Random().NextDouble())));
                            while (!PlayBackRunning)
                            {
                                _playBackPaused = true;
                                if (_randomProgramCancelToken.IsCancellationRequested)
                                {
                                    return;
                                }
                            }
                            CurrentSubStepIndex++;
                        }
                        _playBackPaused = false;
                        await Task.Delay(TimeSpan.FromSeconds(new Random().Next(1, 5) + new Random().NextDouble()));
                        while (!PlayBackRunning)
                        {
                            _playBackPaused = true;
                            if (_randomProgramCancelToken.IsCancellationRequested)
                            {
                                return;
                            }
                        }
                        CurrentSubStepIndex = 0;
                        CurrentIndex++;
                        CardTime = 0;
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
    }
}

