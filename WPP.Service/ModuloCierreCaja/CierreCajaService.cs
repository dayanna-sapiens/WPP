using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloCierreCaja;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.ModuloCierreCaja
{
    public class CierreCajaService : ICierreCajaService
    {
          private IRepository<CierreCaja> repository;

        public CierreCajaService(IRepository<CierreCaja> _repository)
        {
            repository = _repository;
        }

        public CierreCaja Get(long id)
        {
            return repository.Get(id);
        }

        public CierreCaja Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public CierreCaja Create(CierreCaja entity)
        {
            repository.Add(entity);
            return entity;
        }

        public CierreCaja Update(CierreCaja entity)
        {
            repository.Update(entity);
            return entity;
        }

        public void Delete(CierreCaja entity)
        {
            entity.IsDeleted = true;
            repository.Update(entity);
        }

        public bool Contains(CierreCaja item)
        {
            return repository.Contains(item);
        }

        public bool Contains(CierreCaja item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }

        public IList<CierreCaja> ListAll()
        {
            return repository.GetAll();
        }

        public IList<CierreCaja> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }

        public IList<CierreCaja> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate)
        {
            return repository.GetAll(criterias, property, startDate, endDate);
        }

        public int Count()
        {
            return repository.Count<CierreCaja>();
        }

    }
}
