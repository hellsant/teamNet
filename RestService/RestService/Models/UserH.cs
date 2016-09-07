using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestService.Models
{
    public class UserH
    {
        public string Name { get; set; }
        public List<PointH> Points { get; set; }

        public UserH(string name)
        {
            Name = name;
        }
    }
}