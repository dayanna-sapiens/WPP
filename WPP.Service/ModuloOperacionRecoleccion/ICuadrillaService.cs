using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;

namespace WPP.Service.ModuloOperacionRecoleccion
{
    public interface ICuadrillaService
    {
        Cuadrilla Get(long id);
        Cuadrilla Get(IDictionary<string, object> criterias);
        Cuadrilla Create(Cuadrilla entity);
        Cuadrilla Update(Cuadrilla entity);
        void Delete(Cuadrilla entity);
        bool Contains(Cuadrilla item);
        bool Contains(Cuadrilla item, string property, object value);
        IList<Cuadrilla> ListAll();
        IList<Cuadrilla> GetAll(IDictionary<string, object> criterias);
        IPagedList<Cuadrilla> PagingSearch(String descripcion, String chofer, String estado, int page, int itemsPerPage, String sortOrder, long compania);
    }
}
