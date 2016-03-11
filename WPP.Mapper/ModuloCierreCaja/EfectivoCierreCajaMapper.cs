using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloCierreCaja;
using WPP.Model.ModuloCierreCaja;

namespace WPP.Mapper.ModuloCierreCaja
{
    public class EfectivoCierreCajaMapper
    {
        public EfectivoCierreCajaModel GetModel(EfectivoCierreCaja entity)
        {
            AutoMapper.Mapper.CreateMap<EfectivoCierreCaja, EfectivoCierreCajaModel>()
                 .ForMember(dest => dest.CierreCaja, opts => opts.MapFrom(src => src.CierreCaja.Id))
                 .ForMember(dest => dest.Denominacion, opts => opts.MapFrom(src => src.Denominacion.Id))
                 .ForMember(dest => dest.DenominacionDescripcion, opts => opts.MapFrom(src => src.Denominacion.Nombre));

            EfectivoCierreCajaModel clienteModel = AutoMapper.Mapper.Map<EfectivoCierreCajaModel>(entity);
            return clienteModel;
        }


        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<EfectivoCierreCajaModel> GetListaModel(IList<EfectivoCierreCaja> Lista)
        {
            AutoMapper.Mapper.CreateMap<EfectivoCierreCaja, EfectivoCierreCajaModel>()
                 .ForMember(dest => dest.CierreCaja, opts => opts.MapFrom(src => src.CierreCaja.Id))
                 .ForMember(dest => dest.Denominacion, opts => opts.MapFrom(src => src.Denominacion.Id))
                 .ForMember(dest => dest.DenominacionDescripcion, opts => opts.MapFrom(src => src.Denominacion.Nombre));

            IList<EfectivoCierreCajaModel> listaClienteModel = AutoMapper.Mapper.Map<IList<EfectivoCierreCajaModel>>(Lista);
            return listaClienteModel;
        }

        // Convierte Modelo en Entidad
        public EfectivoCierreCaja GetEntity(EfectivoCierreCajaModel modelo, EfectivoCierreCaja entity)
        {
            AutoMapper.Mapper.CreateMap<EfectivoCierreCajaModel, EfectivoCierreCaja>()
                .ForMember(dest => dest.CierreCaja, opts => opts.Ignore())
                .ForMember(dest => dest.Denominacion, opts => opts.Ignore());

            AutoMapper.Mapper.Map<EfectivoCierreCajaModel, EfectivoCierreCaja>(modelo, entity);

            return entity;
        }
    }
}
