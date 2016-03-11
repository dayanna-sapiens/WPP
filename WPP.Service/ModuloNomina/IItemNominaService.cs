using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloNomina;

namespace WPP.Service.ModuloNomina
{
    public interface IItemNominaService
    {
        ItemNomina Get(long id);
        ItemNomina Get(IDictionary<string, object> criterias);
        ItemNomina Create(ItemNomina entity);
        ItemNomina Update(ItemNomina entity);
        void Delete(ItemNomina entity);

        IEnumerable<ItemNomina> ListAll();
        IList<ItemNomina> GetAll(IDictionary<string, object> criterias);
        IList<ItemNomina> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate);
    }
}
