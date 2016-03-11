using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloFacturacion;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.ModuloFacturacion
{
    public class ConsecutivoFacturacionService : IConsecutivoFacturacionService
    {
         private IRepository<ConsecutivoFacturacion> repository;

        public ConsecutivoFacturacionService(IRepository<ConsecutivoFacturacion> _repository)
        {
            repository = _repository;
        }

        public ConsecutivoFacturacion Get(long id)
        {
            return repository.Get(id);
        }

        public ConsecutivoFacturacion Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public ConsecutivoFacturacion Create(ConsecutivoFacturacion entity)
        {
            repository.Add(entity);
            return entity;
        }

        public ConsecutivoFacturacion Update(ConsecutivoFacturacion entity)
        {
            repository.Update(entity);
            return entity;
        }

        public void Delete(ConsecutivoFacturacion entity)
        {
            repository.Update(entity);
        }

        public bool Contains(ConsecutivoFacturacion item)
        {
            return repository.Contains(item);
        }

        public bool Contains(ConsecutivoFacturacion item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }

        public IEnumerable<ConsecutivoFacturacion> ListAll()
        {
            return repository.GetAll();
        }

        public IList<ConsecutivoFacturacion> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }

        public IList<ConsecutivoFacturacion> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate)
        {
            return repository.GetAll(criterias, property, startDate, endDate);
        }

        public int Count()
        {
            return repository.Count<ConsecutivoFacturacion>();
        }
    }
}
