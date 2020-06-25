using ProductionUI.Tests;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProductionUI.Models
{
    public class ProductionCardSubStepCollection : ObservableCollection<ProductionCardSubStepModel>, INotifyPropertyChanged
    {
        private List<ProductionCardSubStepModel> _subSteps = new List<ProductionCardSubStepModel>();
        public List<ProductionCardSubStepModel> SubSteps
        {
            get
            {
                return _subSteps;
            }
            set
            {
                _subSteps = value;
            }
        }

        public ProductionCardSubStepCollection()
        {
        }
    }
}
