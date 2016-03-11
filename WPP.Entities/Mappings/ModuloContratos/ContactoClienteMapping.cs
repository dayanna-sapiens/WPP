using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Entities.Mappings.ModuloContratos
{
    public class ContactoClienteMapping : ClassMap<ContactoCliente>
    {
        public ContactoClienteMapping()
        {
            Table("ContactoCliente");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_ContactoCliente");
            Version(u => u.Version);
            Map(u => u.CreateDate).Not.Nullable();
            Map(u => u.CreatedBy).Not.Nullable();
            Map(u => u.DateLastModified).Nullable();
            Map(u => u.ModifiedBy).Nullable();
            Map(u => u.DeleteDate).Nullable();
            Map(u => u.DeletedBy).Nullable();
            Map(u => u.IsDeleted).Not.Nullable();
            Map(r => r.Nombre).Not.Nullable();
            Map(r => r.Email).Nullable();
            Map(r => r.Telefono1).Nullable();
            Map(r => r.Ext1).Nullable();
            Map(r => r.Telefono2).Nullable();
            Map(r => r.Ext2).Nullable();
            Map(r => r.Cedula).Nullable();
            Map(r => r.Observaciones).Nullable();
            Map(r => r.Horario).Nullable();

            References(c => c.Cliente, "IdCliente").Cascade.None();
        }
    }
}
