using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamNetDB.Model.Interfaces
{
    public interface IResult
    {
        float Result { get; set; }
        string CompetencyId { get; set; }
    }
}
