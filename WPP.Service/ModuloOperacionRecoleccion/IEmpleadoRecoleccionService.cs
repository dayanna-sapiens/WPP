using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;

namespace WPP.Service.ModuloOperacionRecoleccion
{
    public interface IEmpleadoRecoleccionService
    {
        EmpleadoRecoleccion Get(long id);
        EmpleadoRecoleccion Get(IDictionary<string, object> criterias);
        EmpleadoRecoleccion Create(EmpleadoRecoleccion entity);
        EmpleadoRecoleccion Update(EmpleadoRecoleccion entity);
        void Delete(EmpleadoRecoleccion entity);
        bool Contains(EmpleadoRecoleccion item);
        bool Contains(EmpleadoRecoleccion item, string property, object value);
        IList<EmpleadoRecoleccion> ListAll();
        IList<EmpleadoRecoleccion> GetAll(IDictionary<string, object> criterias);
        IPagedList<EmpleadoRecoleccion> PagingSearch(String nombre, String cedula, String codigo, String puesto, int page, int itemsPerPage, String sortOrder);
    }
}
