using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloNomina;
using WPP.Model.ModuloNomina;

namespace WPP.Mapper.ModuloNomina
{
    public class ConsecutivoNominaMapper
    {
        public ConsecutivoNominaModel GetBoletaConsecutivoNominaModel(ConsecutivoNomina boleta)
        {
            AutoMapper.Mapper.CreateMap<ConsecutivoNomina, ConsecutivoNominaModel>()
            .ForMember(dest => dest.Compania, opts => opts.MapFrom(src => src.Compania.Id));

            ConsecutivoNominaModel boletaModel = AutoMapper.Mapper.Map<ConsecutivoNominaModel>(boleta);
            return boletaModel;
        }

        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<ConsecutivoNominaModel> GetListaBoletaModel(IList<ConsecutivoNomina> ListaConsecutivoNomina)
        {
            AutoMapper.Mapper.CreateMap<ConsecutivoNomina, ConsecutivoNominaModel>()
            .ForMember(dest => dest.Compania, opts => opts.MapFrom(src => src.Compania.Id));
            IList<ConsecutivoNominaModel> listaConsecutivoNominaModel = AutoMapper.Mapper.Map<IList<ConsecutivoNominaModel>>(ListaConsecutivoNomina);
            return listaConsecutivoNominaModel;
        }


        // Convierte Modelo en Entidad
        public ConsecutivoNomina GetBoletaConsecutivoNomina(ConsecutivoNominaModel boletaModelo, ConsecutivoNomina boleta)
        {
            AutoMapper.Mapper.CreateMap<ConsecutivoNominaModel, ConsecutivoNomina>()
                .ForMember(dest => dest.Compania, opts => opts.Ignore());

            AutoMapper.Mapper.Map<ConsecutivoNominaModel, ConsecutivoNomina>(boletaModelo, boleta);

            return boleta;
        }
    }
}
