using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ProductionUI.Models
{
    public class ProductionCardSubStepModel 
    {
        public ProductionCardSubStepModel(string subStepName, string[] subStepData)
        {
            SubStepName = subStepName;
            SubStepData = subStepData;
        }

        private string _subStepName;
        public string SubStepName
        {
            get 
            { 
                return _subStepName; 
            }
            set 
            { 
                _subStepName = value;
            }
        }

        public string FullSubStepData
        {
            get
            {
                return string.Join(", ", _subStepData);
            }
        }

        private string[] _subStepData;
        public string[] SubStepData
        {
            get 
            {
                return _subStepData; 
            }
            set 
            {
                _subStepData = value;
            }

        }
    }
}

