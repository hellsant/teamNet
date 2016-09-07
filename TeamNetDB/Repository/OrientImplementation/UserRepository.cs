using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamNetDB.Repository.Interfaces;
using TeamNetDB.TransactionToDatabase.TransactionOrientDBBinary;
using System.Linq.Expressions;
using TeamNetDB.Model.OrientImplementation;
using Orient.Client;
using TeamNetDB.TransactionToDatabase;
using System.Diagnostics;
namespace TeamNetDB.Repository.OrientImplementation
{
    public class UserRepository:IOrientTransactionable<User>,IRepository<User>
    {

        public TeamNetDB.Repository.Interfaces.ITransaction Transaction { get; set; }

        public UserRepository(Transaction transaction)
        {
            this.Transaction = transaction;
        }

        public IEnumerable<User> GetAll()
        {
            var documents = this.Transaction.QueryVertex(VertexName.User, new List<ConditionValue>());
            var users = this.CreateEntitiesFromDocuments(documents);
            return users;
        }

        public User GetSingle(string id)
        {
            var user = this.GetAll().Where(v => v.Id.Equals(id)).Single();
            return user;
        }
        public IEnumerable<User> FindBy(Expression<Func<User, bool>> predicate)
        {
            var allEntities = this.GetAll();
            var entitiesWithPredicate = allEntities.Where(predicate.Compile());
            return entitiesWithPredicate;
        }
        public bool Insert(User entity, IList<string> connectors=null)
        {
            bool inserted = false;
            try
            {
                int lastUserWithSameEmail = this.FindBy(u => u.Email.Equals(entity.Email)).Count();
                if (lastUserWithSameEmail == 0)
                {
                    String encryptedPassword = new Rinjdael().EncryptData(entity.Password);
                    entity.Password = encryptedPassword;
                    List<VertexConnector> vertexConnectors = new List<VertexConnector>();
                    if (connectors != null)
                    {
                        vertexConnectors = new List<VertexConnector>()
                        {
                        new VertexConnector(){EdgeName=EdgesName.UserLevel}
                        };
                        for (int i = 0; i < connectors.Count(); i++)
                        {
                            vertexConnectors[i].To = connectors[i];
                        }
                    }
                    inserted = this.Transaction.InsertVertex(entity, VertexName.User, vertexConnectors);
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }
            return inserted;
        }

        public bool Delete(User entity)
        {
            List<ConditionValue> conditions = new List<ConditionValue>(){
                new ConditionValue(){ConditionColumn="@orid",ValueCondition=entity.Id}
            };
            return this.Transaction.Delete(VertexName.User, conditions);
        }

        public bool Update(User entity)
        {
            OVertex vertexModified = new OVertex();
            vertexModified.SetField("name", entity.Name);
            vertexModified.SetField("lastName", entity.LastName);
            vertexModified.SetField("password", entity.Password);
            vertexModified.SetField("email", entity.Email);
            return this.Transaction.Update(vertexModified, entity.Id);
        }

        public IEnumerable<User> Query(string query)
        {
            var documents = this.Transaction.QueryTraverse(query);
            List<User> users = this.CreateEntitiesFromDocuments(documents);
            return users;
        }

        public override User CreateEntityFromDocument(ODocument document)
        {
            User user = new User()
            {
                Email = document.GetField<string>("email"),
                LastName = document.GetField<string>("lastName"),
                Password = document.GetField<string>("password"),
                Name = document.GetField<string>("name"),
                Id = document.ORID.ToString(),
            };
            return user;
        }
    }
}
