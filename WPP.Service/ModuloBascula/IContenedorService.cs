using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloBascula;

namespace WPP.Service.ModuloBascula
{
    public interface IContenedorService
    {
        
        Contenedor Get(long id);
        Contenedor Get(IDictionary<string, object> criterias);
        Contenedor Create(Contenedor entity);
        Contenedor Update(Contenedor entity);
        void Delete(Contenedor entity);
        bool Contains(Contenedor item);
        bool Contains(Contenedor item, string property, object value);
        IList<Contenedor> ListAll();
        IList<Contenedor> GetAll(IDictionary<string, object> criterias);

        int Count();

        IPagedList<Contenedor> PagingSearch(String descripcion, String codigo, String estado, int page, int itemsPerPage, String sortOrder, long companiaId);

        IList<Contenedor> ContenedorSearch(String contenedor);
    }
}
