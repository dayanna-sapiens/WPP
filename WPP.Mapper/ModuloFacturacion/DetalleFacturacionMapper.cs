using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloFacturacion;
using WPP.Model.ModuloFacturacion;

namespace WPP.Mapper.ModuloFacturacion
{
    public class DetalleFacturacionMapper
    {
        public DetalleFacturacionModel GetModel(DetalleFacturacion entity)
        {
            AutoMapper.Mapper.CreateMap<DetalleFacturacion, DetalleFacturacionModel>()
                .ForMember(dest => dest.Facturacion, opts => opts.MapFrom(src => src.Facturacion.Id))
                .ForMember(dest => dest.Bascula, opts => opts.MapFrom(src => src.Bascula.Id))
                .ForMember(dest => dest.OTR, opts => opts.MapFrom(src => src.OTR.Id))
                .ForMember(dest => dest.Producto, opts => opts.MapFrom(src => src.Producto.Id))
                .ForMember(dest => dest.ProductoDescripcion, opts => opts.MapFrom(src => src.Producto.Descripcion))
                .ForMember(dest => dest.BoletaManual, opts => opts.MapFrom(src => src.BoletaManual.Id));

            DetalleFacturacionModel contenedorModel = AutoMapper.Mapper.Map<DetalleFacturacionModel>(entity);
            return contenedorModel;
        }


        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<DetalleFacturacionModel> GetListaModel(IList<DetalleFacturacion> Lista)
        {
            AutoMapper.Mapper.CreateMap<DetalleFacturacion, DetalleFacturacionModel>()
                .ForMember(dest => dest.Facturacion, opts => opts.MapFrom(src => src.Facturacion.Id))
                .ForMember(dest => dest.Bascula, opts => opts.MapFrom(src => src.Bascula.Id))
                .ForMember(dest => dest.OTR, opts => opts.MapFrom(src => src.OTR.Id))
                .ForMember(dest => dest.Producto, opts => opts.MapFrom(src => src.Producto.Id))
                .ForMember(dest => dest.ProductoDescripcion, opts => opts.MapFrom(src => src.Producto.Descripcion))
                .ForMember(dest => dest.BoletaManual, opts => opts.MapFrom(src => src.BoletaManual.Id));

            IList<DetalleFacturacionModel> listaDetalleFacturacionModel = AutoMapper.Mapper.Map<IList<DetalleFacturacionModel>>(Lista);
            return listaDetalleFacturacionModel;
        }

        // Convierte Modelo en Entidad
        public DetalleFacturacion GetEntity(DetalleFacturacionModel modelo, DetalleFacturacion entity)
        {
            AutoMapper.Mapper.CreateMap<DetalleFacturacionModel, DetalleFacturacion>()
                .ForMember(dest => dest.Bascula, opts => opts.Ignore())
                .ForMember(dest => dest.Facturacion, opts => opts.Ignore())
                .ForMember(dest => dest.OTR, opts => opts.Ignore())
                .ForMember(dest => dest.Producto, opts => opts.Ignore())
                .ForMember(dest => dest.BoletaManual, opts => opts.Ignore());

            AutoMapper.Mapper.Map<DetalleFacturacionModel, DetalleFacturacion>(modelo, entity);

            return entity;
        }
    }
}
