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
        ProductionProgramCollection _productionProgramCollection;

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

            int randSize = new Random().Next(3, 5);
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
            _productionCardDeckViewModel.ProgramName = ProgramName;
            return _productionCardDeckViewModel;
        }

        public ProductionProgramCollection GenerateRandom(ProductionProgramCollection productionPrograms)
        {
            _productionProgramCollection = productionPrograms;

            int randsize = new Random().Next(5, 11);

            string[] makernames =
            {
                "Frank Briggs", "Alfred Travis", "Clarence Wallace", "Oscar Stanley", "Allan Gregory", "Randy Wyatt", "Terrence Hall", "Martin Charles", "Daniel Frazier", "Kevin Phillips",
                "Vernon Merrill", "Gene Nolan", "Kent Kirk", "Don Webb", "Eric Wilcox", "Edward Garrett", "Douglas Jacobs", "Reginald Booth", "Dustin Mathis", "Mason Campbell",
                "Bonnie Willis", "Thelma Frye", "Geneva Fowler", "Emma English", "Martha Harrison", "Jan Livingston", "Krista Hensley", "Yolanda Maxwell", "Elizabeth Lucas", "Whitney Powers",
                "Marion Evans", "Julia Cain", "Ellen O'Neill", "Martha Conrad", "Vicky Harvey", "Kathy Finley", "Tracie Gentry", "Kelsey Workman", "Natalie Petersen", "Whitney Lane"
            };

            string[] relations =
            {
                "Redshift Bundler", "Anode Booster", "Photon Slicer", "Cathode Estimator", "Cell Arranger", "Ptolemy Modifier", "Oersted Perceiver", "Hertz Constructor", "Ito Controller", "Brandt Clamper", "Axion Mixer",
                "Electric Surger", "Exothermic Wrencher", "Rotation Compressor", "Harmonic Migrator", "Exothermic Repeller", "de Fermat Surger", "Broglie Arranger", "Malpighi Estimator", "Mesmer Energizer", "Gamma Bundler",
                "Liquid Sterilizer", "Collision Morpher", "Pressure Sterilizer", "Electric Fuser", "Brongniart Mixer", "Nobel Merger", "Foucault Forger", "Heisenberg Pauser", "Ising Detector", "Plasma Transformer", "Anode Converter",
                "Flexibility Merger", "Gradient Reflector", "Collision Multiplier", "Kapitsa Handler", "Pavlov Retriever", "Heisenberg Compressor", "Marconi Transformer", "Raman Twister"
            };

            for (int i = 0; i < randsize; i++)
            {
                string progname = (char)new Random().Next(65, 90) + (char)new Random().Next(65, 90) + new Random().Next(1, 102).ToString() + " WRev_" + (char)new Random().Next(65, 90);

                _productionProgramCollection.Add(new ProductionProgramModel(progname, relations[new Random().Next(relations.Length)] ,makernames[new Random().Next(makernames.Length)]));
            }

            return _productionProgramCollection;
        }

        public async void RunRandomProgram(ProductionCardDeckViewModel productionCardDeckViewModel)
        {
            for (int i = 0; i < productionCardDeckViewModel.ProductionDeck.Count; i++)
            {
                while (!productionCardDeckViewModel.PlayBackRunning)
                {
                }
                productionCardDeckViewModel.CurrentIndex = i;
                await Task.Delay(TimeSpan.FromSeconds(new Random().Next(1, 5) + new Random().NextDouble()));
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
