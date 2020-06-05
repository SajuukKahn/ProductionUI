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
        private readonly List<ProductionCardModel> _productionCardModel = new List<ProductionCardModel>();

        public MediaElement MediaElement { get; set; }
        
        public ProductionCardDeckModel(MediaElement mediaElement = null)
        {
            MediaElement = mediaElement;
        }
    }
}
