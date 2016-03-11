using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;
using WPP.Model.ModuloOperacionRecoleccion;

namespace WPP.Mapper.ModuloOperacionRecoleccion
{
    public class OTRMapper
    {
        public OTRModel GetBoletaOTRModel(OTR otr)
        {
            AutoMapper.Mapper.CreateMap<OTR, OTRModel>()
            .ForMember(dest => dest.RutaRecoleccion, opts => opts.MapFrom(src => src.RutaRecoleccion.Id))
            .ForMember(dest => dest.RutaRecoleccionDescripcion, opts => opts.MapFrom(src => src.RutaRecoleccion.Descripcion)) 
            .ForMember(dest => dest.Compania, opts => opts.MapFrom(src => src.Compania.Id))
            .ForMember(dest => dest.Origen, opts => opts.MapFrom(src => src.Origen.Id))
            .ForMember(dest => dest.OrigenDescripcion, opts => opts.MapFrom(src => src.Origen.Nombre))
            .ForMember(dest => dest.Equipo, opts => opts.MapFrom(src => src.Equipo.Id))
            .ForMember(dest => dest.NombreEquipo, opts => opts.MapFrom(src => src.Equipo.Nombre))
            .ForMember(dest => dest.Destino, opts => opts.MapFrom(src => src.Destino.Id))
            .ForMember(dest => dest.DestinoDescripcion, opts => opts.MapFrom(src => src.Destino.Nombre))
            .ForMember(dest => dest.Relleno, opts => opts.MapFrom(src => src.Relleno.Id))
            .ForMember(dest => dest.RellenoDescripcion, opts => opts.MapFrom(src => src.Relleno.Nombre))           
            .ForMember(dest => dest.Cuadrilla, opts => opts.MapFrom(src => src.Cuadrilla.Id))
            .ForMember(dest => dest.CuadrillaDescripcion, opts => opts.MapFrom(src => src.Cuadrilla.Descripcion))
            .ForMember(dest => dest.Cliente, opts => opts.MapFrom(src => src.Cliente.Id))
            .ForMember(dest => dest.DescripcionCliente, opts => opts.MapFrom(src => src.Cliente.Nombre))
            .ForMember(dest => dest.Contrato, opts => opts.MapFrom(src => src.Contrato.Id))
            .ForMember(dest => dest.DescripcionContrato, opts => opts.MapFrom(src => src.Contrato.DescripcionContrato));

            OTRModel otrModel = AutoMapper.Mapper.Map<OTRModel>(otr);
            return otrModel;
        }

        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<OTRModel> GetListaBoletaModel(IList<OTR> ListaOTR)
        {
            AutoMapper.Mapper.CreateMap<OTR, OTRModel>()
            .ForMember(dest => dest.RutaRecoleccion, opts => opts.MapFrom(src => src.RutaRecoleccion.Id))
            .ForMember(dest => dest.RutaRecoleccionDescripcion, opts => opts.MapFrom(src => src.RutaRecoleccion.Descripcion))
            .ForMember(dest => dest.Compania, opts => opts.MapFrom(src => src.Compania.Id))
            .ForMember(dest => dest.Origen, opts => opts.MapFrom(src => src.Origen.Id))
            .ForMember(dest => dest.OrigenDescripcion, opts => opts.MapFrom(src => src.Origen.Nombre))
            .ForMember(dest => dest.Equipo, opts => opts.MapFrom(src => src.Equipo.Id))
            .ForMember(dest => dest.NombreEquipo, opts => opts.MapFrom(src => src.Equipo.Nombre))
            .ForMember(dest => dest.Destino, opts => opts.MapFrom(src => src.Destino.Id))
            .ForMember(dest => dest.DestinoDescripcion, opts => opts.MapFrom(src => src.Destino.Nombre))
            .ForMember(dest => dest.Relleno, opts => opts.MapFrom(src => src.Relleno.Id))
            .ForMember(dest => dest.RellenoDescripcion, opts => opts.MapFrom(src => src.Relleno.Nombre))  
            .ForMember(dest => dest.Cuadrilla, opts => opts.MapFrom(src => src.Cuadrilla.Id))
            .ForMember(dest => dest.CuadrillaDescripcion, opts => opts.MapFrom(src => src.Cuadrilla.Descripcion))
            .ForMember(dest => dest.Cliente, opts => opts.MapFrom(src => src.Cliente.Id))
            .ForMember(dest => dest.DescripcionCliente, opts => opts.MapFrom(src => src.Cliente.Nombre))
            .ForMember(dest => dest.Contrato, opts => opts.MapFrom(src => src.Contrato.Id))
            .ForMember(dest => dest.DescripcionContrato, opts => opts.MapFrom(src => src.Contrato.DescripcionContrato));

            IList<OTRModel> listaOTRModel = AutoMapper.Mapper.Map<IList<OTRModel>>(ListaOTR);
            return listaOTRModel;
        }


        // Convierte Modelo en Entidad
        public OTR GetBoletaOTR(OTRModel otrModelo, OTR otr)
        {
            AutoMapper.Mapper.CreateMap<OTRModel, OTR>()
                .ForMember(dest => dest.RutaRecoleccion, opts => opts.Ignore())
                .ForMember(dest => dest.Compania, opts => opts.Ignore())
                .ForMember(dest => dest.Equipo, opts => opts.Ignore())
                .ForMember(dest => dest.Destino, opts => opts.Ignore())
                .ForMember(dest => dest.Origen, opts => opts.Ignore())
                .ForMember(dest => dest.Relleno, opts => opts.Ignore())
                .ForMember(dest => dest.Cuadrilla, opts => opts.Ignore())
                .ForMember(dest => dest.Cliente, opts => opts.Ignore())
                .ForMember(dest => dest.Contrato, opts => opts.Ignore())
                .ForMember(dest => dest.ListaViajesOTR, opts => opts.Ignore());

            AutoMapper.Mapper.Map<OTRModel, OTR>(otrModelo, otr);

            return otr;
        }
    }
}
