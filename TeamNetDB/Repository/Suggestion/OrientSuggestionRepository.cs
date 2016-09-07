using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamNetDB.Model;
using Orient.Client;
using TeamNetDB.TransactionToDatabase;
using TeamNetDB.TransactionToDatabase.TransactionOrientDBBinary;

namespace TeamNetDB.Repository.Suggestion
{
    public class OrientSuggestionRepository : IRepositoryH<Model.SuggestionData>
    {
        /// <summary>
        /// Returns a list of query results.
        /// </summary>
        public List<Model.SuggestionData> Collection { get; private set; }

        /// <summary>
        /// Class constructor initializes the variables.
        /// </summary>
        /// <param name="newDataBase"></param>
        public OrientSuggestionRepository(Orient.Client.ODatabase newDataBase)
        {
            this.database = newDataBase;
            this.transaction = new TransactionBinary(this.database);
        }

        /// <summary>
        /// Removes a suggestion database.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(Model.SuggestionData entity)
        {
            int aux = this.database.Query("Delete From Suggestion Where name = '"+entity.Name+"'").Count;
            if (aux > 0)
            {
                return true;
            }
            return false;
        }

        public bool Insert(Model.SuggestionData entity, List<string> Rid)
        {
            bool created = true;
            try
            {
                this.transaction.InsertVertex(entity, VertexName.Suggestion,
                    this.GenerateEdges(new EdgesSuggestion().GetEdgesName(), Rid));
            }
            catch (Exception ex)
            {
                ex.ToString();
                created = false;
            }
            return created;
        }


        /// <summary>
        /// Inserts a new suggestion to the database and make the edges related.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Insert(Model.SuggestionData entity)
        {
            bool created = true;
            
            try
            {
                this.transaction.InsertVertex(this.CreateDataSuggestion(entity),
                    VertexName.Suggestion, this.GenerateEdges(new EdgesSuggestion().GetEdgesName(),
                    this.CreateRid(entity)));
            }
            catch (Exception ex)
            {
                ex.ToString();
                created = false;
            }
            return created;
        }

        private Model.Suggestion CreateDataSuggestion(Model.SuggestionData entity)
        {
            return new Model.Suggestion { name = entity.Name, 
                description = entity.Description, estimatedTime = entity.EstimatedTime, 
                score = entity.Score, valuation = entity.Valuation };
        }
        public bool Update(Model.SuggestionData entity)
        {
            throw new NotImplementedException();
        }

