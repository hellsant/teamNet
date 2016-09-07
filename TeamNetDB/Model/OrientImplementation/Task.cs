using Orient.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamNetDB.Model.Interfaces;
using TeamNetDB.Repository;
using TeamNetDB.TransactionToDatabase;

namespace TeamNetDB.Model.OrientImplementation
{
    public class Task : ITask, IEntity, IDeleteable
    {
        [OProperty(Alias = "rid",Serializable = false)]
        public string Id
        {
            get;
            set;
        }

        [OProperty(Alias = "description")]
        public string Description
        {
            get;
            set;
        }

        [OProperty(Alias = "endDate")]
        public string EndDate
        {
            get;
            set;
        }

        [OProperty(Alias = "initialDate")]
        public string InitialDate
        {
            get;
            set;
        }

        [OProperty(Alias = "isSuggestion")]
        public bool IsSuggestion
        {
            get;
            set;
        }

        [OProperty(Alias = "finalProduct")]
        public string FinalProduct
        {
            get;
            set;
        }

        [OProperty(Alias = "name")]
        public string Name
        {
            get;
            set;
        }

        [OProperty(Alias = "progress")]
        public int Progress
        {
            get;
            set;
        }

        [OProperty(Alias = "in_ReviewerTask",Serializable = false)]
        public string ReviewerId
        {
            get;
            set;
        }

        [OProperty(Alias = "score")]
        public double Score
        {
            get;
            set;
        }

        [OProperty(Alias = "state")]
        public string State
        {
            get;
            set;
        }

        [OProperty(Alias = "in_UserTask",Serializable = false)]
        public string UserId
        {
            get;
            set;
        }

        [OProperty(Alias = "in_CompetencyTask",Serializable = false)]
        public string CompetencyId
        {
            get;
            set;
        }

        [OProperty(Alias = "isApproved")]
        public bool IsApproved 
        { 
            get; 
            set; 
        }
    }
}
