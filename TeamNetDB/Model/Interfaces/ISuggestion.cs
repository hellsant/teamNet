using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamNetDB.Model.Interfaces
{
    public interface ISuggestion
    {
        string Description { get; set; }
        int EstimatedTime { get; set; }
        string Name { get; set; }
        int Score { get; set; }
        int Valuation { get; set; }
    }
}
