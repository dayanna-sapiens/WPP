using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;

namespace WPP.Service.ModuloOperacionRecoleccion
{
    public interface IOTRService
    {
        OTR Get(long id);
        OTR Get(IDictionary<string, object> criterias);
        OTR Create(OTR entity);
        OTR Update(OTR entity);
        void Delete(OTR entity);
        bool Contains(OTR item);
        bool Contains(OTR item, string property, object value);
        IList<OTR> ListAll();
        IList<OTR> GetAll(IDictionary<string, object> criterias);
        IList<OTR> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate);

        IPagedList<OTR> PagingSearch(String otr, String equipo, String estado, String rutaRecoleccion, String tipo, int page, int itemsPerPage, String sortOrder, long compania);

        IList<OTR> OTRSearch(long contrato, DateTime hasta, DateTime desde, long compania);

    }
}
