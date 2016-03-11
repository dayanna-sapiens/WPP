using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;

namespace WPP.Service.ModuloOperacionRecoleccion
{
    public interface IRellenoSanitarioService
    {
        RellenoSanitario Get(long id);
        RellenoSanitario Get(IDictionary<string, object> criterias);
        RellenoSanitario Create(RellenoSanitario entity);
        RellenoSanitario Update(RellenoSanitario entity);
        void Delete(RellenoSanitario entity);
        bool Contains(RellenoSanitario item);
        bool Contains(RellenoSanitario item, string property, object value);
        IList<RellenoSanitario> ListAll();
        IList<RellenoSanitario> GetAll(IDictionary<string, object> criterias);

        int Count();

        IPagedList<RellenoSanitario> PagingSearch(String nombre, String estado, int page, int itemsPerPage, String sortOrder);
    
    }
}
