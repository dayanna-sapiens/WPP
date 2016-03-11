using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloBascula;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.ModuloBascula
{
    public class PagoBasculaService : IPagoBasculaService
    {
        private IRepository<PagoBascula> repository;

        public PagoBasculaService(IRepository<PagoBascula> _repository)
        {
            repository = _repository;
        }

        public PagoBascula Get(long id)
        {
            return repository.Get(id);
        }

        public PagoBascula Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public PagoBascula Create(PagoBascula entity)
        {
            repository.Add(entity);
            return entity;
        }
        public PagoBascula Update(PagoBascula entity)
        {
            repository.Update(entity);
            return entity;
        }
        public void Delete(PagoBascula entity)
        {
            entity.IsDeleted = true;
            repository.Update(entity);
        }
        public bool Contains(PagoBascula item)
        {
            return repository.Contains(item);
        }
        public bool Contains(PagoBascula item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }
        public IList<PagoBascula> ListAll()
        {
            return repository.GetAll();
        }
        public IList<PagoBascula> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }
       
        public int Count()
        {
            return repository.Count<PagoBascula>();
        }
    }
}
