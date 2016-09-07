using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestService.Models
{
    public class PointH
    {
        public string Name { get; set; }
        public List<Result> Results { get; set; }
        public List<ExpectedH> Expecteds { get; set; }

        public PointH(string name, List<Result> results, List<ExpectedH> expected)
        {
            Name = name;
            Results = results;
            Expecteds = expected;
        }

    }
}