using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloNomina;

namespace WPP.Service.ModuloNomina
{
    public interface IJornadaService
    {
        Jornada Get(long id);
        Jornada Get(IDictionary<string, object> criterias);
        Jornada Create(Jornada entity);
        Jornada Update(Jornada entity);
        IList<Jornada> ListAll();
        IList<Jornada> GetAll(IDictionary<string, object> criterias);
        IPagedList<Jornada> PagingSearch(String descripcion, String tipo, int page, int itemsPerPage, String sortOrder);
    }
}
