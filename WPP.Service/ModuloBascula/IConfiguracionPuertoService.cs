using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloBascula;

namespace WPP.Service.ModuloBascula
{
    public interface IConfiguracionPuertoService
    {
        ConfiguracionPuerto Get(long id);
        ConfiguracionPuerto Get(IDictionary<string, object> criterias);
        ConfiguracionPuerto Create(ConfiguracionPuerto entity);
        ConfiguracionPuerto Update(ConfiguracionPuerto entity);
        void Delete(ConfiguracionPuerto entity);
        bool Contains(ConfiguracionPuerto item);
        bool Contains(ConfiguracionPuerto item, string property, object value);
        IList<ConfiguracionPuerto> ListAll();
        IList<ConfiguracionPuerto> GetAll(IDictionary<string, object> criterias);
    }
}
