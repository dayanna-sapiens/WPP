using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Service.ModuloContratos
{
    public interface IServicioService
    {
        Servicio Get(long id);
        Servicio Get(IDictionary<string, object> criterias);
        Servicio Create(Servicio entity);
        Servicio Update(Servicio entity);
        void Delete(Servicio entity);
        bool Contains(Servicio item);
        bool Contains(Servicio item, string property, object value);
        IList<Servicio> ListAll();
        IList<Servicio> GetAll(IDictionary<string, object> criterias);
       int Count();

        IPagedList<Servicio> PagingSearch(String nombre, long compania, String activo, int page, int itemsPerPage, String sortOrder);
    
    }
}
