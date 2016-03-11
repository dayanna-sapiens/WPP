using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.ModuloContratos
{
    public class CategoriaProductoService: ICategoriaProductoService
    {
         private IRepository<CategoriaProducto> repository;
         public CategoriaProductoService(IRepository<CategoriaProducto> _repository)
        {
            repository = _repository;
        }

         public CategoriaProducto Get(long id)
        {
            return repository.Get(id);
        }

         public CategoriaProducto Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public IList<CategoriaProducto> GetByType(String tipo)
        {
            IDictionary<string, object> crit = new Dictionary<string, object>();
            crit.Add("Tipo", tipo);
            return repository.GetAll(crit);
        }



         public CategoriaProducto Create(CategoriaProducto entity)
        {
            repository.Add(entity);
            return entity;
        }

         public CategoriaProducto Update(CategoriaProducto entity)
        {
            repository.Update(entity);
            return entity;
        }

         public void Delete(CategoriaProducto entity)
        {
            repository.Remove(entity);
        }

         public bool Contains(CategoriaProducto item)
        {
            return repository.Contains(item);
        }

         public bool Contains(CategoriaProducto item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }

         public IList<CategoriaProducto> ListAll()
        {
            return repository.GetAll();
        }

         public IList<CategoriaProducto> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }

         public IList<CategoriaProducto> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate)
        {
            return repository.GetAll(criterias, property, startDate, endDate);
        }

        public int Count()
        {
            return repository.Count<CategoriaProducto>();
        }

        public IPagedList<CategoriaProducto> PagingSearch(String nombre, int page, int itemsPerPage, String sortOrder)
        {
            var query = repository.GetQuery();

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