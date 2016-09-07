using Orient.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamNetDB.Model.Interfaces;

namespace TeamNetDB.Model.OrientImplementation
{
    public class Result360 : IResult, IEntity
    {
        [OProperty(Alias = "rid")]
        public string Id
        {
            get;
            set;
        }

        [OProperty(Alias = "result")]
        public float Result
        {
            get;
            set;
        }

        [OProperty(Alias = "management")]
        public string Management
        {
            get;
            set;
        }

        [OProperty(Alias = "in_CompetencyResult360")]
        public string CompetencyId
        { 
            get;
            set; 
        }

        [OProperty(Alias = "in_UserResult360")]
        public string UserId
        {
            get;
            set;
        }

        [OProperty(Alias = "in_EvaluatorResult360")]
        public string EvaluatorId
        {
            get;
            set;
        }
    }
}
