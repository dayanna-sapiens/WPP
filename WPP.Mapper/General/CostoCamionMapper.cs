using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;
using WPP.Model.General;

namespace WPP.Mapper.General
{
    public class CostoCamionMapper
    {
        public CostoCamionModel GetCostoHoraModel(CostoCamion catalogo)
        {
            AutoMapper.Mapper.CreateMap<CostoCamion, CostoCamionModel>()
                .ForMember(dest => dest.Tipo, opts => opts.MapFrom(src => src.Tipo.Id));
            CostoCamionModel catalogoModelo = AutoMapper.Mapper.Map<CostoCamionModel>(catalogo);
            return catalogoModelo;
        }

        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<CostoCamionModel> GetListaCostoHoraModel(IList<CostoCamion> ListaCostoHora)
        {
            AutoMapper.Mapper.CreateMap<CostoCamion, CostoCamionModel>()
                .ForMember(dest => dest.Tipo, opts => opts.MapFrom(src => src.Tipo.Id));
            IList<CostoCamionModel> listaCostoHoraModel = AutoMapper.Mapper.Map<IList<CostoCamionModel>>(ListaCostoHora);
            return listaCostoHoraModel;
        }

        // Convierte Modelo en Entidad
        public CostoCamion GetCostoHora(CostoCamionModel catalogoModelo, CostoCamion catalogo)
        {
            AutoMapper.Mapper.CreateMap<CostoCamionModel, CostoCamion>()
                .ForMember(dest => dest.Tipo, opts => opts.Ignore());
            AutoMapper.Mapper.Map<CostoCamionModel, CostoCamion>(catalogoModelo, catalogo);
            return catalogo;
        }
    }
}
