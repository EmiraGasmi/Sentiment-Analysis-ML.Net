using Microsoft.ML.Data;
using Microsoft.ML.Runtime.Api;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetMachineLearningApp.Structures
{
    class PredictedData
    {
        [ColumnName("PredictedLabel")]
        public bool Toxicity { get; set; }

        public float Probability { get; set; }

        public float Score { get; set; }

    }
}
