using Orient.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TeamNetDB;
using TeamNetDB.Model.Interfaces;
using TeamNetDB.Model.OrientImplementation;
using TeamNetDB.Repository.Interfaces;
using TeamNetDB.TransactionToDatabase;
using TeamNetDB.TransactionToDatabase.TransactionOrientDBBinary;

namespace TeamNetDB.Repository.OrientImplementation
{
    public class SuggestionRepository : IOrientTransactionable<SuggestionData>, IRepository<SuggestionData>
    {
        /// <summary>
        /// Initializes global variables.
        /// </summary>
        /// <param name="transaction"></param>
        public SuggestionRepository(Transaction transaction)
        {
            this.Transaction = transaction;
        }

        /// <summary>
        /// Manage the Transaction object.
        /// </summary>
        public TeamNetDB.Repository.Interfaces.ITransaction Transaction { get; set; }

        /// <summary>
        /// Returns all suggestions.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SuggestionData> GetAll()
        {
            var suggestions = this.GenerateTraverse(querySuggestion);
            return suggestions;
        }

        /// <summary>
        /// Search a suggestion by its id.
        /// </summary>
        /// <param name="id">to search</param>
        /// <returns>a suggrstion</returns>
        public SuggestionData GetSingle(string id)
        {
            var suggestion = this.GetAll().Where(v => v.Id.Equals(id)).Single();
            return suggestion;
        }

        /// <summary>
        /// Search all suggestions that match the different conditions.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<SuggestionData> FindBy(Expression<Func<SuggestionData, bool>> predicate)
        {
            var allEntities = this.GetAll();
            var entitiesWithPredicate = allEntities.Where(predicate.Compile());
            return entitiesWithPredicate;
        }

        /// <summary>
        /// The method handles the suggestions insert database.
        /// </summary>
        /// <param name="entity">suggestion object</param>
        /// <param name="connectors">datas the edges</param>
        /// <returns>bool</returns>
        public bool Insert(SuggestionData entity, IList<string> connectors)
        {
            bool created = true;

            try
            {
                this.Transaction.InsertVertex(this.CreateDataSuggestion(entity),
                    VertexName.Suggestion, this.GenerateEdges(this.CreateRid(entity)));
            }
            catch (Exception ex)
            {
                ex.ToString();
                created = false;
            }
            return created;
        }

        public bool Delete(SuggestionData entity)
        {
            throw new NotImplementedException();
        }

