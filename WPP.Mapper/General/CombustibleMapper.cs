using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;
using WPP.Model.General;

namespace WPP.Mapper.General
{
    public class CombustibleMapper
    {
        public CombustibleModel GetCombustibleModel(Combustible catalogo)
        {
            AutoMapper.Mapper.CreateMap<Combustible, CombustibleModel>();
            CombustibleModel catalogoModelo = AutoMapper.Mapper.Map<CombustibleModel>(catalogo);
            return catalogoModelo;
        }

        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<CombustibleModel> GetListaCombustibleModel(IList<Combustible> ListaCombustible)
        {
            AutoMapper.Mapper.CreateMap<Combustible, CombustibleModel>();
            IList<CombustibleModel> listaCombustibleModel = AutoMapper.Mapper.Map<IList<CombustibleModel>>(ListaCombustible);
            return listaCombustibleModel;
        }

        // Convierte Modelo en Entidad
        public Combustible GetCombustible(CombustibleModel catalogoModelo, Combustible catalogo)
        {
            AutoMapper.Mapper.CreateMap<CombustibleModel, Combustible>();
            AutoMapper.Mapper.Map<CombustibleModel, Combustible>(catalogoModelo, catalogo);
            return catalogo;
        }
    }
}
