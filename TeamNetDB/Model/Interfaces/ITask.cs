using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamNetDB.Model.Interfaces
{
    public interface ITask
    {
        string Description { get; set; }
        string EndDate { get; set; }
        string InitialDate { get; set; }
        bool IsSuggestion { get; set; }
        string FinalProduct { get; set; }
        string Name { get; set; }
        int Progress { get; set; }
        string ReviewerId { get; set; }
        double Score { get; set; }
        string UserId { get; set; }
        string CompetencyId { get; set; }
        string State { get; set; }
        bool IsApproved { get; set; }
    }
}
