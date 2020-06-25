using ProductionUI.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;

namespace ProductionUI.Models
{
    public class ProductionCardModel : IProductionCard , INotifyPropertyChanged
    {

        public ProductionCardModel( ImageSource imageSource = null,
                                    bool isActiveStep = false,
                                    bool stepComplete = false,
                                    string stepTitle = "No Data")
        {
            StepImage = imageSource;
            IsActiveStep = isActiveStep;
            StepComplete = stepComplete;
            StepTitle = stepTitle;
            StepStatus = ProductionCardStepStatus.Waiting;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private List<ProductionCardSubStepModel> _cardSubSteps = new List<ProductionCardSubStepModel>();
        public List<ProductionCardSubStepModel> CardSubSteps
        { 
            get
            {
                return _cardSubSteps;
            }
            set
            {
                _cardSubSteps = value;
                OnPropertyChanged(nameof(CardSubSteps));
            }
        }

        private ProductionCardStepStatus _stepStatus;
        public ProductionCardStepStatus StepStatus
        {
            get
            {
                return _stepStatus;
            }
            set
            {
                _stepStatus = value;
                OnPropertyChanged(nameof(StepStatus));
            }
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
                OnPropertyChanged(nameof(StepTitle));
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
                OnPropertyChanged(nameof(StepImage));
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
                OnPropertyChanged(nameof(StepComplete));
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
                OnPropertyChanged(nameof(IsActiveStep));
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
                OnPropertyChanged(nameof(StepPassed));
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
                OnPropertyChanged(nameof(BreakOnError));
            }
        }

    }
}
