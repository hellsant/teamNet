using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeamNetDB.Model.OrientImplementation;

namespace RestService.Models
{
    public class ResponseModel
    {
        /// <summary>
        /// this a comment from review user
        /// </summary>
        public string CommentsFromReviewer { get; set; }

        /// <summary>
        /// this a comment from requester user
        /// </summary>
        public string CommentsFromRequester { get; set; }

        /// <summary>
        /// this date of request
        /// </summary>
        public string RequestDate { get; set; }

        /// <summary>
        /// this date of revision 
        /// </summary>
        public string RevisionDate { get; set; }

        /// <summary>
        /// this bool for verify is approved a request
        /// </summary>
        public bool Approved { get; set; }

        /// <summary>
        /// this a bool for verify is reviewed a requets
        /// </summary>
        public bool Reviewed { get; set; }

        /// <summary>
        /// this id for reviwer user
        /// </summary>
        public string ReviewerName { get; set; }

        /// <summary>
        /// this id from task
        /// </summary>
        public string TaskTitle { get; set; }

        /// <summary>
        /// this is description from task
        /// </summary>
        public string TaskDescription { get; set; }

        /// <summary>
        /// this is the list of references from task
        /// </summary>
        public List<Reference> References { get; set; }

    }
}