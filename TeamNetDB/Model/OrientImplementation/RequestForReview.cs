using Orient.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamNetDB.Model.Interfaces;

namespace TeamNetDB.Model.OrientImplementation
{
    public class RequestForReview:IRequestForReview , IEntity
    {
        [OProperty(Alias = "rid")]
        public string Id
        {
            get;
            set;
        }

        [OProperty(Alias = "commentsFromReviewer")]
        public string CommentsFromReviewer
        {
            get;
            set;
        }

        [OProperty(Alias = "commentsFromRequester")]
        public string CommentsFromRequester
        {
            get;
            set;
        }


        [OProperty(Alias = "requestDate")]
        public string RequestDate
        {
            get;
            set;
        }

        [OProperty(Alias = "revisionDate")]
        public string RevisionDate 
        { 
            get; 
            set; 
        }


        [OProperty(Alias = "approved")]
        public bool Approved
        {
            get;
            set;
        }

        [OProperty(Alias = "reviewed")]
        public bool Reviewed
        {
            get;
            set;
        }

        [OProperty(Alias = "in_Reviewer",Serializable=false)]
        public string ReviewerId 
        { 
            get; 
            set; 
        }

        [OProperty(Alias = "in_TaskForReview",Serializable = false)]
        public string TaskId
        {
            get;
            set;
        }
    }
}
