using ProductionUI.Models;
using ProductionUI.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private readonly Random random = new Random();

        ProcessContainerViewModel _pcm;
        ProductionCardDeckModel _pcdm;

        public ProcessContainerViewModel GenerateRandom(ProcessContainerViewModel pcm)
        {
            _pcm = pcm;

            _pcm.ProcessLocation = ProcessLocation;
            _pcm.ProgramName = ProgramName;
            
            return _pcm;
        }

        public ProductionCardDeckModel GenerateRandom(ProductionCardDeckModel pcdm)
        {
            _pcdm = pcdm;

            int randSize = random.Next(30, 120);


            for (int i = 0; i < randSize; i++)
            {
                BitmapImage image = new BitmapImage(new Uri("/Tests/TestImages/PCB" + new Random().Next(1, 9).ToString() + ".bmp", UriKind.Relative));
                ProductionCardModel cardModel = new ProductionCardModel()
                {
                StepImage = image,
                    StepDataDisplay = new Random().Next().ToString() + " Step Data"
                };
                _pcdm.Add(cardModel);
            }

            return _pcdm;
        }

        private string ProcessLocation
        {
            get
            {
                return "Zone " + random.Next(1, 4).ToString();
            }
        }

        private string ProgramName
        {
            get
            {
                return "Program " + random.Next(1, 30).ToString();
            }
        }
    }
}
