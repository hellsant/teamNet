using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamNetDB.Model.Interfaces
{
    public interface IRole
    {
        string Name { get; set; }
        string Description { get; set; }
    }
}
