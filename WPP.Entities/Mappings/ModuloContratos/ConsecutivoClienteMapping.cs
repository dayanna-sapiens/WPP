using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Entities.Mappings.ModuloContratos
{
    public class ConsecutivoClienteMapping: ClassMap<ConsecutivoCliente>
    {
        public ConsecutivoClienteMapping()
        {
            Table("ConsecutivoCliente");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_ConsecutivoCliente");
            Map(u => u.Secuencia).Not.Nullable();
        }
    }
}
