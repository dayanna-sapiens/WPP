using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloBascula;

namespace WPP.Service.ModuloBascula
{
    public interface IEquipoService
    {
        Equipo Get(long id);
        Equipo Get(IDictionary<string, object> criterias);
        Equipo Create(Equipo entity);
        Equipo Update(Equipo entity);
        void Delete(Equipo entity);
        bool Contains(Equipo item);
        bool Contains(Equipo item, string property, object value);
        IList<Equipo> ListAll();
        IList<Equipo> GetAll(IDictionary<string, object> criterias);
       
        int Count();

        IPagedList<Equipo> PagingSearch(String nombre, String placa, String marca, 
                String tipo, int page, int itemsPerPage, String sortOrder,  long companiaId);
        IList<Equipo> EquipoSearch(String equipo);
    }
}
