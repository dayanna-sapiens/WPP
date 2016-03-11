using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;

namespace WPP.Service.Generales
{
    public interface ITipoCambioService
    {
        TipoCambio Get(long id);
        TipoCambio Get(IDictionary<string, object> criterias);
        TipoCambio Create(TipoCambio entity);
        TipoCambio Update(TipoCambio entity);
        void Delete(TipoCambio entity);
        IList<TipoCambio> GetAll(IDictionary<string, object> criterias);
    }
}
