using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeamNetDB.Model.OrientImplementation;

namespace RestService.Models
{
    public class RequestModel
    {
        public string RequestDate { get; set; }
        public string Name { get; set; }
        public string TaskTitle { get; set; }
        public string TaskDescription { get; set; }
        public string CommentsFromRequester { get; set; }
        public string CommentsFromReviewer { get; set; }
        public List<Reference> References { get; set; }
        public string RequestId { get; set; }
    }
}