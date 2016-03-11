using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Service.ModuloContratos
{
    public interface IUbicacionClienteService
    {
        UbicacionCliente Get(long id);
        UbicacionCliente Create(UbicacionCliente entity);
        UbicacionCliente Update(UbicacionCliente entity);
        IList<UbicacionCliente> ListAll();
        IList<UbicacionCliente> GetAll(IDictionary<string, object> criterias);
        void Delete(UbicacionCliente entity);
    }
}
