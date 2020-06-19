using ProductionUI.Models;
using ProductionUI.ViewModels;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ProductionUI.Tests
{
    public class RandomProgramGenerator
    {
        ProductionCardCollection _productionCardDeckModel;
        ProductionCardDeckViewModel _productionCardDeckViewModel;

        public ProductionCardCollection GenerateRandom(ProductionCardCollection productionCardDeckModel)
        {

            string[] TitleArray =
            {
                "PolyLine 3D",
                "Area",
                "Line",
                "PolyLine",
                "Arc",
                "Spiral",
                "Dot",
                "Part Presense Check"
            };
            _productionCardDeckModel = productionCardDeckModel;

            int randSize = new Random().Next(12, 45);
            BitmapImage image;//= new BitmapImage(new Uri("/Tests/TestImages/PCB" + new Random().Next(1, 9).ToString() + ".bmp", UriKind.Relative));
            image = new BitmapImage(new Uri("/Tests/TestImages/PCB5.bmp", UriKind.Relative));

            ProductionCardModel firstCardModel = new ProductionCardModel()
            {
                StepTitle = "Fiducial Check",
                StepImage = image
            };

            _productionCardDeckModel.Add(firstCardModel);
            ProductionCardModel secondCardModel = new ProductionCardModel()
            {
                StepTitle = "Surface Height Check",
                StepImage = image
            };

            _productionCardDeckModel.Add(secondCardModel);

            for (int i = 0; i < randSize; i++)
            {
                ProductionCardModel cardModel = new ProductionCardModel()
                {
                    StepTitle = TitleArray[new Random().Next(TitleArray.Length)]
                };
                if(i % 5 == 0)
                {
                    cardModel.StepImage = image;
                }

                _productionCardDeckModel.Add(cardModel);
            }

            return _productionCardDeckModel;
        }

        public ProductionCardDeckViewModel GenerateRandom(ProductionCardDeckViewModel productionCardDeckViewModel)
        {
            _productionCardDeckViewModel = productionCardDeckViewModel;
            _productionCardDeckViewModel.ProcessLocation = ProcessLocation;
            _productionCardDeckViewModel.ProgramName = ProgramName;
            return _productionCardDeckViewModel;
        }

        public async void RunRandomProgram(ProductionCardDeckViewModel productionCardDeckViewModel)
        {
            for (int i = 0; i < productionCardDeckViewModel.ProductionDeck.Count - 1; i++)
            {
                while (!productionCardDeckViewModel.PlayBackRunning)
                {
                }
                productionCardDeckViewModel.CurrentIndex = i;
                Debug.WriteLine("Next Card: " + i);
                await Task.Delay(TimeSpan.FromSeconds(new Random().Next(1, 5) + new Random().NextDouble()));
            }
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
