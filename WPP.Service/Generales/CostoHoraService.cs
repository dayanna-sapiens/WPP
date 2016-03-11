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
    public class CostoHoraService : ICostoHoraService
    {
          private IRepository<CostoHora> repository;

        public CostoHoraService(IRepository<CostoHora> _repository)
        {
            repository = _repository;
        }

        public CostoHora Get(long id)
        {
            return repository.Get(id);
        }

        public CostoHora Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public CostoHora Create(CostoHora entity)
        {
            repository.Add(entity);
            return entity;
        }

        public CostoHora Update(CostoHora entity)
        {
            repository.Update(entity);
            return entity;
        }

        public IList<CostoHora> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }
        public IList<CostoHora> ListAll()
        {
            return repository.GetAll();
        }

        public CostoHora GetByDate(DateTime fecha, string tipo)
        {
            var query = repository.GetQuery();
            query = query.Where(u => u.IsDeleted == false && u.Tipo == tipo);

            query = query.Where(u => u.FechaInicio.AddDays(-1) <= fecha && u.FechaFin.AddDays(1) >= fecha);


            return query.ToList().FirstOrDefault();
        }

        public IPagedList<CostoHora> PagingSearch(String desde, String hasta, int page, int itemsPerPage, String sortOrder)
        {
            var query = repository.GetQuery() ;

            query = query.Where(u => u.IsDeleted == false);

            if (!String.IsNullOrEmpty(desde))
            {
                DateTime fecha = Convert.ToDateTime(desde);
                query = query.Where(u => u.FechaInicio >= fecha.AddDays(-1));
            }
            if (!String.IsNullOrEmpty(hasta))
            {
                DateTime fecha = Convert.ToDateTime(hasta);
                query = query.Where(u => u.FechaFin <= fecha.AddDays(1));
            }


            switch (sortOrder)
            {
                case "FechaInicialAsc":
                    return query.OrderBy(u => u.FechaInicio).ToPagedList(page, itemsPerPage);
                case "FechaInicialDesc":
                    return query.OrderByDescending(u => u.FechaInicio).ToPagedList(page, itemsPerPage);
                case "FechaFinalAsc":
                    return query.OrderBy(u => u.FechaFin).ToPagedList(page, itemsPerPage);
                case "FechaFinalDesc":
                    return query.OrderByDescending(u => u.FechaFin).ToPagedList(page, itemsPerPage);               
                default:
                    return query.OrderByDescending(u => u.FechaFin).ToPagedList(page, itemsPerPage);
                  
            }

        }
    }
}
