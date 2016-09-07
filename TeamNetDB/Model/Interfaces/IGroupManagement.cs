using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamNetDB.Model.Interfaces
{
    public interface IGroupManagement
    {
        string GroupName { get; set; }
        List<string> RidMembers { get; set; }
        string Member { get; set; }
    }
}
