using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Model.ModuloContratos;

namespace WPP.Mapper.ModuloContratos
{
    public class ProductoContratoMapper
    {

        public ProductoContratoModel GetProductoContratoModel(ProductoContrato producto)
        {
            AutoMapper.Mapper.CreateMap<ProductoContrato, ProductoContratoModel>()
            .ForMember(dest => dest.EsquemaRelevanciaDescripcion, opts => opts.MapFrom(src => src.EsquemaRelevancia.Nombre))
            .ForMember(dest => dest.EsquemaRelevancia, opts => opts.MapFrom(src => src.EsquemaRelevancia.Id))
            .ForMember(dest => dest.ContratoDescripcion, opts => opts.MapFrom(src => src.Contrato.DescripcionContrato))
            .ForMember(dest => dest.Contrato, opts => opts.MapFrom(src => src.Contrato.Id))
            .ForMember(dest => dest.Servicio, opts => opts.MapFrom(src => src.Servicio.Id))
            .ForMember(dest => dest.ServicioDescripcion, opts => opts.MapFrom(src => src.Servicio.Nombre))
            .ForMember(dest => dest.Producto, opts => opts.MapFrom(src => src.Producto.Id))
            .ForMember(dest => dest.ProductoDescripcion, opts => opts.MapFrom(src => src.Producto.Descripcion))
            .ForMember(dest => dest.Frecuecia, opts => opts.MapFrom(src => src.Frecuecia.Id))
            .ForMember(dest => dest.FrecueciaDescripcion, opts => opts.MapFrom(src => src.Frecuecia.Nombre))
            .ForMember(dest => dest.UbicacionDescripcion, opts => opts.MapFrom(src => src.Ubicacion.Descripcion))
            .ForMember(dest => dest.Ubicacion, opts => opts.MapFrom(src => src.Ubicacion.Id))
            .ForMember(dest => dest.RutaRecoleccion, opts => opts.MapFrom(src => src.RutaRecoleccion.Id))
            .ForMember(dest => dest.RutaRecoleccionDescripcion, opts => opts.MapFrom(src => src.RutaRecoleccion.Descripcion))
            .ForMember(dest => dest.ProductoFosa, opts => opts.MapFrom(src => src.ProductoFosa.Id))
            .ForMember(dest => dest.ProductoFosaDescripcion, opts => opts.MapFrom(src => src.ProductoFosa.Descripcion))
            .ForMember(dest => dest.Proyecto, opts => opts.MapFrom(src => src.Proyecto.Id))
            .ForMember(dest => dest.ProyectoDescripcion, opts => opts.MapFrom(src => src.Proyecto.Nombre))
            .ForMember(dest => dest.Recoleccion, opts => opts.MapFrom(src => src.Recoleccion.Id))
            .ForMember(dest => dest.RecoleccionDescripcion, opts => opts.MapFrom(src => src.Recoleccion.Descripcion));

            ProductoContratoModel contratoModel = AutoMapper.Mapper.Map<ProductoContratoModel>(producto);
         //   contratoModel.EstadoDescripcion = contratoModel.Activo == true ? "Activo" : "Inactivo";
            return contratoModel;
        }

        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<ProductoContratoModel> GetListaProductoContratoModel(IList<ProductoContrato> ListaProducto)
        {
            AutoMapper.Mapper.CreateMap<ProductoContrato, ProductoContratoModel>()
            .ForMember(dest => dest.EsquemaRelevanciaDescripcion, opts => opts.MapFrom(src => src.EsquemaRelevancia.Nombre))
            .ForMember(dest => dest.EsquemaRelevancia, opts => opts.MapFrom(src => src.EsquemaRelevancia.Id))
            .ForMember(dest => dest.ContratoDescripcion, opts => opts.MapFrom(src => src.Contrato.DescripcionContrato))
            .ForMember(dest => dest.Contrato, opts => opts.MapFrom(src => src.Contrato.Id))
            .ForMember(dest => dest.Servicio, opts => opts.MapFrom(src => src.Servicio.Id))
            .ForMember(dest => dest.ServicioDescripcion, opts => opts.MapFrom(src => src.Servicio.Nombre))
            .ForMember(dest => dest.Producto, opts => opts.MapFrom(src => src.Producto.Id))
            .ForMember(dest => dest.ProductoDescripcion, opts => opts.MapFrom(src => src.Producto.Descripcion))
            .ForMember(dest => dest.Frecuecia, opts => opts.MapFrom(src => src.Frecuecia.Id))
            .ForMember(dest => dest.FrecueciaDescripcion, opts => opts.MapFrom(src => src.Frecuecia.Nombre))
            .ForMember(dest => dest.UbicacionDescripcion, opts => opts.MapFrom(src => src.Ubicacion.Descripcion))
            .ForMember(dest => dest.Ubicacion, opts => opts.MapFrom(src => src.Ubicacion.Id))
            .ForMember(dest => dest.RutaRecoleccion, opts => opts.MapFrom(src => src.RutaRecoleccion.Id))
            .ForMember(dest => dest.RutaRecoleccionDescripcion, opts => opts.MapFrom(src => src.RutaRecoleccion.Descripcion))
            .ForMember(dest => dest.ProductoFosa, opts => opts.MapFrom(src => src.ProductoFosa.Id))
            .ForMember(dest => dest.ProductoFosaDescripcion, opts => opts.MapFrom(src => src.ProductoFosa.Descripcion))
            .ForMember(dest => dest.Proyecto, opts => opts.MapFrom(src => src.Proyecto.Id))
            .ForMember(dest => dest.ProyectoDescripcion, opts => opts.MapFrom(src => src.Proyecto.Nombre))
            .ForMember(dest => dest.Recoleccion, opts => opts.MapFrom(src => src.Recoleccion.Id))
            .ForMember(dest => dest.RecoleccionDescripcion, opts => opts.MapFrom(src => src.Recoleccion.Descripcion));
            IList<ProductoContratoModel> listaProductoModel = AutoMapper.Mapper.Map<IList<ProductoContratoModel>>(ListaProducto);
            return listaProductoModel;
        }

        // Convierte Modelo en Entidad
        public ProductoContrato GetProductoContrato(ProductoContratoModel productoModelo, ProductoContrato producto)
        {
            AutoMapper.Mapper.CreateMap<ProductoContratoModel, ProductoContrato >()
                .ForMember(dest => dest.Ubicacion, opts => opts.Ignore())
                .ForMember(dest => dest.Contrato, opts => opts.Ignore())
                .ForMember(dest => dest.Producto, opts => opts.Ignore())
                .ForMember(dest => dest.Frecuecia, opts => opts.Ignore())
                .ForMember(dest => dest.EsquemaRelevancia, opts => opts.Ignore())
                .ForMember(dest => dest.RutaRecoleccion, opts => opts.Ignore())
                .ForMember(dest => dest.Servicio, opts => opts.Ignore())
                .ForMember(dest => dest.ProductoFosa, opts => opts.Ignore())
                .ForMember(dest => dest.Proyecto, opts => opts.Ignore())
                .ForMember(dest => dest.Recoleccion, opts => opts.Ignore());

            AutoMapper.Mapper.Map<ProductoContratoModel, ProductoContrato>(productoModelo, producto);

            //producto.Activo = productoModelo.Estado == "1" ? true : false;

            return producto;
        }
    }
}
