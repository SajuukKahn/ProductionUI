using ProductionUI.Interfaces;
using System.ComponentModel;
using System.Windows.Media;

namespace ProductionUI.Models
{
    public class ProductionCardModel : IProductionCard , INotifyPropertyChanged
    {

        public ProductionCardModel(ImageSource imageSource = null,
                                    bool isActiveStep = false,
                                    bool stepComplete = false,
                                    string stepActiveTime = "0",
                                    string stepTitle = "No Data",
                                    string stepStatus = "Running")
        {

       //    _dispatchTimer = new DispatcherTimer
       //    {
       //        IsEnabled = true,
       //        Interval = TimeSpan.FromMilliseconds(1)
       //    };
       //    _dispatchTimer.Tick += Tick;
       //    ResetRunTime();

            StepImage = imageSource;
            IsActiveStep = isActiveStep;
            StepComplete = stepComplete;
            StepActiveTime = stepActiveTime;
            StepTitle = stepTitle;
            StepStatus = stepStatus;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private string _stepTitle;
        public string StepTitle
        {
            get
            {
                return _stepTitle;
            }
            set
            {
                _stepTitle = value;
                OnPropertyChanged("StepTitle");
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

        private bool _stepRequiresUserInteraction;
        public bool StepRequiresUserInteraction
        {
            get
            {
                return _stepRequiresUserInteraction;
            }
            set
            {
                _stepRequiresUserInteraction = value;
                OnPropertyChanged("StepRequiresUserInteraction");
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
               // if (value == false)
               // {
               //     _dispatchTimer.Stop();
               // }
               // else
               // {
               //     ResetRunTime();
               //     _dispatchTimer.Start();
               // }
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

      //  private readonly DispatcherTimer _dispatchTimer;
      //
      //  private DateTime _startTime;
      //
      //
      //  public void StopCardTimer()
      //  {
      //      _dispatchTimer.Stop();
      //  }
      //
      //  public void StartCardTimer()
      //  {
      //      ResetRunTime();
      //      _dispatchTimer.Start();
      //  }
      //
      //  public void ResetRunTime()
      //  {
      //      _startTime = System.DateTime.Now;
      //  }
      //
      //  private void Tick(object sender, EventArgs e)
      //  {
      //      StepActiveTime = Convert.ToString(DateTime.Now - _startTime);
      //  }
      //
      //  ~ProductionCardModel()
      //  {
      //      _dispatchTimer.Stop();
      //      _dispatchTimer.IsEnabled = false;
      //  }

    }
}
