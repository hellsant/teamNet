using Orient.Client;
using System.Collections.Generic;
using TeamNetDB.Repository.Interfaces;
namespace TeamNetDB.Repository.OrientImplementation
{
    public abstract class  IOrientTransactionable<T>
    {
        public ITransaction Transaction { get; set; }

        public virtual List<T> CreateEntitiesFromDocuments(List<ODocument> documents)
        {
            List<T> entities = new List<T>();
            foreach (var document in documents)
            {
                T entity = CreateEntityFromDocument(document);
                if(entity!=null)
                {
                    entities.Add(entity);
                }
            }
            return entities;
        }

        public abstract T CreateEntityFromDocument(ODocument doc);
        
    }
}
