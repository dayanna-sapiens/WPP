using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloCierreCaja;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.ModuloCierreCaja
{
    public class EfectivoCierreCajaService : IEfectivoCierreCajaService
    {
        private IRepository<EfectivoCierreCaja> repository;

        public EfectivoCierreCajaService(IRepository<EfectivoCierreCaja> _repository)
        {
            repository = _repository;
        }

        public EfectivoCierreCaja Get(long id)
        {
            return repository.Get(id);
        }

        public EfectivoCierreCaja Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public EfectivoCierreCaja Create(EfectivoCierreCaja entity)
        {
            repository.Add(entity);
            return entity;
        }

        public EfectivoCierreCaja Update(EfectivoCierreCaja entity)
        {
            repository.Update(entity);
            return entity;
        }

        public void Delete(EfectivoCierreCaja entity)
        {
            repository.Update(entity);
        }

        public bool Contains(EfectivoCierreCaja item)
        {
            return repository.Contains(item);
        }

        public bool Contains(EfectivoCierreCaja item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }

        public IList<EfectivoCierreCaja> ListAll()
        {
            return repository.GetAll();
        }

        public IList<EfectivoCierreCaja> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }

        public IList<EfectivoCierreCaja> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate)
        {
            return repository.GetAll(criterias, property, startDate, endDate);
        }

        public int Count()
        {
            return repository.Count<EfectivoCierreCaja>();
        }
    }
}
