using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloFacturacion;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.ModuloDetalleFacturacion
{
    public class DetalleFacturacionService : IDetalleFacturacionService
    {
         private IRepository<DetalleFacturacion> repository;

        public DetalleFacturacionService(IRepository<DetalleFacturacion> _repository)
        {
            repository = _repository;
        }

        public DetalleFacturacion Get(long id)
        {
            return repository.Get(id);
        }

        public DetalleFacturacion Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public DetalleFacturacion Create(DetalleFacturacion entity)
        {
            repository.Add(entity);
            return entity;
        }

        public DetalleFacturacion Update(DetalleFacturacion entity)
        {
            repository.Update(entity);
            return entity;
        }

        public void Delete(DetalleFacturacion entity)
        {
            entity.IsDeleted = true;
            repository.Update(entity);
        }

        public bool Contains(DetalleFacturacion item)
        {
            return repository.Contains(item);
        }

        public bool Contains(DetalleFacturacion item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }

        public IList<DetalleFacturacion> ListAll()
        {
            return repository.GetAll();
        }

        public IList<DetalleFacturacion> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }

        public IList<DetalleFacturacion> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate)
        {
            return repository.GetAll(criterias, property, startDate, endDate);
        }

        public int Count()
        {
            return repository.Count<DetalleFacturacion>();
        }

        public IPagedList<DetalleFacturacion> PagingSearch(String nombre, long companiaId, String activo, int page, int itemsPerPage, String sortOrder)
        {
            //var query = repository.GetQuery();

            //query = query.Where(u => u.IsDeleted == false);

            //query = query.Where(u => u.Compania.Id == companiaId);

            //if (activo == "Activo")
            //    query = query.Where(u => u.Activo == true);
            //else if(activo == "Inactivo")
            //    query = query.Where(u => u.Activo == false);
            
                

            //if (!String.IsNullOrEmpty(nombre))
            //    query = query.Where(u => u.Nombre.ToString().Contains(nombre));


            //switch (sortOrder)
            //{
            //    case "NombreAsc":
            //        return query.OrderBy(u => u.Nombre).ToPagedList(page, itemsPerPage);
            //    case "NombreDesc":
            //        return query.OrderByDescending(u => u.Nombre).ToPagedList(page, itemsPerPage);
            //     default:
            //        return query.OrderBy(u => u.Nombre).ToPagedList(page, itemsPerPage);

            //}
            return null;
        }
    }
}
