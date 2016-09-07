using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamNetDB.Model.Interfaces
{
    public interface IReference
    {
        /// <summary>
        /// This id from task.
        /// </summary>
        string TaskId { get; set; }


        /// <summary>
        /// This name from url reference
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// this url, the reference of wok
        /// </summary>
        string Url { get; set; }
    }
}
