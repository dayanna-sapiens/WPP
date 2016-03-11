using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloNomina;
using WPP.Model.ModuloNomina;

namespace WPP.Mapper.ModuloNomina
{
    public class JornadaMapper
    {
        public JornadaModel GetJornadaModel(Jornada boleta)
        {
            AutoMapper.Mapper.CreateMap<Jornada, JornadaModel>();

            JornadaModel boletaModel = AutoMapper.Mapper.Map<JornadaModel>(boleta);
            return boletaModel;
        }

        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<JornadaModel> GetListaJornadaModel(IList<Jornada> ListaNomina)
        {
            AutoMapper.Mapper.CreateMap<Jornada, JornadaModel>();
            IList<JornadaModel> listaNominaModel = AutoMapper.Mapper.Map<IList<JornadaModel>>(ListaNomina);
            return listaNominaModel;
        }


        // Convierte Modelo en Entidad
        public Jornada GetJornada(JornadaModel boletaModelo, Jornada boleta)
        {
            AutoMapper.Mapper.CreateMap<JornadaModel, Jornada>();

            AutoMapper.Mapper.Map<JornadaModel, Jornada>(boletaModelo, boleta);

            return boleta;
        }
    }
}
