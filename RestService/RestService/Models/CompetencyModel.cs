using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestService.Models
{
    public class CompetencyModel
    {
        public string Name { get; set; }
        public float Average { get; set; }
        public List<ResultModel> Results { get; set; }
        public List<ExpectedModel> Expecteds { get; set; }
        public List<ValueDescriptionModel> Descriptions { get; set; }

        public CompetencyModel(string name, List<ResultModel> results, List<ExpectedModel> expected, List<ValueDescriptionModel> descriptions)
        {
            Name = name;
            Results = results;
            Expecteds = expected;
            Descriptions = descriptions;
        }

        public CompetencyModel()
        {
            // TODO: Complete member initialization
        }

    }
}