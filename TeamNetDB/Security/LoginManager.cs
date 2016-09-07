using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamNetDB.Repository;

namespace TeamNetDB.Security
{
    public class LoginManager
    {
        public LoginManager()
        {
            this.encryptor = new Rinjdael();
        }

        public Permission AuthenticateUser(string email, string passwod)
        {
            Permission permission = null;
            if (email != null && passwod != null)
            {
                string encryptedPassword = this.encryptor.EncryptData(passwod);
                var result = UnitOfWork.GetInstance().UserRepository.FindBy(e => e.Email == email && e.Password == encryptedPassword);
                if (result.Count() == 1)
                {
                    var user = result.Single();
                    permission = new Permission()
                    {
                        Acepted = true,
                        Name = user.Name,
                        LastName = user.LastName,
                        Orid = user.Id
                    };
                    CredentialsRetriever.Orid = permission.Orid;
                }
            }
            return permission;
        }

        private IEncryptor encryptor;
    }

    public class Permission
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Orid { get; set; }
        public bool Acepted { get; set; }
    }
}
