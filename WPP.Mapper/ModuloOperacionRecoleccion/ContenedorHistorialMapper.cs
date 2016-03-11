using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;
using WPP.Model.ModuloOperacionRecoleccion;

namespace WPP.Mapper.ModuloOperacionRecoleccion
{
    public class ContenedorHistorialMapper
    {
        public ContenedorHistorialModel GetModel(ContenedorHistorial entity)
        {
            AutoMapper.Mapper.CreateMap<ContenedorHistorial, ContenedorHistorialModel>()
            .ForMember(dest => dest.Contendedor, opts => opts.MapFrom(src => src.Contenedor.Id))
            .ForMember(dest => dest.CodigoContenedor, opts => opts.MapFrom(src => src.Contenedor.Codigo))
            .ForMember(dest => dest.OTR, opts => opts.MapFrom(src => src.OTR.Id))
            .ForMember(dest => dest.ConsecutivoOTR, opts => opts.MapFrom(src => src.OTR.Consecutivo));

            ContenedorHistorialModel clienteModel = AutoMapper.Mapper.Map<ContenedorHistorialModel>(entity);
            return clienteModel;
        }


        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<ContenedorHistorialModel> GetListaModel(IList<ContenedorHistorial> Lista)
        {
            AutoMapper.Mapper.CreateMap<ContenedorHistorial, ContenedorHistorialModel>()
            .ForMember(dest => dest.Contendedor, opts => opts.MapFrom(src => src.Contenedor.Id))
            .ForMember(dest => dest.CodigoContenedor, opts => opts.MapFrom(src => src.Contenedor.Codigo))
            .ForMember(dest => dest.OTR, opts => opts.MapFrom(src => src.OTR.Id))
            .ForMember(dest => dest.ConsecutivoOTR, opts => opts.MapFrom(src => src.OTR.Consecutivo));

            IList<ContenedorHistorialModel> listaClienteModel = AutoMapper.Mapper.Map<IList<ContenedorHistorialModel>>(Lista);
            return listaClienteModel;
        }

        // Convierte Modelo en Entidad
        public ContenedorHistorial GetEntity(ContenedorHistorialModel modelo, ContenedorHistorial entity)
        {
            AutoMapper.Mapper.CreateMap<ContenedorHistorialModel, ContenedorHistorial>()
                .ForMember(dest => dest.Contenedor, opts => opts.Ignore())
                .ForMember(dest => dest.OTR, opts => opts.Ignore());

            AutoMapper.Mapper.Map<ContenedorHistorialModel, ContenedorHistorial>(modelo, entity);

            return entity;
        }
    }
}
