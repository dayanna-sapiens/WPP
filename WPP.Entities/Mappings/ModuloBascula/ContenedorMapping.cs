using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloBascula;

namespace WPP.Entities.Mappings.ModuloBascula
{
    public class ContenedorMapping : ClassMap<Contenedor>
    {
        public ContenedorMapping()
        {
            Table("Contenedor");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_Contenedor");
            Version(u => u.Version);
            Map(u => u.CreateDate).Not.Nullable();
            Map(u => u.CreatedBy).Not.Nullable();
            Map(u => u.DateLastModified).Nullable();
            Map(u => u.ModifiedBy).Nullable();
            Map(u => u.DeleteDate).Nullable();
            Map(u => u.DeletedBy).Nullable();
            Map(u => u.IsDeleted).Not.Nullable();

            Map(u => u.Descripcion).Not.Nullable();
            Map(u => u.Codigo).Not.Nullable();
            Map(u => u.Estado).Not.Nullable();
            Map(u => u.Peso).Nullable();
            Map(u => u.Eje1).Nullable();
            Map(u => u.Eje2).Nullable();
            Map(u => u.Eje3).Nullable();

        }
    }
}
