using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloNomina;

namespace WPP.Service.ModuloNomina
{
    public interface IDiasFestivosService
    {
        DiasFestivos Get(long id);
        DiasFestivos Get(IDictionary<string, object> criterias);
        DiasFestivos Create(DiasFestivos entity);
        DiasFestivos Update(DiasFestivos entity);
        void Delete(DiasFestivos entity);

        IEnumerable<DiasFestivos> ListAll();
        IList<DiasFestivos> GetAll(IDictionary<string, object> criterias);
        IList<DiasFestivos> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate);
        int Count();
        IPagedList<DiasFestivos> PagingSearch(string Dia, string Mes, string Descripcion, int page, int itemsPerPage, String sortOrder);
    }
}
