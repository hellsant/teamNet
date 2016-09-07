using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestService.Models
{
    /// <summary>
    /// Model of the log.
    /// </summary>
    public class LogModel
    {
        public string Logger { get; set; }

        public long Timestamp { get; set; }

        public string Level { get; set; }

        public string Url { get; set; }

        public string Message { get; set; }

        /// <summary>
        /// Method ToString.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} - {1} - {2} - {3} - {4}", Logger, Timestamp, Level, Url, Message);
        }
    }
}