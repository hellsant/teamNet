using Orient.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamNetDB.Model.Interfaces;
using TeamNetDB.Repository;

namespace TeamNetDB.TransactionToDatabase.TransactionOrientDBBinary
{
    public class TransactionBinary : ITransaction
    {
        #region Constructor
        /// <summary>
        /// Constructo of the class.
        /// </summary>
        /// <param name="newDatabase"></param>
        public TransactionBinary(ODatabase newDatabase)
        {
            this.database = newDatabase;
        }
        #endregion Constructor
        #region Insert
        /// <summary>
        /// Vertex values ​​inserted to the database.
        /// </summary>
        /// <param name="objectToBeInsert">Inserting values ​​for the database</param>
        /// <param name="name">name of the vertex.</param>
        /// <returns>true if insert, false no insert</returns>
        public bool InsertVertex(IEntityBase objectToBeInsert, VertexName name, List<VertexConnector> connector)
        {
            bool created = true;
            try
            {
                this.Orid=this.database.Create.Vertex(name.ToString()).Set(objectToBeInsert).Run().ORID.ToString();
                this.CreatingEdgesHandler(connector);

            }
            catch (Exception ex)
            {
                ex.ToString();
                created = false;
            }
            return created;
        }

        /// <summary>
        /// The method is responsible for creating an edge relationship based start vertex and end vertex.
        /// </summary>
        /// <param name="name">Is the name the edge that will connect the two vertices.</param>
        /// <param name="RidFrom">The Rid of the data of the vertex from which initiates the relationship.</param>
        /// <param name="RidTo">The Rid of the vertex data of the relationship ends where.</param>
        /// <returns></returns>
        public bool CreateEdgeRelationship(EdgesName name, string RidFrom, string RidTo)
        {
            bool created = true;
            if (!RidFrom.Equals("") && !RidTo.Equals(""))
            {
                try
                {
                    this.database
                        .Create.Edge(name.ToString())
                        .Cluster(name.ToString())
                        .From(new ORID(RidFrom))
                        .To(new ORID(RidTo))
                        .Run();

                }
                catch (Exception ex)
                {
                    ex.ToString();
                    created = false;
                }
            }
            else
            {
                created = false;
            }
            return created;
        }
        #endregion Insert

        #region Delete
        /// <summary>
        /// Removes an item from a vertex of the database.
        /// </summary>
        /// <param name="vertex">from where it will eliminate</param>
        /// <param name="conditionColumn">provided that column is taken</param>
        /// <param name="valueCondition">value of the condition to eliminate</param>
        /// <returns>a value of true if the data is removed to</returns>
        public bool Delete(VertexName vertex, List<ConditionValue> condition)
        {
            bool answer = true;
            try
            {
                this.builder = new BuilderString();
                string delete = "Delete From " + vertex.ToString() + " Where ";
                this.database.Command(this.builder.StructureString(delete,condition));
            }
            catch (Exception e)
            {
                e.ToString();
                answer = false;
            }
            return answer;
        }

        /// <summary>
        /// The method removes an item from the database.
        /// </summary>
        /// <param name="vertex">delete vertex name</param>
        /// <returns>truth value</returns>
        public bool Delete(VertexName vertex)
        {
            bool answer = true;
            try
            {
                this.builder = new BuilderString();
                string delete = "Delete From " + vertex.ToString();
                this.database.Command(delete);
            }
            catch (Exception e)
            {
                e.ToString();
                answer = false;
            }
            return answer;
        }

        /// <summary>
        /// The method is responsible for removing an edge between two vertices relationship with their respective Rid.
        /// </summary>
        /// <param name="fromRid">It is the vertex data Rid start where you want to delete.</param>
        /// <param name="toRid">Is the ID of the data from the vertex to where you want to delete.</param>
        /// <returns>Returns a true value if the edge is successfully removed from relationship.</returns>
        public bool DeleteEdgeRelationship(string fromRid, string toRid)
        {
            bool answer = true;
            try
            {
                string deleteEdge = "Delete Edge form " + fromRid + " To "+ toRid;
                this.database.Command(deleteEdge);
            }
            catch (Exception e)
            {
                e.ToString();
                answer = false;
            }
            return answer;
        }
        #endregion Delete

