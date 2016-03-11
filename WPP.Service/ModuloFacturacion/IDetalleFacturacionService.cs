using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloFacturacion;

namespace WPP.Service.ModuloDetalleFacturacion
{
    public interface IDetalleFacturacionService
    {
        DetalleFacturacion Get(long id);
        DetalleFacturacion Get(IDictionary<string, object> criterias);
        DetalleFacturacion Create(DetalleFacturacion entity);
        DetalleFacturacion Update(DetalleFacturacion entity);
        void Delete(DetalleFacturacion entity);
        bool Contains(DetalleFacturacion item);
        bool Contains(DetalleFacturacion item, string property, object value);
        IList<DetalleFacturacion> ListAll();
        IList<DetalleFacturacion> GetAll(IDictionary<string, object> criterias);
        int Count();

        IPagedList<DetalleFacturacion> PagingSearch(String nombre, long compania, String activo, int page, int itemsPerPage, String sortOrder);
    
    }
}
