using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Service.ModuloContratos
{
    public interface IContratoBitacoraService
    {
        ContratoBitacora Get(long id);
        ContratoBitacora Get(IDictionary<string, object> criterias);
        ContratoBitacora Create(ContratoBitacora entity);
        ContratoBitacora Update(ContratoBitacora entity);
        void Delete(ContratoBitacora entity);
        bool Contains(ContratoBitacora item);
        bool Contains(ContratoBitacora item, string property, object value);
        IList<ContratoBitacora> ListAll();
        IList<ContratoBitacora> GetAll(IDictionary<string, object> criterias);
        IList<ContratoBitacora> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate);
        int Count();

    }
}
