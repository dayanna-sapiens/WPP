using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Entities.Mappings.ModuloContratos
{
    public class ConsecutivoContratoMapping : ClassMap<ConsecutivoContrato>
    {
        public ConsecutivoContratoMapping()
        {
            Table("ConsecutivoContrato");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_ConsecutivoContrato");
            Map(u => u.Secuencia).Not.Nullable();
            References(c => c.Compania, "CompaniaId").LazyLoad().Cascade.None();
        }
    }
    
}
