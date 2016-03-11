using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Entities.Mappings.Generales
{
    public class ProductoMapping: ClassMap<Producto>
    {
        public ProductoMapping() 
        {
            Table("Producto");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_Producto");
            Version(u => u.Version);
            Map(u => u.CreateDate).Not.Nullable();
            Map(u => u.CreatedBy).Not.Nullable();
            Map(u => u.DateLastModified).Nullable();
            Map(u => u.ModifiedBy).Nullable();
            Map(u => u.DeleteDate).Nullable();
            Map(u => u.DeletedBy).Nullable();
            Map(u => u.IsDeleted).Not.Nullable();

            Map(r => r.Descripcion).Not.Nullable();
            Map(r => r.RutasRecoleccion).Nullable();
            Map(r => r.Compania).Not.Nullable();
            Map(r => r.Precio).Nullable();
            Map(r => r.Moneda).Nullable();

            References(c => c.Estado, "EstadoId").LazyLoad().Cascade.None();
            References(c => c.ProcesoCobro, "ProcesoCobroId").LazyLoad().Cascade.None();
            References(c => c.Tamano, "TamanoId").LazyLoad().Cascade.None();
            References(c => c.TipoEquipo, "TipoEquipoId").LazyLoad().Cascade.None();
            References(c => c.UnidadCobro, "UnidadId").LazyLoad().Cascade.None();
            References(c => c.Categoria, "CategoriaId").LazyLoad().Cascade.None();
         

        }
    }
}
