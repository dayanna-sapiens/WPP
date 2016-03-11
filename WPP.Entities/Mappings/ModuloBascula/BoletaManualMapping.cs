using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloBascula;

namespace WPP.Entities.Mappings.ModuloBascula
{
    public class BoletaManualMapping : ClassMap<BoletaManual>
    {
        public BoletaManualMapping()
        {
            Table("BoletaManual");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_BoletaManual");
            Version(u => u.Version);
            Map(u => u.CreateDate).Not.Nullable();
            Map(u => u.CreatedBy).Not.Nullable();
            Map(u => u.DateLastModified).Nullable();
            Map(u => u.ModifiedBy).Nullable();
            Map(u => u.DeleteDate).Nullable();
            Map(u => u.DeletedBy).Nullable();
            Map(u => u.IsDeleted).Not.Nullable();

            Map(u => u.NumeroBoleta).Not.Nullable();
            Map(u => u.Fecha).Not.Nullable();
            Map(u => u.Hora).Nullable();
            Map(u => u.DescripcionCliente).Nullable();
            Map(u => u.Estado).Not.Nullable();
            Map(u => u.Observaciones).Nullable();
            Map(u => u.PesoBruto).Not.Nullable();
            Map(u => u.PesoNeto).Not.Nullable();
            Map(u => u.PesoTara).Not.Nullable();
            Map(r => r.Facturada).Not.Nullable();

            References(c => c.Compania, "CompaniaId").LazyLoad().Cascade.None();
            References(c => c.Sitio, "SitioId").LazyLoad().Cascade.None();
            References(c => c.OTR, "OTRId").LazyLoad().Cascade.None();
        }
    }
}
