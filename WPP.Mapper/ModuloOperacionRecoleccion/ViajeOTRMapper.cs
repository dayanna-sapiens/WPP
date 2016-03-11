using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;
using WPP.Model.ModuloOperacionRecoleccion;

namespace WPP.Mapper.ModuloOperacionRecoleccion
{
    public class ViajeOTRMapper
    {
        public ViajeOTRModel GetViajeOTRModel(ViajeOTR otr)
        {
            AutoMapper.Mapper.CreateMap<ViajeOTR, ViajeOTRModel>()
            .ForMember(dest => dest.Compania, opts => opts.MapFrom(src => src.Compania.Id))
            .ForMember(dest => dest.Contenedor, opts => opts.MapFrom(src => src.Contenedor.Id))
            .ForMember(dest => dest.ContenedorDescripcion, opts => opts.MapFrom(src => src.Contenedor.Descripcion))
            .ForMember(dest => dest.Accion, opts => opts.MapFrom(src => src.Accion.Id))
            .ForMember(dest => dest.AccionDescripcion, opts => opts.MapFrom(src => src.Accion.Nombre))
            .ForMember(dest => dest.Viaje, opts => opts.MapFrom(src => src.Viaje.Id))
            .ForMember(dest => dest.ViajeDescripcion, opts => opts.MapFrom(src => src.Viaje.Descripcion))
            .ForMember(dest => dest.Tamano, opts => opts.MapFrom(src => src.Tamano.Id))
            .ForMember(dest => dest.TamanoDescripcion, opts => opts.MapFrom(src => src.Tamano.Nombre))
            .ForMember(dest => dest.TipoEquipo, opts => opts.MapFrom(src => src.TipoEquipo.Id))
            .ForMember(dest => dest.TipoEquipoDescripcion, opts => opts.MapFrom(src => src.TipoEquipo.Nombre))
            .ForMember(dest => dest.OTR, opts => opts.MapFrom(src => src.OTR.Id));

            ViajeOTRModel otrModel = AutoMapper.Mapper.Map<ViajeOTRModel>(otr);
            return otrModel;
        }

        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<ViajeOTRModel> GetListaViajeOTRModel(IList<ViajeOTR> ListaViajeOTR)
        {
            AutoMapper.Mapper.CreateMap<ViajeOTR, ViajeOTRModel>()
            .ForMember(dest => dest.Compania, opts => opts.MapFrom(src => src.Compania.Id))
            .ForMember(dest => dest.Contenedor, opts => opts.MapFrom(src => src.Contenedor.Id))
            .ForMember(dest => dest.ContenedorDescripcion, opts => opts.MapFrom(src => src.Contenedor.Descripcion))
            .ForMember(dest => dest.Accion, opts => opts.MapFrom(src => src.Accion.Id))
            .ForMember(dest => dest.AccionDescripcion, opts => opts.MapFrom(src => src.Accion.Nombre))
            .ForMember(dest => dest.Viaje, opts => opts.MapFrom(src => src.Viaje.Id))
            .ForMember(dest => dest.ViajeDescripcion, opts => opts.MapFrom(src => src.Viaje.Descripcion))
            .ForMember(dest => dest.Tamano, opts => opts.MapFrom(src => src.Tamano.Id))
            .ForMember(dest => dest.TamanoDescripcion, opts => opts.MapFrom(src => src.Tamano.Nombre))
            .ForMember(dest => dest.TipoEquipo, opts => opts.MapFrom(src => src.TipoEquipo.Id))
            .ForMember(dest => dest.TipoEquipoDescripcion, opts => opts.MapFrom(src => src.TipoEquipo.Nombre))
            .ForMember(dest => dest.OTR, opts => opts.MapFrom(src => src.OTR.Id));

            IList<ViajeOTRModel> listaOTRModel = AutoMapper.Mapper.Map<IList<ViajeOTRModel>>(ListaViajeOTR);
            return listaOTRModel;
        }


        // Convierte Modelo en Entidad
        public ViajeOTR GetViajeOTR(ViajeOTRModel viajeOtrModelo, ViajeOTR viajeOtr)
        {
            AutoMapper.Mapper.CreateMap<ViajeOTRModel, ViajeOTR>()
                .ForMember(dest => dest.OTR, opts => opts.Ignore())
                .ForMember(dest => dest.Compania, opts => opts.Ignore())
                .ForMember(dest => dest.Contenedor, opts => opts.Ignore())
                .ForMember(dest => dest.Accion, opts => opts.Ignore())
                .ForMember(dest => dest.TipoEquipo, opts => opts.Ignore())
                .ForMember(dest => dest.Tamano, opts => opts.Ignore())
                .ForMember(dest => dest.Viaje, opts => opts.Ignore());

            AutoMapper.Mapper.Map<ViajeOTRModel, ViajeOTR>(viajeOtrModelo, viajeOtr);

            return viajeOtr;
        }
    }
}