        #region Query
        /// <summary>
        /// Executes specific queries only vertices.
        /// </summary>
        /// <param name="vertex"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public List<ODocument> QueryVertex(VertexName vertex, List<ConditionValue> condition)
        {
            this.builder = new BuilderString();
            
            

            string query = "Select From " + vertex.ToString() + " ";
            if (condition.Count > 0)
            {
                query += " Where ";
            }
            return this.ExecuteQuery(this.builder.StructureString(query, condition));
        }

        /// <summary>
        /// Obtains Rid of a vertex with a condition.
        /// </summary>
        /// <param name="vertex">vertex name</param>
        /// <param name="condition">conthe condition to offload.</param>
        /// <returns>rid string value</returns>
        public string GetRid(VertexName vertex, List<ConditionValue> condition)
        {
            this.builder = new BuilderString();
            string query = "Select From " + vertex.ToString() + " Where ";
            List<ODocument> documents = this.ExecuteQuery(this.builder.StructureString(query, condition));
            return documents[0].ORID.ToString();
        }

        /// <summary>
        /// Gets the rid through consultation
        /// </summary>
        /// <param name="query">query to search</param>
        /// <returns></returns>
        public string GetRid(string query)
        {
            List<ODocument> documents = this.ExecuteQuery(query);
            return documents[0].ORID.ToString();
        }

        #region QueryTraverse
        /// <summary>
        /// Runs type queries traverse.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<ODocument> QueryTraverse(string query)
        {
            return this.ExecuteQuery(query);
        }
        #endregion QueryTraverse
        #endregion Query

        #region Update
        /// <summary>
        /// Run the update to a vertex.
        /// </summary>
        /// <param name="vertex"></param>
        /// <param name="setCondition"></param>
        /// <param name="whereCondition"></param>
        /// <returns></returns>
        public bool Update(VertexName vertex, List<ConditionValue> setCondition, List<ConditionValue> whereCondition)
        {
            bool answer = true;
            try
            {
                this.builder = new BuilderString();
                string update = "Update "+vertex.ToString()+" Set ";
                this.database.Command(this.builder.StructureString(this.builder.StructureString(update, setCondition) +" Where ",
                    whereCondition));
            }
            catch (Exception e)
            {
                e.ToString();
                answer = false;
            }
            return answer;
        }
        #endregion Update

        #region Private

        /// <summary>
        /// Manages the connection to the database.
        /// </summary>
        private ODatabase database;

        /// <summary>
        /// Build a string.
        /// </summary>
        private BuilderString builder = null;

        /// <summary>
        /// Manage Rid of vertex.
        /// </summary>
        private string Orid{get;set;}

        /// <summary>
        /// Execute a query to the database.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private List<ODocument> ExecuteQuery(string query)
        {
            return this.database.Query(query);
        }

        #region CreateEdge

        /// <summary>
        /// Sort create all edges having a vertex.
        /// </summary>
        /// <param name="connector">List edges, from an to</param>
        private void CreatingEdgesHandler(List<VertexConnector> connector)
        {
            foreach (VertexConnector data in connector)
            {
                this.CreateEdge(data.EdgeName, data.To,data.IsInEdge);
            }
        }


        /// <summary>
        /// Create an edge linking vertices.
        /// </summary>
        /// <param name="name">Name edge</param>
        /// <param name="from">where to start</param>
        /// <param name="to">where just</param>
        private void CreateEdge(EdgesName name, String to,bool inEdge = false)
        {
            if (EdgesName.UserSuggestion == name)
            {
                this.database
                      .Create.Edge(name.ToString())
                      .Cluster(name.ToString())
                      .From(new ORID(to))
                      .To(new ORID(this.Orid))
                      .Run();
            }
            else
            {
                if (inEdge)
                {
                    this.database
                    .Create.Edge(name.ToString())
                    .Cluster(name.ToString())
                    .From(new ORID(to))
                    .To(new ORID(this.Orid))
                    .Run();
                }
                else
                {
                    this.database
                    .Create.Edge(name.ToString())
                    .Cluster(name.ToString())
                    .From(new ORID(this.Orid))
                    .To(new ORID(to))
                    .Run();
                }
            }
        }
        #endregion CreateEdge
        #endregion Private

    }
}
