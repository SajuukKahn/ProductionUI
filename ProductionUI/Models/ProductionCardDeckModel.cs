using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;

namespace ProductionUI.Models
{
    public class ProductionCardDeckModel : ObservableCollection<ProductionCardModel>
    {
        public List<ProductionCardModel> ProductionCardModels
        {
            get
            {
                return _productionCardModel;
            }
            set
            {
                _productionCardModel = value;
            }
        }
        private List<ProductionCardModel> _productionCardModel = new List<ProductionCardModel>();

        public MediaElement MediaElement { get; set; }
        
        public ProductionCardDeckModel(MediaElement mediaElement = null)
        {
            MediaElement = mediaElement;
        }
    }
}
