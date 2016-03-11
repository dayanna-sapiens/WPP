using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloFacturacion;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.ModuloFacturacion
{
    public class ReversionService : IReversionService
    {
          private IRepository<Reversion> repository;

        public ReversionService(IRepository<Reversion> _repository)
        {
            repository = _repository;
        }

        public Reversion Get(long id)
        {
            return repository.Get(id);
        }

        public Reversion Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public Reversion Create(Reversion entity)
        {
            repository.Add(entity);
            return entity;
        }

        public Reversion Update(Reversion entity)
        {
            repository.Update(entity);
            return entity;
        }

        public void Delete(Reversion entity)
        {
            entity.IsDeleted = true;
            repository.Update(entity);
        }

        public bool Contains(Reversion item)
        {
            return repository.Contains(item);
        }

        public bool Contains(Reversion item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }

        public IList<Reversion> ListAll()
        {
            return repository.GetAll();
        }

        public IList<Reversion> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }

        public IList<Reversion> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate)
        {
            return repository.GetAll(criterias, property, startDate, endDate);
        }

        public long ReversionSearch(long factura, long compania) 
        {
            long cantidad = 0;
            var query = repository.GetQuery();

            query = query.Where(u => u.IsDeleted == false);
            query = query.Where(u => u.Compania.Id == compania);
            query = query.Where(u => u.Facturacion.Id == factura);
            query = query.Where(u => u.CreateDate.Year == DateTime.Now.Year);

            cantidad = query.ToList().Count;

            return cantidad;
        }


    }
}
