using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloFacturacion;

namespace WPP.Service.ModuloFacturacion
{
    public interface IReversionService
    {
        Reversion Get(long id);
        Reversion Get(IDictionary<string, object> criterias);
        Reversion Create(Reversion entity);
        Reversion Update(Reversion entity);
        void Delete(Reversion entity);
        bool Contains(Reversion item);
        bool Contains(Reversion item, string property, object value);
        IList<Reversion> ListAll();
        IList<Reversion> GetAll(IDictionary<string, object> criterias);
        long ReversionSearch(long factura, long compania);
        
    }
}
