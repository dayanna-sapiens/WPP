using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloNomina;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.ModuloNomina
{
    public class DiasFestivosService : IDiasFestivosService
    {
          private IRepository<DiasFestivos> repository;

        public DiasFestivosService(IRepository<DiasFestivos> _repository)
        {
            repository = _repository;
        }

        public DiasFestivos Get(long id)
        {
            return repository.Get(id);
        }

        public DiasFestivos Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public DiasFestivos Create(DiasFestivos entity)
        {
            repository.Add(entity);
            return entity;
        }

        public DiasFestivos Update(DiasFestivos entity)
        {
            repository.Update(entity);
            return entity;
        }

        public void Delete(DiasFestivos entity)
        {
            repository.Update(entity);
        }

        public bool Contains(DiasFestivos item)
        {
            return repository.Contains(item);
        }

        public bool Contains(DiasFestivos item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }

        public IEnumerable<DiasFestivos> ListAll()
        {
            return repository.GetAll();
        }

        public IList<DiasFestivos> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }

        public IList<DiasFestivos> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate)
        {
            return repository.GetAll(criterias, property, startDate, endDate);
        }

        public int Count()
        {
            return repository.Count<DiasFestivos>();
        }

        public IPagedList<DiasFestivos> PagingSearch(string Dia, string Mes, string Descripcion, int page, int itemsPerPage, String sortOrder)
        {
            var query = repository.GetQuery();

            query = query.Where(u => u.IsDeleted == false);

            if (!String.IsNullOrEmpty(Descripcion))
                query = query.Where(u => u.Descripcion.ToString().Contains(Descripcion));

            if (!String.IsNullOrEmpty(Dia))
                query = query.Where(u => u.Dia == Convert.ToInt32(Dia));

            if (!String.IsNullOrEmpty(Mes))
                query = query.Where(u => u.Mes == Convert.ToInt32(Mes));

            switch (sortOrder)
            {
                case "DescripcionAsc":
                    return query.OrderBy(u => u.Descripcion).ToPagedList(page, itemsPerPage);
                case "DescripcionDesc":
                    return query.OrderByDescending(u => u.Descripcion).ToPagedList(page, itemsPerPage);
                case "DiaAsc":
                    return query.OrderBy(u => u.Dia).ToPagedList(page, itemsPerPage);
                case "DiaDesc":
                    return query.OrderByDescending(u => u.Dia).ToPagedList(page, itemsPerPage);
                case "MesAsc":
                    return query.OrderBy(u => u.Mes).ToPagedList(page, itemsPerPage);
                case "MesDesc":
                    return query.OrderByDescending(u => u.Mes).ToPagedList(page, itemsPerPage);
                default:
                    return query.OrderBy(u => u.Descripcion).ToPagedList(page, itemsPerPage);

            }
            return null;
        }
    }
}
