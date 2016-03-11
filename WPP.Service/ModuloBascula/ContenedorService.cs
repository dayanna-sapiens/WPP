using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloBascula;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.ModuloBascula
{
    public class ContenedorService : IContenedorService
    {
        private IRepository<Contenedor> repository;

        public ContenedorService(IRepository<Contenedor> _repository)
        {
            repository = _repository;
        }

        public Contenedor Get(long id)
        {
            return repository.Get(id);
        }

        public Contenedor Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public Contenedor Create(Contenedor entity)
        {
            repository.Add(entity);
            return entity;
        }
        public Contenedor Update(Contenedor entity)
        {
            repository.Update(entity);
            return entity;
        }
        public void Delete(Contenedor entity)
        {
            entity.IsDeleted = true;
            repository.Update(entity);
        }
        public bool Contains(Contenedor item)
        {
            return repository.Contains(item);
        }
        public bool Contains(Contenedor item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }
        public IList<Contenedor> ListAll()
        {
            return repository.GetAll();
        }
        public IList<Contenedor> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }
       
        public int Count()
        {
            return repository.Count<Contenedor>();
        }
        

        public IList<Contenedor> ContenedorSearch(String contenedor)
        {
            var query = repository.GetQuery();

            query = query.Where(u => u.IsDeleted == false);

            if (!String.IsNullOrEmpty(contenedor))
            {
                query = query.Where(u => u.Codigo.ToUpper().Contains(contenedor.ToUpper()));
            }

            return query.ToList<Contenedor>();
        }
        
        public IPagedList<Contenedor> PagingSearch(String descripcion, String codigo, String estado, int page, int itemsPerPage, String sortOrder, long companiaId)
        {
            var query = repository.GetQuery();

            query = query.Where(u => u.IsDeleted == false);

            if (!String.IsNullOrEmpty(descripcion))
                query = query.Where(u => u.Descripcion.ToUpper().Contains(descripcion.ToUpper()));

            if (!String.IsNullOrEmpty(codigo))
                query = query.Where(u => u.Codigo.ToUpper().Contains(codigo.ToUpper()));
            
            if (!String.IsNullOrEmpty(estado))
                query = query.Where(u => u.Estado.ToUpper().Contains(estado.ToUpper()));

           
            switch (sortOrder)
            {
                case "DescripcionAsc":
                    return query.OrderBy(u => u.Descripcion).ToPagedList(page, itemsPerPage);
                case "DescripcionDesc":
                    return query.OrderByDescending(u => u.Descripcion).ToPagedList(page, itemsPerPage);
                case "CodigoAsc":
                    return query.OrderBy(u => u.Codigo).ToPagedList(page, itemsPerPage);
                case "CodigoDesc":
                    return query.OrderByDescending(u => u.Codigo).ToPagedList(page, itemsPerPage);
                case "EstadoAsc":
                    return query.OrderBy(u => u.Estado).ToPagedList(page, itemsPerPage);
                case "EstadoDesc":
                    return query.OrderByDescending(u => u.Estado).ToPagedList(page, itemsPerPage);    
                default:
                    return query.OrderBy(u => u.Descripcion).ToPagedList(page, itemsPerPage);

            }
        }
    }
}
