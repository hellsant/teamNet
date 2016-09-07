using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamNetDB.Model.Interfaces
{
    public interface ISuggestionData
    {
        string Description { get; set; }
        int EstimatedTime { get; set; }
        string Time { get; set; }
        string Name { get; set; }
        int Score { get; set; }
        int Valuation { get; set; }
        string CreatorOfSuggestion { get; set; }
        string Competency { get; set; }
        string CreatedTo { get; set; }
        int ValueCompetecyLevel { get; set; }
        string DescriptionCompetencyLevel { get; set; }
        string Level { get; set; }
    }
}
