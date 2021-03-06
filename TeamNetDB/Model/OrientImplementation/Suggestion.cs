﻿using Orient.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamNetDB.Model.Interfaces;

namespace TeamNetDB.Model.OrientImplementation
{
    public class Suggestion : ISuggestion, IEntity
    {
        [OProperty(Alias = "rid")]
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

        [OProperty(Alias = "estimatedTime")]
        public int EstimatedTime
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

        [OProperty(Alias = "score")]
        public int Score
        {
            get;
            set;
        }

        [OProperty(Alias = "valuation")]
        public int Valuation
        {
            get;
            set;
        }
    }
}
