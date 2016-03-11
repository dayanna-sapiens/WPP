using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloBascula;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.ModuloBascula
{
    public class ConsecutivoBoletaService : IConsecutivoBoletaService
    {
         private IRepository<ConsecutivoBoleta> repository;

        public ConsecutivoBoletaService(IRepository<ConsecutivoBoleta> _repository)
        {
            repository = _repository;
        }

        public ConsecutivoBoleta Get(long id)
        {
            return repository.Get(id);
        }

        public ConsecutivoBoleta Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public ConsecutivoBoleta Create(ConsecutivoBoleta entity)
        {
            repository.Add(entity);
            return entity;
        }

        public ConsecutivoBoleta Update(ConsecutivoBoleta entity)
        {
            repository.Update(entity);
            return entity;
        }

        public void Delete(ConsecutivoBoleta entity)
        {
            repository.Update(entity);
        }

        public bool Contains(ConsecutivoBoleta item)
        {
            return repository.Contains(item);
        }

        public bool Contains(ConsecutivoBoleta item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }

        public IEnumerable<ConsecutivoBoleta> ListAll()
        {
            return repository.GetAll();
        }

        public IList<ConsecutivoBoleta> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }

        public IList<ConsecutivoBoleta> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate)
        {
            return repository.GetAll(criterias, property, startDate, endDate);
        }

        public int Count()
        {
            return repository.Count<ConsecutivoBoleta>();
        }
    }
}
