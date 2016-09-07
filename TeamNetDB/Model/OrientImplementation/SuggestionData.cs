using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamNetDB.Model.Interfaces;

namespace TeamNetDB.Model.OrientImplementation
{
    public class SuggestionData : ISuggestionData,IEntity
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public int EstimatedTime { get; set; }
        public string Time { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public int Valuation { get; set; }
        public string CreatorOfSuggestion { get; set; }
        public string Competency { get; set; }
        public string CreatedTo { get; set; }
        public int ValueCompetecyLevel { get; set; }
        public string DescriptionCompetencyLevel { get; set; }
        public string Level { get; set; }
    }
}
