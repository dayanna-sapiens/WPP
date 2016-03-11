using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloBascula;
using WPP.Model.ModuloBascula;

namespace WPP.Mapper.ModuloBascula
{
    public class PagoBaculaMapper
    {
        public PagoBasculaModel GetPagoBasculaModel(PagoBascula pago)
        {
            AutoMapper.Mapper.CreateMap<PagoBascula, PagoBasculaModel>()
            .ForMember(dest => dest.Bacula, opts => opts.MapFrom(src => src.Boleta.Id))
            .ForMember(dest => dest.Banco, opts => opts.MapFrom(src => src.Banco.Id))
            .ForMember(dest => dest.BancoDescripcion, opts => opts.MapFrom(src => src.Banco.Nombre))
            .ForMember(dest => dest.Boleta, opts => opts.MapFrom(src => src.Boleta.Boleta))
            .ForMember(dest => dest.Cierre, opts => opts.MapFrom(src => src.Cierre.Id));

            PagoBasculaModel pagoModel = AutoMapper.Mapper.Map<PagoBasculaModel>(pago);
            return pagoModel;
        }

        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<PagoBasculaModel> GetListaPagoBasculaModel(IList<PagoBascula> ListaPago)
        {
            AutoMapper.Mapper.CreateMap<PagoBascula, PagoBasculaModel>()
            .ForMember(dest => dest.Bacula, opts => opts.MapFrom(src => src.Boleta.Id))
            .ForMember(dest => dest.Banco, opts => opts.MapFrom(src => src.Banco.Id))
            .ForMember(dest => dest.BancoDescripcion, opts => opts.MapFrom(src => src.Banco.Nombre))
            .ForMember(dest => dest.Boleta, opts => opts.MapFrom(src => src.Boleta.Boleta))
            .ForMember(dest => dest.Fecha, opts => opts.MapFrom(src => src.Boleta.Fecha))
            .ForMember(dest => dest.Cierre, opts => opts.MapFrom(src => src.Cierre.Id));

            IList<PagoBasculaModel> listaPagoModel = AutoMapper.Mapper.Map<IList<PagoBasculaModel>>(ListaPago);
            return listaPagoModel;
        }


        // Convierte Modelo en Entidad
        public PagoBascula GetPagoBascula(PagoBasculaModel pagoModelo, PagoBascula pago)
        {
            AutoMapper.Mapper.CreateMap<PagoBasculaModel, PagoBascula>()
                .ForMember(dest => dest.Boleta, opts => opts.Ignore())
                .ForMember(dest => dest.Banco, opts => opts.Ignore())
                .ForMember(dest => dest.Cierre, opts => opts.Ignore());

            AutoMapper.Mapper.Map<PagoBasculaModel, PagoBascula>(pagoModelo, pago);

            return pago;
        }
    }
}
