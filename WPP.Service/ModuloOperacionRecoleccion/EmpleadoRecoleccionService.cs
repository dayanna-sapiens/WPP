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
    public class EmpleadoRecoleccionService : IEmpleadoRecoleccionService
    {
       private IRepository<EmpleadoRecoleccion> repository;

        public EmpleadoRecoleccionService(IRepository<EmpleadoRecoleccion> _repository)
        {
            repository = _repository;
        }

        public EmpleadoRecoleccion Get(long id)
        {
            return repository.Get(id);
        }

        public EmpleadoRecoleccion Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public EmpleadoRecoleccion Create(EmpleadoRecoleccion entity)
        {
            repository.Add(entity);
            return entity;
        }
        public EmpleadoRecoleccion Update(EmpleadoRecoleccion entity)
        {
            repository.Update(entity);
            return entity;
        }
        public void Delete(EmpleadoRecoleccion entity)
        {
            entity.IsDeleted = true;
            repository.Update(entity);
        }
        public bool Contains(EmpleadoRecoleccion item)
        {
            return repository.Contains(item);
        }
        public bool Contains(EmpleadoRecoleccion item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }
        public IList<EmpleadoRecoleccion> ListAll()
        {
            return repository.GetAll();
        }
        public IList<EmpleadoRecoleccion> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }


        public IPagedList<EmpleadoRecoleccion> PagingSearch(String nombre, String cedula, String codigo, String puesto, int page, int itemsPerPage, String sortOrder)
        {
            var query = repository.GetQuery();

            query = query.Where(u => u.IsDeleted == false);

            if (!String.IsNullOrEmpty(nombre))
                query = query.Where(u => u.Nombre.ToUpper().Contains(nombre.ToUpper()));

            if (!String.IsNullOrEmpty(cedula))
                query = query.Where(u => u.Cedula.ToUpper().Contains(cedula.ToUpper()));

            if (!String.IsNullOrEmpty(codigo))
                query = query.Where(u => u.Codigo.ToUpper().Contains(codigo.ToUpper()));

            if (!String.IsNullOrEmpty(puesto))
                query = query.Where(u => u.Puesto.ToUpper().Contains(puesto.ToUpper()));



            switch (sortOrder)
            {
                case "NombreAsc":
                    return query.OrderBy(u => u.Nombre).ToPagedList(page, itemsPerPage);
                case "NombreDesc":
                    return query.OrderByDescending(u => u.Nombre).ToPagedList(page, itemsPerPage);
                case "CedulaAsc":
                    return query.OrderBy(u => u.Cedula).ToPagedList(page, itemsPerPage);
                case "CedulaDesc":
                    return query.OrderByDescending(u => u.Cedula).ToPagedList(page, itemsPerPage);
                case "CodigoAsc":
                    return query.OrderBy(u => u.Codigo).ToPagedList(page, itemsPerPage);
                case "CodigoDesc":
                    return query.OrderByDescending(u => u.Codigo).ToPagedList(page, itemsPerPage);
                case "PuestoAsc":
                    return query.OrderBy(u => u.Puesto).ToPagedList(page, itemsPerPage);
                case "PuestoDesc":
                    return query.OrderByDescending(u => u.Puesto).ToPagedList(page, itemsPerPage);
                default:
                    return query.OrderBy(u => u.Nombre).ToPagedList(page, itemsPerPage);

            }
            return null;
        }
    }
}
