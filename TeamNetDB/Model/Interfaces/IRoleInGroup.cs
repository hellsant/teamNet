using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamNetDB.Model.Interfaces
{
    public interface IRoleInGroup
    {
        /// <summary>
        /// Manages code relationship between user, group and role.
        /// </summary>
        string Name { get; set; }
    }
}
