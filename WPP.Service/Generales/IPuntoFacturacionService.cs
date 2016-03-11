
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;

namespace WPP.Service.ModuloContratos
{
    public interface IPuntoFacturacionService 
    {
        PuntoFacturacion Get(long id);
        PuntoFacturacion Get(IDictionary<string, object> criterias);
        PuntoFacturacion Create(PuntoFacturacion entity);
        PuntoFacturacion Update(PuntoFacturacion entity);
        IList<PuntoFacturacion> ListAll();
        IList<PuntoFacturacion> GetAll(IDictionary<string, object> criterias);
        IPagedList<PuntoFacturacion> PagingSearch(String nombre, int page, int itemsPerPage, String sortOrder);
    }
}
