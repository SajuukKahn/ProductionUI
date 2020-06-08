using ProductionUI.Models;
using ProductionUI.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace ProductionUI.Tests
{
    class RandomProgramGenerator
    {
        ProcessContainerViewModel _processContainerViewModel;
        ProductionCardDeckModel _productionCardDeckModel;

        public ProcessContainerViewModel GenerateRandom(ProcessContainerViewModel processContainerViewModel)
        {
            _processContainerViewModel = processContainerViewModel;

            _processContainerViewModel.ProcessLocation = ProcessLocation;
            _processContainerViewModel.ProgramName = ProgramName;
            
            return _processContainerViewModel;
        }

        public ProductionCardDeckModel GenerateRandom(ProductionCardDeckModel productionCardDeckModel)
        {

            _productionCardDeckModel = productionCardDeckModel;
            
            int randSize = new Random().Next(30, 120);
            BitmapImage image;//= new BitmapImage(new Uri("/Tests/TestImages/PCB" + new Random().Next(1, 9).ToString() + ".bmp", UriKind.Relative));
            image = new BitmapImage(new Uri("/Tests/TestImages/PCB5.bmp", UriKind.Relative));

            for (int i = 0; i < randSize; i++)
            {
                ProductionCardModel cardModel = new ProductionCardModel()
                {
                    StepDataDisplay = new Random().Next().ToString() + " Step Data"
                };
                if(i % 5 == 0)
                {
                    cardModel.StepImage = image;
                }

                _productionCardDeckModel.Add(cardModel);
            }

            return _productionCardDeckModel;
        }

        private string ProcessLocation
        {
            get
            {
                return "Zone " + new Random().Next(1, 4).ToString();
            }
        }

        private string ProgramName
        {
            get
            {
                return "Program " + new Random().Next(1, 30).ToString();
            }
        }
    }
}
