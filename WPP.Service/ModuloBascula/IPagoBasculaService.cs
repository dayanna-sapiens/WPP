using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloBascula;

namespace WPP.Service.ModuloBascula
{
    public interface IPagoBasculaService
    {
        PagoBascula Get(long id);
        PagoBascula Get(IDictionary<string, object> criterias);
        PagoBascula Create(PagoBascula entity);
        PagoBascula Update(PagoBascula entity);
        void Delete(PagoBascula entity);
        bool Contains(PagoBascula item);
        bool Contains(PagoBascula item, string property, object value);
        IList<PagoBascula> ListAll();
        IList<PagoBascula> GetAll(IDictionary<string, object> criterias);

        int Count();
    }
}
