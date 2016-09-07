using Orient.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamNetDB.Model.Interfaces;

namespace TeamNetDB.Model.OrientImplementation
{
    public class Expected : IExpected, IEntity
    {
        [OProperty(Alias = "rid")]
        public string Id
        {
            get;
            set;
        }

        [OProperty(Alias = "valueExpected")]
        public int ValueExpected
        {
            get;
            set;
        }

        [OProperty(Alias = "priority")]
        public int Priority
        {
            get;
            set;
        }

        [OProperty(Alias = "in_CompetencyExpected")]
        public string CompetencyId
        {
            get;
            set;
        }

        [OProperty(Alias = "in_LevelExpected")]
        public string LevelId
        {
            get;
            set;
        }
    }
}
