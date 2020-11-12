using Microsoft.ML.Runtime.Api;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetMachineLearningApp.Structures
{
    class AnalyzedData
    {
        [ColumnName("Review")]
        public String content { get; set; }

        [ColumnName("Label")]
        public bool toxicity { get; set; }

    }
}
