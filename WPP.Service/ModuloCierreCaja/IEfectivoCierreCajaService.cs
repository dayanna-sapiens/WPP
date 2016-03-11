using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloCierreCaja;

namespace WPP.Service.ModuloCierreCaja
{
    public interface IEfectivoCierreCajaService
    {
        EfectivoCierreCaja Get(long id);
        EfectivoCierreCaja Get(IDictionary<string, object> criterias);
        EfectivoCierreCaja Create(EfectivoCierreCaja entity);
        EfectivoCierreCaja Update(EfectivoCierreCaja entity);
        void Delete(EfectivoCierreCaja entity);
        bool Contains(EfectivoCierreCaja item);
        bool Contains(EfectivoCierreCaja item, string property, object value);
        IList<EfectivoCierreCaja> ListAll();
        IList<EfectivoCierreCaja> GetAll(IDictionary<string, object> criterias);
        IList<EfectivoCierreCaja> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate);
        int Count();
    }
}
