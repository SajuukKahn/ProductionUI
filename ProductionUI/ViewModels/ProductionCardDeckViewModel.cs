using ProductionUI.Commands;
using ProductionUI.Models;
using ProductionUI.Tests;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProductionUI.ViewModels
{
    public class ProductionCardDeckViewModel : INotifyPropertyChanged
    {
        public ProductionCardDeckModel ProductionDeck { get; private set; }
        private readonly ProductionCardDeckModel _productionDeck = new ProductionCardDeckModel();

        public static ProductionCardModel SelectedIndex;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public ProductionCardDeckViewModel()
        {
            ProductionDeck = new RandomProgramGenerator().GenerateRandom(_productionDeck);
            SelectedIndex = ProductionDeck[0];
        }
    }
}

