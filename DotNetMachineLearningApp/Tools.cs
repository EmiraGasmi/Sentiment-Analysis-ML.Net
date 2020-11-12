using DotNetMachineLearningApp.Structures;
using Microsoft.ML;
using Microsoft.ML.Core.Data;
using Microsoft.ML.Data;
using Microsoft.ML.Runtime;
using Microsoft.ML.Runtime.Data;
using System;
using System.IO;

namespace DotNetMachineLearningApp
{
    public static class Tools
    {
        private static string basedataSetLoc = @"../../../Data";
        private static string trainingSetLoc = $"{basedataSetLoc}/training_data.csv";
        private static string baseModelLoc = @"../../../MachineLearningModel";
        private static string generatedModelLoc = $"{baseModelLoc}/SentimentAnalysisModel.zip";
        public static TextLoader CreateTextLoader(MLContext mLContext)
        {
            TextLoader textLoader = mLContext.Data.TextReader(new TextLoader.Arguments()
            {
                Separator = ",",
                HasHeader = true,
                Column = new[]
                {
                    new TextLoader.Column("Review", DataKind.Text,0),
                    new TextLoader.Column("Label", DataKind.Bool, 1)
                }

            });
            return textLoader;

        }


        public static void BuildTrainEvaluateSaveModel(MLContext mLContext)
        {
            TextLoader textLoader = CreateTextLoader(mLContext);
            IDataView TrainingDataView = textLoader.Read(trainingSetLoc);

            var DataProcessingPipeline = mLContext.Transforms.Text.FeaturizeText("Review", "Features");

            var Trainer = mLContext.BinaryClassification.Trainers.FastTree(label: "Label", features: "Features");

            var TrainingPipeline = DataProcessingPipeline.Append(Trainer);

            ITransformer TrainingModelTransformer = TrainingPipeline.Fit(TrainingDataView);

            using (var fs = new FileStream(generatedModelLoc, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                mLContext.Model.Save(TrainingModelTransformer, fs);
            }

        }


        public static void TestPrediction(MLContext mLContext)
        {
            while (true)
            {

                Console.WriteLine("Enter review or type quit");
                var text = Console.ReadLine();
                if (text.ToLower().Equals("quit"))
                {
                    break;
                }
                AnalyzedData analyzedData = new AnalyzedData { content = text };
                ITransformer trainedModel;

                using (var stream = new FileStream(generatedModelLoc, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    trainedModel = mLContext.Model.Load(stream);
                }

                var PredictionFunction = trainedModel.MakePredictionFunction<AnalyzedData, PredictedData>(mLContext);

                var result = PredictionFunction.Predict(analyzedData);
                Console.WriteLine("=====================================================================================================");

                Console.WriteLine($"Text: {analyzedData.content} | Toxic ?: { Convert.ToBoolean(result.Toxicity)} ");
                Console.WriteLine($"Probability: {result.Probability}");
                Console.WriteLine($"Score: {result.Score}");
               

            }

            

        }
    }



    
}
