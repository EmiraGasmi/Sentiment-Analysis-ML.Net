using Microsoft.ML;
using System;

namespace DotNetMachineLearningApp
{
    class Program
    {



        static void Main(string[] args)
        {
            MLContext mlContext = new MLContext(seed:1);

            if (GetActionToBePerformedFromUser())
            {
                Console.WriteLine("Training model in process");
                Tools.BuildTrainEvaluateSaveModel(mlContext);
                Console.WriteLine("Training complete");
                if (GetActionToBePerformedFromUser(true))
                {
                    Tools.TestPrediction(mlContext);
                }
            }
            else
            {
                Tools.TestPrediction(mlContext);
            }
        }


        private static bool GetActionToBePerformedFromUser()
        {
            Console.WriteLine("1- Train Data");
            Console.WriteLine("2- Test Data");
            Console.WriteLine("=====================================================================================================");

            var input = Console.ReadLine();
            if(input.Equals("1"))
            {
                return true;
            }
            else if(input.Equals("2"))
            {
                return false;
            }
            else
            {
                Console.Write("Invalid Input");
                return GetActionToBePerformedFromUser();
            }
        }

        private static bool GetActionToBePerformedFromUser(bool TestModel)
        {
            Console.WriteLine("Do you want to test the model now? Y/N");
            var input = Console.ReadLine();
            if (input.Equals("Y") || input.Equals("y"))
            {
                return true;
            }
            return false;
        }







    }
}
