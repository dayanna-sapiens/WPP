using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;

namespace WPP.Service.ModuloContratos
{
    public interface ICategoriaProductoService
    {
        CategoriaProducto Get(long id);
        IList<CategoriaProducto> GetByType(String tipo);
        CategoriaProducto Get(IDictionary<string, object> criterias);
        CategoriaProducto Create(CategoriaProducto entity);
        CategoriaProducto Update(CategoriaProducto entity);
        void Delete(CategoriaProducto entity);
        bool Contains(CategoriaProducto item);
        bool Contains(CategoriaProducto item, string property, object value);
        IList<CategoriaProducto> ListAll();
        IList<CategoriaProducto> GetAll(IDictionary<string, object> criterias);
        IList<CategoriaProducto> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate);
        IPagedList<CategoriaProducto> PagingSearch(String nombre, int page, int itemsPerPage, String sortOrder);

        int Count();
    }
}
