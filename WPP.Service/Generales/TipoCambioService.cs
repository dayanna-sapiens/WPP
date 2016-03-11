using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.Generales
{
    public class TipoCambioService : ITipoCambioService
    {
          private IRepository<TipoCambio> repository;
          public TipoCambioService(IRepository<TipoCambio> _repository)
        {
            repository = _repository;
        }

         public TipoCambio Get(long id)
        {
            return repository.Get(id);
        }

        public  TipoCambio Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

       
        public  TipoCambio Create( TipoCambio entity)
        {
            repository.Add(entity);
            return entity;
        }

        public  TipoCambio Update( TipoCambio entity)
        {
            repository.Update(entity);
            return entity;
        }

        public void Delete( TipoCambio entity)
        {
            repository.Remove(entity);
        }
        public IList<TipoCambio> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }
    }
}
