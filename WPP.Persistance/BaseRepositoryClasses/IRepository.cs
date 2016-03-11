using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;

namespace WPP.Persistance.BaseRepositoryClasses
{
    public interface IRepository<T> : IEnumerable<T> where T : Entity
    {
        void Add(T item);
        void Update(T item);
        bool Contains(T item);
        bool Contains(T item, string property, object value);
        bool Remove(T item);
        T Get(IDictionary<string, object> criterias);
        T Get(long id);
        IList<T> GetAll();
        IList<T> GetAll(IDictionary<string, object> criterias);
        IList<T> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate);
        int Count { get; }

        IQueryable<T> GetQuery();
    }
}
