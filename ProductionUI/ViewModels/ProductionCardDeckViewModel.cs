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
        private readonly ProductionCardDeckModel _productionDeck = new ProductionCardDeckModel();

        public static ProductionCardModel SelectedIndex;

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

        private ProductionCardModel _currentIndex;
        public ProductionCardModel CurrentIndex
        {
            get
            {
                return _currentIndex;
            }
            set
            {
                _currentIndex = value;
                OnPropertyChanged("CurrentIndex");
            }
        }

        public void NextCard(string StepDataLog, bool StepPassed = true, bool StepComplete = true)
        {
            SelectedIndex.StepComplete = StepComplete;
            SelectedIndex.StepDataLog = StepDataLog;
            SelectedIndex.StepPassed = StepPassed;
            SelectedIndex.IsActiveStep = false;
            SelectedIndex.stopCardTimer();
            SelectedIndex = ProductionDeck[ProductionDeck.IndexOf(SelectedIndex) + 1];
            SelectedIndex.IsActiveStep = true;
            SelectedIndex.startCardTimer();
        }

        public ICommand ChangeCardCommand { get; set; }

        public ProductionCardDeckViewModel()
        {
            ProductionDeck = new RandomProgramGenerator().GenerateRandom(_productionDeck);
            //first or default (null check) - ryan says do it in the XAML selectedindex =0 in the xaml
            SelectedIndex = ProductionDeck[0];
            //ProcessImage = new MediaElement();
            //ProcessImage.Source = new Uri("/Tests/TestImages/PCB" + new Random().Next(1, 9).ToString() + ".bmp", UriKind.Relative);
            ProcessDisplay = new BitmapImage(new Uri("/Tests/TestImages/PCB" + new Random().Next(1, 9).ToString() + ".bmp", UriKind.Relative));

            ChangeCardCommand = new RelayCommand(o => ChangeCardMethod());
        }

        private void ChangeCardMethod()
        {
            Debug.WriteLine("Change Card Fired");
        }
    }
}

