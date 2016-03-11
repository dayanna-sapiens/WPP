
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;

namespace WPP.Entities.Mappings.ModuloOperacionRecoleccion
{
    public class ViajeOTRMapping : ClassMap<ViajeOTR>
    {
        public ViajeOTRMapping()
        {
            Table("ViajeOTR");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_ViajeOTR");
            Version(u => u.Version);
            Map(u => u.CreateDate).Not.Nullable();
            Map(u => u.CreatedBy).Not.Nullable();
            Map(u => u.DateLastModified).Nullable();
            Map(u => u.ModifiedBy).Nullable();
            Map(u => u.DeleteDate).Nullable();
            Map(u => u.DeletedBy).Nullable();
            Map(u => u.IsDeleted).Not.Nullable();

            Map(r => r.Observaciones).Nullable();

            References(c => c.Compania, "CompaniaId").LazyLoad().Cascade.None();
            References(c => c.OTR, "OTRId").LazyLoad().Cascade.None();
            References(c => c.Accion, "AccionId").LazyLoad().Cascade.None();
            References(c => c.TipoEquipo, "TipoEquipoId").LazyLoad().Cascade.None();
            References(c => c.Tamano, "TamanoId").LazyLoad().Cascade.None();
            References(c => c.Contenedor, "ContenedorId").LazyLoad().Cascade.None();
            References(c => c.Viaje, "ViajeId").LazyLoad().Cascade.None();

            
        }
    }
}
