using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloNomina;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.ModuloNomina
{
    public class ItemNominaService : IItemNominaService
    {
          private IRepository<ItemNomina> repository;

        public ItemNominaService(IRepository<ItemNomina> _repository)
        {
            repository = _repository;
        }

        public ItemNomina Get(long id)
        {
            return repository.Get(id);
        }

        public ItemNomina Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public ItemNomina Create(ItemNomina entity)
        {
            repository.Add(entity);
            return entity;
        }

        public ItemNomina Update(ItemNomina entity)
        {
            repository.Update(entity);
            return entity;
        }

        public void Delete(ItemNomina entity)
        {
            repository.Update(entity);
        }

        public bool Contains(ItemNomina item)
        {
            return repository.Contains(item);
        }

        public bool Contains(ItemNomina item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }

        public IEnumerable<ItemNomina> ListAll()
        {
            return repository.GetAll();
        }

        public IList<ItemNomina> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }

        public IList<ItemNomina> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate)
        {
            return repository.GetAll(criterias, property, startDate, endDate);
        }

    }
}
