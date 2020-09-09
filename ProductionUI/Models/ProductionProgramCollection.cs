using ProductionUI.Tests;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ProductionUI.Models
{
    public class ProductionProgramCollection : ObservableCollection<ProductionProgramModel>, INotifyPropertyChanged
    {
        private List<ProductionProgramModel> _productionProgramList = new List<ProductionProgramModel>();
        public List<ProductionProgramModel> ProductionProgramList
        {
            get
            {
                return _productionProgramList;
            }
            set
            {
                _productionProgramList = value;
            }
        }


        public ProductionProgramCollection()
        {
            RandomProgramGenerator randomProgramGenerator = new RandomProgramGenerator();
            randomProgramGenerator.GenerateRandom(this);
        }
    }
}
