using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Entities.Mappings.ModuloContratos
{
    public class CompaniaMapping : ClassMap<Compania>
    {
        public CompaniaMapping()
        {
            Table("Compania");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_Compania");
            Version(u => u.Version);
            Map(u => u.CreateDate).Not.Nullable();
            Map(u => u.CreatedBy).Not.Nullable();
            Map(u => u.DateLastModified).Nullable();
            Map(u => u.ModifiedBy).Nullable();
            Map(u => u.DeleteDate).Nullable();
            Map(u => u.DeletedBy).Nullable();
            Map(u => u.IsDeleted).Not.Nullable();
            Map(r => r.Nombre).Not.Nullable();
            Map(r => r.NombreCorto).Not.Nullable();
            Map(r => r.Telefono).Not.Nullable();
            Map(r => r.Cedula).Not.Nullable();
            Map(r => r.Email).Not.Nullable();
            Map(r => r.RepresentanteLegal).Not.Nullable();
            Map(r => r.ClienteId).Nullable();
            

            References(c => c.Tipo, "TipoId").LazyLoad().Cascade.None();
            References(c => c.Grupo, "GrupoId").LazyLoad().Cascade.None();

            HasManyToMany(c => c.Usuarios)
            .Table("Usuario_Compania")
            .ParentKeyColumn("CompaniaId")
            .ChildKeyColumn("UsuarioId")
            .LazyLoad()
            .Inverse()
            .AsBag();

        }
    }
}
