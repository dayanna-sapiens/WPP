using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloBascula;

namespace WPP.Service.ModuloBascula
{
    public interface IConsecutivoReciboPagoService
    {
        ConsecutivoReciboPago Get(long id);
        ConsecutivoReciboPago Get(IDictionary<string, object> criterias);
        ConsecutivoReciboPago Create(ConsecutivoReciboPago entity);
        ConsecutivoReciboPago Update(ConsecutivoReciboPago entity);
        void Delete(ConsecutivoReciboPago entity);

        IEnumerable<ConsecutivoReciboPago> ListAll();
        IList<ConsecutivoReciboPago> GetAll(IDictionary<string, object> criterias);
        IList<ConsecutivoReciboPago> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate);
        int Count();
    }
}
