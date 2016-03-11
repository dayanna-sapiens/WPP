using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloCierreCaja;

namespace WPP.Entities.Mappings.ModuloCierreCaja
{
    public class ConsecutivoCierreCajaMapping: ClassMap<ConsecutivoCierreCaja>
    {
        public ConsecutivoCierreCajaMapping()
        {
            Table("ConsecutivoCierreCaja");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_ConsecutivoCierreCaja");
            Map(u => u.Secuencia).Not.Nullable();
            References(c => c.Compania, "CompaniaId").LazyLoad().Cascade.None();
        }
    }
}
