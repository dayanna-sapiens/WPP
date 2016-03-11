using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Entities.Mappings.ModuloContratos
{
    public class ContratoBitacoraMapping: ClassMap<ContratoBitacora>
    {
        public ContratoBitacoraMapping()
        {
            Table("ContratoBitacora");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_ContratoBitacora");
        
            Map(u => u.CreateDate).Not.Nullable();
            Map(u => u.CreatedBy).Not.Nullable();
            Map(u => u.Campo).Nullable();
            Map(u => u.valorAnterior).Nullable();
            Map(u => u.valorNuevo).Nullable();

            References(c => c.Contrato, "IdContrato").Cascade.None();
        }

    }
}
