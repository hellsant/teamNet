using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RestService.Models
{
    public class ValueDescriptionModel
    {
        public int Value { get; set; }
        public string Description { get; set; }

        public ValueDescriptionModel(int value, string description)
        {
            this.Value = value;
            this.Description = description;
        }

        public ValueDescriptionModel()
        {
            // TODO: Complete member initialization
        }
    }
}
