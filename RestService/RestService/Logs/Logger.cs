using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace RestService.Logs
{
    /// <summary>
    /// Logger class.
    /// </summary>
    public class Logger
    {
        private static ILog log;

        /// <summary>
        /// protected constructor.
        /// </summary>
        protected Logger() { }

        /// <summary>
        /// Instance of the logger.
        /// </summary>
        /// <returns></returns>
        public static ILog Instance()
        {
            if (log != null)
            {
                return log;
            }
            else
            {
                 log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
                 return log;
            }
        }

    }
}