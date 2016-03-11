using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Entities.Mappings.ModuloContratos
{
    public class UbicacionClienteMapping: ClassMap<UbicacionCliente>
    {
        public UbicacionClienteMapping()
        {
            Table("UbicacionCliente");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_UbicacionCliente");
            Version(u => u.Version);
            Map(u => u.CreateDate).Not.Nullable();
            Map(u => u.CreatedBy).Not.Nullable();
            Map(u => u.DateLastModified).Nullable();
            Map(u => u.ModifiedBy).Nullable();
            Map(u => u.DeleteDate).Nullable();
            Map(u => u.DeletedBy).Nullable();
            Map(u => u.IsDeleted).Not.Nullable();
            Map(r => r.Descripcion).Not.Nullable();
            Map(r => r.Email).Nullable();
            Map(r => r.Direccion).Nullable();
            Map(r => r.Telefono).Nullable();
            Map(r => r.Contacto).Nullable();

            References(c => c.Cliente, "IdCliente").Cascade.None();
            References(c => c.Estado, "IdEstado").Cascade.None();
        }
    }
}
