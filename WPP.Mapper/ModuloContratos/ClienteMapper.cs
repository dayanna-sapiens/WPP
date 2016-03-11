using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Model.ModuloContratos;

namespace WPP.Mapper.ModuloContratos
{
    public class ClienteMapper
    {
        public ClienteModel GetClienteModel(Cliente compania)
        {
            AutoMapper.Mapper.CreateMap<Cliente, ClienteModel>().ForMember(dest => dest.Canton, opts => opts.MapFrom(src => src.Canton.Id))
                .ForMember(dest => dest.Distrito, opts => opts.MapFrom(src => src.Distrito.Id))
                .ForMember(dest => dest.Provincia, opts => opts.MapFrom(src => src.Provincia.Id))
                .ForMember(dest => dest.Tipo, opts => opts.MapFrom(src => src.Tipo.Id))
                .ForMember(dest => dest.Grupo, opts => opts.MapFrom(src => src.Grupo.Id));

            ClienteModel clienteModel = AutoMapper.Mapper.Map<ClienteModel>(compania);
            return clienteModel;
        }


        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<ClienteModel> GetListaClienteModel(IList<Cliente> ListaCliente)
        {
            AutoMapper.Mapper.CreateMap<Cliente, ClienteModel>()
               .ForMember(dest => dest.Canton, opts => opts.MapFrom(src => src.Canton.Id))
                .ForMember(dest => dest.Distrito, opts => opts.MapFrom(src => src.Distrito.Id))
                .ForMember(dest => dest.Provincia, opts => opts.MapFrom(src => src.Provincia.Id))
                .ForMember(dest => dest.Tipo, opts => opts.MapFrom(src => src.Tipo.Id))
                .ForMember(dest => dest.Grupo, opts => opts.MapFrom(src => src.Grupo.Id));
            IList<ClienteModel> listaClienteModel = AutoMapper.Mapper.Map<IList<ClienteModel>>(ListaCliente);
            return listaClienteModel;
        }

        // Convierte Modelo en Entidad
        public Cliente GetCliente(ClienteModel clienteModelo, Cliente cliente)
        {
            AutoMapper.Mapper.CreateMap<ClienteModel, Cliente>()
                .ForMember(dest => dest.Canton, opts => opts.Ignore())
                .ForMember(dest => dest.Distrito, opts => opts.Ignore())
                .ForMember(dest => dest.Provincia, opts => opts.Ignore())
                .ForMember(dest => dest.Tipo, opts => opts.Ignore())
                .ForMember(dest => dest.Grupo, opts => opts.Ignore())
                .ForMember(dest => dest.Contactos, opts => opts.Ignore())
                .ForMember(dest => dest.Ubicaciones, opts => opts.Ignore())
                .ForMember(dest => dest.Contratos, opts => opts.Ignore());

            AutoMapper.Mapper.Map<ClienteModel,Cliente>(clienteModelo, cliente);
            
            return cliente;
        }


    }
}
