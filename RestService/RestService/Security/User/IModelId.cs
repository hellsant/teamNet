using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestService.Security.User
{
    public interface IModelId
    {
        Guid Id { get; }
    }
}
