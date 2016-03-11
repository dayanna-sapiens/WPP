using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloFacturacion;

namespace WPP.Entities.Mappings.ModuloFacturacion
{
    public class DetalleFacturacionMapping : ClassMap<DetalleFacturacion>
    {
        public DetalleFacturacionMapping()
        {
            Table("DetalleFacturacion");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_DetalleFacturacion");
            Version(u => u.Version);
            Map(u => u.CreateDate).Not.Nullable();
            Map(u => u.CreatedBy).Not.Nullable();
            Map(u => u.DateLastModified).Nullable();
            Map(u => u.ModifiedBy).Nullable();
            Map(u => u.DeleteDate).Nullable();
            Map(u => u.DeletedBy).Nullable();
            Map(u => u.IsDeleted).Not.Nullable();

            Map(u => u.Moneda).Nullable();
            Map(u => u.Monto).Nullable();
            Map(u => u.Periodo).Nullable();
            Map(u => u.Cantidad).Nullable();

            References(c => c.Facturacion, "FacturacionId").LazyLoad().Cascade.None();
            References(c => c.Producto, "ProductoId").LazyLoad().Cascade.None();
            References(c => c.OTR, "OTRId").LazyLoad().Cascade.None();
            References(c => c.Bascula, "BasculaId").LazyLoad().Cascade.None();
            References(c => c.BoletaManual, "BoletaManualId").LazyLoad().Cascade.None();
        }

    }
}
