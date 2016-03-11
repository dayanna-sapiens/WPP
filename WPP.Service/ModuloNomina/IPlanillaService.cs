using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloNomina;

namespace WPP.Service.ModuloNomina
{
    public interface IPlanillaService
    {
        Planilla Get(long id);
        Planilla Get(IDictionary<string, object> criterias);
        Planilla Create(Planilla entity);
        Planilla Update(Planilla entity);
        void Delete(Planilla entity);
        bool Contains(Planilla item);
        bool Contains(Planilla item, string property, object value);
        IList<Planilla> ListAll();
        IList<Planilla> GetAll(IDictionary<string, object> criterias);
        int Count();

        //IPagedList<Planialla> PagingSearch(long consecutivo, string fechaInicio, string fechaFin, String estado, long compania, int page, int itemsPerPage, String sortOrder);
 
    }
}
