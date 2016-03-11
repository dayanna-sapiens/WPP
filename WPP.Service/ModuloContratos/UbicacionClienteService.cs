using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.ModuloContratos
{
    public class UbicacionClienteService : IUbicacionClienteService
    {
       
        private IRepository<UbicacionCliente> repository;

        public UbicacionClienteService(IRepository<UbicacionCliente> repository)
        {
            this.repository = repository;
        }

        public UbicacionCliente Get(long id)
        {
            return repository.Get(id);
        }

        public UbicacionCliente Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public UbicacionCliente Create(UbicacionCliente entity)
        {
            repository.Add(entity);
            return entity;
        }
        public UbicacionCliente Update(UbicacionCliente entity)
        {
            repository.Update(entity);
            return entity;
        }
        public void Delete(UbicacionCliente entity)
        {
            repository.Remove(entity);
        }

        public IList<UbicacionCliente> ListAll()
        {
            return repository.GetAll();
        }

        public IList<UbicacionCliente> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }

    }
}
