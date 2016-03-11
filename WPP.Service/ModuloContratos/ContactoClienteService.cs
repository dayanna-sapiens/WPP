using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.ModuloContratos
{
    public class ContactoClienteService : IContactoClienteService
    {
        private IRepository<ContactoCliente> repository;

        public ContactoClienteService(IRepository<ContactoCliente> repository)
        {
            this.repository = repository;
        }

        public ContactoCliente Get(long id)
        {
            return repository.Get(id);
        }
        public ContactoCliente Create(ContactoCliente entity)
        {
            repository.Add(entity);
            return entity;
        }
        public ContactoCliente Update(ContactoCliente entity)
        {
            repository.Update(entity);
            return entity;
        }
        public void Delete(ContactoCliente entity)
        {
            repository.Remove(entity);
        }

    }
}
