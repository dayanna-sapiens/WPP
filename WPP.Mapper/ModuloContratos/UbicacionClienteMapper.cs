using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Model.ModuloContratos;

namespace WPP.Mapper.ModuloContratos
{
    public class UbicacionClienteMapper
    {
        public UbicacionClienteModel GetContratoClienteModel(UbicacionCliente contato)
        {
            AutoMapper.Mapper.CreateMap<UbicacionCliente, UbicacionClienteModel>()
                .ForMember(dest => dest.Cliente, opts => opts.MapFrom(src => src.Cliente.Id))
                .ForMember(dest => dest.Estado, opts => opts.MapFrom(src => src.Estado.Id))
                .ForMember(dest => dest.DescripcionEstado, opts => opts.MapFrom(src => src.Estado.Nombre))
                .ForMember(dest => dest.Descripción, opts => opts.MapFrom(src => src.Descripcion));

            UbicacionClienteModel ubicacionModel = AutoMapper.Mapper.Map<UbicacionClienteModel>(contato);
            return ubicacionModel;
        }

        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<UbicacionClienteModel> GetListaProductoModel(IList<UbicacionCliente> ListaUbicacionCliente)
        {
            AutoMapper.Mapper.CreateMap<UbicacionCliente, UbicacionClienteModel>()
                 .ForMember(dest => dest.Cliente, opts => opts.MapFrom(src => src.Cliente.Id))
                 .ForMember(dest => dest.Estado, opts => opts.MapFrom(src => src.Estado.Id))
                 .ForMember(dest => dest.DescripcionEstado, opts => opts.MapFrom(src => src.Estado.Nombre))
                 .ForMember(dest => dest.Descripción, opts => opts.MapFrom(src => src.Descripcion));
            IList<UbicacionClienteModel> listaUbicacionClienteModel = AutoMapper.Mapper.Map<IList<UbicacionClienteModel>>(ListaUbicacionCliente);
            return listaUbicacionClienteModel;
        }

        // Convierte Modelo en Entidad
        public UbicacionCliente GetUbicacionCliente(UbicacionClienteModel ubicacionModelo, UbicacionCliente ubicacion)
        {
            AutoMapper.Mapper.CreateMap<UbicacionClienteModel, UbicacionCliente>()
                .ForMember(dest => dest.Cliente , opts => opts.Ignore())
                .ForMember(dest => dest.Estado, opts => opts.Ignore());

            AutoMapper.Mapper.Map<UbicacionClienteModel, UbicacionCliente>(ubicacionModelo, ubicacion);
            ubicacion.Descripcion = ubicacionModelo.Descripción;

            return ubicacion;
        }
    }
}
