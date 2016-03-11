using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;
using WPP.Model.ModuloOperacionRecoleccion;

namespace WPP.Mapper.ModuloOperacionRecoleccion
{
    public class CuadrillaMapper
    {
        public CuadrillaModel GetModel(Cuadrilla entity)
        {
            AutoMapper.Mapper.CreateMap<Cuadrilla, CuadrillaModel>()
            .ForMember(dest => dest.Compania, opts => opts.MapFrom(src => src.Compania.Id));

            CuadrillaModel clienteModel = AutoMapper.Mapper.Map<CuadrillaModel>(entity);
            return clienteModel;
        }


        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<CuadrillaModel> GetListaModel(IList<Cuadrilla> Lista)
        {
            AutoMapper.Mapper.CreateMap<Cuadrilla, CuadrillaModel>()
            .ForMember(dest => dest.Compania, opts => opts.MapFrom(src => src.Compania.Id));

            IList<CuadrillaModel> listaClienteModel = AutoMapper.Mapper.Map<IList<CuadrillaModel>>(Lista);
            return listaClienteModel;
        }

        // Convierte Modelo en Entidad
        public Cuadrilla GetEntity(CuadrillaModel modelo, Cuadrilla entity)
        {
            AutoMapper.Mapper.CreateMap<CuadrillaModel, Cuadrilla>()
                .ForMember(dest => dest.Compania, opts => opts.Ignore())
                .ForMember(dest => dest.ListaEmpleados, opts => opts.Ignore());

            AutoMapper.Mapper.Map<CuadrillaModel, Cuadrilla>(modelo, entity);

            return entity;
        }
    }
}
