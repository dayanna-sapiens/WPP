
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;

namespace WPP.Entities.Mappings.Generales
{
    public class UsuarioMapping : ClassMap<Usuario>
    {
        public UsuarioMapping()
        {
            Table("Usuario");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_Usuario");
            Version(u => u.Version);
            Map(u => u.CreateDate).Not.Nullable();
            Map(u => u.CreatedBy).Not.Nullable();
            Map(u => u.DateLastModified).Nullable();
            Map(u => u.ModifiedBy).Nullable();
            Map(u => u.DeleteDate).Nullable();
            Map(u => u.DeletedBy).Nullable();
            Map(u => u.IsDeleted).Not.Nullable();
            Map(r => r.Nombre).Not.Nullable();
            Map(r => r.Apellido1).Not.Nullable();
            Map(r => r.Apellido2).Not.Nullable();
            Map(r => r.FechaNac).Not.Nullable();
            Map(r => r.Cedula).Not.Nullable();
            Map(r => r.Telefono).Not.Nullable();
            Map(r => r.Email).Not.Nullable().Unique();
            Map(r => r.Password).Not.Nullable();
            Map(r => r.Roles).Not.Nullable();
            Map(r => r.NumeroEmpleado).Nullable();
            Map(r => r.PasswordActivo).Nullable();

            HasManyToMany(u => u.Companias)
             .Table("Usuario_Compania")
             .ParentKeyColumn("UsuarioId")
             .ChildKeyColumn("CompaniaId")
             .Cascade.SaveUpdate()
             .LazyLoad()
             .AsBag();
        }
    }
}
