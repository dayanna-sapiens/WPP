using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;

namespace WPP.Service.Generales
{
    public interface ICombustibleService
    {
        Combustible Get(long id);
        Combustible Get(IDictionary<string, object> criterias);
        Combustible Create(Combustible entity);
        Combustible Update(Combustible entity);
        IList<Combustible> ListAll();
        IList<Combustible> GetAll(IDictionary<string, object> criterias);
        IPagedList<Combustible> PagingSearch(String desde, String hasta, int page, int itemsPerPage, String sortOrder);
    }
}
