using Orient.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamNetDB.Model.Interfaces;
using TeamNetDB.Model.OrientImplementation;
using TeamNetDB.Repository.Interfaces;
using TeamNetDB.TransactionToDatabase;

namespace TeamNetDB.Repository.OrientImplementation
{
    public class PublicationRepository : IOrientTransactionable<Publication>, IRepository<Publication>
    {
        /// <summary>
        /// Manage the Transaction object.
        /// </summary>
        public TeamNetDB.Repository.Interfaces.ITransaction Transaction { get; set; }

        /// <summary>
        /// Initialize a new PublicationRepository.
        /// </summary>
        public PublicationRepository(Transaction transaction)
        {
            this.Transaction = transaction;
        }

        /// <summary>
        /// Returns all publications.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Publication> GetAll()
        {
            string query = "SELECT FROM publication";
            IList<ODocument> documents = this.Transaction.QueryTraverse(query);
            IList<Publication> publications = this.RecoverAllPublications(documents);
            return publications;
        }

        /// <summary>
        /// Creates the object Publication.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public override Publication CreateEntityFromDocument(Orient.Client.ODocument doc)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Recover all publications stored in database.
        /// </summary>
        /// <param name="documents"> Contains all publications </param>
        /// <returns> List of all publications </returns>
        private IList<Publication> RecoverAllPublications(IList<ODocument> documents)
        {
            IList<Publication> result = new List<Publication>();
            string publicator = string.Empty;
            string date = string.Empty;
            string comment = string.Empty;
            string userId = string.Empty;
            List<string> groupsIds;
            List<string> links;
            foreach(ODocument document in documents) {
                List<ODocument> traverseDocuments = this.Transaction.QueryTraverse(
                    "select from ( traverse * from " + document.ORID.ToString() +
                    " while $depth <= 2 ) where @class = 'Reference' or @class = 'User' or @class = 'Group'");
                    comment = document.GetField<string>("comment");
                    date = document.GetField<string>("date");
                links = new List<string>();
                groupsIds = new List<string>();
                foreach (ODocument traverseDocument in traverseDocuments)
                    {
                    switch (traverseDocument.ORID.ToString().Substring(0, 3))
                    {
                        case ("#13"):
                        {
                            string userName = traverseDocument.GetField<string>("name");
                            string userLastname = traverseDocument.GetField<string>("lastName");
                            userId = traverseDocument.ORID.ToString();
                            publicator = userName + " " + userLastname;
                            break;
                    }
                        case ("#48"):
                    {
                            links.Add(traverseDocument.GetField<string>("url"));
                            break;
                        }
                        case ("#43"):
                        {
                            groupsIds.Add(traverseDocument.ORID.ToString());
                            break;
                        }
                    }
                }
                Publication publication = new Publication()
                    { Comment = comment, Date = date, Publicator = publicator, TaskLinks = links,
                        UserId = userId, GroupsIds = groupsIds };
                result.Add(publication);
            }
            return result;
        }

        /// <summary>
        /// Search a publication by its id.
        /// </summary>
        /// <param name="id"> Id to search </param>
        /// <returns> a publiation that contains this id </returns>
        public Publication GetSingle(string id)
        {
            var publication = this.GetAll().Where(v => v.Id.Equals(id)).Single();
            return publication;
        }

        /// <summary>
        /// Return an IEnumerable collection that contains all those publications that
        /// matches with te predicate given as a lambda function.
        /// </summary>
        /// <param name="predicate">A lambda function that will work as a constraints for the query.</param>
        /// <returns>An IEnumerable collection</returns>
        public IEnumerable<Publication> FindBy(System.Linq.Expressions.Expression<Func<Publication, bool>> predicate)
        {
            var publications = this.GetAll();
            var publicationsWithPredicate = publications.Where(predicate.Compile());
            return publicationsWithPredicate;
        }

        /// <summary>
        /// Insert a new Publication in database.
        /// </summary>
        /// <param name="entity"> Object to insert </param>
        /// <param name="conectors"> List of vertexes that has relationship with publication </param>
        /// <returns> returns true if the entity has been inserted </returns>
        public bool Insert(Publication entity, IList<string> conectors = null)
        {
            bool created = false;
            try
            {
                this.Transaction.InsertVertex(this.createDataPublication(entity),
                    VertexName.Publication, this.GenerateEdges(this.CreateRid(entity)));
                created = true;
            } catch (Exception ex)
            {
                ex.ToString();
            }
            return created;
        }

        /// <summary>
        /// Recover data from publication.
        /// </summary>
        /// <param name="entity"> Object that constains all datas </param>
        /// <returns> Return a PublicationData object </returns>
        private IEntity createDataPublication(Publication entity)
        {
            return new PublicationData
            {
                Comment = entity.Comment,
                Date = DateTime.Now.ToString()
            };
        }

        /// <summary>
        /// Create edges that connect with Publication vertex.
        /// </summary>
        /// <param name="rids"> vertexes orids </param>
        /// <returns> List of edges connectord </returns>
        private List<VertexConnector> GenerateEdges(List<string> rids)
        {
            IList<EdgesName> edges = new List<EdgesName>();
            edges.Add(EdgesName.PublicationUser);
            edges.Add(EdgesName.ContentPublication);
            edges.Add(EdgesName.PublicationGroup);
            List<VertexConnector> connectors = new List<VertexConnector>();
            for (int i = 0; i < rids.Count; i++)
            {
                VertexConnector connector = new VertexConnector();
                if (i >= 2)
                    connector.EdgeName = edges.Last();
                else
                connector.EdgeName = edges[i];
                connector.To = rids[i];
                connectors.Add(connector);
            }
            return connectors;
        }

        /// <summary>
        /// Create a orid of vertexes.
        /// </summary>
        /// <param name="entity"> publication object </param>
        /// <returns> Create a orid of vertexes </returns>
        private List<string> CreateRid(Publication entity)
        {
            List<string> rids = new List<string>();
            rids.Add(entity.Publicator);
            rids.Add(entity.Task);
            rids.AddRange(entity.GroupsIds);
            return rids;
        }
        
        public bool Delete(Publication entity)
        {
            throw new NotImplementedException();
        }

        public bool Update(Publication entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Publication> Query(string query)
        {
            throw new NotImplementedException();
        }
    }
}
