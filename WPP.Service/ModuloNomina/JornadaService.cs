using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloNomina;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.ModuloNomina
{
    public class JornadaService : IJornadaService
    {
         private IRepository<Jornada> repository;

        public JornadaService(IRepository<Jornada> _repository)
        {
            repository = _repository;
        }

        public Jornada Get(long id)
        {
            return repository.Get(id);
        }

        public Jornada Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public Jornada Create(Jornada entity)
        {
            repository.Add(entity);
            return entity;
        }

        public Jornada Update(Jornada entity)
        {
            repository.Update(entity);
            return entity;
        }

        public IList<Jornada> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }
        public IList<Jornada> ListAll()
        {
            return repository.GetAll();
        }


        public IPagedList<Jornada> PagingSearch(String descripcion, String tipo, int page, int itemsPerPage, String sortOrder)
        {
            var query = repository.GetQuery() ;

            query = query.Where(u => u.IsDeleted == false);

            if (!String.IsNullOrEmpty(descripcion))
            {
                query = query.Where(u => u.Descripcion.ToUpper().Contains(descripcion.ToUpper()));
            }
            if (!String.IsNullOrEmpty(tipo))
            {
                query = query.Where(u => u.Tipo.ToUpper().Contains(tipo.ToUpper()));
            }


            switch (sortOrder)
            {
                case "DescripcionAsc":
                    return query.OrderBy(u => u.Descripcion).ToPagedList(page, itemsPerPage);
                case "DescripcionDesc":
                    return query.OrderByDescending(u => u.Descripcion).ToPagedList(page, itemsPerPage);
                case "TipoAsc":
                    return query.OrderBy(u => u.Tipo).ToPagedList(page, itemsPerPage);
                case "TipoDesc":
                    return query.OrderByDescending(u => u.Tipo).ToPagedList(page, itemsPerPage);               
                default:
                    return query.OrderByDescending(u => u.Descripcion).ToPagedList(page, itemsPerPage);
                  
            }

        }
    }
}
