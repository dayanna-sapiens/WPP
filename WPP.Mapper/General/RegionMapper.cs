using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects;
using WPP.Model.General;

namespace WPP.Mapper.General
{
    public class RegionMapper
    {
        // Convierte Entidad en Modelo
        public RegionModel GetRegionModel(Region region)
        {
            AutoMapper.Mapper.CreateMap<Region, RegionModel>();
            RegionModel regionModelo = AutoMapper.Mapper.Map<RegionModel>(region);
            return regionModelo;
        }

        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<RegionModel> GetListaRegionModel(IList<Region> ListaRegion)
        {
            AutoMapper.Mapper.CreateMap<Region, RegionModel>();
            IList<RegionModel> listaRegionModel = AutoMapper.Mapper.Map<IList<RegionModel>>(ListaRegion);
            return listaRegionModel;
        }

        // Convierte Modelo en Entidad
        public Region GetRegion(RegionModel regionModelo, Region region)
        {
            AutoMapper.Mapper.CreateMap<RegionModel, Region>();
            AutoMapper.Mapper.Map<RegionModel, Region>(regionModelo, region);
            return region;
        }
    }
}
