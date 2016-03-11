using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;
using WPP.Model.ModuloContratos;
using WPP.Model.ModuloOperacionRecoleccion;

namespace WPP.Mapper.ModuloOperacionRecoleccion
{
    public class RutaRecoleccionMapper
    {
        public RutaRecoleccionModel GetRutaRecoleccionModel(RutaRecoleccion contato)
        {
            AutoMapper.Mapper.CreateMap<RutaRecoleccion, RutaRecoleccionModel>()
                .ForMember(dest => dest.Compania, opts => opts.MapFrom(src => src.Compania.Id));

            RutaRecoleccionModel rutaModel = AutoMapper.Mapper.Map<RutaRecoleccionModel>(contato);
            return rutaModel;
        }

        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<RutaRecoleccionModel> GetListaRutaRecoleccionModel(IList<RutaRecoleccion> ListaRutaRecoleccion)
        {
            AutoMapper.Mapper.CreateMap<RutaRecoleccion, RutaRecoleccionModel>()
                 .ForMember(dest => dest.Compania, opts => opts.MapFrom(src => src.Compania.Id));
            IList<RutaRecoleccionModel> listaRutaRecoleccionModel = AutoMapper.Mapper.Map<IList<RutaRecoleccionModel>>(ListaRutaRecoleccion);
            return listaRutaRecoleccionModel;
        }

        // Convierte Modelo en Entidad
        public RutaRecoleccion GetRutaRecoleccion(RutaRecoleccionModel rutaModelo, RutaRecoleccion ruta)
        {
            AutoMapper.Mapper.CreateMap<RutaRecoleccionModel, RutaRecoleccion>()
                .ForMember(dest => dest.Compania, opts => opts.Ignore())
                .ForMember(dest => dest.Rutas, opts => opts.Ignore());

            AutoMapper.Mapper.Map<RutaRecoleccionModel, RutaRecoleccion>(rutaModelo, ruta);

            return ruta;
        }
    }
}
