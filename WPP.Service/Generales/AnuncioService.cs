using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.Generales
{
    public class AnuncioService : IAnuncioService
    {
        private IRepository<Anuncio> repository;
         public AnuncioService(IRepository<Anuncio> _repository)
        {
            repository = _repository;
        }

         public Anuncio Get(long id)
        {
            return repository.Get(id);
        }

        public  Anuncio Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public IList<Anuncio> GetByType(String tipo)
        {
            IDictionary<string, object> crit = new Dictionary<string, object>();
            crit.Add("Tipo", tipo);
            return repository.GetAll(crit);
        }



        public  Anuncio Create( Anuncio entity)
        {
            repository.Add(entity);
            return entity;
        }

        public  Anuncio Update( Anuncio entity)
        {
            repository.Update(entity);
            return entity;
        }

        public void Delete( Anuncio entity)
        {
            repository.Remove(entity);
        }

        public bool Contains(Anuncio item)
        {
            return repository.Contains(item);
        }

        public bool Contains( Anuncio item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }

        public IEnumerable< Anuncio> ListAll()
        {
            return repository.GetAll();
        }

        public IList< Anuncio> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }

        public IList< Anuncio> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate)
        {
            return repository.GetAll(criterias, property, startDate, endDate);
        }

        public int Count()
        {
            return repository.Count<Anuncio>();
        }

    }
}
