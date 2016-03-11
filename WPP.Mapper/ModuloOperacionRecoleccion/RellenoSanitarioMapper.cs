using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;
using WPP.Model.ModuloOperacionRecoleccion;

namespace WPP.Mapper.ModuloOperacionRecoleccion
{
    public class RellenoSanitarioMapper
    {
        public RellenoSanitarioModel GetModel(RellenoSanitario entity)
        {
            AutoMapper.Mapper.CreateMap<RellenoSanitario, RellenoSanitarioModel>();

            RellenoSanitarioModel clienteModel = AutoMapper.Mapper.Map<RellenoSanitarioModel>(entity);
            return clienteModel;
        }


        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<RellenoSanitarioModel> GetListaModel(IList<RellenoSanitario> Lista)
        {
            AutoMapper.Mapper.CreateMap<RellenoSanitario, RellenoSanitarioModel>();

            IList<RellenoSanitarioModel> listaClienteModel = AutoMapper.Mapper.Map<IList<RellenoSanitarioModel>>(Lista);
            return listaClienteModel;
        }

        // Convierte Modelo en Entidad
        public RellenoSanitario GetEntity(RellenoSanitarioModel modelo, RellenoSanitario entity)
        {
            AutoMapper.Mapper.CreateMap<RellenoSanitarioModel, RellenoSanitario>();

            AutoMapper.Mapper.Map<RellenoSanitarioModel, RellenoSanitario>(modelo, entity);

            return entity;
        }
    }
}
