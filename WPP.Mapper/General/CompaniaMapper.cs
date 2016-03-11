using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Model;


namespace WPP.Mapper
{
    public class CompaniaMapper
    {

        // Convierte Entidad en Modelo
        public CompaniaModel GetCompaniaModel(Compania compania)
        {
            AutoMapper.Mapper.CreateMap<Compania, CompaniaModel>()
                .ForMember(dest => dest.Tipo, opts => opts.MapFrom(src => src.Tipo.Id))
                .ForMember(dest => dest.Grupo, opts => opts.MapFrom(src => src.Grupo.Id));

            CompaniaModel companiaModelo = AutoMapper.Mapper.Map<CompaniaModel>(compania);
            return companiaModelo;
        }

        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<CompaniaModel> GetListaCompaniaModel(IList<Compania> ListaCompania)
        {
            AutoMapper.Mapper.CreateMap<Compania, CompaniaModel>()
                .ForMember(dest => dest.Tipo, opts => opts.MapFrom(src => src.Tipo.Id))
                .ForMember(dest => dest.Grupo, opts => opts.MapFrom(src => src.Grupo.Id));
            IList<CompaniaModel> listaCompaniaModel = AutoMapper.Mapper.Map<IList<CompaniaModel>>(ListaCompania);
            return listaCompaniaModel;
        }

        // Convierte Modelo en Entidad
        public Compania GetCompania(CompaniaModel companiaModelo, Compania compania)
        {
            AutoMapper.Mapper.CreateMap<CompaniaModel, Compania>()
                .ForMember(dest => dest.Tipo, opts => opts.Ignore())
                .ForMember(dest => dest.Grupo, opts => opts.Ignore());
            AutoMapper.Mapper.Map<CompaniaModel, Compania>(companiaModelo, compania);
            return compania;
        }

      
    }
}
