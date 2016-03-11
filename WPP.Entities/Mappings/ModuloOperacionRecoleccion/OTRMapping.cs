using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;

namespace WPP.Entities.Mappings.ModuloOperacionRecoleccion
{
    public class OTRMapping : ClassMap<OTR>
    {
        public OTRMapping()
        {
            Table("OTR");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_OTR");
            Version(u => u.Version);
            Map(u => u.CreateDate).Not.Nullable();
            Map(u => u.CreatedBy).Not.Nullable();
            Map(u => u.DateLastModified).Nullable();
            Map(u => u.ModifiedBy).Nullable();
            Map(u => u.DeleteDate).Nullable();
            Map(u => u.DeletedBy).Nullable();
            Map(u => u.IsDeleted).Not.Nullable();

            Map(r => r.Fecha).Not.Nullable();
            Map(r => r.Estado).Not.Nullable();
            Map(r => r.Consecutivo).Not.Nullable();
            Map(r => r.Tipo).Not.Nullable();
            Map(r => r.Observaciones).Nullable();
           // Map(r => r.OTRMadre).Nullable();
            //Map(r => r.OTRHijas).Nullable();
            Map(r => r.DesechosRecibidos).Nullable();
            Map(r => r.ObservacionesTrabajo).Nullable();
            Map(r => r.ObservacionesMecanicas).Nullable();
            Map(r => r.SalidaRelleno).Nullable();
            Map(r => r.FinalizaOTR).Nullable();
            Map(r => r.IniciaRecoleccion).Nullable();
            Map(r => r.FinalizaRecoleccion).Nullable();
            Map(r => r.InicioDemora).Nullable();
            Map(r => r.FinalizaDemora).Nullable();
            Map(r => r.IngresoRelleno).Nullable();
            Map(r => r.KilometrajeInicio).Nullable();
            Map(r => r.KilometrajeFin).Nullable();
            Map(r => r.Combustible).Nullable();
            Map(r => r.HorimetroInicio).Nullable();
            Map(r => r.HorimetroFin).Nullable();
            Map(r => r.Facturada).Not.Nullable();

            References(c => c.Compania, "CompaniaId").LazyLoad().Cascade.None();
            References(c => c.Equipo, "EquipoId").LazyLoad().Cascade.None();
            References(c => c.Destino, "DestinoId").LazyLoad().Cascade.None();
            References(c => c.Origen, "OrigenId").LazyLoad().Cascade.None();
            References(c => c.Relleno, "RellenoId").LazyLoad().Cascade.None();
            References(c => c.RutaRecoleccion, "RutaRecoleccionId").LazyLoad().Cascade.None();
            References(c => c.Cuadrilla, "CuadrillaId").LazyLoad().Cascade.None();
            References(c => c.Cliente, "ClienteId").LazyLoad().Cascade.None();
            References(c => c.Contrato, "ContratoId").LazyLoad().Cascade.None();

            HasMany(p => p.ListaViajesOTR)
              .KeyColumn("OTRId")
              .Inverse()
              .LazyLoad()
              .Cascade.AllDeleteOrphan()
              .AsBag();

            //HasManyToMany(u => u.ListaOTRHijas)
            //.Table("OTRMadre_OTRHija")
            //.ParentKeyColumn("IdOTRMadre")
            //.ChildKeyColumn("IdOTRHija")
            //.Cascade.SaveUpdate()
            //.LazyLoad()
            //.AsBag();
        }
    }
}
