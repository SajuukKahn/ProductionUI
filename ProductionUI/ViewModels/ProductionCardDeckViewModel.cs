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
        

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
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

        private ProgramStep programStep;

        public ProductionCardDeckViewModel()
        {
            ProductionDeck = new RandomProgramGenerator().GenerateRandom(_productionDeck);
            //first or default (null check) - ryan says do it in the XAML selectedindex =0 in the xaml
            SelectedIndex = ProductionDeck[0];
            programStep = new ProgramStep(this, ProductionDeck.Count);
        }
    }
}

