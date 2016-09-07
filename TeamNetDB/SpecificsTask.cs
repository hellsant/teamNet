using Orient.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamNetDB.Model.Interfaces;

namespace TeamNetDB
{
    public class SpecificsTask: IEntity
    {
        [OProperty(Alias = "rid")]
        public string Id { get; set; }
   
        [OProperty(Alias = "creatingDate")]
        public string CreatingDate { get; set; }

        [OProperty(Alias = "description")]
        public string Description { get; set; }

        [OProperty(Alias = "endDate")]
        public string EndDate { get; set; }

        [OProperty(Alias = "finalProduct")]
        public string FinalProduct { get; set; }

        [OProperty(Alias = "initialDate")]
        public string InitialDate { get; set; }

        [OProperty(Alias = "isApproved")]
        public bool IsApproved { get; set; }

        [OProperty(Alias = "isSuggestion")]
        public bool IsSuggestion { get; set; }

        [OProperty(Alias = "link")]
        public string Link { get; set; }

        [OProperty(Alias = "name")]
        public string Name { get; set; }

        [OProperty(Alias = "progress")]
        public int Progress { get; set; }

        [OProperty(Alias = "reviewer")]
        public string Reviewer { get; set; }

        [OProperty(Alias = "score")]
        public double Score { get; set; }

        [OProperty(Alias = "state")]
        public string State { get; set; }
    }
}
