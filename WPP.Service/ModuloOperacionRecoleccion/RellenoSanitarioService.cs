using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.ModuloOperacionRecoleccion
{
    public class RellenoSanitarioService : IRellenoSanitarioService
    {
         private IRepository<RellenoSanitario> repository;

        public RellenoSanitarioService(IRepository<RellenoSanitario> _repository)
        {
            repository = _repository;
        }

        public RellenoSanitario Get(long id)
        {
            return repository.Get(id);
        }

        public RellenoSanitario Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public RellenoSanitario Create(RellenoSanitario entity)
        {
            repository.Add(entity);
            return entity;
        }
        public RellenoSanitario Update(RellenoSanitario entity)
        {
            repository.Update(entity);
            return entity;
        }
        public void Delete(RellenoSanitario entity)
        {
            entity.IsDeleted = true;
            repository.Update(entity);
        }
        public bool Contains(RellenoSanitario item)
        {
            return repository.Contains(item);
        }
        public bool Contains(RellenoSanitario item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }
        public IList<RellenoSanitario> ListAll()
        {
            return repository.GetAll();
        }
        public IList<RellenoSanitario> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }
       
        public int Count()
        {
            return repository.Count<RellenoSanitario>();
        }

        public IPagedList<RellenoSanitario> PagingSearch(String nombre, String estado, int page, int itemsPerPage, String sortOrder)
        {
            var query = repository.GetQuery();

            query = query.Where(u => u.IsDeleted == false);

            if (!String.IsNullOrEmpty(nombre))
                query = query.Where(u => u.Nombre.ToUpper().Contains(nombre.ToUpper()));

            if (!String.IsNullOrEmpty(estado))
            {
                bool estadoBool = estado == "Activo" ? true : false;
                query = query.Where(u => u.Estado.Equals(estadoBool));
            }


            switch (sortOrder)
            {
                case "NombreAsc":
                    return query.OrderBy(u => u.Nombre).ToPagedList(page, itemsPerPage);
                case "NombreDesc":
                    return query.OrderByDescending(u => u.Nombre).ToPagedList(page, itemsPerPage);
                case "EstadoAsc":
                    return query.OrderBy(u => u.Estado).ToPagedList(page, itemsPerPage);
                case "EstadoDesc":
                    return query.OrderByDescending(u => u.Estado).ToPagedList(page, itemsPerPage);              
                default:
                    return query.OrderBy(u => u.Nombre).ToPagedList(page, itemsPerPage);

            }
        }
    }
}
