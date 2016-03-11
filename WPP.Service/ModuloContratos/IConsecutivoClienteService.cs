using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Service.ModuloContratos
{
    public interface IConsecutivoClienteService
    {
        ConsecutivoCliente Get(long id);
        ConsecutivoCliente Get(IDictionary<string, object> criterias);
        ConsecutivoCliente Create(ConsecutivoCliente entity);
        ConsecutivoCliente Update(ConsecutivoCliente entity);
        void Delete(ConsecutivoCliente entity);

        IEnumerable<ConsecutivoCliente> GetAll();
        IEnumerable<ConsecutivoCliente> ListAll();
        IList<ConsecutivoCliente> GetAll(IDictionary<string, object> criterias);
        IList<ConsecutivoCliente> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate);

        int Count();
    }
}
