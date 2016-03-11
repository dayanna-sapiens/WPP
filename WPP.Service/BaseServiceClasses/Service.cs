using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.BaseServiceClasses
{
    public class Service<T> : IService<T> where T : Entity
    {
        #region Attributes
        //private readonly IQueryManager queryManager;
        private IRepository<T> repository;
        #endregion

        #region Properties
       /* public IQueryManager QueryManager
        {
            get { return queryManager; }
        }*/

        public IRepository<T> Repository
        {
            get { return repository; }
        }
        #endregion

        #region Constructors
        /*public Service(IRepository<T> repository, IQueryManager queryManager)
        {
            this.repository = repository;
            this.queryManager = queryManager;
        }*/

        public Service(IRepository<T> repository)
        {
            this.repository = repository;
        }
        #endregion

        #region Methods
        public T Get(long id)
        {
            return Repository.Get(id);
        }
        public T Get(IDictionary<string, object> criterias)
        {
            return Repository.Get(criterias);
        }
        public T Create(T entity)
        {
            Repository.Add(entity);
            return entity;
        }
        public T Update(T entity)
        {
            Repository.Update(entity);
            return entity;
        }
        public void Delete(T entity)
        {
            Repository.Remove(entity);
        }
        public IEnumerable<T> ListAll()
        {
            return Repository.GetAll();

        }

        public IList<T> GetAll(IDictionary<string, object> criterias)
        {
            return Repository.GetAll(criterias);
        }

        public IList<T> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate)
        {
            return Repository.GetAll(criterias, property, startDate, endDate);
        }

        public bool Contains(T item)
        {
            return Repository.Contains(item);
        }
        public bool Contains(T item, string property, object value)
        {
            return Repository.Contains(item, property, value);
        }
        public int Count()
        {
            return Repository.Count<T>();
        }

        #endregion


    }
}
