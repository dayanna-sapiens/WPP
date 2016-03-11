using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;

namespace WPP.Service.Generales
{
    public interface IAnuncioService
    {
        Anuncio Get(long id);
        IList<Anuncio> GetByType(String tipo);
        Anuncio Get(IDictionary<string, object> criterias);
        Anuncio Create(Anuncio entity);
        Anuncio Update(Anuncio entity);
        void Delete(Anuncio entity);
        bool Contains(Anuncio item);
        bool Contains(Anuncio item, string property, object value);
        IEnumerable<Anuncio> ListAll();
        IList<Anuncio> GetAll(IDictionary<string, object> criterias);
        IList<Anuncio> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate);

        int Count();
    }
}
