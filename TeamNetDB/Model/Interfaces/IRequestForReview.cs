using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamNetDB.Model.Interfaces
{
    interface IRequestForReview
    {
        /// <summary>
        /// this a comment from review user
        /// </summary>
        string CommentsFromReviewer { get; set; }

        /// <summary>
        /// this a comment from requester user
        /// </summary>
        string CommentsFromRequester { get; set; }

        /// <summary>
        /// this date of request
        /// </summary>
        string RequestDate { get; set; }

        /// <summary>
        /// this date of revision 
        /// </summary>
        string RevisionDate { get; set; }

        /// <summary>
        /// this bool for verify is approved a request
        /// </summary>
        bool  Approved { get; set; }

        /// <summary>
        /// this a bool for verify is reviewed a requets
        /// </summary>
        bool Reviewed { get; set; }

        /// <summary>
        /// this id for reviwer user
        /// </summary>
        string ReviewerId { get; set; }

        /// <summary>
        /// this id from task
        /// </summary>
        string TaskId { get; set; }

    }
}
