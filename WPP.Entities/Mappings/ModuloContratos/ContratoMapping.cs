using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Entities.Mappings.ModuloContratos
{
    public class ContratoMapping : ClassMap<Contrato>
    {
        public ContratoMapping()
        {
            Table("Contrato");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_Contrato");
            Version(u => u.Version);
            Map(u => u.CreateDate).Not.Nullable();
            Map(u => u.CreatedBy).Not.Nullable();
            Map(u => u.DateLastModified).Nullable();
            Map(u => u.ModifiedBy).Nullable();
            Map(u => u.DeleteDate).Nullable();
            Map(u => u.DeletedBy).Nullable();
            Map(u => u.IsDeleted).Not.Nullable();
            Map(u => u.Moneda).Not.Nullable();
            Map(u => u.FacturarColones).Nullable();
            Map(u => u.PagoContado).Nullable();

            Map(u => u.Numero).Nullable();
            Map(u => u.DescripcionContrato).Not.Nullable();
            References(c => c.Cliente, "IdCliente").LazyLoad().Cascade.None();
            Map(u => u.FechaInicio).Not.Nullable();
            Map(u => u.Estado).Not.Nullable();
            Map(u => u.ModoFacturacion).Nullable();
            Map(u => u.DiaCorteMes).Nullable();
            References(c => c.DiaCorteSemana, "DiaCorteSemanaId").LazyLoad().Cascade.None();
            Map(u => u.DiaCorteEsMes).Not.Nullable();
            References(c => c.PuntoFacturacion, "PuntoFacturacionId").LazyLoad().Cascade.None();
            Map(u => u.DiasAvisoPrevioVencimiento).Not.Nullable();
            References(c => c.Repesaje, "RepesajeId").LazyLoad().Cascade.None();
            Map(u => u.NumeroFormulario).Nullable();
            Map(u => u.Observaciones).Nullable();
            Map(u => u.ObservacionesFactura).Nullable();
            References(c => c.Compania, "CompaniaId").LazyLoad().Cascade.None();
            References(c => c.Ejecutivo, "UsuarioId").LazyLoad().Cascade.None();

            HasMany(p => p.Productos)
          .KeyColumn("ContratoId")
          .Inverse()
          .LazyLoad()
          .Cascade.AllDeleteOrphan()
          .AsBag();

        }
    }
}
