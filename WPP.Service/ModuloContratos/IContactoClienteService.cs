using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Service.ModuloContratos
{
    public interface IContactoClienteService
    {
        ContactoCliente Get(long id);
        ContactoCliente Create(ContactoCliente entity);
        ContactoCliente Update(ContactoCliente entity);
        void Delete(ContactoCliente entity);
       
    }
}
