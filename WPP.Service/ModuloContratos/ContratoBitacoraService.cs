using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.ModuloContratos
{
    public class ContratoBitacoraService : IContratoBitacoraService
    {
        private IRepository<ContratoBitacora> repository;

        public ContratoBitacoraService(IRepository<ContratoBitacora> _repository)
        {
            repository = _repository;
        }

        public ContratoBitacora Get(long id)
        {
            return repository.Get(id);
        }

        public ContratoBitacora Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public ContratoBitacora Create(ContratoBitacora entity)
        {
            repository.Add(entity);
            return entity;
        }

        public ContratoBitacora Update(ContratoBitacora entity)
        {
            repository.Update(entity);
            return entity;
        }

        public void Delete(ContratoBitacora entity)
        {            
            repository.Update(entity);
        }

        public bool Contains(ContratoBitacora item)
        {
            return repository.Contains(item);
        }

        public bool Contains(ContratoBitacora item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }

        public IList<ContratoBitacora> ListAll()
        {
            return repository.GetAll();
        }

        public IList<ContratoBitacora> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }

        public IList<ContratoBitacora> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate)
        {
            return repository.GetAll(criterias, property, startDate, endDate);
        }

        public int Count()
        {
            return repository.Count<ContratoBitacora>();
        }

    }
}
