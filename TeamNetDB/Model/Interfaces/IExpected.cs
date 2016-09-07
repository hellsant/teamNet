using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamNetDB.Model.Interfaces
{
    public interface IExpected
    {
        int ValueExpected { get; set; }
        int Priority { get; set; }
    }
}
