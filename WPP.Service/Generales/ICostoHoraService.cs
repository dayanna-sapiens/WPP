using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;

namespace WPP.Service.Generales
{
    public interface ICostoHoraService
    {
        CostoHora Get(long id);
        CostoHora Get(IDictionary<string, object> criterias);
        CostoHora Create(CostoHora entity);
        CostoHora Update(CostoHora entity);
        IList<CostoHora> ListAll();
        IList<CostoHora> GetAll(IDictionary<string, object> criterias);
        CostoHora GetByDate(DateTime fecha, string tipo);
        IPagedList<CostoHora> PagingSearch(String desde, String hasta, int page, int itemsPerPage, String sortOrder);
    }
}
