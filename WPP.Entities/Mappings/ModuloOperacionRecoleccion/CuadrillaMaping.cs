using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;

namespace WPP.Entities.Mappings.ModuloOperacionRecoleccion
{
    public class CuadrillaMaping : ClassMap<Cuadrilla>
    {
        public CuadrillaMaping()
        {
            Table("Cuadrilla");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_Cuadrilla");
            Version(u => u.Version);
            Map(u => u.CreateDate).Not.Nullable();
            Map(u => u.CreatedBy).Not.Nullable();
            Map(u => u.DateLastModified).Nullable();
            Map(u => u.ModifiedBy).Nullable();
            Map(u => u.DeleteDate).Nullable();
            Map(u => u.DeletedBy).Nullable();
            Map(u => u.IsDeleted).Not.Nullable();
            Map(r => r.Estado).Not.Nullable();
            Map(r => r.Descripcion).Nullable();
            
            References(c => c.Compania, "CompaniaId").LazyLoad().Cascade.None();

            HasManyToMany(u => u.ListaEmpleados)
          .Table("Cuadrilla_EpleadoRecoleccion")
          .ParentKeyColumn("IdEmpleadoRecoleccion")
          .ChildKeyColumn("IdCuadrilla")
          .Cascade.SaveUpdate()
          .LazyLoad()
          .AsBag();
        }
    }
}
