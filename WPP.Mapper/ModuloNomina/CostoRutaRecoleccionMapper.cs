using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloNomina;
using WPP.Model.ModuloNomina;

namespace WPP.Mapper.ModuloNomina
{
    public class CostoRutaRecoleccionMapper
    {
        public CostoRutaRecoleccionModel GetModel(CostoRutaRecoleccion entity)
        {
            AutoMapper.Mapper.CreateMap<CostoRutaRecoleccion, CostoRutaRecoleccionModel>()
                .ForMember(dest => dest.RutaRecoleccion, opts => opts.MapFrom(src => src.RutaRecoleccion.Id))
                .ForMember(dest => dest.RutaRecoleccionDescripcion, opts => opts.MapFrom(src => src.RutaRecoleccion.Descripcion));

            CostoRutaRecoleccionModel contenedorModel = AutoMapper.Mapper.Map<CostoRutaRecoleccionModel>(entity);
            return contenedorModel;
        }


        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<CostoRutaRecoleccionModel> GetListaModel(IList<CostoRutaRecoleccion> Lista)
        {
            AutoMapper.Mapper.CreateMap<CostoRutaRecoleccion, CostoRutaRecoleccionModel>()
                .ForMember(dest => dest.RutaRecoleccion, opts => opts.MapFrom(src => src.RutaRecoleccion.Id))
                .ForMember(dest => dest.RutaRecoleccionDescripcion, opts => opts.MapFrom(src => src.RutaRecoleccion.Descripcion));

            IList<CostoRutaRecoleccionModel> listaCostoRutaRecoleccionModel = AutoMapper.Mapper.Map<IList<CostoRutaRecoleccionModel>>(Lista);
            return listaCostoRutaRecoleccionModel;
        }

        // Convierte Modelo en Entidad
        public CostoRutaRecoleccion GetEntity(CostoRutaRecoleccionModel modelo, CostoRutaRecoleccion entity)
        {
            AutoMapper.Mapper.CreateMap<CostoRutaRecoleccionModel, CostoRutaRecoleccion>()
                .ForMember(dest => dest.RutaRecoleccion, opts => opts.Ignore());

            AutoMapper.Mapper.Map<CostoRutaRecoleccionModel, CostoRutaRecoleccion>(modelo, entity);

            return entity;
        }
    }
}
