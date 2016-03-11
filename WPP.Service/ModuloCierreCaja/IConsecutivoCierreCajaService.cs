using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloCierreCaja;

namespace WPP.Service.ModuloCierreCaja
{
    public interface IConsecutivoCierreCajaService
    {
        ConsecutivoCierreCaja Get(long id);
        ConsecutivoCierreCaja Get(IDictionary<string, object> criterias);
        ConsecutivoCierreCaja Create(ConsecutivoCierreCaja entity);
        ConsecutivoCierreCaja Update(ConsecutivoCierreCaja entity);
        void Delete(ConsecutivoCierreCaja entity);

        IEnumerable<ConsecutivoCierreCaja> GetAll();
        IEnumerable<ConsecutivoCierreCaja> ListAll();
        IList<ConsecutivoCierreCaja> GetAll(IDictionary<string, object> criterias);
        IList<ConsecutivoCierreCaja> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate);

        int Count();
    }
}
