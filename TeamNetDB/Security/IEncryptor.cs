using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamNetDB.Security
{
    interface IEncryptor
    {
        String EncryptData(String data);
        String DecryptData(String data);
    }
}
