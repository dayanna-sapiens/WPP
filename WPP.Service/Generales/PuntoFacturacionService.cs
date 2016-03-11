
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
    public class PuntoFacturacionService : IPuntoFacturacionService
    {
        private IRepository<PuntoFacturacion> repository;

        public PuntoFacturacionService(IRepository<PuntoFacturacion> _repository)
        {
            repository = _repository;
        }

        public PuntoFacturacion Get(long id)
        {
            return repository.Get(id);
        }

        public PuntoFacturacion Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public PuntoFacturacion Create(PuntoFacturacion entity)
        {
            repository.Add(entity);
            return entity;
        }

        public PuntoFacturacion Update(PuntoFacturacion entity)
        {
            repository.Update(entity);
            return entity;
        }

        public IList<PuntoFacturacion> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }
        public IList<PuntoFacturacion> ListAll()
        {
            return repository.GetAll();
        }

        public IPagedList<PuntoFacturacion> PagingSearch(String nombre, int page, int itemsPerPage, String sortOrder)
        {
            var query = repository.GetQuery() ;

            query = query.Where(u => u.IsDeleted == false);

            if (!String.IsNullOrEmpty(nombre))
                query = query.Where(u => u.Nombre.ToUpper().Contains(nombre.ToUpper()));


            switch (sortOrder)
            {
                case "NombreAsc":
                    return query.OrderBy(u => u.Nombre).ToPagedList(page, itemsPerPage);
                case "NombreDesc":
                    return query.OrderByDescending(u => u.Nombre).ToPagedList(page, itemsPerPage);
              
               
                default:
                    return query.OrderBy(u => u.Nombre).ToPagedList(page, itemsPerPage);
                  
            }

        }
    }
}
