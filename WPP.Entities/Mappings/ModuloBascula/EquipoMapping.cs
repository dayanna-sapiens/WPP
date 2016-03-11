using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloBascula;

namespace WPP.Entities.Mappings.ModuloBascula
{
    public class EquipoMapping : ClassMap<Equipo>
    {
        public EquipoMapping()
        {
            Table("Equipo");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_Equipo");
            Version(u => u.Version);
            Map(u => u.CreateDate).Not.Nullable();
            Map(u => u.CreatedBy).Not.Nullable();
            Map(u => u.DateLastModified).Nullable();
            Map(u => u.ModifiedBy).Nullable();
            Map(u => u.DeleteDate).Nullable();
            Map(u => u.DeletedBy).Nullable();
            Map(u => u.IsDeleted).Not.Nullable();

            Map(u => u.Nombre).Not.Nullable();
            Map(u => u.Placa).Not.Nullable();
            Map(u => u.Marca).Not.Nullable();
            Map(u => u.Peso).Nullable();
            Map(u => u.Eje1).Nullable();
            Map(u => u.Eje2).Nullable();
            Map(u => u.Eje3).Nullable();
            Map(u => u.Tipo).Not.Nullable();
            Map(u => u.TipoCombustible).Not.Nullable();

            References(c => c.Compania, "CompaniaId").LazyLoad().Cascade.None();
        }
    }
}
