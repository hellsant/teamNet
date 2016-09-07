using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TeamNetDB.Repository.Interfaces
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();

        T GetSingle(string id);

        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);

        bool Insert(T entity,IList<string> conectors = null);

        bool Delete(T entity);

        bool Update(T entity);

        IEnumerable<T> Query(string query);
    }
}
