using ProductionUI.Commands;
using ProductionUI.Models;
using ProductionUI.Tests;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

        private MediaElement _processImage;
        public MediaElement ProcessImage
        {
            get
            {
                return _processImage;
            }
            set
            {
                _processImage = value;
                OnPropertyChanged("ProcessImage");
            }
        }


        private ImageSource _processDisplay;
        public ImageSource ProcessDisplay
        {
            get
            {
                return _processDisplay;
            }
            set
            {
                _processDisplay = value;
                OnPropertyChanged("ProcessDisplay");
            }
        }

        private int _currentIndex;
        public int CurrentIndex
        {
            get
            {
                return _currentIndex;
            }
            set
            {
                _currentIndex = NextCard(value);
                OnPropertyChanged("CurrentIndex");
            }
        }

        public int NextCard(int value, string StepDataLog = "no data", bool StepPassed = true, bool StepComplete = true)
        {
            if (CurrentIndex > -1 && CurrentIndex < _productionDeck.Count)
            {

                bool setCompleted = false;
                int i = value, ceiling = CurrentIndex;

                if (CurrentIndex < value)
                {
                    setCompleted = true;
                    i = CurrentIndex;
                    ceiling = value;
                }

                for (int j = i; j <= ceiling; j++)
                {
                    if (setCompleted)
                    {
                        _productionDeck[j].StepPassed = StepPassed;
                        _productionDeck[j].StepDataLog = StepDataLog;
                        _productionDeck[j].StepComplete = StepComplete;
                    }
                    else
                    {
                        _productionDeck[j].StepPassed = false;
                        _productionDeck[j].StepDataLog = null;
                        _productionDeck[j].StepComplete = false;
                    }
                    _productionDeck[j].IsActiveStep = false;
                    _productionDeck[j].stopCardTimer();
                }

                _productionDeck[value].IsActiveStep = true;
                _productionDeck[value].StepComplete = false;
                _productionDeck[value].startCardTimer();
            }

            return value;
        }

        public ProductionCardDeckViewModel()
        {
            ProductionDeck = new RandomProgramGenerator().GenerateRandom(_productionDeck);
            ProcessDisplay = new BitmapImage(new Uri("/Tests/TestImages/PCB" + new Random().Next(1, 9).ToString() + ".bmp", UriKind.Relative));
        }

    }
}

