using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;
using WPP.Model.General;

namespace WPP.Mapper.General
{
    public class PuntoFacturacionMapper
    {
           // Convierte Entidad en Modelo
        public  PuntoFacturacionModel GetPuntoFacturacionModel(PuntoFacturacion entity)
        {
            AutoMapper.Mapper.CreateMap<PuntoFacturacion, PuntoFacturacionModel>();

            PuntoFacturacionModel model = AutoMapper.Mapper.Map<PuntoFacturacionModel>(entity);
            return model;
        }

        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<PuntoFacturacionModel> GetListaPuntoFacturacionModel(IList<PuntoFacturacion> lista)
        {
            AutoMapper.Mapper.CreateMap<PuntoFacturacion, PuntoFacturacionModel>();

            IList<PuntoFacturacionModel> listaModel = AutoMapper.Mapper.Map<IList<PuntoFacturacionModel>>(lista);
            return listaModel;
        }

        // Convierte Modelo en Entidad
        public PuntoFacturacion GetPuntoFacturacion(PuntoFacturacionModel model, PuntoFacturacion entity)
        {
            AutoMapper.Mapper.CreateMap<PuntoFacturacionModel, PuntoFacturacion>();

            AutoMapper.Mapper.Map<PuntoFacturacionModel, PuntoFacturacion>(model, entity);
            return entity;
        }
    }
}
