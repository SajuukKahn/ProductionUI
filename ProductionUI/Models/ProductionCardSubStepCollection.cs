using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ProductionUI.Models
{
    class ProductionCardSubStepCollection : ObservableCollection<ProductionCardSubStepModel>
    {
        public List<ProductionCardSubStepModel> ProductionCardSubStepModel
        {
            get
            {
                return _productionCardSubStepModel;
            }
            set
            {
                _productionCardSubStepModel = value;
            }
        }
        private List<ProductionCardSubStepModel> _productionCardSubStepModel = new List<ProductionCardSubStepModel>();

    }
}