        public Model.SuggestionData Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Model.SuggestionData> GetAll(string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create list query a database.
        /// </summary>
        /// <param name="documents"></param>
        /// <returns></returns>
        private void ListCreator(List<ODocument> documents)
        {
            int i = 0;
            string name = string.Empty;
            string description = string.Empty;
            int estimatedTime = 0;
            string createdTo = string.Empty;
            string level = string.Empty;
            string point = string.Empty;
            string creatorOfSuggestion = string.Empty;
            int score = 0;
            int valuation = 0;
            string time = string.Empty;
            string valueCompetencyLevel = string.Empty;
            string descriptionCompetencyLevel= string.Empty;
            foreach (var document in documents)
            {
                if (i == 0)
                {
                    name = document.GetField<String>("name");
                    description = document.GetField<String>("description");
                    estimatedTime = document.GetField<int>("estimatedTime");
                    score = document.GetField<int>("score");
                    valuation = document.GetField<int>("valuation");
                }
                else if (i == 1)
                {
                     createdTo = document.GetField<String>("name");
                }
                else if (i == 2)
                {
                    level = document.GetField<String>("name");
                }
                else if (i == 3)
                {
                    point = document.GetField<String>("name");
                }
                else if (i == 4)
                {
                    creatorOfSuggestion = document.GetField<string>("name");
                }
                else if(i==5)
                {
                    valueCompetencyLevel = document.GetField<string>("value");
                    descriptionCompetencyLevel = document.GetField<string>("description");
                }
                else if(i==6)
                {
                    time = document.GetField<string>("name");
                    i = 0;

                    Model.SuggestionData task = new Model.SuggestionData()
                    {
                        Name = name,
                        Description = description,
                        EstimatedTime = estimatedTime,
                        Time = time,
                        Score = score,
                        Valuation = valuation,
                        CreatorOfSuggestion = creatorOfSuggestion,
                        Competency = point,
                        Level = level,
                        CreatedTo = createdTo,
                        ValueCompetencyLevel = valueCompetencyLevel,
                        DescriptionCompetencyLevel = descriptionCompetencyLevel
                    };
                    this.Collection.Add(task);
                }
                i++;
            }
        }

        /// <summary>
        /// Get suggestions of a point.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Model.SuggestionData Get(string name)
        {
            List<string> rids = new List<string>();
            List<ConditionValue> condition = new List<ConditionValue>();
            condition.Add(new ConditionValue() { ConditionColumn = "name", ValueCondition = name });
            rids.Add(this.transaction.GetRid(VertexName.Competency, condition));


            string query = "SELECT FROM (TRAVERSE * FROM " + rids[0] + ") where @Class <> 'Result360' and @Class <> 'Expected' and @Class <> 'Evaluator' Order By valuation DESC";
            List<ODocument> documents = this.transaction.QueryTraverse(query);
            this.ListCreator(documents);
            return null;
        }

        /// <summary>
        /// Gets all suggestions.
        /// </summary>
        /// <returns></returns>
        public Model.SuggestionData GetAll()
        {
            string query = "SELECT FROM suggestion Order By valuation DESC";
            this.Collection = new List<Model.SuggestionData>();
            this.GenerateTraverse(query);
            return null;
        }

        private void GenerateTraverse(string query)
        {
            List<string> rids = new List<string>();
            List<ODocument> documents = this.transaction.QueryTraverse(query);
            
            foreach (var document in documents)
            {
                this.ListCreator(this.transaction.QueryTraverse("select from ( traverse * from " + document.ORID.ToString() + " while $depth <= 1 )"));
            }
        }

        /// <summary>
        /// Manager queries to the database.
        /// </summary>
        private Orient.Client.ODatabase database;


        public IEnumerable<Model.SuggestionData> GetAllEntities()
        {
            throw new NotImplementedException();
        }

        private List<VertexConnector> GenerateEdges(List<EdgesName>edges, List<string> rids)
        {
            List<VertexConnector> connectors = new List<VertexConnector>();
            for (int i = 0; i < edges.Count; i++ )
            {
                VertexConnector connector = new VertexConnector();
                connector.EdgeName=edges[i];
                connector.To = rids[i];
                connectors.Add(connector);
            }
            return connectors;
        }


        public IPermission Get(string value1, string value2)
        {
            throw new NotImplementedException();
        }

        private List<string> CreateRid(Model.SuggestionData entity)
        {
            List<string> rids = new List<string>();
            List<ConditionValue> condition = new List<ConditionValue>();
            condition.Add(new ConditionValue() { ConditionColumn = "name", ValueCondition = entity.CreatorOfSuggestion });
            rids.Add(this.transaction.GetRid(VertexName.User, condition));

            condition = new List<ConditionValue>();
            condition.Add(new ConditionValue() { ConditionColumn = "name", ValueCondition = entity.Competency });
            rids.Add(this.transaction.GetRid(VertexName.Competency, condition));

            condition = new List<ConditionValue>();
            condition.Add(new ConditionValue() { ConditionColumn = "name", ValueCondition = entity.Level });
            rids.Add(this.transaction.GetRid(VertexName.Level, condition));

            condition = new List<ConditionValue>();
            condition.Add(new ConditionValue() { ConditionColumn = "name", ValueCondition = entity.CreatedTo });
            rids.Add(this.transaction.GetRid(VertexName.User, condition));

            condition = new List<ConditionValue>();
            condition.Add(new ConditionValue() { ConditionColumn = "value", ValueCondition = entity.ValueCompetencyLevel });
            rids.Add(this.transaction.GetRid(VertexName.CompetencyLevel, condition));

            condition = new List<ConditionValue>();
            condition.Add(new ConditionValue() { ConditionColumn = "name", ValueCondition = entity.Time });
            rids.Add(this.transaction.GetRid(VertexName.Time, condition));

            return rids;
        }

        private ITransaction transaction;
    }
}
