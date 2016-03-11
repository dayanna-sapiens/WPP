using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;

namespace WPP.Service.ModuloOperacionRecoleccion
{
    public interface IRutaRecoleccionService
    {
        RutaRecoleccion Get(long id);
        RutaRecoleccion Get(IDictionary<string, object> criterias);
        RutaRecoleccion Create(RutaRecoleccion entity);
        RutaRecoleccion Update(RutaRecoleccion entity);
        IList<RutaRecoleccion> ListAll();
        IList<RutaRecoleccion> GetAll(IDictionary<string, object> criterias);
        void Delete(RutaRecoleccion entity);

        IPagedList<RutaRecoleccion> PagingSearch(String descripcion, String activo, String tipo, long compania, int page, int itemsPerPage, String sortOrder);
    }
}
