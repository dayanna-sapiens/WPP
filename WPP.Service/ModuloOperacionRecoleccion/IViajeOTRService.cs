using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;

namespace WPP.Service.ModuloOperacionRecoleccion
{
    public interface IViajeOTRService
    {
        ViajeOTR Get(long id);
        ViajeOTR Get(IDictionary<string, object> criterias);
        ViajeOTR Create(ViajeOTR entity);
        ViajeOTR Update(ViajeOTR entity);
        void Delete(ViajeOTR entity);
        bool Contains(ViajeOTR item);
        bool Contains(ViajeOTR item, string property, object value);
        IList<ViajeOTR> ListAll();
        IList<ViajeOTR> GetAll(IDictionary<string, object> criterias);
        IList<ViajeOTR> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate);
     
    }
}
