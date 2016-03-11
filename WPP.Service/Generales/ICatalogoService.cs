using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;

namespace WPP.Service.Generales
{
    public interface ICatalogoService
    {
        Catalogo Get(long id);
        IList<Catalogo> GetByType(String tipo);
        Catalogo Get(IDictionary<string, object> criterias);
        Catalogo Create(Catalogo entity);
        Catalogo Update(Catalogo entity);
        void Delete(Catalogo entity);
        bool Contains(Catalogo item);
        bool Contains(Catalogo item, string property, object value);
        IEnumerable<Catalogo> ListAll();
        IList<Catalogo> GetAll(IDictionary<string, object> criterias);
        IList<Catalogo> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate);

        int Count();
    }
}
