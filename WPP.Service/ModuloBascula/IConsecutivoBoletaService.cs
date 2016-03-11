using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloBascula;

namespace WPP.Service.ModuloBascula
{
    public interface IConsecutivoBoletaService
    {
        ConsecutivoBoleta Get(long id);
        ConsecutivoBoleta Get(IDictionary<string, object> criterias);
        ConsecutivoBoleta Create(ConsecutivoBoleta entity);
        ConsecutivoBoleta Update(ConsecutivoBoleta entity);
        void Delete(ConsecutivoBoleta entity);

        IEnumerable<ConsecutivoBoleta> ListAll();
        IList<ConsecutivoBoleta> GetAll(IDictionary<string, object> criterias);
        IList<ConsecutivoBoleta> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate);
        int Count();
    }
}
