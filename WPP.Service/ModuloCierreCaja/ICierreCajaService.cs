using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloCierreCaja;

namespace WPP.Service.ModuloCierreCaja
{
    public interface ICierreCajaService
    {
        CierreCaja Get(long id);
        CierreCaja Get(IDictionary<string, object> criterias);
        CierreCaja Create(CierreCaja entity);
        CierreCaja Update(CierreCaja entity);
        void Delete(CierreCaja entity);
        bool Contains(CierreCaja item);
        bool Contains(CierreCaja item, string property, object value);
        IList<CierreCaja> ListAll();
        IList<CierreCaja> GetAll(IDictionary<string, object> criterias);
        IList<CierreCaja> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate);
        int Count();

    }
}
