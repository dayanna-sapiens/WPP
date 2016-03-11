using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;

namespace WPP.Service.ModuloContratos
{
    public interface IClienteBitacoraService
    {
        ClienteBitacora Get(long id);
        ClienteBitacora Get(IDictionary<string, object> criterias);
        ClienteBitacora Create(ClienteBitacora entity);
        ClienteBitacora Update(ClienteBitacora entity);
        void Delete(ClienteBitacora entity);
        bool Contains(ClienteBitacora item);
        bool Contains(ClienteBitacora item, string property, object value);
        IList<ClienteBitacora> ListAll();
        IList<ClienteBitacora> GetAll(IDictionary<string, object> criterias);
        IList<ClienteBitacora> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate);
        int Count();

    }
}
