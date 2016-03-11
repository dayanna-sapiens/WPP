using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.ModuloContratos
{
    public class ClienteBitacoraService : IClienteBitacoraService
    {
        private IRepository<ClienteBitacora> repository;

        public ClienteBitacoraService(IRepository<ClienteBitacora> _repository)
        {
            repository = _repository;
        }

        public ClienteBitacora Get(long id)
        {
            return repository.Get(id);
        }

        public ClienteBitacora Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public ClienteBitacora Create(ClienteBitacora entity)
        {
            repository.Add(entity);
            return entity;
        }

        public ClienteBitacora Update(ClienteBitacora entity)
        {
            repository.Update(entity);
            return entity;
        }

        public void Delete(ClienteBitacora entity)
        {            
            repository.Update(entity);
        }

        public bool Contains(ClienteBitacora item)
        {
            return repository.Contains(item);
        }

        public bool Contains(ClienteBitacora item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }

        public IList<ClienteBitacora> ListAll()
        {
            return repository.GetAll();
        }

        public IList<ClienteBitacora> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }

        public IList<ClienteBitacora> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate)
        {
            return repository.GetAll(criterias, property, startDate, endDate);
        }

        public int Count()
        {
            return repository.Count<ClienteBitacora>();
        }

    }
}
