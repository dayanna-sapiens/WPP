using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;

namespace WPP.Entities.Mappings.ModuloOperacionRecoleccion
{
    public class ConsecutivoOTRMapping: ClassMap<ConsecutivoOTR>
    {
        public ConsecutivoOTRMapping()
        {
            Table("ConsecutivoOTR");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_ConsecutivoOTR");
            Map(u => u.Secuencia).Not.Nullable();
            References(c => c.Compania, "CompaniaId").LazyLoad().Cascade.None();
        }
    }
}
