using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;
using WPP.Model.General;

namespace WPP.Mapper.General
{
    public class CatalogoMapper
    {
        // Convierte Entidad en Modelo
        public CatalogoModel GetCatalogoModel(Catalogo catalogo)
        {
            AutoMapper.Mapper.CreateMap<Catalogo, CatalogoModel>();
            CatalogoModel catalogoModelo = AutoMapper.Mapper.Map<CatalogoModel>(catalogo);
            return catalogoModelo;
        }

        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<CatalogoModel> GetListaCatalogoModel(IList<Catalogo> ListaCatalogo)
        {
            AutoMapper.Mapper.CreateMap<Catalogo, CatalogoModel>();
            IList<CatalogoModel> listaCatalogoModel = AutoMapper.Mapper.Map<IList<CatalogoModel>>(ListaCatalogo);
            return listaCatalogoModel;
        }

        // Convierte Modelo en Entidad
        public Catalogo GetCatalogo(CatalogoModel catalogoModelo, Catalogo catalogo)
        {
            AutoMapper.Mapper.CreateMap<CatalogoModel, Catalogo>();
            AutoMapper.Mapper.Map<CatalogoModel, Catalogo>(catalogoModelo, catalogo);
            return catalogo;
        }
    }
}
