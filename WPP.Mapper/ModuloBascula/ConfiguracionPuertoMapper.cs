using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloBascula;
using WPP.Model.ModuloBascula;

namespace WPP.Mapper.ModuloBascula
{
    public class ConfiguracionPuertoMapper
    {
        public ConfiguracionPuertoModel GetModel(ConfiguracionPuerto entity)
        {
            AutoMapper.Mapper.CreateMap<ConfiguracionPuerto, ConfiguracionPuertoModel>();

            ConfiguracionPuertoModel clienteModel = AutoMapper.Mapper.Map<ConfiguracionPuertoModel>(entity);
            return clienteModel;
        }


        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<ConfiguracionPuertoModel> GetListaModel(IList<ConfiguracionPuerto> Lista)
        {
            AutoMapper.Mapper.CreateMap<ConfiguracionPuerto, ConfiguracionPuertoModel>();

            IList<ConfiguracionPuertoModel> listaClienteModel = AutoMapper.Mapper.Map<IList<ConfiguracionPuertoModel>>(Lista);
            return listaClienteModel;
        }

        // Convierte Modelo en Entidad
        public ConfiguracionPuerto GetEntity(ConfiguracionPuertoModel modelo, ConfiguracionPuerto entity)
        {
            AutoMapper.Mapper.CreateMap<ConfiguracionPuertoModel, ConfiguracionPuerto>();

            AutoMapper.Mapper.Map<ConfiguracionPuertoModel, ConfiguracionPuerto>(modelo, entity);

            return entity;
        }
    }
}
