using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RestService.Models
{
    public class ResultModel
    {
        public string Id { get; set; }
        public string Evaluator { get; set; }
        public float Value { get; set; }
        public string Management { get; set; }
        public int Members { get; set; }

        public ResultModel(string evaluator, float value, string management)
        {
            Evaluator = evaluator;
            Value = value;
            Management = management;
        }

        public ResultModel()
        {
            // TODO: Complete member initialization
        }
    }
}
