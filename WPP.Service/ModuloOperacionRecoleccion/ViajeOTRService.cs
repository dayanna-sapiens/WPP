using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.ModuloOperacionRecoleccion
{
    public class ViajeOTRService : IViajeOTRService
    {
        private IRepository<ViajeOTR> repository;

        public ViajeOTRService(IRepository<ViajeOTR> _repository)
        {
            repository = _repository;
        }

        public ViajeOTR Get(long id)
        {
            return repository.Get(id);
        }

        public ViajeOTR Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public ViajeOTR Create(ViajeOTR entity)
        {
            repository.Add(entity);
            return entity;
        }

        public ViajeOTR Update(ViajeOTR entity)
        {
            repository.Update(entity);
            return entity;
        }

        public void Delete(ViajeOTR entity)
        {
            entity.IsDeleted = true;
            repository.Update(entity);
        }

        public bool Contains(ViajeOTR item)
        {
            return repository.Contains(item);
        }

        public bool Contains(ViajeOTR item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }

        public IList<ViajeOTR> ListAll()
        {
            return repository.GetAll();
        }

        public IList<ViajeOTR> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }

        public IList<ViajeOTR> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate)
        {
            return repository.GetAll(criterias, property, startDate, endDate);
        }

        public int Count()
        {
            return repository.Count<ViajeOTR>();
        }

    }
}
