using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.ModuloContratos
{
    public class ConsecutivoContratoService: IConsecutivoContratoService
    {
         private IRepository<ConsecutivoContrato> repository;

        public ConsecutivoContratoService(IRepository<ConsecutivoContrato> _repository)
        {
            repository = _repository;
        }

        public ConsecutivoContrato Get(long id)
        {
            return repository.Get(id);
        }

        public ConsecutivoContrato Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public ConsecutivoContrato Create(ConsecutivoContrato entity)
        {
            repository.Add(entity);
            return entity;
        }

        public ConsecutivoContrato Update(ConsecutivoContrato entity)
        {
            repository.Update(entity);
            return entity;
        }

        public void Delete(ConsecutivoContrato entity)
        {
            repository.Update(entity);
        }

        public bool Contains(ConsecutivoContrato item)
        {
            return repository.Contains(item);
        }

        public bool Contains(ConsecutivoContrato item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }

        public IEnumerable<ConsecutivoContrato> ListAll()
        {
            return repository.GetAll();
        }

        public IList<ConsecutivoContrato> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }

        public IList<ConsecutivoContrato> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate)
        {
            return repository.GetAll(criterias, property, startDate, endDate);
        }

        public int Count()
        {
            return repository.Count<ConsecutivoContrato>();
        }
    }
}
