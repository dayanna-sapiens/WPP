using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloFacturacion;

namespace WPP.Service.ModuloFacturacion
{
    public interface IConsecutivoFacturacionService
    {
        ConsecutivoFacturacion Get(long id);
        ConsecutivoFacturacion Get(IDictionary<string, object> criterias);
        ConsecutivoFacturacion Create(ConsecutivoFacturacion entity);
        ConsecutivoFacturacion Update(ConsecutivoFacturacion entity);
        void Delete(ConsecutivoFacturacion entity);
        IEnumerable<ConsecutivoFacturacion> ListAll();
        IList<ConsecutivoFacturacion> GetAll(IDictionary<string, object> criterias);
        IList<ConsecutivoFacturacion> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate);
        int Count();
    }
}
