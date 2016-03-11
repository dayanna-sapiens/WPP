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
    public class CuadrillaService : ICuadrillaService
    {
         private IRepository<Cuadrilla> repository;

        public CuadrillaService(IRepository<Cuadrilla> _repository)
        {
            repository = _repository;
        }

        public Cuadrilla Get(long id)
        {
            return repository.Get(id);
        }

        public Cuadrilla Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public Cuadrilla Create(Cuadrilla entity)
        {
            repository.Add(entity);
            return entity;
        }
        public Cuadrilla Update(Cuadrilla entity)
        {
            repository.Update(entity);
            return entity;
        }
        public void Delete(Cuadrilla entity)
        {
            entity.IsDeleted = true;
            repository.Update(entity);
        }
        public bool Contains(Cuadrilla item)
        {
            return repository.Contains(item);
        }
        public bool Contains(Cuadrilla item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }
        public IList<Cuadrilla> ListAll()
        {
            return repository.GetAll();
        }
        public IList<Cuadrilla> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }

        public IPagedList<Cuadrilla> PagingSearch(String descripcion, String chofer, String estado, int page, int itemsPerPage, String sortOrder, long compania)
        {
            var query = repository.GetQuery();

             query = query.Where(u => u.IsDeleted == false && u.Compania.Id == compania);

             if (!String.IsNullOrEmpty(descripcion))
                 query = query.Where(u => u.Descripcion.ToUpper().Contains(descripcion.ToUpper()));

            if (!String.IsNullOrEmpty(estado))
                query = query.Where(u => u.Estado.ToUpper().Contains(estado.ToUpper()));

            //if (!String.IsNullOrEmpty(chofer))
            //{
            //    query = query.Where(u => u.ListaEmpleados.Where(s => s.Puesto == "Chofer"));

            //}


            switch (sortOrder)
            {
                case "DescripcionAsc":
                    return query.OrderBy(u => u.Descripcion).ToPagedList(page, itemsPerPage);
                case "DescripcionDesc":
                    return query.OrderByDescending(u => u.Descripcion).ToPagedList(page, itemsPerPage);
                case "EstadoAsc":
                    return query.OrderBy(u => u.Estado).ToPagedList(page, itemsPerPage);
                case "EstadoDesc":
                    return query.OrderByDescending(u => u.Estado).ToPagedList(page, itemsPerPage);
                //case "ChoferAsc":
                //    return query.OrderBy(u => u.Ruta.Contrato.Cliente.Nombre).ToPagedList(page, itemsPerPage);
                //case "ChoferDesc":
                //    return query.OrderByDescending(u => u.Ruta.Contrato.Cliente.Nombre).ToPagedList(page, itemsPerPage);                
                default:
                    return query.OrderBy(u => u.Descripcion).ToPagedList(page, itemsPerPage);

            }

        }
      
    }
}
