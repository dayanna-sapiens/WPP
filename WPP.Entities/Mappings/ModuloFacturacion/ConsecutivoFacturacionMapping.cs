using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloFacturacion;

namespace WPP.Entities.Mappings.ModuloFacturacion
{
    public class ConsecutivoFacturacionMapping : ClassMap<ConsecutivoFacturacion>
    {
        public ConsecutivoFacturacionMapping()
        {
            Table("ConsecutivoFacturacion");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_ConsecutivoFacturacion");
            Map(u => u.Secuencia).Not.Nullable();
            References(c => c.Compania, "CompaniaId").LazyLoad().Cascade.None();
        }
    }
}
