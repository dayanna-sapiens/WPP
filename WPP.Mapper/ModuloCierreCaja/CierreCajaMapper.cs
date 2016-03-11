using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloCierreCaja;
using WPP.Model.ModuloCierreCaja;

namespace WPP.Mapper.ModuloCierreCaja
{
    public class CierreCajaMapper
    {
        public CierreCajaModel GetModel(CierreCaja entity)
        {
            AutoMapper.Mapper.CreateMap<CierreCaja, CierreCajaModel>()
                 .ForMember(dest => dest.Compania, opts => opts.MapFrom(src => src.Compania.Id));

            CierreCajaModel clienteModel = AutoMapper.Mapper.Map<CierreCajaModel>(entity);
            return clienteModel;
        }


        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<CierreCajaModel> GetListaModel(IList<CierreCaja> Lista)
        {
            AutoMapper.Mapper.CreateMap<CierreCaja, CierreCajaModel>()
                 .ForMember(dest => dest.Compania, opts => opts.MapFrom(src => src.Compania.Id));

            IList<CierreCajaModel> listaClienteModel = AutoMapper.Mapper.Map<IList<CierreCajaModel>>(Lista);
            return listaClienteModel;
        }

        // Convierte Modelo en Entidad
        public CierreCaja GetEntity(CierreCajaModel modelo, CierreCaja entity)
        {
            AutoMapper.Mapper.CreateMap<CierreCajaModel, CierreCaja>()
                .ForMember(dest => dest.Compania, opts => opts.Ignore())
                .ForMember(dest => dest.ListaPagoEfectivo, opts => opts.Ignore())
                .ForMember(dest => dest.Creditos, opts => opts.Ignore())
                .ForMember(dest => dest.Pagos, opts => opts.Ignore());

            AutoMapper.Mapper.Map<CierreCajaModel, CierreCaja>(modelo, entity);

            return entity;
        }
    }
}
