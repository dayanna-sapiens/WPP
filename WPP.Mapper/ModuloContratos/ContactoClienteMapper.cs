using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Model.General;
using WPP.Model.ModuloContratos;

namespace WPP.Mapper.ModuloContratos
{
    public class ContactoClienteMapper
    {
        public ContactoModel GetContratoClienteModel(ContactoCliente contato)
        {
            AutoMapper.Mapper.CreateMap<ContactoCliente, ContactoModel>()
                .ForMember(dest => dest.ParentId, opts => opts.MapFrom(src => src.Cliente.Id));


            ContactoModel contactoModel = AutoMapper.Mapper.Map<ContactoModel>(contato);
            return contactoModel;
        }

        // Convierte Modelo en Entidad
        public ContactoCliente GetContactoCliente(ContactoModel contactoModelo, ContactoCliente contacto)
        {
            AutoMapper.Mapper.CreateMap<ContactoModel, ContactoCliente>();

            AutoMapper.Mapper.Map<ContactoModel, ContactoCliente>(contactoModelo, contacto);
            
            return contacto;
        }


    }
}
