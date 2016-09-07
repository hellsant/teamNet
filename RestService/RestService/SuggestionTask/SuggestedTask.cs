using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using TeamNetDB.Repository;

namespace ResrServices.SuggestionTask
{
    [DataContract]
    public class SuggestedTask :IEntityBase
    {
        public string Rid { get; set; }
        [DataMember]
        public int Point { get; set; }
        [DataMember]
        public int Value { get; set; }
        [DataMember]
        public int Date { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string User { get; set; }

        [DataMember(Name = "Id")]
        public int Id { get; set; }

    }
}