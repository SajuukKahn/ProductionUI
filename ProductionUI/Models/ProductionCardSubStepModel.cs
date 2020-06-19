using System;
using System.Collections.Generic;
using System.Text;

namespace ProductionUI.Models
{
    class ProductionCardSubStepModel
    {

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

