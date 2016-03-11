using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.ModuloOperacionRecoleccion
{
    public class ConsecutivoOTRService: IConsecutivoOTRService
    {
         private IRepository<ConsecutivoOTR> repository;

        public ConsecutivoOTRService(IRepository<ConsecutivoOTR> _repository)
        {
            repository = _repository;
        }

        public ConsecutivoOTR Get(long id)
        {
            return repository.Get(id);
        }

        public ConsecutivoOTR Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public ConsecutivoOTR Create(ConsecutivoOTR entity)
        {
            repository.Add(entity);
            return entity;
        }

        public ConsecutivoOTR Update(ConsecutivoOTR entity)
        {
            repository.Update(entity);
            return entity;
        }

        public void Delete(ConsecutivoOTR entity)
        {
            repository.Update(entity);
        }

        public bool Contains(ConsecutivoOTR item)
        {
            return repository.Contains(item);
        }

        public bool Contains(ConsecutivoOTR item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }

        public IEnumerable<ConsecutivoOTR> ListAll()
        {
            return repository.GetAll();
        }

        public IList<ConsecutivoOTR> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }

        public IList<ConsecutivoOTR> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate)
        {
            return repository.GetAll(criterias, property, startDate, endDate);
        }

        public int Count()
        {
            return repository.Count<ConsecutivoOTR>();
        }
    }
}
