using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.ModuloContratos
{
    public class ProductoService: IProductoService
    {
        private IRepository<Producto> repository;

        public ProductoService(IRepository<Producto> _repository)
        {
            repository = _repository;
        }

        public Producto Get(long id)
        {
            return repository.Get(id);
        }

        public Producto Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public Producto Create(Producto entity)
        {
            repository.Add(entity);
            return entity;
        }

        public Producto Update(Producto entity)
        {
            repository.Update(entity);
            return entity;
        }

        public void Delete(Producto entity)
        {
            entity.IsDeleted = true;
            repository.Update(entity);
        }

        public bool Contains(Producto item)
        {
            return repository.Contains(item);
        }

        public bool Contains(Producto item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }

        public IList<Producto> ListAll()
        {
            return repository.GetAll();
        }

        public IList<Producto> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }

        public IList<Producto> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate)
        {
            return repository.GetAll(criterias, property, startDate, endDate);
        }

        public int Count()
        {
            return repository.Count<Producto>();
        }

        public IPagedList<Producto> PagingSearch(String descripcion, String tipo, String estado, String unidad, String equipo, int page, int itemsPerPage, String sortOrder, long compania)
        {
            var query = repository.GetQuery();

            query = query.Where(u => u.IsDeleted == false && u.Compania == compania);

            if (!String.IsNullOrEmpty(descripcion))
                query = query.Where(u => u.Descripcion.ToUpper().Contains(descripcion.ToUpper()));

            if (!String.IsNullOrEmpty(estado))
                query = query.Where(u => u.Estado.Nombre.ToUpper().Contains(estado.ToUpper()));

            if (!String.IsNullOrEmpty(unidad))
                query = query.Where(u => u.UnidadCobro.Nombre.ToUpper().Contains(unidad.ToUpper()));

            if (!String.IsNullOrEmpty(tipo))
                query = query.Where(u => u.Categoria.Nombre.ToUpper().Contains(tipo.ToUpper()));

            if (!String.IsNullOrEmpty(equipo))
                query = query.Where(u => u.TipoEquipo.Nombre.ToString().Contains( equipo));

            switch (sortOrder)
            {
                case "DescripcionAsc":
                    return query.OrderBy(u => u.Descripcion).ToPagedList(page, itemsPerPage);
                case "DescripcionDesc":
                    return query.OrderByDescending(u => u.Descripcion).ToPagedList(page, itemsPerPage);
                case "EstadoAsc":
                    return query.OrderBy(u => u.Estado.Nombre).ToPagedList(page, itemsPerPage);
                case "EstadoDesc":
                    return query.OrderByDescending(u => u.Estado.Nombre).ToPagedList(page, itemsPerPage);
                case "UnidadAsc":
                    return query.OrderBy(u => u.UnidadCobro.Nombre).ToPagedList(page, itemsPerPage);
                case "UnidadDesc":
                    return query.OrderByDescending(u => u.UnidadCobro.Nombre).ToPagedList(page, itemsPerPage);
                case "TipoAsc":
                    return query.OrderBy(u => u.Categoria.Nombre).ToPagedList(page, itemsPerPage);
                case "TipoDesc":
                    return query.OrderByDescending(u => u.Categoria.Nombre).ToPagedList(page, itemsPerPage);
                case "EquipoAsc":
                    return query.OrderBy(u => u.TipoEquipo.Nombre).ToPagedList(page, itemsPerPage);
                case "EquipoDesc":
                    return query.OrderByDescending(u => u.TipoEquipo.Nombre).ToPagedList(page, itemsPerPage);
                default:
                    return query.OrderBy(u => u.Descripcion).ToPagedList(page, itemsPerPage);

            }

        }
    }
}
