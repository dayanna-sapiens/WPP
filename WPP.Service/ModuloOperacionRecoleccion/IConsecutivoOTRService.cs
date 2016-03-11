using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;

namespace WPP.Service.ModuloOperacionRecoleccion
{
    public interface IConsecutivoOTRService
    {
        ConsecutivoOTR Get(long id);
        ConsecutivoOTR Get(IDictionary<string, object> criterias);
        ConsecutivoOTR Create(ConsecutivoOTR entity);
        ConsecutivoOTR Update(ConsecutivoOTR entity);
        void Delete(ConsecutivoOTR entity);

        IEnumerable<ConsecutivoOTR> ListAll();
        IList<ConsecutivoOTR> GetAll(IDictionary<string, object> criterias);
        IList<ConsecutivoOTR> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate);
        int Count();
    }
}
