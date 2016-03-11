using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.ModuloOperacionRecoleccion
{
    public class RutaRecoleccionService : IRutaRecoleccionService
    {
       
        private IRepository<RutaRecoleccion> repository;

        public RutaRecoleccionService(IRepository<RutaRecoleccion> repository)
        {
            this.repository = repository;
        }

        public RutaRecoleccion Get(long id)
        {
            return repository.Get(id);
        }

        public RutaRecoleccion Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public RutaRecoleccion Create(RutaRecoleccion entity)
        {
            repository.Add(entity);
            return entity;
        }
        public RutaRecoleccion Update(RutaRecoleccion entity)
        {
            repository.Update(entity);
            return entity;
        }
        public void Delete(RutaRecoleccion entity)
        {
            repository.Remove(entity);
        }

        public IList<RutaRecoleccion> ListAll()
        {
            return repository.GetAll();
        }

        public IList<RutaRecoleccion> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }

        public IPagedList<RutaRecoleccion> PagingSearch(String descripcion, String activo, String tipo, long compania, int page, int itemsPerPage, String sortOrder)
        {
            var query = repository.GetQuery();

            query = query.Where(u => u.IsDeleted == false);

            query = query.Where(u => u.Compania.Id == compania);

           if (!String.IsNullOrEmpty(activo))
                query = query.Where(u => u.Estado.ToUpper().Contains(activo.ToUpper()));

            if (!String.IsNullOrEmpty(descripcion))
                query = query.Where(u => u.Descripcion.ToUpper().Contains(descripcion.ToUpper()));

            if (!String.IsNullOrEmpty(tipo))
                query = query.Where(u => u.Tipo.ToUpper().Contains(tipo.ToUpper()));

            switch (sortOrder)
            {
                case "DescripcionAsc":
                    return query.OrderBy(u => u.Descripcion).ToPagedList(page, itemsPerPage);
                case "DescripcionDesc":
                    return query.OrderByDescending(u => u.Descripcion).ToPagedList(page, itemsPerPage);
                case "ActivoAsc":
                    return query.OrderBy(u => u.Estado).ToPagedList(page, itemsPerPage);
                case "ActivoDesc":
                    return query.OrderByDescending(u => u.Estado).ToPagedList(page, itemsPerPage);
                case "TipoAsc":
                    return query.OrderBy(u => u.Tipo).ToPagedList(page, itemsPerPage);
                case "TipoDesc":
                    return query.OrderByDescending(u => u.Tipo).ToPagedList(page, itemsPerPage);
                default:
                    return query.OrderBy(u => u.Descripcion).ToPagedList(page, itemsPerPage);

            }
        }
    }
}
