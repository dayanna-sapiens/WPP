using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.ModuloContratos
{
    public class ConsecutivoClienteService : IConsecutivoClienteService
    {
        private IRepository<ConsecutivoCliente> repository;

        public ConsecutivoClienteService(IRepository<ConsecutivoCliente> _repository)
        {
            repository = _repository;
        }

        public ConsecutivoCliente Get(long id)
        {
            return repository.Get(id);
        }

        public ConsecutivoCliente Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public ConsecutivoCliente Create(ConsecutivoCliente entity)
        {
            repository.Add(entity);
            return entity;
        }

        public ConsecutivoCliente Update(ConsecutivoCliente entity)
        {
            repository.Update(entity);
            return entity;
        }

        public void Delete(ConsecutivoCliente entity)
        {
            repository.Update(entity);
        }

        public bool Contains(ConsecutivoCliente item)
        {
            return repository.Contains(item);
        }

        public bool Contains(ConsecutivoCliente item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }

        public IEnumerable<ConsecutivoCliente> ListAll()
        {
            return repository.GetAll();
        }

        public IEnumerable<ConsecutivoCliente> GetAll()
        {
            return repository.GetQuery();
        }

        public IList<ConsecutivoCliente> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }

        public IList<ConsecutivoCliente> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate)
        {
            return repository.GetAll(criterias, property, startDate, endDate);
        }

        public int Count()
        {
            return repository.Count<ConsecutivoCliente>();
        }
    }
}
