using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloCierreCaja;

namespace WPP.Entities.Mappings.ModuloCierreCaja
{
    public class EfectivoCierreCajaMapping : ClassMap<EfectivoCierreCaja>
    {
        public EfectivoCierreCajaMapping()
        {
            Table("EfectivoCierreCaja");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_EfectivoCierreCaja");
            Version(u => u.Version);
            Map(u => u.CreateDate).Not.Nullable();
            Map(u => u.CreatedBy).Not.Nullable();
            Map(u => u.DateLastModified).Nullable();
            Map(u => u.ModifiedBy).Nullable();
            Map(u => u.DeleteDate).Nullable();
            Map(u => u.DeletedBy).Nullable();
            Map(u => u.IsDeleted).Not.Nullable();

            Map(r => r.Cantidad).Not.Nullable();

            References(c => c.CierreCaja, "CierreCajaId").LazyLoad().Cascade.None();
            References(c => c.Denominacion, "DenominacionId").LazyLoad().Cascade.None();
        }
    }
}
