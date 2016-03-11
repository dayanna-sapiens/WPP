using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;

namespace WPP.Entities.Mappings.ModuloContratos
{
    public class ClienteBitacoraMapping : ClassMap<ClienteBitacora>
    {
        public ClienteBitacoraMapping()
        {
            Table("ClienteBitacora");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_ClienteBitacora");
        
            Map(u => u.CreateDate).Not.Nullable();
            Map(u => u.CreatedBy).Not.Nullable();
            Map(u => u.Campo).Nullable();
            Map(u => u.valorAnterior).Nullable();
            Map(u => u.valorNuevo).Nullable();

            References(c => c.Cliente, "IdCliente").Cascade.None();
        }
    }
}
