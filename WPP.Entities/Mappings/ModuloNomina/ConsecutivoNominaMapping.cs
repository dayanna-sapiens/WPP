using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloNomina;

namespace WPP.Entities.Mappings.ModuloNomina
{
    public class ConsecutivoNominaMapping : ClassMap<ConsecutivoNomina>
    {
        public ConsecutivoNominaMapping()
        {
            Table("ConsecutivoNomina");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_ConsecutivoNomina");
            Map(u => u.Secuencia).Not.Nullable();
            References(c => c.Compania, "CompaniaId").LazyLoad().Cascade.None();
        }
    }
}
