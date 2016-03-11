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
    public class ServicioService : IServicioService
    {
        private IRepository<Servicio> repository;

        public ServicioService(IRepository<Servicio> _repository)
        {
            repository = _repository;
        }

        public Servicio Get(long id)
        {
            return repository.Get(id);
        }

        public Servicio Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public Servicio Create(Servicio entity)
        {
            repository.Add(entity);
            return entity;
        }

        public Servicio Update(Servicio entity)
        {
            repository.Update(entity);
            return entity;
        }

        public void Delete(Servicio entity)
        {
            entity.IsDeleted = true;
            repository.Update(entity);
        }

        public bool Contains(Servicio item)
        {
            return repository.Contains(item);
        }

        public bool Contains(Servicio item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }

        public IList<Servicio> ListAll()
        {
            return repository.GetAll();
        }

        public IList<Servicio> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }

        public IList<Servicio> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate)
        {
            return repository.GetAll(criterias, property, startDate, endDate);
        }

        public int Count()
        {
            return repository.Count<Servicio>();
        }

        public IPagedList<Servicio> PagingSearch(String nombre, long companiaId, String activo, int page, int itemsPerPage, String sortOrder)
        {
            var query = repository.GetQuery();

            query = query.Where(u => u.IsDeleted == false);

            query = query.Where(u => u.Compania.Id == companiaId);

            if (activo == "Activo")
                query = query.Where(u => u.Activo == true);
            else if(activo == "Inactivo")
                query = query.Where(u => u.Activo == false);
            
                

            if (!String.IsNullOrEmpty(nombre))
                query = query.Where(u => u.Nombre.ToString().Contains(nombre));

          


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
