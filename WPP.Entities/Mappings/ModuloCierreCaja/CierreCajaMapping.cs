using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloCierreCaja;

namespace WPP.Entities.Mappings.ModuloCierreCaja
{
    public class CierreCajaMapping:ClassMap<CierreCaja>
    {
        public CierreCajaMapping()
        {
            Table("CierreCaja");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_CierreCaja");
            Version(u => u.Version);
            Map(u => u.CreateDate).Not.Nullable();
            Map(u => u.CreatedBy).Not.Nullable();
            Map(u => u.DateLastModified).Nullable();
            Map(u => u.ModifiedBy).Nullable();
            Map(u => u.DeleteDate).Nullable();
            Map(u => u.DeletedBy).Nullable();
            Map(u => u.IsDeleted).Not.Nullable();

            Map(r => r.AjusteCaja).Nullable();
            Map(r => r.BalanceApertura).Not.Nullable();
            Map(r => r.BalanceCierre).Nullable();
            Map(r => r.ConteoTotal).Nullable();
            Map(r => r.Efectivo).Nullable();
            Map(r => r.Moneda).Not.Nullable();
            Map(r => r.Tarjetas).Nullable();
            Map(r => r.Transferencia).Nullable();
            Map(r => r.Diferencia).Nullable();
            Map(r => r.Consecutivo).Nullable();


            References(c => c.Compania, "CompaniaId").LazyLoad().Cascade.None();

            HasMany(p => p.ListaPagoEfectivo)
             .KeyColumn("CierreCajaId")
             .Inverse()
             .LazyLoad()
             .Cascade.AllDeleteOrphan()
             .AsBag();

            HasMany(p => p.Pagos)
            .KeyColumn("CierreCajaId")
            .Inverse()
            .LazyLoad()
            .Cascade.AllDeleteOrphan()
            .AsBag();

            HasMany(p => p.Creditos)
                .KeyColumn("CierreCajaId")
                .Inverse()
                .LazyLoad()
                .Cascade.AllDeleteOrphan()
                .AsBag();
            
        }
    }
}
