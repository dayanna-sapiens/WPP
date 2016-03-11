using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloFacturacion;

namespace WPP.Entities.Mappings.ModuloFacturacion
{
    public class FacturacionMapping: ClassMap<Facturacion>
    {
        public FacturacionMapping()
        {
            Table("Facturacion");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_Facturacion");
            Version(u => u.Version);
            Map(u => u.CreateDate).Not.Nullable();
            Map(u => u.CreatedBy).Not.Nullable();
            Map(u => u.DateLastModified).Nullable();
            Map(u => u.ModifiedBy).Nullable();
            Map(u => u.DeleteDate).Nullable();
            Map(u => u.DeletedBy).Nullable();
            Map(u => u.IsDeleted).Not.Nullable();

            Map(u => u.Descripcion).Nullable();
            Map(u => u.Monto).Nullable();
            Map(u => u.FechaDesde).Nullable();
            Map(u => u.FechaHasta).Nullable();
            Map(u => u.Moneda).Nullable();
            Map(u => u.Estado).Nullable();
            Map(u => u.Observaciones).Nullable();
            Map(u => u.Consecutivo).Nullable();

            References(c => c.Compania, "CompaniaId").LazyLoad().Cascade.None();
            References(c => c.Contrato, "ContratoId").LazyLoad().Cascade.None();


            HasMany(p => p.ListaDetalleFacturacion)
              .KeyColumn("FacturacionId")
              .Inverse()
              .LazyLoad()
              .Cascade.AllDeleteOrphan()
              .AsBag();
        }

    }
}
