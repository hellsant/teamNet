using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamNetDB.Model.Interfaces
{
    interface ICompetencyLevel
    {
        int Value { get; set; }
        string Description { get; set; }
    }
}
