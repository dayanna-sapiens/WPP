using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Service.ModuloContratos
{
    public interface IProductoService
    {
        Producto Get(long id);
        Producto Get(IDictionary<string, object> criterias);
        Producto Create(Producto entity);
        Producto Update(Producto entity);
        void Delete(Producto entity);
        bool Contains(Producto item);
        bool Contains(Producto item, string property, object value);
        IList<Producto> ListAll();
        IList<Producto> GetAll(IDictionary<string, object> criterias);
        IList<Producto> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate);
        int Count();

        IPagedList<Producto> PagingSearch(String descripcion, String tipo, String estado, String unidad, String equipo, int page, int itemsPerPage, String sortOrder, long compania);
    
    }
}
