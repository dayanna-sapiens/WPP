
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;

namespace WPP.Service.ModuloContratos
{
    public interface IUsuarioService 
    {
        Usuario Get(long id);
        Usuario Get(IDictionary<string, object> criterias);
        Usuario Create(Usuario entity);
        Usuario Update(Usuario entity);
        void Delete(Usuario entity);
        bool Contains(Usuario item);
        bool Contains(Usuario item, string property, object value);
        IEnumerable<Usuario> ListAll();
        IList<Usuario> GetAll(IDictionary<string, object> criterias);
        IList<Usuario> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate);

        int Count();
        
        IPagedList<Usuario> PagingSearch(String email, String nombre, String apellido1, String apellido2, String cedula,int page, int itemsPerPage, String sortOrder);
    }
}