        public bool Update(SuggestionData entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The method is responsible for conducting a structured query to the database.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IEnumerable<SuggestionData> Query(string query)
        {
            var documents = this.Transaction.QueryTraverse(query);
            List<SuggestionData> suggestions = this.CreateEntitiesFromDocuments(documents);
            return suggestions;
        }

        /// <summary>
        /// The method is responsible for creating the object DataSuggestion.
        /// </summary>
        /// <param name="documents"></param>
        /// <returns></returns>
        public override List<SuggestionData> CreateEntitiesFromDocuments(List<ODocument> documents)
        {
            List<SuggestionData> listSuggestion = new List<SuggestionData>();
            SuggestionData suggestion = new SuggestionData() { Level = "junior" };
            int index = 0;
            foreach (var document in documents)
            {
                switch (index)
                {
                    case 0:
                        suggestion.Name = document.GetField<String>("name");
                        suggestion.Description = document.GetField<String>("description");
                        suggestion.EstimatedTime = document.GetField<int>("estimatedTime");
                        suggestion.Score = document.GetField<int>("score");
                        suggestion.Valuation = document.GetField<int>("valuation");
                        break;
                    case 1:
                        suggestion.Time = document.GetField<string>("name");
                        break;
                    case 2:
                        suggestion.ValueCompetecyLevel = document.GetField<int>("value");
                        suggestion.DescriptionCompetencyLevel = document.GetField<string>("description");
                        break;
                    case 3:
                        suggestion.Competency = document.GetField<String>("name");
                        break;
                    case 4:
                        var name = document.GetField<string>("name");
                        var lastName = document.GetField<string>("lastName");
                        suggestion.CreatorOfSuggestion = string.Format(fullName, name, lastName);
                        break;
                    case 5:
                        name = document.GetField<string>("name");
                        lastName = document.GetField<string>("lastName");
                        suggestion.CreatedTo = string.Format(fullName, name, lastName);
                        break;
                }
                index++;
            }
            listSuggestion.Add(suggestion);
            return listSuggestion;
        }

        /// <summary>
        /// Creates the object Suggestion.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public override SuggestionData CreateEntityFromDocument(ODocument document)
        {
            SuggestionData expected = new SuggestionData()
            {
                Name = document.GetField<String>("name"),
                Description = document.GetField<String>("description"),
                EstimatedTime = document.GetField<int>("estimatedTime"),
                Score = document.GetField<int>("score"),
                Valuation = document.GetField<int>("valuation"),
                Id = document.ORID.ToString()
            };
            return expected;
        }

        /// <summary>
        /// Generates the query traverse.
        /// </summary>
        /// <param name="query">The only query suggestions.</param>
        /// <returns>List suggestions.</returns>
        private List<SuggestionData> GenerateTraverse(string query)
        {
            List<ODocument> documents = this.Transaction.QueryTraverse(query);
            List<SuggestionData> suggestions = new List<SuggestionData>();
            foreach (var document in documents)
            {
                List<ODocument> aux = this.Transaction.QueryTraverse("select from ( traverse * from " + document.ORID.ToString() + " while $depth <= 1 )");
                if (aux.Count > 0)
                {
                    suggestions.Add(this.CreateEntitiesFromDocuments(aux)[0]);
                }
            }
            return suggestions;
        }

        /// <summary>
        /// The method is responsible for creating the relationship of edges with vertex.
        /// </summary>
        /// <param name="rids">Ordi of the vertex</param>
        /// <returns>list edges</returns>
        private List<VertexConnector> GenerateEdges(List<string> rids)
        {
            List<EdgesName> edges = this.ListSuggestionEdgesName();
            List<VertexConnector> connectors = new List<VertexConnector>();
            for (int i = 0; i < edges.Count; i++)
            {
                VertexConnector connector = new VertexConnector();
                connector.EdgeName = edges[i];
                connector.To = rids[i];
                connectors.Add(connector);
            }
            return connectors;
        }

        /// <summary>
        /// The method creates the edges that have relation with the vertex suggestion.
        /// </summary>
        /// <returns>List edges</returns>
        private List<EdgesName> ListSuggestionEdgesName()
        {
            List<EdgesName> edges = new List<EdgesName>();
            edges.Add(EdgesName.SuggestionUser);
            edges.Add(EdgesName.SuggestionCompetency);
            edges.Add(EdgesName.SuggestionUser);
            edges.Add(EdgesName.SuggestionCompetencyLevel);
            edges.Add(EdgesName.SuggestionTime);
            return edges;
        }

        /// <summary>
        /// he method aims to get rids of vertices that relate.
        /// </summary>
        /// <param name="entity">Object SugestionData</param>
        /// <returns>List by rids</returns>
        private List<string> CreateRid(SuggestionData entity)
        {
            List<string> rids = new List<string>()
            {
                string.Format(user, entity.CreatorOfSuggestion),
                string.Format(competency, entity.Competency),
                string.Format(user, entity.CreatedTo),
                string.Format(competencyLevel, entity.ValueCompetecyLevel),
                string.Format(time, entity.Time),
            };
            return rids;
        }

        /// <summary>
        /// The method is responsible for creating the object suggestion.
        /// </summary>
        /// <param name="entity">is object DataSuggestion</param>
        /// <returns>Object suggestion</returns>
        private Suggestion CreateDataSuggestion(SuggestionData entity)
        {
            return new Suggestion
            {
                Name = entity.Name,
                Description = entity.Description,
                EstimatedTime = entity.EstimatedTime,
                Score = entity.Score,
                Valuation = entity.Valuation
            };
        }

        private string querySuggestion = "SELECT FROM suggestion";
        private string fullName = "{0} {1}";
        private string user = "#13:{0}";
        private string competency = "#12:{0}";
        private string competencyLevel = "#28:{0}";
        private string time = "#39:{0}";
    }
}
