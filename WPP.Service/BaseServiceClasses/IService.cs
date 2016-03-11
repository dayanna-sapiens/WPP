using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;

namespace WPP.Service.BaseServiceClasses
{
    public interface IService<T> where T : Entity
    {
        //IQueryManager QueryManager { get; }
        T Get(long id);
        T Get(IDictionary<string, object> criterias);
        T Create(T entity);
        T Update(T entity);
        void Delete(T entity);
        bool Contains(T item);
        bool Contains(T item, string property, object value);
        IEnumerable<T> ListAll();
        IList<T> GetAll(IDictionary<string, object> criterias);
        IList<T> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate);

        int Count();

    }
}
