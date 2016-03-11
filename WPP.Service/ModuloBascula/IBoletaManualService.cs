using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloBascula;

namespace WPP.Service.ModuloBoletaManual
{
    public interface IBoletaManualService
    {
        BoletaManual Get(long id);
        BoletaManual Get(IDictionary<string, object> criterias);
        BoletaManual Create(BoletaManual entity);
        BoletaManual Update(BoletaManual entity);
        void Delete(BoletaManual entity);

        IEnumerable<BoletaManual> ListAll();
        IList<BoletaManual> GetAll(IDictionary<string, object> criterias);
        IList<BoletaManual> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate);
        int Count();


        IPagedList<BoletaManual> PagingSearch(String boleta, String otr, String equipo, String estado, long compania, int page, int itemsPerPage, String sortOrder);


    }
}
