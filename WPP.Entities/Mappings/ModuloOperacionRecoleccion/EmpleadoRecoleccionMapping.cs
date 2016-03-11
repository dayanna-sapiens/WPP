using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;

namespace WPP.Entities.Mappings.ModuloOperacionRecoleccion
{
    public class EmpleadoRecoleccionMapping: ClassMap<EmpleadoRecoleccion>
    {
        public EmpleadoRecoleccionMapping()
        {
            Table("EmpleadoRecoleccion");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_EmpleadoRecoleccion");
            Version(u => u.Version);
            Map(u => u.CreateDate).Not.Nullable();
            Map(u => u.CreatedBy).Not.Nullable();
            Map(u => u.DateLastModified).Nullable();
            Map(u => u.ModifiedBy).Nullable();
            Map(u => u.DeleteDate).Nullable();
            Map(u => u.DeletedBy).Nullable();
            Map(u => u.IsDeleted).Not.Nullable();
            Map(r => r.Nombre).Not.Nullable();
            Map(r => r.Cedula).Nullable();
            Map(r => r.Puesto).Nullable();
            Map(r => r.Estado).Nullable();
            Map(r => r.Codigo).Nullable();

            References(c => c.Compania, "CompaniaId").LazyLoad().Cascade.None();

            References(c => c.Jornada, "JornadaId").LazyLoad().Cascade.None();
        }
    }
}
