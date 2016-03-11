using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Entities.Mappings.ModuloContratos
{
    public class ProductoContratoMapping : ClassMap<ProductoContrato>
    {
        public ProductoContratoMapping() 
        {
            Table("ProductoContrato");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_ProductoContrato");
            Version(u => u.Version);
            Map(u => u.CreateDate).Not.Nullable();
            Map(u => u.CreatedBy).Not.Nullable();
            Map(u => u.DateLastModified).Nullable();
            Map(u => u.ModifiedBy).Nullable();
            Map(u => u.DeleteDate).Nullable();
            Map(u => u.DeletedBy).Nullable();
            Map(u => u.IsDeleted).Not.Nullable();

         
            Map(r => r.Descripcion).Not.Nullable();
            Map(r => r.FechaInicial).Not.Nullable();
            Map(r => r.FechaFinal).Not.Nullable();
            Map(r => r.Ruta).Nullable();
            Map(r => r.Cantidad).Not.Nullable();
            Map(r => r.CobrarProductoCliente).Nullable();
            Map(r => r.CuentaContableCredito).Not.Nullable();
            Map(r => r.Monto).Not.Nullable();
            Map(r => r.Descuento).Nullable();
            Map(r => r.Total).Nullable();
            Map(r => r.Estado).Not.Nullable();
            Map(r => r.SubItem).Nullable();
            Map(r => r.DiasSemana).Nullable();
            Map(r => r.PrecioFosa).Nullable();
            Map(r => r.FechaEstado).Not.Nullable();
            Map(r => r.LigadoRecoleccion).Not.Nullable();

            References(c => c.Contrato, "ContratoId").LazyLoad().Cascade.None();
            References(c => c.RutaRecoleccion, "RutaRecoleccionId").LazyLoad().Cascade.None();
            References(c => c.Frecuecia, "FrecueciaId").LazyLoad().Cascade.None();
            References(c => c.Ubicacion, "UbicacionId").LazyLoad().Cascade.None();
            References(c => c.EsquemaRelevancia, "EsquemaRelevanciaId").LazyLoad().Cascade.None();
            References(c => c.Servicio, "ServicioId").LazyLoad().Cascade.None();
            References(c => c.Producto, "ProductoId").LazyLoad().Cascade.None();
            References(c => c.ProductoFosa, "ProductoFosaId").LazyLoad().Cascade.None();
            References(c => c.Proyecto, "ProyectoId").LazyLoad().Cascade.None();
            References(c => c.Recoleccion, "RecoleccionId").LazyLoad().Cascade.None();


        }
    }
}
