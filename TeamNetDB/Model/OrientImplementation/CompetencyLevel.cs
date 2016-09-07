using Orient.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamNetDB.Model.Interfaces;
using TeamNetDB.Repository;

namespace TeamNetDB.Model.OrientImplementation
{
    public class CompetencyLevel : IEntity,ICompetencyLevel
    {

        [OProperty(Alias = "value")]
        public int Value
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

        [OProperty(Alias = "in_CompetencyCompetencyLevel")]
        public string CompetencyId
        {
            get;
            set;
        }

        [OProperty(Alias = "rid")]
        public string Id
        {
            get;
            set;
        }

    }
}
