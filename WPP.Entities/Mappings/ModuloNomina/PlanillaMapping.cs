using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloNomina;

namespace WPP.Entities.Mappings.ModuloNomina
{
    public class PlanillaMapping : ClassMap<Planilla>
    {
        public PlanillaMapping()
        {
            Table("Planilla");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_Planilla");
            Map(u => u.CreateDate).Not.Nullable();
            Map(u => u.CreatedBy).Not.Nullable();
            Map(u => u.Version).Not.Nullable();
            Map(u => u.DateLastModified).Nullable();
            Map(u => u.IsDeleted).Not.Nullable();
            Map(u => u.ModifiedBy).Nullable();
            Map(u => u.DeleteDate).Nullable();
            Map(u => u.DeletedBy).Nullable();

            Map(u => u.Descripcion).Nullable();
            Map(u => u.Estado).Nullable();

            References(c => c.Compania, "CompaniaId").LazyLoad().Cascade.None();

            HasMany(p => p.DetallesNomina)
                 .KeyColumn("PlanillaId")
                 .Inverse()
                 .LazyLoad()
                 .Cascade.AllDeleteOrphan()
                 .AsBag();
        }
    }
}
