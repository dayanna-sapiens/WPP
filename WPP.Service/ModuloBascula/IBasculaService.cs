using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloBascula;

namespace WPP.Service.ModuloBascula
{
    public interface IBasculaService
    {
        Bascula Get(long id);
        Bascula Get(IDictionary<string, object> criterias);
        Bascula Create(Bascula entity);
        Bascula Update(Bascula entity);
        void Delete(Bascula entity);

        IEnumerable<Bascula> ListAll();
        IList<Bascula> GetAll(IDictionary<string, object> criterias);
        IList<Bascula> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate);
        int Count();


        IPagedList<Bascula> PagingSearch(String boleta, String cliente, String equipo, String estado, long compania, int page, int itemsPerPage, String sortOrder);


        IList<Bascula> BasculaSearch(long contrato, DateTime hasta, DateTime desde, long compania);
    }
}
