using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;

namespace WPP.Service.Generales
{
    public interface ICostoCamionService
    {
        CostoCamion Get(long id);
        CostoCamion Get(IDictionary<string, object> criterias);
        CostoCamion Create(CostoCamion entity);
        CostoCamion Update(CostoCamion entity);
        IList<CostoCamion> ListAll();
        IList<CostoCamion> GetAll(IDictionary<string, object> criterias);
        IPagedList<CostoCamion> PagingSearch(String desde, String hasta, int page, int itemsPerPage, String sortOrder);
    }
}
