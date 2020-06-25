using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace ProductionUI.Models
{
    public class ProductionCardCollection : ObservableCollection<ProductionCardModel>
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

        public ProductionCardCollection(MediaElement mediaElement = null)
        {
            MediaElement = mediaElement;
        }
    }
}
