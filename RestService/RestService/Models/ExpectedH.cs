using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestService.Models
{
    public class ExpectedH
    {
        public string Level { get; set; }
        public int ValueExpected { get; set; }
        public int Priority { get; set; }
        public ExpectedH(string level, int value, int priority)
        {
            Level = level;
            ValueExpected = value;
            Priority = priority;
        }
    }
}