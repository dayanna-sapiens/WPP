using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloBascula;
using WPP.Model.ModuloBascula;

namespace WPP.Mapper.ModuloBascula
{
    public class EquipoMapper
    {
        public EquipoModel GetModel(Equipo entity)
        {
            AutoMapper.Mapper.CreateMap<Equipo, EquipoModel>();

            EquipoModel clienteModel = AutoMapper.Mapper.Map<EquipoModel>(entity);
            return clienteModel;
        }


        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<EquipoModel> GetListaModel(IList<Equipo> Lista)
        {
            AutoMapper.Mapper.CreateMap<Equipo, EquipoModel>();

            IList<EquipoModel> listaClienteModel = AutoMapper.Mapper.Map < IList<EquipoModel>>(Lista);
            return listaClienteModel;
        }

        // Convierte Modelo en Entidad
        public Equipo GetEntity(EquipoModel modelo, Equipo entity)
        {
            AutoMapper.Mapper.CreateMap<EquipoModel, Equipo>();

            AutoMapper.Mapper.Map<EquipoModel, Equipo>(modelo, entity);

            return entity;
        }
    }
}
