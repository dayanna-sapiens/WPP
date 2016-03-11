using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.Generales
{
    public class CombustibleService : ICombustibleService
    {
        private IRepository<Combustible> repository;

        public CombustibleService(IRepository<Combustible> _repository)
        {
            repository = _repository;
        }

        public Combustible Get(long id)
        {
            return repository.Get(id);
        }

        public Combustible Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public Combustible Create(Combustible entity)
        {
            repository.Add(entity);
            return entity;
        }

        public Combustible Update(Combustible entity)
        {
            repository.Update(entity);
            return entity;
        }

        public IList<Combustible> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }
        public IList<Combustible> ListAll()
        {
            return repository.GetAll();
        }

        public IPagedList<Combustible> PagingSearch(String desde, String hasta, int page, int itemsPerPage, String sortOrder)
        {
            var query = repository.GetQuery() ;

            query = query.Where(u => u.IsDeleted == false);

            if (!String.IsNullOrEmpty(desde))
            {
                DateTime fecha = Convert.ToDateTime(desde);
                query = query.Where(u => u.FechaDesde >= fecha.AddDays(-1));
            }
            if (!String.IsNullOrEmpty(hasta))
            {
                DateTime fecha = Convert.ToDateTime(hasta);
                query = query.Where(u => u.FechaHasta <= fecha.AddDays(1));
            }


            switch (sortOrder)
            {
                case "FechaInicialAsc":
                    return query.OrderBy(u => u.FechaDesde).ToPagedList(page, itemsPerPage);
                case "FechaInicialDesc":
                    return query.OrderByDescending(u => u.FechaDesde).ToPagedList(page, itemsPerPage);
                case "FechaFinalAsc":
                    return query.OrderBy(u => u.FechaHasta).ToPagedList(page, itemsPerPage);
                case "FechaFinalDesc":
                    return query.OrderByDescending(u => u.FechaHasta).ToPagedList(page, itemsPerPage);               
                default:
                    return query.OrderByDescending(u => u.FechaHasta).ToPagedList(page, itemsPerPage);
                  
            }

        }
    }
}
