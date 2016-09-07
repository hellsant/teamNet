using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamNetDB.Model.Interfaces
{
    public interface IPublication
    {
        string Comment { get; set; }

        string Date { get; set; }

        string Publicator { get; set; }

        string Task { get; set; }

        string UserId { get; set; }

        List<string> GroupsIds { get; set; }

        List<string> TaskLinks { get; set;}
    }
}
