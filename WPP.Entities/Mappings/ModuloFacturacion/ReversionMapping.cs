using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloFacturacion;

namespace WPP.Entities.Mappings.ModuloFacturacion
{
    public class ReversionMapping: ClassMap<Reversion>
    {
        public ReversionMapping()
        {
            Table("Reversion");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_Reversion");
            Version(u => u.Version);
            Map(u => u.CreateDate).Not.Nullable();
            Map(u => u.CreatedBy).Not.Nullable();
            Map(u => u.DateLastModified).Nullable();
            Map(u => u.ModifiedBy).Nullable();
            Map(u => u.DeleteDate).Nullable();
            Map(u => u.DeletedBy).Nullable();
            Map(u => u.IsDeleted).Not.Nullable();

            Map(u => u.Observaciones).Nullable();
            Map(u => u.Consecutivo).Nullable();

            References(c => c.Facturacion, "FacturacionId").LazyLoad().Cascade.None();
            References(c => c.Compania, "CompaniaId").LazyLoad().Cascade.None();
        }
    }
}
