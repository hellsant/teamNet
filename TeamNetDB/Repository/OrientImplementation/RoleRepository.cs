using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using TeamNetDB.Model.OrientImplementation;
using TeamNetDB.Repository.Interfaces;
using TeamNetDB.TransactionToDatabase;

namespace TeamNetDB.Repository.OrientImplementation
{
    public class RoleRepository : IOrientTransactionable<Role>, IRepository<Role>
    {
        /// <summary>
        /// Constructor of role repository class wich receives as a parameter the transaction
        /// </summary>
        /// <param name="newTransaction">The transaction which performs the queries.</param>
        public RoleRepository(Transaction newTransaction)
        {
            Transaction = newTransaction;
        }

        /// <summary>
        /// The variable where the transaction class is stored.
        /// </summary>
        public TeamNetDB.Repository.Interfaces.ITransaction Transaction { get; set; }

        /// <summary>
        /// Gets all the Roles available for the groups.
        /// </summary>
        /// <returns>An IEnumerable collection which contains all roles availables.</returns>
        public IEnumerable<Role> GetAll()
        {
            var documents = this.Transaction.QueryVertex(VertexName.Role, new List<ConditionValue>());
            List<Role> roles = this.CreateEntitiesFromDocuments(documents);
            return roles;
        }

        /// <summary>
        /// Gets a single role given its name
        /// </summary>
        /// <param name="roleName">The name of the role to be get.</param>
        /// <returns>A single role wich matches the name given.</returns>
        public Role GetSingle(string roleName)
        {
            Role role;
            try
            {
                role = this.GetAll().Where(singleRole => singleRole.Name.Equals(roleName)).Single();
            }
            catch (Exception)
            {
                role = null;
            }
            return role;
        }

        /// <summary>
        /// Finds an specific role given any other parameter as constraint.
        /// </summary>
        /// <param name="predicate">A Lambda which specifies the type of filtering.</param>
        /// <returns>An IEnumerable collection which contains all the roles that matches the predicate given.</returns>
        public IEnumerable<Role> FindBy(System.Linq.Expressions.Expression<Func<Role, bool>> predicate)
        {
            var allEntities = this.GetAll();
            var entitiesWithPredicate = allEntities.Where(predicate.Compile());
            return entitiesWithPredicate;
        }

        /// <summary>
        /// Inserts a new Role into database
        /// </summary>
        /// <param name="entity">The type of entity to be added.</param>
        /// <param name="conectors">The list of connectors for the new rol</param>
        /// <returns>A bolean value which indicates if the role has been inserted correctly.</returns>
        public bool Insert(Role entity, IList<string> conectors = null)
        {
            List<VertexConnector> vertexConnectors = new List<VertexConnector>();
            return this.Transaction.InsertVertex(entity, VertexName.Role, vertexConnectors);
        }

        /// <summary>
        /// Deletes a role given itself.
        /// </summary>
        /// <param name="entity">The role to be deleted.</param>
        /// <returns>A boolean value which indicates if the role has been deleted.</returns>
        public bool Delete(Role entity)
        {
            bool isDeleted = false;
            try
            {
                List<ConditionValue> constraints = FillVertexConstraints(entity.Id);
                isDeleted = this.Transaction.Delete(VertexName.Role, constraints);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
                isDeleted = false;
            }
            return isDeleted;
            
        }

        /// <summary>
        /// Updates a role with a role given, this role will contain all the changes performed in the role mentioned.
        /// </summary>
        /// <param name="entity">The role that has changed.</param>
        /// <returns>A boolean value which indicates wether the role has been deleted or not.</returns>
        public bool Update(Role entity)
        {
            bool updated = false;
            try
            {
                List<ConditionValue> propertiesModified = FillChangedProperties(entity);
                List<ConditionValue> classConstraints = FillVertexConstraints(entity.Id);
                updated = this.Transaction.Update(VertexName.Role, propertiesModified, classConstraints);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
                updated = false;
            }
            return updated;
        }

        public IEnumerable<Role> Query(string query)
        {
            var documents = this.Transaction.QueryTraverse(query);
            List<Role> roles = this.CreateEntitiesFromDocuments(documents);
            return roles;
        }
        /// <summary>
        /// Creates a new entity Role from a document obtained from database.
        /// </summary>
        /// <param name="document">The document which has been obtained from database.</param>
        /// <returns>A single role which is an istance of the model cas</returns>
        public override Role CreateEntityFromDocument(Orient.Client.ODocument document)
        {
            Role role;
            try
            {
                role = new Role()
                {
                    Name = document.GetField<string>("name"),
                    Description = document.GetField<string>("description"),
                    Id = document.ORID.ToString()
                };
                return role;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Fills the Where condition for Update method.
        /// </summary>
        /// <param name="classId">The id of the vertex which is being deleted.</param>
        /// <returns>A List of condition value which is used for the Transaction and contains the rid of the 
        /// vertex to be deleted.</returns>
        private List<ConditionValue> FillVertexConstraints(string classId)
        {
            return new List<ConditionValue> {
            new ConditionValue { ConditionColumn = "@rid", ValueCondition = classId}
            };
        }

        /// <summary>
        /// Returns a List of Properties to be changed given the class name an the entity which contains the 
        /// changes for the vertex.
        /// </summary>
        /// <param name="className">the name of the class where its method's name are gonna be taken.</param>
        /// <param name="entity">The entity which contains the changes.</param>
        /// <returns></returns>
        private List<ConditionValue> FillChangedProperties(Role entity)
        {
            List<ConditionValue> conditions = new List<ConditionValue> 
            {
                new ConditionValue(){ ConditionColumn = "name", ValueCondition = entity.Name},
                new ConditionValue(){ ConditionColumn = "description", ValueCondition = entity.Description},
            };
            return conditions;
        }
    }
}
