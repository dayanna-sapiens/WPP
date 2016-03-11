using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;

namespace WPP.Entities.Mappings.Generales
{
    public class CostoCamionMapping: ClassMap<CostoCamion>
    {
        public CostoCamionMapping()
        {
            Table("CostoCamion");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_CostoCamion");
            Version(u => u.Version);
            Map(u => u.CreateDate).Not.Nullable();
            Map(u => u.CreatedBy).Not.Nullable();
            Map(u => u.DateLastModified).Nullable();
            Map(u => u.ModifiedBy).Nullable();
            Map(u => u.DeleteDate).Nullable();
            Map(u => u.DeletedBy).Nullable();
            Map(u => u.IsDeleted).Not.Nullable();
            Map(r => r.Monto).Not.Nullable();
            Map(r => r.FechaDesde).Not.Nullable();
            Map(r => r.FechaHasta).Not.Nullable();

            References(c => c.Tipo, "TipoId").LazyLoad().Cascade.None();
        }
    }
}
