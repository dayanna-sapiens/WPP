using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Service.ModuloContratos
{
    public interface IClienteService
    {
        Cliente Get(long id);
        Cliente Get(IDictionary<string, object> criterias);
        Cliente Create(Cliente entity);
        Cliente Update(Cliente entity);
        void Delete(Cliente entity);
        bool Contains(Cliente item);
        bool Contains(Cliente item, string property, object value);
        IList<Cliente> ListAll();
        IList<Cliente> GetAll(IDictionary<string, object> criterias);
        IList<Cliente> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate);
        int Count();

        IPagedList<Cliente> PagingSearch(String nombre, String nombreCorto, String grupo, String tipo, String numero, int page, int itemsPerPage, String sortOrder);

        IList<Cliente> ClienteSearch(String cliente);
    }
}
