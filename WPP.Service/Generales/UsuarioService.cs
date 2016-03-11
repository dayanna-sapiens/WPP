
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;
using WPP.Persistance.BaseRepositoryClasses;
using WPP.Service.BaseServiceClasses;
using PagedList;

namespace WPP.Service.ModuloContratos
{
    public class UsuarioService : IUsuarioService
    {
        private IRepository<Usuario> repository;

        public UsuarioService(IRepository<Usuario> _repository)
        {
            repository = _repository;
        }

        public Usuario Get(long id)
        {
            return repository.Get(id);
        }

          public Usuario Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

          public Usuario Create(Usuario entity)
        {
            repository.Add(entity);
            return entity;
        }

          public Usuario Update(Usuario entity)
        {
            repository.Update(entity);
            return entity;
        }

          public void Delete(Usuario entity)
        {
            repository.Remove(entity);
        }

          public bool Contains(Usuario item)
        {
            return repository.Contains(item);
        }

          public bool Contains(Usuario item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }

          public IEnumerable<Usuario> ListAll()
        {
            return repository.GetAll();
        }

          public IList<Usuario> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }

          public IList<Usuario> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate)
        {
            return repository.GetAll(criterias, property, startDate, endDate);
        }

        public int Count()
        {
            return repository.Count<Usuario>();
        }

        public IPagedList<Usuario> PagingSearch(String email, String nombre, String apellido1, String apellido2, String cedula, int page, int itemsPerPage, String sortOrder)
        {
            var query = repository.GetQuery() ;

            query = query.Where(u => u.IsDeleted == false);

            if (!String.IsNullOrEmpty(email))
                query = query.Where(u => u.Email.ToUpper().Contains(email.ToUpper()));

            if (!String.IsNullOrEmpty(nombre))
                query = query.Where(u => u.Nombre.ToUpper().Contains(nombre.ToUpper()));

            if (!String.IsNullOrEmpty(apellido1))
                query = query.Where(u => u.Apellido1.ToUpper().Contains(apellido1.ToUpper()));

            if (!String.IsNullOrEmpty(apellido2))
                query = query.Where(u => u.Apellido2.ToUpper().Contains(apellido2.ToUpper()));

            if (!String.IsNullOrEmpty(cedula))
                query = query.Where(u => u.Cedula.ToUpper().Contains(cedula.ToUpper()));

            switch (sortOrder)
            {
                case "NombreAsc":
                    return query.OrderBy(u => u.Nombre).ToPagedList(page, itemsPerPage);
                case "NombreDesc":
                    return query.OrderByDescending(u => u.Nombre).ToPagedList(page, itemsPerPage);
                case "EmailAsc":
                    return query.OrderBy(u => u.Email).ToPagedList(page, itemsPerPage);
                case "EmailDesc":
                    return query.OrderByDescending(u => u.Email).ToPagedList(page, itemsPerPage);
                case "Apellido1Asc":
                    return query.OrderBy(u => u.Apellido1).ToPagedList(page, itemsPerPage);
                case "Apellido1Desc":
                    return query.OrderByDescending(u => u.Apellido1).ToPagedList(page, itemsPerPage);
                case "Apellido2Asc":
                    return query.OrderBy(u => u.Apellido2).ToPagedList(page, itemsPerPage);
                case "Apellido2Desc":
                    return query.OrderByDescending(u => u.Apellido2).ToPagedList(page, itemsPerPage);
                case "CedulaAsc":
                    return query.OrderBy(u => u.Cedula).ToPagedList(page, itemsPerPage);
                case "CedulaDesc":
                    return query.OrderByDescending(u => u.Cedula).ToPagedList(page, itemsPerPage);
                default:
                    return query.OrderBy(u => u.Nombre).ToPagedList(page, itemsPerPage);
                  
            }

        }
    }
}
