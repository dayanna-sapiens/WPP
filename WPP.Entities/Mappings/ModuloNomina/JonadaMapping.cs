using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloNomina;

namespace WPP.Entities.Mappings.ModuloNomina
{
    public class JonadaMapping : ClassMap<Jornada>
    {
        public JonadaMapping()
        {
            Table("Jornada");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_Jornada");
            Version(u => u.Version);
            Map(u => u.CreateDate).Not.Nullable();
            Map(u => u.CreatedBy).Not.Nullable();
            Map(u => u.DateLastModified).Nullable();
            Map(u => u.ModifiedBy).Nullable();
            Map(u => u.DeleteDate).Nullable();
            Map(u => u.DeletedBy).Nullable();
            Map(u => u.IsDeleted).Not.Nullable();

            Map(u => u.Tipo).Not.Nullable();
            Map(u => u.HoraInicio).Not.Nullable();
            Map(u => u.HoraFin).Not.Nullable();
            Map(u => u.Descripcion).Nullable();

        }

    }
}
