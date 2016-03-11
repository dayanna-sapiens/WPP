using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloNomina;
using WPP.Model;

namespace WPP.Mapper.ModuloNomina
{
    public class ItemNominaMapper
    {
        public ItemNominaModel GetModel(ItemNomina entity)
        {
            AutoMapper.Mapper.CreateMap<ItemNomina, ItemNominaModel>()
                .ForMember(dest => dest.Empleado, opts => opts.MapFrom(src => src.Empleado.Id))
                .ForMember(dest => dest.CodigoEmpleado, opts => opts.MapFrom(src => src.Empleado.Codigo))
                .ForMember(dest => dest.NombreEmpleado, opts => opts.MapFrom(src => src.Empleado.Nombre))
                .ForMember(dest => dest.Nomina, opts => opts.MapFrom(src => src.Nomina.Id))
                .ForMember(dest => dest.DescripcionNomina, opts => opts.MapFrom(src => src.Nomina.Descripcion));

            ItemNominaModel contenedorModel = AutoMapper.Mapper.Map<ItemNominaModel>(entity);
            return contenedorModel;
        }


        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<ItemNominaModel> GetListaModel(IList<ItemNomina> Lista)
        {
            AutoMapper.Mapper.CreateMap<ItemNomina, ItemNominaModel>()
                .ForMember(dest => dest.Empleado, opts => opts.MapFrom(src => src.Empleado.Id))
                .ForMember(dest => dest.CodigoEmpleado, opts => opts.MapFrom(src => src.Empleado.Codigo))
                .ForMember(dest => dest.NombreEmpleado, opts => opts.MapFrom(src => src.Empleado.Nombre))
                .ForMember(dest => dest.Nomina, opts => opts.MapFrom(src => src.Nomina.Id))
                .ForMember(dest => dest.DescripcionNomina, opts => opts.MapFrom(src => src.Nomina.Descripcion));

            IList<ItemNominaModel> listaItemNominaModel = AutoMapper.Mapper.Map<IList<ItemNominaModel>>(Lista);
            return listaItemNominaModel;
        }

        // Convierte Modelo en Entidad
        public ItemNomina GetEntity(ItemNominaModel modelo, ItemNomina entity)
        {
            AutoMapper.Mapper.CreateMap<ItemNominaModel, ItemNomina>()
                .ForMember(dest => dest.Empleado, opts => opts.Ignore())
                .ForMember(dest => dest.Nomina, opts => opts.Ignore());

            AutoMapper.Mapper.Map<ItemNominaModel, ItemNomina>(modelo, entity);

            return entity;
        }
    }
}
