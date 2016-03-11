using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.ModuloContratos
{
    public class ProductoContratoService : IProductoContratoService
    {
        private IRepository<ProductoContrato> repository;

        public ProductoContratoService(IRepository<ProductoContrato> _repository)
        {
            repository = _repository;
        }

        public ProductoContrato Get(long id)
        {
            return repository.Get(id);
        }

        public ProductoContrato Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public ProductoContrato Create(ProductoContrato entity)
        {
            repository.Add(entity);
            return entity;
        }

        public ProductoContrato Update(ProductoContrato entity)
        {
            repository.Update(entity);
            return entity;
        }

        public void Delete(ProductoContrato entity)
        {
            entity.IsDeleted = true;
            repository.Update(entity);
        }

        public bool Contains(ProductoContrato item)
        {
            return repository.Contains(item);
        }

        public bool Contains(ProductoContrato item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }

        public IList<ProductoContrato> ListAll()
        {
            return repository.GetAll();
        }

        public IList<ProductoContrato> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }

        public IList<ProductoContrato> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate)
        {
            return repository.GetAll(criterias, property, startDate, endDate);
        }

        public int Count()
        {
            return repository.Count<ProductoContrato>();
        }
    }
}
