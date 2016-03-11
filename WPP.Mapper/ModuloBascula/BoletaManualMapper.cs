using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloBascula;
using WPP.Model.ModuloBascula;

namespace WPP.Mapper.ModuloBascula
{
    public class BoletaManualMapper
    {
        public BoletaManualModel GetModel(BoletaManual entity)
        {
            AutoMapper.Mapper.CreateMap<BoletaManual, BoletaManualModel>()
                .ForMember(dest => dest.Sitio, opts => opts.MapFrom(src => src.Sitio.Id))
                .ForMember(dest => dest.SitioDescripcion, opts => opts.MapFrom(src => src.Sitio.Nombre))
                .ForMember(dest => dest.Compania, opts => opts.MapFrom(src => src.Compania.Id))
                .ForMember(dest => dest.OTR, opts => opts.MapFrom(src => src.OTR.Id))
                .ForMember(dest => dest.OTRConsecutivo, opts => opts.MapFrom(src => src.OTR.Consecutivo));

            BoletaManualModel contenedorModel = AutoMapper.Mapper.Map<BoletaManualModel>(entity);
            return contenedorModel;
        }


        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<BoletaManualModel> GetListaModel(IList<BoletaManual> Lista)
        {
            AutoMapper.Mapper.CreateMap<BoletaManual, BoletaManualModel>()
                .ForMember(dest => dest.Sitio, opts => opts.MapFrom(src => src.Sitio.Id))
                .ForMember(dest => dest.SitioDescripcion, opts => opts.MapFrom(src => src.Sitio.Nombre))
                .ForMember(dest => dest.Compania, opts => opts.MapFrom(src => src.Compania.Id))
                .ForMember(dest => dest.OTR, opts => opts.MapFrom(src => src.OTR.Id))
                .ForMember(dest => dest.OTRConsecutivo, opts => opts.MapFrom(src => src.OTR.Consecutivo));

            IList<BoletaManualModel> listaBoletaManualModel = AutoMapper.Mapper.Map<IList<BoletaManualModel>>(Lista);
            return listaBoletaManualModel;
        }

        // Convierte Modelo en Entidad
        public BoletaManual GetEntity(BoletaManualModel modelo, BoletaManual entity)
        {
            AutoMapper.Mapper.CreateMap<BoletaManualModel, BoletaManual>()
                .ForMember(dest => dest.OTR, opts => opts.Ignore())
                .ForMember(dest => dest.Sitio, opts => opts.Ignore())
                .ForMember(dest => dest.Compania, opts => opts.Ignore());

            AutoMapper.Mapper.Map<BoletaManualModel, BoletaManual>(modelo, entity);

            return entity;
        }
    }
}
