using ProductionUI.Models;
using ProductionUI.Tests;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Controls;

namespace ProductionUI.ViewModels
{
    public class ProductionCardDeckViewModel : INotifyPropertyChanged
    {
        public ProductionCardDeckModel ProductionDeck { get; private set; }
        private ProductionCardDeckModel _productionDeck = new ProductionCardDeckModel();

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public ProductionCardDeckViewModel()
        {
            ProductionDeck = new RandomProgramGenerator().GenerateRandom(_productionDeck);
        }

    }
}

