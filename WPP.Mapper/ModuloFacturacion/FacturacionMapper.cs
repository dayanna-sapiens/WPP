using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloFacturacion;
using WPP.Model.ModuloFacturacion;

namespace WPP.Mapper.ModuloFacturacion
{
    public class FacturacionMapper
    {
        public FacturacionModel GetModel(Facturacion entity)
        {
            AutoMapper.Mapper.CreateMap<Facturacion, FacturacionModel>()
                .ForMember(dest => dest.Compania, opts => opts.MapFrom(src => src.Compania.Id))
                .ForMember(dest => dest.Contrato, opts => opts.MapFrom(src => src.Contrato.Id))
                .ForMember(dest => dest.ContratoDescripcion, opts => opts.MapFrom(src => src.Contrato.DescripcionContrato))
                .ForMember(dest => dest.Cliente, opts => opts.MapFrom(src => src.Contrato.Cliente.Id))
                .ForMember(dest => dest.ClienteDescripcion, opts => opts.MapFrom(src => src.Contrato.Cliente.Nombre));

            FacturacionModel contenedorModel = AutoMapper.Mapper.Map<FacturacionModel>(entity);
            return contenedorModel;
        }


        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<FacturacionModel> GetListaModel(IList<Facturacion> Lista)
        {
            AutoMapper.Mapper.CreateMap<Facturacion, FacturacionModel>()
                .ForMember(dest => dest.Contrato, opts => opts.MapFrom(src => src.Contrato.Id))
                .ForMember(dest => dest.ContratoDescripcion, opts => opts.MapFrom(src => src.Contrato.DescripcionContrato))
                .ForMember(dest => dest.Cliente, opts => opts.MapFrom(src => src.Contrato.Cliente.Id))
                .ForMember(dest => dest.ClienteDescripcion, opts => opts.MapFrom(src => src.Contrato.Cliente.Nombre))
                .ForMember(dest => dest.Compania, opts => opts.MapFrom(src => src.Compania.Id));

            IList<FacturacionModel> listaFacturacionModel = AutoMapper.Mapper.Map<IList<FacturacionModel>>(Lista);
            return listaFacturacionModel;
        }

        // Convierte Modelo en Entidad
        public Facturacion GetEntity(FacturacionModel modelo, Facturacion entity)
        {
            AutoMapper.Mapper.CreateMap<FacturacionModel, Facturacion>()
                .ForMember(dest => dest.Contrato, opts => opts.Ignore())
                .ForMember(dest => dest.Compania, opts => opts.Ignore())
                .ForMember(dest => dest.ListaDetalleFacturacion, opts => opts.Ignore());

            AutoMapper.Mapper.Map<FacturacionModel, Facturacion>(modelo, entity);

            return entity;
        }
    }
}
