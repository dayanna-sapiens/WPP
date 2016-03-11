using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloNomina;

namespace WPP.Service.ModuloNomina
{
    public interface IConsecutivoNominaService
    {
        ConsecutivoNomina Get(long id);
        ConsecutivoNomina Get(IDictionary<string, object> criterias);
        ConsecutivoNomina Create(ConsecutivoNomina entity);
        ConsecutivoNomina Update(ConsecutivoNomina entity);
        void Delete(ConsecutivoNomina entity);

        IEnumerable<ConsecutivoNomina> ListAll();
        IList<ConsecutivoNomina> GetAll(IDictionary<string, object> criterias);
        IList<ConsecutivoNomina> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate);
        int Count();
    }
}
