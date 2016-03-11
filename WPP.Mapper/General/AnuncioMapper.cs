using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;
using WPP.Model.General;

namespace WPP.Mapper.General
{
    public class AnuncioMapper
    {
        // Convierte Entidad en Modelo
        public AnuncioModel GetAnuncioModel(Anuncio anuncio)
        {
            AutoMapper.Mapper.CreateMap<Anuncio, AnuncioModel>();
            AnuncioModel anuncioModelo = AutoMapper.Mapper.Map<AnuncioModel>(anuncio);
            return anuncioModelo;
        }

        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<AnuncioModel> GetListaAnuncioModel(IList<Anuncio> ListaAnuncio)
        {
            AutoMapper.Mapper.CreateMap<Anuncio, AnuncioModel>();
            IList<AnuncioModel> listaAnuncioModel = AutoMapper.Mapper.Map<IList<AnuncioModel>>(ListaAnuncio);
            return listaAnuncioModel;
        }

        // Convierte Modelo en Entidad
        public Anuncio GetAnuncio(AnuncioModel anuncioModelo, Anuncio anuncio)
        {
            AutoMapper.Mapper.CreateMap<AnuncioModel, Anuncio>();
            AutoMapper.Mapper.Map<AnuncioModel, Anuncio>(anuncioModelo, anuncio);
            return anuncio;
        }
    }
}
