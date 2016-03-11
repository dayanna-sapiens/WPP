using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;

namespace WPP.Entities.Mappings.Generales
{
    public class TipoCambioMapping : ClassMap<TipoCambio>
    {
        public TipoCambioMapping()
        {
            Table("TipoCambio");
           // Id(x => x.Id).Column("ID").GeneratedBy.Guid();
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_TipoCambio");
            Map(r => r.Fecha).Not.Nullable();
            Map(r => r.Tipo).Not.Nullable();
            Map(r => r.Valor).Not.Nullable();
        }
    }
}
