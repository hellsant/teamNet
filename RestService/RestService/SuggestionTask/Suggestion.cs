using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using TeamNetDB.Repository;

namespace ResrServices.SuggestionTask
{
    [DataContract]
    public class Suggestion :IEntityBase
    {
        public string Rid { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int EstimatedTime { get; set; }
        [DataMember]
        public int Score { get; set; }
        [DataMember]
        public int Valuation { get; set; }
        [DataMember]
        public string CreatorOfSuggestion { get; set; }
        [DataMember]
        public string Area { get; set; }
        [DataMember]
        public string Point { get; set; }
        [DataMember]
        public string Level { get; set; }
        [DataMember]
        public string CreatedTo { get; set; }
        
        [DataMember(Name = "Id")]
        public int Id { get; set; }

    }
}