using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;
using WPP.Model.General;

namespace WPP.Mapper.General
{
    public class CostoHoraMapper
    {
        public CostoHoraModel GetCostoHoraModel(CostoHora catalogo)
        {
            AutoMapper.Mapper.CreateMap<CostoHora, CostoHoraModel>();
            CostoHoraModel catalogoModelo = AutoMapper.Mapper.Map<CostoHoraModel>(catalogo);
            return catalogoModelo;
        }

        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<CostoHoraModel> GetListaCostoHoraModel(IList<CostoHora> ListaCostoHora)
        {
            AutoMapper.Mapper.CreateMap<CostoHora, CostoHoraModel>();
            IList<CostoHoraModel> listaCostoHoraModel = AutoMapper.Mapper.Map<IList<CostoHoraModel>>(ListaCostoHora);
            return listaCostoHoraModel;
        }

        // Convierte Modelo en Entidad
        public CostoHora GetCostoHora(CostoHoraModel catalogoModelo, CostoHora catalogo)
        {
            AutoMapper.Mapper.CreateMap<CostoHoraModel, CostoHora>();
            AutoMapper.Mapper.Map<CostoHoraModel, CostoHora>(catalogoModelo, catalogo);
            return catalogo;
        }
    }
}
