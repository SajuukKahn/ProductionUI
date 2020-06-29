using ProductionUI.Models;
using ProductionUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace ProductionUI.Tests
{
    public class RandomProgramGenerator
    {

        //TODO:
        /*
         *
         * due: July 1
         * 
         * MVVM relationship clarification and clearer separation of concerns
         * 
         * If a model (ProductionCardDeckModel) basically has nothing in it you can just get rid of it. It's perfectly fine to have no model for a View/ViewModel pair, and in fact that tends to happen more often than not.
         * Get in the habit of creating an interface for all your Views and ViewModels as well. You'll need those for registration later when doing injection.
         * 
         * later:
         * prism integration
         * telerik controls
         */

        ProductionCardCollection _productionCardCollection;
        ProductionCardDeckViewModel _productionCardDeckViewModel;
        ProductionProgramCollection _productionProgramCollection;

        private string[] RandomCoordinates()
        {
            return new string[] { new Random().Next(-20000, 200000).ToString(), new Random().Next(-20000, 200000).ToString(), new Random().Next(-9000, 100000).ToString() };
        }

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
            _productionCardCollection = productionCardDeckModel;

            BitmapImage image;//= new BitmapImage(new Uri("/Tests/TestImages/PCB" + new Random().Next(1, 9).ToString() + ".bmp", UriKind.Relative));
            image = new BitmapImage(new Uri("/Tests/TestImages/PCB5.bmp", UriKind.Relative));

            List<ProductionCardSubStepModel> fidList = new List<ProductionCardSubStepModel>
            {
                new ProductionCardSubStepModel("Fiducial A", RandomCoordinates()),
                new ProductionCardSubStepModel("Fiducial B", RandomCoordinates())
            };
            ProductionCardModel firstCardModel = new ProductionCardModel()
            {
                CardSubSteps = fidList,
                StepTitle = "Fiducial Check",
                StepImage = image
            };

            _productionCardCollection.Add(firstCardModel);
            int randSize = new Random().Next(4, 30);
            List<ProductionCardSubStepModel> surfaceList = new List<ProductionCardSubStepModel>();
            for (int i = 0; i < randSize; i++)
            {
                surfaceList.Add(new ProductionCardSubStepModel("Surface " + (i + 1), RandomCoordinates()));
            }
            ProductionCardModel secondCardModel = new ProductionCardModel()
            {
                CardSubSteps = surfaceList,
                StepTitle = "Surface Height Check",
                StepImage = image
            };
            _productionCardCollection.Add(secondCardModel);

            randSize = new Random().Next(15, 200);
            for (int i = 0; i < randSize; i++)
            {
                string stepTitle = TitleArray[new Random().Next(TitleArray.Length)];
                int randSteps = new Random().Next(1, 12);
                List<ProductionCardSubStepModel> randomSteps = new List<ProductionCardSubStepModel>();

                for (int j = 0; j < randSteps; j++)
                {
                    randomSteps.Add(new ProductionCardSubStepModel(stepTitle + (j + 1), RandomCoordinates()));
                }

                ProductionCardModel cardModel = new ProductionCardModel()
                {
                    CardSubSteps = randomSteps,
                    StepTitle = stepTitle,
                };

                if(new Random().Next(0, 3) == 1)
                {
                    cardModel.StepImage = image;
                }

                _productionCardCollection.Add(cardModel);
            }

            return _productionCardCollection;
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
                string progname = ((char)new Random().Next(65, 90)).ToString() + ((char)new Random().Next(65, 90)).ToString() + new Random().Next(1, 102).ToString() + " Rev_" + (char)new Random().Next(65, 90);

                _productionProgramCollection.Add(new ProductionProgramModel(progname, relations[new Random().Next(relations.Length)] ,makernames[new Random().Next(makernames.Length)]));
            }

            return _productionProgramCollection;
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
