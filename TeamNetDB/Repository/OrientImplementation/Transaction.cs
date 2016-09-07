using Orient.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamNetDB.ConnectionDatabase;
using TeamNetDB.Model.Interfaces;
using TeamNetDB.TransactionToDatabase;

namespace TeamNetDB.Repository.OrientImplementation
{
    public class Transaction : TeamNetDB.Repository.Interfaces.ITransaction
    {
        public Transaction(ConnectionBinaryOrientDB connectionDB)
        {
            this.connectionDB = connectionDB;
        }
        
        #region Insert
        /// <summary>
        /// Vertex values ​​inserted to the database.
        /// </summary>
        /// <param name="objectToBeInsert">Inserting values ​​for the database</param>
        /// <param name="name">name of the vertex.</param>
        /// <returns>true if insert, false no insert</returns>
        public bool InsertVertex(IEntity objectToBeInsert, VertexName name, List<VertexConnector> connector)
        {
            bool created = true;
            try
            {
                this.ConnectDB();
                this.Orid = this.database.Create.Vertex(name.ToString()).Set(objectToBeInsert).Run().ORID.ToString();
                this.CreatingEdgesHandler(connector);
                this.CloseDB();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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

                this.ConnectDB();
                this.database.Command(this.builder.StructureString(delete, condition));
                this.CloseDB();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                answer = false;
            }
            return answer;
        }

        public bool Delete(VertexName vertex)
        {
            bool answer = true;
            try
            {
                this.builder = new BuilderString();
                string delete = "Delete From " + vertex.ToString();

                this.ConnectDB();
                this.database.Command(delete);
                this.CloseDB();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
            List<ODocument> documents = this.ExecuteQuery(this.builder.StructureString(query, condition));
            return documents;
        }

        public string GetRid(VertexName vertex, List<ConditionValue> condition)
        {
            this.builder = new BuilderString();
            string query = "Select From " + vertex.ToString() + " Where ";

            List<ODocument> documents = this.ExecuteQuery(this.builder.StructureString(query, condition));
            if (documents.Count == 0)
            {
                return string.Empty;
            }
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
            this.ConnectDB();
            List<ODocument> documents = this.ExecuteQuery(query);
            this.CloseDB();
            return documents;
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
                string update = "Update " + vertex.ToString() + " Set ";

                this.ConnectDB();
                this.database.Command(this.builder.StructureString(this.builder.StructureString(update, setCondition, true) + " Where ",
                    whereCondition));
                this.CloseDB();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                answer = false;
            }
            return answer;
        }

        public bool Update(OVertex vertex, string id)
        {
            if (vertex == null)
            {
                return false;
            }

            try
            {
                this.ConnectDB();
                this.database.Update(vertex)
                             .Where("@rid").Equals(id)
                             .Limit(1)
                             .Run();
            this.CloseDB();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        #endregion Update

        #region Private

        /// <summary>
        /// Manages the connection to the database.
        /// </summary>
        private ODatabase database;

        private ConnectionBinaryOrientDB connectionDB;

        /// <summary>
        /// Build a string.
        /// </summary>
        private BuilderString builder;

        /// <summary>
        /// Manage Rid of vertex.
        /// </summary>
        private string Orid { get; set; }

        /// <summary>
        /// Execute a query to the database.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private List<ODocument> ExecuteQuery(string query)
        {
            try
            {
                this.ConnectDB();
                List<ODocument> documents = this.database.Query(query);
                this.CloseDB();

                return documents;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<ODocument>();
        }
        }

        private void ConnectDB()
        {
            this.database = this.connectionDB.Connection() as ODatabase;
        }

        private void CloseDB()
        {
            this.database.Dispose();
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
        private void CreateEdge(EdgesName name, String to,bool isInEdge = false)
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
                if (isInEdge)
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
