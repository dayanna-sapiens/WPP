using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Model.ModuloContratos;

namespace WPP.Mapper.ModuloContratos
{
    public class ContratoMapper
    {
        public ContratoModel GetContratoModel(Contrato compania)
        {
            AutoMapper.Mapper.CreateMap<Contrato, ContratoModel>()
            .ForMember(dest => dest.DescripcionCliente, opts => opts.MapFrom(src => src.Cliente.Nombre))
            .ForMember(dest => dest.NumeroCliente, opts => opts.MapFrom(src => src.Cliente.Id))
            .ForMember(dest => dest.PuntoFacturacion, opts => opts.MapFrom(src => src.PuntoFacturacion.Id))
            .ForMember(dest => dest.PuntoFacturacionDescripcion, opts => opts.MapFrom(src => src.PuntoFacturacion.Nombre))
            //.ForMember(dest => dest.Estado, opts => opts.MapFrom(src => src.Estado.Id))
            //.ForMember(dest => dest.EstadoDescripcion, opts => opts.MapFrom(src => src.Estado.Nombre))
            .ForMember(dest => dest.DiaCorteSemana, opts => opts.MapFrom(src => src.DiaCorteSemana.Id))
            .ForMember(dest => dest.DiaCorteSemanaDescripcion, opts => opts.MapFrom(src => src.DiaCorteSemana.Nombre))
            .ForMember(dest => dest.Repesaje, opts => opts.MapFrom(src => src.Repesaje.Id))
            .ForMember(dest => dest.RepesajeDescripcion, opts => opts.MapFrom(src => src.Repesaje.Nombre))
            .ForMember(dest => dest.Compania, opts => opts.MapFrom(src => src.Compania.Id))
            .ForMember(dest => dest.EjecutivoNombre, opts => opts.MapFrom(src => src.Ejecutivo.Nombre + " " + src.Ejecutivo.Apellido1 + " " + src.Ejecutivo.Apellido2))
            .ForMember(dest => dest.Ejecutivo, opts => opts.MapFrom(src => src.Ejecutivo.Id));

            ContratoModel contratoModel = AutoMapper.Mapper.Map<ContratoModel>(compania);
            return contratoModel;
        }

        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<ContratoModel> GetListaClienteModel(IList<Contrato> ListaContrato)
        {
            AutoMapper.Mapper.CreateMap<Contrato, ContratoModel>()
            .ForMember(dest => dest.DescripcionCliente, opts => opts.MapFrom(src => src.Cliente.Nombre))
            .ForMember(dest => dest.NumeroCliente, opts => opts.MapFrom(src => src.Cliente.Id))
            .ForMember(dest => dest.PuntoFacturacion, opts => opts.MapFrom(src => src.PuntoFacturacion.Id))
            .ForMember(dest => dest.PuntoFacturacionDescripcion, opts => opts.MapFrom(src => src.PuntoFacturacion.Nombre))
            //.ForMember(dest => dest.Estado, opts => opts.MapFrom(src => src.Estado.Id))
            //.ForMember(dest => dest.EstadoDescripcion, opts => opts.MapFrom(src => src.Estado.Nombre))
            .ForMember(dest => dest.DiaCorteSemana, opts => opts.MapFrom(src => src.DiaCorteSemana.Id))
            .ForMember(dest => dest.DiaCorteSemanaDescripcion, opts => opts.MapFrom(src => src.DiaCorteSemana.Nombre))
            .ForMember(dest => dest.Repesaje, opts => opts.MapFrom(src => src.Repesaje.Id))
            .ForMember(dest => dest.RepesajeDescripcion, opts => opts.MapFrom(src => src.Repesaje.Nombre))
            .ForMember(dest => dest.Compania, opts => opts.MapFrom(src => src.Compania.Id))
            .ForMember(dest => dest.EjecutivoNombre, opts => opts.MapFrom(src => src.Ejecutivo.Nombre + " " + src.Ejecutivo.Apellido1 + " " + src.Ejecutivo.Apellido2))
            .ForMember(dest => dest.Ejecutivo, opts => opts.MapFrom(src => src.Ejecutivo.Id));
            IList<ContratoModel> listaContratoModel = AutoMapper.Mapper.Map<IList<ContratoModel>>(ListaContrato);
            return listaContratoModel;
        }

        // Convierte Modelo en Entidad
        public Contrato GetContrato(ContratoModel contratoModelo, Contrato contrato)
        {
            AutoMapper.Mapper.CreateMap<ContratoModel, Contrato>()
                .ForMember(dest => dest.Cliente, opts => opts.Ignore())
                .ForMember(dest => dest.PuntoFacturacion, opts => opts.Ignore())
                .ForMember(dest => dest.DiaCorteSemana, opts => opts.Ignore())
                .ForMember(dest => dest.Repesaje, opts => opts.Ignore())
                .ForMember(dest => dest.Productos, opts => opts.Ignore())
                .ForMember(dest => dest.Compania, opts => opts.Ignore())
                .ForMember(dest => dest.Ejecutivo, opts => opts.Ignore());

            AutoMapper.Mapper.Map<ContratoModel, Contrato>(contratoModelo, contrato);

            return contrato;
        }
    }
}
