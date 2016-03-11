using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloBascula;
using WPP.Model.ModuloBascula;

namespace WPP.Mapper.ModuloBascula
{
    public class BasculaMapper
    {
        public BasculaModel GetBoletaBasculaModel(Bascula boleta)
        {
            AutoMapper.Mapper.CreateMap<Bascula, BasculaModel>()
            .ForMember(dest => dest.Contrato, opts => opts.MapFrom(src => src.Contrato.Id))
            .ForMember(dest => dest.ContratoDescripcion, opts => opts.MapFrom(src => src.Contrato.DescripcionContrato))
            .ForMember(dest => dest.Cliente, opts => opts.MapFrom(src => src.Contrato.Cliente.Nombre))
            .ForMember(dest => dest.NumeroCliente, opts => opts.MapFrom(src => src.Contrato.Cliente.Id))
            .ForMember(dest => dest.Compania, opts => opts.MapFrom(src => src.Contrato.Compania.Id))
            .ForMember(dest => dest.Producto, opts => opts.MapFrom(src => src.Producto.Id))
            .ForMember(dest => dest.DescripcionProducto, opts => opts.MapFrom(src => src.Producto.Descripcion))
            .ForMember(dest => dest.Equipo, opts => opts.MapFrom(src => src.Equipo.Id))
            .ForMember(dest => dest.PlacaEquipo, opts => opts.MapFrom(src => src.Equipo.Nombre))
            .ForMember(dest => dest.OTR, opts => opts.MapFrom(src => src.OTR.Id))
            .ForMember(dest => dest.ConsecutivoOTR, opts => opts.MapFrom(src => src.OTR.Consecutivo))
            .ForMember(dest => dest.CierreCaja, opts => opts.MapFrom(src => src.CierreCaja.Id));

            BasculaModel boletaModel = AutoMapper.Mapper.Map<BasculaModel>(boleta);
            return boletaModel;
        }

        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<BasculaModel> GetListaBoletaModel(IList<Bascula> ListaBascula)
        {
            AutoMapper.Mapper.CreateMap<Bascula, BasculaModel>()
              .ForMember(dest => dest.Contrato, opts => opts.MapFrom(src => src.Contrato.Id))
            .ForMember(dest => dest.ContratoDescripcion, opts => opts.MapFrom(src => src.Contrato.DescripcionContrato))
            .ForMember(dest => dest.Cliente, opts => opts.MapFrom(src => src.Contrato.Cliente.Nombre))
            .ForMember(dest => dest.NumeroCliente, opts => opts.MapFrom(src => src.Contrato.Cliente.Id))
            .ForMember(dest => dest.Compania, opts => opts.MapFrom(src => src.Contrato.Compania.Id))
            .ForMember(dest => dest.Producto, opts => opts.MapFrom(src => src.Producto.Id))
            .ForMember(dest => dest.DescripcionProducto, opts => opts.MapFrom(src => src.Producto.Descripcion))
            .ForMember(dest => dest.Equipo, opts => opts.MapFrom(src => src.Equipo.Id))
            .ForMember(dest => dest.PlacaEquipo, opts => opts.MapFrom(src => src.Equipo.Nombre))
            .ForMember(dest => dest.OTR, opts => opts.MapFrom(src => src.OTR.Id))
            .ForMember(dest => dest.ConsecutivoOTR, opts => opts.MapFrom(src => src.OTR.Consecutivo))
            .ForMember(dest => dest.CierreCaja, opts => opts.MapFrom(src => src.CierreCaja.Id));
            IList<BasculaModel> listaBasculaModel = AutoMapper.Mapper.Map<IList<BasculaModel>>(ListaBascula);
            return listaBasculaModel;
        }


        // Convierte Modelo en Entidad
        public Bascula GetBoletaBascula(BasculaModel boletaModelo, Bascula boleta)
        {
            AutoMapper.Mapper.CreateMap<BasculaModel, Bascula>()
                .ForMember(dest => dest.Contrato, opts => opts.Ignore())
                .ForMember(dest => dest.Compania, opts => opts.Ignore())
                .ForMember(dest => dest.OTR, opts => opts.Ignore())
                .ForMember(dest => dest.Producto, opts => opts.Ignore())
                .ForMember(dest => dest.Equipo, opts => opts.Ignore())
                .ForMember(dest => dest.ListaPagos, opts => opts.Ignore())
                .ForMember(dest => dest.CierreCaja, opts => opts.Ignore());

            AutoMapper.Mapper.Map<BasculaModel, Bascula>(boletaModelo, boleta);

            return boleta;
        }

    }
}
