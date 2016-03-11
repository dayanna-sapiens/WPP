using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloBascula;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.ModuloBascula
{
    public class ConfiguracionPuertoService : IConfiguracionPuertoService
    {
    private IRepository<ConfiguracionPuerto> repository;

        public ConfiguracionPuertoService(IRepository<ConfiguracionPuerto> _repository)
        {
            repository = _repository;
        }

        public ConfiguracionPuerto Get(long id)
        {
            return repository.Get(id);
        }

        public ConfiguracionPuerto Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public ConfiguracionPuerto Create(ConfiguracionPuerto entity)
        {
            repository.Add(entity);
            return entity;
        }
        public ConfiguracionPuerto Update(ConfiguracionPuerto entity)
        {
            repository.Update(entity);
            return entity;
        }
        public void Delete(ConfiguracionPuerto entity)
        {
            entity.IsDeleted = true;
            repository.Update(entity);
        }
        public bool Contains(ConfiguracionPuerto item)
        {
            return repository.Contains(item);
        }
        public bool Contains(ConfiguracionPuerto item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }
        public IList<ConfiguracionPuerto> ListAll()
        {
            return repository.GetAll();
        }
        public IList<ConfiguracionPuerto> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }
    

    
    }
}
