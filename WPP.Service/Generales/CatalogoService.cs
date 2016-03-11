using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.Generales
{
     public class CatalogoService : ICatalogoService
    {
         private IRepository<Catalogo> repository;
         public CatalogoService(IRepository<Catalogo> _repository)
        {
            repository = _repository;
        }

         public Catalogo Get(long id)
        {
            return repository.Get(id);
        }

        public  Catalogo Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public IList<Catalogo> GetByType(String tipo)
        {
            IDictionary<string, object> crit = new Dictionary<string, object>();
            crit.Add("Tipo", tipo);
            return repository.GetAll(crit);
        }



        public  Catalogo Create( Catalogo entity)
        {
            repository.Add(entity);
            return entity;
        }

        public  Catalogo Update( Catalogo entity)
        {
            repository.Update(entity);
            return entity;
        }

        public void Delete( Catalogo entity)
        {
            repository.Remove(entity);
        }

        public bool Contains(Catalogo item)
        {
            return repository.Contains(item);
        }

        public bool Contains( Catalogo item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }

        public IEnumerable< Catalogo> ListAll()
        {
            return repository.GetAll();
        }

        public IList< Catalogo> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }

        public IList< Catalogo> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate)
        {
            return repository.GetAll(criterias, property, startDate, endDate);
        }

        public int Count()
        {
            return repository.Count<Catalogo>();
        }


       
    }
}
