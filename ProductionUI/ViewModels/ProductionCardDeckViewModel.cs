using ProductionUI.Commands;
using ProductionUI.Models;
using ProductionUI.Tests;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProductionUI.ViewModels
{
    public class ProductionCardDeckViewModel : INotifyPropertyChanged
    {
        public ProductionCardDeckModel ProductionDeck { get; private set; }
        private ProductionCardDeckModel _productionDeck = new ProductionCardDeckModel();

        public ICommand LeftManyArrow { get; set; }
        public ICommand LeftArrow { get; set; }
        public ICommand RightManyArrow { get; set; }
        public ICommand RightArrow { get; set; }
        public ICommand VertCheck { get; set; }
        public ICommand DeleteButton { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public ProductionCardDeckViewModel()
        {
            ProductionDeck = new RandomProgramGenerator().GenerateRandom(_productionDeck);
            LeftManyArrow = new RelayCommand(o => LeftManyArrowPressed());
            LeftArrow = new RelayCommand(o => LeftArrowPressed());
            RightManyArrow = new RelayCommand(o => RightManyArrowPressed());
            RightArrow = new RelayCommand(o => RightArrowPressed());
            VertCheck = new RelayCommand(o => VertCheckPressed());
            DeleteButton = new RelayCommand(o => DeleteButtonPressed());

        }

        private void LeftManyArrowPressed()
        {
            throw new NotImplementedException();
        }

        private void LeftArrowPressed()
        {
            throw new NotImplementedException();
        }

        private void RightManyArrowPressed()
        {
            throw new NotImplementedException();
        }

        private void RightArrowPressed()
        {
            throw new NotImplementedException();
        }

        private void VertCheckPressed()
        {
            throw new NotImplementedException();
        }

        private void DeleteButtonPressed()
        {
            throw new NotImplementedException();
        }
    }
}

