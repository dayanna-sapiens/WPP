using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;
using WPP.Model.ModuloOperacionRecoleccion;

namespace WPP.Mapper.ModuloOperacionRecoleccion
{
    public class EmpleadoRecoleccionMapper
    {
        public EmpleadoRecoleccionModel GetModel(EmpleadoRecoleccion entity)
        {
            AutoMapper.Mapper.CreateMap<EmpleadoRecoleccion, EmpleadoRecoleccionModel>()
            .ForMember(dest => dest.Compania, opts => opts.MapFrom(src => src.Compania.Id))
            .ForMember(dest => dest.Jornada, opts => opts.MapFrom(src => src.Jornada.Id))
            .ForMember(dest => dest.JornadaDescripcion, opts => opts.MapFrom(src => src.Jornada.Descripcion));

            EmpleadoRecoleccionModel clienteModel = AutoMapper.Mapper.Map<EmpleadoRecoleccionModel>(entity);
            return clienteModel;
        }


        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<EmpleadoRecoleccionModel> GetListaModel(IList<EmpleadoRecoleccion> Lista)
        {
            AutoMapper.Mapper.CreateMap<EmpleadoRecoleccion, EmpleadoRecoleccionModel>()
            .ForMember(dest => dest.Compania, opts => opts.MapFrom(src => src.Compania.Id))
            .ForMember(dest => dest.Jornada, opts => opts.MapFrom(src => src.Jornada.Id))
            .ForMember(dest => dest.JornadaDescripcion, opts => opts.MapFrom(src => src.Jornada.Descripcion));
            
            IList<EmpleadoRecoleccionModel> listaClienteModel = AutoMapper.Mapper.Map<IList<EmpleadoRecoleccionModel>>(Lista);
            return listaClienteModel;
        }

        // Convierte Modelo en Entidad
        public EmpleadoRecoleccion GetEntity(EmpleadoRecoleccionModel modelo, EmpleadoRecoleccion entity)
        {
            AutoMapper.Mapper.CreateMap<EmpleadoRecoleccionModel, EmpleadoRecoleccion>()
                .ForMember(dest => dest.Compania, opts => opts.Ignore())
                .ForMember(dest => dest.Jornada, opts => opts.Ignore());

            AutoMapper.Mapper.Map<EmpleadoRecoleccionModel, EmpleadoRecoleccion>(modelo, entity);

            return entity;
        }
    }
}
