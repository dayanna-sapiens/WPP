using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Service.ModuloContratos
{
    public interface IConsecutivoContratoService
    {
        ConsecutivoContrato Get(long id);
        ConsecutivoContrato Get(IDictionary<string, object> criterias);
        ConsecutivoContrato Create(ConsecutivoContrato entity);
        ConsecutivoContrato Update(ConsecutivoContrato entity);
        void Delete(ConsecutivoContrato entity);
        IEnumerable<ConsecutivoContrato> ListAll();
        IList<ConsecutivoContrato> GetAll(IDictionary<string, object> criterias);
        IList<ConsecutivoContrato> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate);
        int Count();
    }
}
