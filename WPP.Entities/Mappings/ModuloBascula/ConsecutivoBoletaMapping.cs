using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloBascula;

namespace WPP.Entities.Mappings.ModuloBascula
{
    public class ConsecutivoBoletaMapping : ClassMap<ConsecutivoBoleta>
    {
        public ConsecutivoBoletaMapping()
        {
            Table("ConsecutivoBoleta");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_ConsecutivoBoleta");
            Map(u => u.Secuencia).Not.Nullable();
            References(c => c.Compania, "CompaniaId").LazyLoad().Cascade.None();
        }
    }
}
