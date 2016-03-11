using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloNomina;
using WPP.Model.ModuloNomina;

namespace WPP.Mapper.ModuloNomina
{
    public class PlanillaMapper
    {
        public PlanillaModel GetBoletaNominaModel(Planilla boleta)
        {
            AutoMapper.Mapper.CreateMap<Planilla, PlanillaModel>()
            .ForMember(dest => dest.Compania, opts => opts.MapFrom(src => src.Compania.Id));

            PlanillaModel boletaModel = AutoMapper.Mapper.Map<PlanillaModel>(boleta);
            return boletaModel;
        }

        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<PlanillaModel> GetListaBoletaModel(IList<Planilla> ListaNomina)
        {
            AutoMapper.Mapper.CreateMap<Planilla, PlanillaModel>()
            .ForMember(dest => dest.Compania, opts => opts.MapFrom(src => src.Compania.Id));
            IList<PlanillaModel> listaNominaModel = AutoMapper.Mapper.Map<IList<PlanillaModel>>(ListaNomina);
            return listaNominaModel;
        }


        // Convierte Modelo en Entidad
        public Planilla GetBoletaNomina(PlanillaModel boletaModelo, Planilla boleta)
        {
            AutoMapper.Mapper.CreateMap<PlanillaModel, Planilla>()
                .ForMember(dest => dest.Compania, opts => opts.Ignore())
                .ForMember(dest => dest.DetallesNomina, opts => opts.Ignore());

            AutoMapper.Mapper.Map<PlanillaModel, Planilla>(boletaModelo, boleta);

            return boleta;
        }
    }
}
