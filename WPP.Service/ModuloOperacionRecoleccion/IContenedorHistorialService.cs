using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;

namespace WPP.Service.ModuloOperacionRecoleccion
{
    public interface IContenedorHistorialService
    {
        ContenedorHistorial Get(long id);
        ContenedorHistorial Get(IDictionary<string, object> criterias);
        ContenedorHistorial Create(ContenedorHistorial entity);
        ContenedorHistorial Update(ContenedorHistorial entity);
        void Delete(ContenedorHistorial entity);
        bool Contains(ContenedorHistorial item);
        bool Contains(ContenedorHistorial item, string property, object value);
        IList<ContenedorHistorial> ListAll();
        IList<ContenedorHistorial> GetAll(IDictionary<string, object> criterias);

        int Count();

        IPagedList<ContenedorHistorial> PagingSearch(String nombre, String estado, int page, int itemsPerPage, String sortOrder);
    
    }
}
