using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Model.ModuloContratos;

namespace WPP.Mapper.ModuloContratos
{
    public class ServicioMapper
    {
        // Convierte Entidad en Modelo
        public ServicioModel GetServicioModel(Servicio servicio)
        {
            AutoMapper.Mapper.CreateMap<Servicio, ServicioModel>()
            .ForMember(dest => dest.Compania, opts => opts.MapFrom(src => src.Compania.Id));
                

            ServicioModel productoModelo = AutoMapper.Mapper.Map<ServicioModel>(servicio);
            return productoModelo;
        }

        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<ServicioModel> GetListaServicioModel(IList<Servicio> ListaProducto)
        {
            AutoMapper.Mapper.CreateMap<Servicio, ServicioModel>()
            .ForMember(dest => dest.Compania, opts => opts.MapFrom(src => src.Compania.Id));
           
            IList<ServicioModel> listaProductoModel = AutoMapper.Mapper.Map<IList<ServicioModel>>(ListaProducto);
            return listaProductoModel;
        }

        // Convierte Modelo en Entidad
        public Servicio GetServicio(ServicioModel servicioModel, Servicio servicio)
        {
            AutoMapper.Mapper.CreateMap<ServicioModel, Servicio>()
            .ForMember(dest => dest.Compania, opts => opts.Ignore());

            AutoMapper.Mapper.Map<ServicioModel, Servicio>(servicioModel, servicio);
            return servicio;
        }
    }
}
