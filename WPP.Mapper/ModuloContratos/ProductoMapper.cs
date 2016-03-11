using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Model.ModuloContratos;

namespace WPP.Mapper.ModuloContratos
{
    public class ProductoMapper
    {
        // Convierte Entidad en Modelo
        public ProductoModel GetProductoModel(Producto producto)
        {
            AutoMapper.Mapper.CreateMap<Producto, ProductoModel>()
                .ForMember(dest => dest.Estado, opts => opts.MapFrom(src => src.Estado.Id))
                .ForMember(dest => dest.ProcesoCobro, opts => opts.MapFrom(src => src.ProcesoCobro.Id))
                .ForMember(dest => dest.TipoEquipo, opts => opts.MapFrom(src => src.TipoEquipo.Id))
                .ForMember(dest => dest.UnidadCobro, opts => opts.MapFrom(src => src.UnidadCobro.Id))
                .ForMember(dest => dest.Categoria, opts => opts.MapFrom(src => src.Categoria.Id))
                .ForMember(dest => dest.Tamano, opts => opts.MapFrom(src => src.Tamano.Id))
                .ForMember(dest => dest.EstadoNombre, opts => opts.MapFrom(src => src.Estado.Nombre))
                .ForMember(dest => dest.ProcesoCobroNombre, opts => opts.MapFrom(src => src.ProcesoCobro.Nombre))
                .ForMember(dest => dest.TipoEquipoNombre, opts => opts.MapFrom(src => src.TipoEquipo.Nombre))
                .ForMember(dest => dest.UnidadCobroNombre, opts => opts.MapFrom(src => src.UnidadCobro.Nombre))
                .ForMember(dest => dest.CategoriaNombre, opts => opts.MapFrom(src => src.Categoria.Nombre))
                .ForMember(dest => dest.TamanoNombre, opts => opts.MapFrom(src => src.Tamano.Nombre));

            ProductoModel productoModelo = AutoMapper.Mapper.Map<ProductoModel>(producto);
            return productoModelo;
        }

        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<ProductoModel> GetListaProductoModel(IList<Producto> ListaProducto)
        {
            AutoMapper.Mapper.CreateMap<Producto, ProductoModel>()
               .ForMember(dest => dest.Estado, opts => opts.MapFrom(src => src.Estado.Id))
                .ForMember(dest => dest.ProcesoCobro, opts => opts.MapFrom(src => src.ProcesoCobro.Id))
                .ForMember(dest => dest.TipoEquipo, opts => opts.MapFrom(src => src.TipoEquipo.Id))
                .ForMember(dest => dest.UnidadCobro, opts => opts.MapFrom(src => src.UnidadCobro.Id))
                .ForMember(dest => dest.Categoria, opts => opts.MapFrom(src => src.Categoria.Id))
                .ForMember(dest => dest.EstadoNombre, opts => opts.MapFrom(src => src.Estado.Nombre))
                .ForMember(dest => dest.ProcesoCobroNombre, opts => opts.MapFrom(src => src.ProcesoCobro.Nombre))
                .ForMember(dest => dest.TipoEquipoNombre, opts => opts.MapFrom(src => src.TipoEquipo.Nombre))
                .ForMember(dest => dest.UnidadCobroNombre, opts => opts.MapFrom(src => src.UnidadCobro.Nombre))
                .ForMember(dest => dest.CategoriaNombre, opts => opts.MapFrom(src => src.Categoria.Nombre))
                .ForMember(dest => dest.Tamano, opts => opts.Ignore());
            IList<ProductoModel> listaProductoModel = AutoMapper.Mapper.Map<IList<ProductoModel>>(ListaProducto);
            return listaProductoModel;
        }

        // Convierte Modelo en Entidad
        public Producto GetProducto(ProductoModel productoModelo, Producto producto)
        {
            AutoMapper.Mapper.CreateMap<ProductoModel, Producto>()
                .ForMember(dest => dest.Estado, opts => opts.Ignore())
                .ForMember(dest => dest.ProcesoCobro, opts => opts.Ignore())
                .ForMember(dest => dest.TipoEquipo, opts => opts.Ignore())
                .ForMember(dest => dest.UnidadCobro, opts => opts.Ignore())
                .ForMember(dest => dest.Tamano, opts => opts.Ignore())
                .ForMember(dest => dest.Categoria, opts => opts.Ignore());
            AutoMapper.Mapper.Map<ProductoModel, Producto>(productoModelo, producto);
            return producto;
        }

    }
}
