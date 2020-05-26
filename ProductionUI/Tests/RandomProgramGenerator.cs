using ProductionUI.Models;
using ProductionUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

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

            int randSize = random.Next(3, 9);

            for (int i = 0; i < randSize; i++)
            {
                ProductionCardModel cardModel = new ProductionCardModel();
                cardModel.StepDataDisplay = new Random().Next().ToString() + " Step Data";
                cardModel.StepPointData = "Step Point Data: " + random.Next().ToString();
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
