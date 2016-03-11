using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloNomina;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.ModuloNomina
{
    public class ConsecutivoNominaService : IConsecutivoNominaService
    {
         private IRepository<ConsecutivoNomina> repository;

        public ConsecutivoNominaService(IRepository<ConsecutivoNomina> _repository)
        {
            repository = _repository;
        }

        public ConsecutivoNomina Get(long id)
        {
            return repository.Get(id);
        }

        public ConsecutivoNomina Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public ConsecutivoNomina Create(ConsecutivoNomina entity)
        {
            repository.Add(entity);
            return entity;
        }

        public ConsecutivoNomina Update(ConsecutivoNomina entity)
        {
            repository.Update(entity);
            return entity;
        }

        public void Delete(ConsecutivoNomina entity)
        {
            repository.Update(entity);
        }

        public bool Contains(ConsecutivoNomina item)
        {
            return repository.Contains(item);
        }

        public bool Contains(ConsecutivoNomina item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }

        public IEnumerable<ConsecutivoNomina> ListAll()
        {
            return repository.GetAll();
        }

        public IList<ConsecutivoNomina> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }

        public IList<ConsecutivoNomina> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate)
        {
            return repository.GetAll(criterias, property, startDate, endDate);
        }

        public int Count()
        {
            return repository.Count<ConsecutivoNomina>();
        }
    }
}
