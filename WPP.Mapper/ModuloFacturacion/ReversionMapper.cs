using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloFacturacion;
using WPP.Model.ModuloFacturacion;

namespace WPP.Mapper.ModuloFacturacion
{
    public class ReversionMapper
    {
        public ReversionModel GetModel(Reversion entity)
        {
            AutoMapper.Mapper.CreateMap<Reversion, ReversionModel>()
                .ForMember(dest => dest.Facturacion, opts => opts.MapFrom(src => src.Facturacion.Id))
                .ForMember(dest => dest.ConsecutivoFacturacion, opts => opts.MapFrom(src => src.Facturacion.Consecutivo))
                .ForMember(dest => dest.Compania, opts => opts.MapFrom(src => src.Compania.Id));

            ReversionModel contenedorModel = AutoMapper.Mapper.Map<ReversionModel>(entity);
            return contenedorModel;
        }


        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<ReversionModel> GetListaModel(IList<Reversion> Lista)
        {
            AutoMapper.Mapper.CreateMap<Reversion, ReversionModel>()
                .ForMember(dest => dest.Facturacion, opts => opts.MapFrom(src => src.Facturacion.Id))
                .ForMember(dest => dest.ConsecutivoFacturacion, opts => opts.MapFrom(src => src.Facturacion.Consecutivo))
                .ForMember(dest => dest.Compania, opts => opts.MapFrom(src => src.Compania.Id));

            IList<ReversionModel> listaFacturacionModel = AutoMapper.Mapper.Map<IList<ReversionModel>>(Lista);
            return listaFacturacionModel;
        }

        // Convierte Modelo en Entidad
        public Reversion GetEntity(ReversionModel modelo, Reversion entity)
        {
            AutoMapper.Mapper.CreateMap<ReversionModel, Reversion>()
                .ForMember(dest => dest.Facturacion, opts => opts.Ignore())
                .ForMember(dest => dest.Compania, opts => opts.Ignore());

            AutoMapper.Mapper.Map<ReversionModel, Reversion>(modelo, entity);

            return entity;
        }
    }
}
