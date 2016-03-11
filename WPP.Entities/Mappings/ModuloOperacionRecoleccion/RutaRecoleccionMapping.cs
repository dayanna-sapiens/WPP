using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;

namespace WPP.Entities.Mappings.ModuloOperacionRecoleccion
{
    public class RutaRecoleccionMapping : ClassMap<RutaRecoleccion>
    {
        public RutaRecoleccionMapping()
        {
            Table("RutaRecoleccion");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_RutaRecoleccion");
            Version(u => u.Version);
            Map(u => u.CreateDate).Not.Nullable();
            Map(u => u.CreatedBy).Not.Nullable();
            Map(u => u.DateLastModified).Nullable();
            Map(u => u.ModifiedBy).Nullable();
            Map(u => u.DeleteDate).Nullable();
            Map(u => u.DeletedBy).Nullable();
            Map(u => u.IsDeleted).Not.Nullable();
            Map(u => u.Descripcion).Not.Nullable();
            Map(u => u.Estado).Not.Nullable();
            Map(u => u.Tipo).Not.Nullable();

            References(c => c.Compania, "CompaniaId").LazyLoad().Cascade.None();

            HasManyToMany(u => u.Rutas)
              .Table("RutaRecoleccion_Producto")
              .ParentKeyColumn("IdRutaRecoleccion")
              .ChildKeyColumn("IdProducto")
              .Cascade.SaveUpdate()
              .LazyLoad()
              .AsBag();
        }
    }
}
