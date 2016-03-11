using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloBascula;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.ModuloBascula
{
    public class ConsecutivoReciboPagoService : IConsecutivoReciboPagoService
    {
        private IRepository<ConsecutivoReciboPago> repository;

        public ConsecutivoReciboPagoService(IRepository<ConsecutivoReciboPago> _repository)
        {
            repository = _repository;
        }

        public ConsecutivoReciboPago Get(long id)
        {
            return repository.Get(id);
        }

        public ConsecutivoReciboPago Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public ConsecutivoReciboPago Create(ConsecutivoReciboPago entity)
        {
            repository.Add(entity);
            return entity;
        }

        public ConsecutivoReciboPago Update(ConsecutivoReciboPago entity)
        {
            repository.Update(entity);
            return entity;
        }

        public void Delete(ConsecutivoReciboPago entity)
        {
            repository.Update(entity);
        }

        public bool Contains(ConsecutivoReciboPago item)
        {
            return repository.Contains(item);
        }

        public bool Contains(ConsecutivoReciboPago item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }

        public IEnumerable<ConsecutivoReciboPago> ListAll()
        {
            return repository.GetAll();
        }

        public IList<ConsecutivoReciboPago> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }

        public IList<ConsecutivoReciboPago> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate)
        {
            return repository.GetAll(criterias, property, startDate, endDate);
        }

        public int Count()
        {
            return repository.Count<ConsecutivoReciboPago>();
        }
    }
}
