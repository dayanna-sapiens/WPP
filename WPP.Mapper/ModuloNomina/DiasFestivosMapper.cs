using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloNomina;
using WPP.Model.ModuloNomina;

namespace WPP.Mapper.ModuloNomina
{
    public class DiasFestivosMapper
    {
        public DiasFestivosModel GetBoletaDiasFestivosModel(DiasFestivos boleta)
        {
            AutoMapper.Mapper.CreateMap<DiasFestivos, DiasFestivosModel>();

            DiasFestivosModel boletaModel = AutoMapper.Mapper.Map<DiasFestivosModel>(boleta);
            return boletaModel;
        }

        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<DiasFestivosModel> GetListaBoletaModel(IList<DiasFestivos> ListaDiasFestivos)
        {
            AutoMapper.Mapper.CreateMap<DiasFestivos, DiasFestivosModel>();
            IList<DiasFestivosModel> listaDiasFestivosModel = AutoMapper.Mapper.Map<IList<DiasFestivosModel>>(ListaDiasFestivos);
            return listaDiasFestivosModel;
        }


        // Convierte Modelo en Entidad
        public DiasFestivos GetBoletaDiasFestivos(DiasFestivosModel boletaModelo, DiasFestivos boleta)
        {
            AutoMapper.Mapper.CreateMap<DiasFestivosModel, DiasFestivos>();

            AutoMapper.Mapper.Map<DiasFestivosModel, DiasFestivos>(boletaModelo, boleta);

            return boleta;
        }
    }
}
