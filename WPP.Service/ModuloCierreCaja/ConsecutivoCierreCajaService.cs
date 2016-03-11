using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloCierreCaja;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.ModuloCierreCaja
{
    public class ConsecutivoCierreCajaService : IConsecutivoCierreCajaService
    {
          private IRepository<ConsecutivoCierreCaja> repository;

        public ConsecutivoCierreCajaService(IRepository<ConsecutivoCierreCaja> _repository)
        {
            repository = _repository;
        }

        public ConsecutivoCierreCaja Get(long id)
        {
            return repository.Get(id);
        }

        public ConsecutivoCierreCaja Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public ConsecutivoCierreCaja Create(ConsecutivoCierreCaja entity)
        {
            repository.Add(entity);
            return entity;
        }

        public ConsecutivoCierreCaja Update(ConsecutivoCierreCaja entity)
        {
            repository.Update(entity);
            return entity;
        }

        public void Delete(ConsecutivoCierreCaja entity)
        {
            repository.Update(entity);
        }

        public bool Contains(ConsecutivoCierreCaja item)
        {
            return repository.Contains(item);
        }

        public bool Contains(ConsecutivoCierreCaja item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }

        public IEnumerable<ConsecutivoCierreCaja> ListAll()
        {
            return repository.GetAll();
        }

        public IEnumerable<ConsecutivoCierreCaja> GetAll()
        {
            return repository.GetQuery();
        }

        public IList<ConsecutivoCierreCaja> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }

        public IList<ConsecutivoCierreCaja> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate)
        {
            return repository.GetAll(criterias, property, startDate, endDate);
        }

        public int Count()
        {
            return repository.Count<ConsecutivoCierreCaja>();
        }
    }
}
