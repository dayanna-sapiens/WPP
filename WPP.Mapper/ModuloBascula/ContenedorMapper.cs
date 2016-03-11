using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloBascula;
using WPP.Model.ModuloBascula;

namespace WPP.Mapper.ModuloBascula
{
    public class ContenedorMapper
    {
        public ContenedorModel GetModel(Contenedor entity)
        {
            AutoMapper.Mapper.CreateMap<Contenedor, ContenedorModel>();

            ContenedorModel contenedorModel = AutoMapper.Mapper.Map<ContenedorModel>(entity);
            return contenedorModel;
        }


        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<ContenedorModel> GetListaModel(IList<Contenedor> Lista)
        {
            AutoMapper.Mapper.CreateMap<Contenedor, ContenedorModel>();

            IList<ContenedorModel> listaContenedorModel = AutoMapper.Mapper.Map<IList<ContenedorModel>>(Lista);
            return listaContenedorModel;
        }

        // Convierte Modelo en Entidad
        public Contenedor GetEntity(ContenedorModel modelo, Contenedor entity)
        {
            AutoMapper.Mapper.CreateMap<ContenedorModel, Contenedor>();

            AutoMapper.Mapper.Map<ContenedorModel, Contenedor>(modelo, entity);

            return entity;
        }
    }
}
