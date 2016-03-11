using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloBascula;

namespace WPP.Entities.Mappings.ModuloBascula
{
    public class PagoBasculaMapping : ClassMap<PagoBascula>
    {
        public PagoBasculaMapping()
        {
            Table("PagoBascula");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_PagoBascula");
            Version(u => u.Version);
            Map(u => u.CreateDate).Not.Nullable();
            Map(u => u.CreatedBy).Not.Nullable();
            Map(u => u.DateLastModified).Nullable();
            Map(u => u.ModifiedBy).Nullable();
            Map(u => u.DeleteDate).Nullable();
            Map(u => u.DeletedBy).Nullable();
            Map(u => u.IsDeleted).Not.Nullable();

            Map(u => u.Monto).Not.Nullable();
            Map(u => u.Moneda).Not.Nullable();
            Map(u => u.FormaPago).Not.Nullable();
            Map(u => u.TipoCambio).Nullable();
            Map(u => u.NumeroAprobacion).Nullable();
            Map(u => u.NumeroTarjeta).Nullable();
            Map(u => u.CierreCaja).Nullable();
            Map(u => u.Fecha).Nullable();
            Map(u => u.DetalleTransferencia).Nullable();
            Map(u => u.NumeroTransferencia).Nullable();

            References(c => c.Boleta, "BoletaId").LazyLoad().Cascade.None();
            References(c => c.Banco, "BancoId").LazyLoad().Cascade.None();
            References(c => c.Cierre, "CierreCajaId").LazyLoad().Cascade.None();
        }
    }
}
