using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamNetDB.Cleaner
{
    public interface IDataBaseCleaner
    {
        void CleanDataBase();
        void SetUpInitialData();
    }
}
