using Orient.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeamNetDB.Model;
using TeamNetDB.Model.Interfaces;
using TeamNetDB.Repository.Interfaces;

namespace TeamNetDB.Repository
{
    public class DeleteManager
    {
        public static bool Delete<T>(IRepository<T> repository, T toDelete)  where T : IEntity {
            short deleteType = GetDeleteType();
            bool result = false;
            if (deleteType == 0)
            {
                IDeleteable toDeleteCasted = (IDeleteable)toDelete;
                toDeleteCasted.State = "disabled";
                var castedResult = (T)toDeleteCasted;
                result = repository.Update(castedResult);
            }
            else
            {
                result = repository.Delete(toDelete);
            }

            return result;
        }

        private static short GetDeleteType()
        {
            ODatabase database = UnitOfWork.GetInstance().GetDatabase();
            List<ODocument> deleteType = database.Select("value").From("SystemConfig").ToList();
            database.Dispose();

            return short.Parse(deleteType[0].GetField<short>("value").ToString());
        }
    }
}

