using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ProductionUI.Models
{
    public class ProductionCardDeckModel : ObservableCollection<ProductionCardModel>
    {
        private readonly IList<ProductionCardModel> _list = new List<ProductionCardModel>();

        public ProductionCardDeckModel()
        {

        }
    }
}
