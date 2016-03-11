using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;

namespace WPP.Entities.Mappings.ModuloOperacionRecoleccion
{
    public class ContenedorHistorialMapping : ClassMap<ContenedorHistorial>
    {
        public ContenedorHistorialMapping()
        {
            Table("ContenedorHistorial");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_ContenedorHistorial");
            Version(u => u.Version);
            Map(u => u.CreateDate).Not.Nullable();
            Map(u => u.CreatedBy).Not.Nullable();
            Map(u => u.DateLastModified).Nullable();
            Map(u => u.ModifiedBy).Nullable();
            Map(u => u.DeleteDate).Nullable();
            Map(u => u.DeletedBy).Nullable();
            Map(u => u.IsDeleted).Not.Nullable();
            Map(u => u.Ubicacion).Not.Nullable();
            Map(u => u.Cliente).Not.Nullable();
            Map(u => u.Fecha).Not.Nullable();


            References(c => c.OTR, "OTRId").LazyLoad().Cascade.None();
            References(c => c.Contenedor, "ContenedorId").LazyLoad().Cascade.None();

        }
    }
}
