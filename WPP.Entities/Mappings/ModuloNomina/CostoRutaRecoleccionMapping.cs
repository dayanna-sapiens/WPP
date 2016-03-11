using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloNomina;

namespace WPP.Entities.Mappings.ModuloNomina
{
    public class CostoRutaRecoleccionMapping : ClassMap<CostoRutaRecoleccion>
    {
        public CostoRutaRecoleccionMapping()
        {
            Table("CostoRutaRecoleccion");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_CostoRutaRecoleccion");
            Map(u => u.Costo).Not.Nullable();
            Map(u => u.FechaDesde).Not.Nullable();
            Map(u => u.FechaHasta).Not.Nullable();
            Version(u => u.Version);
            Map(u => u.CreateDate).Not.Nullable();
            Map(u => u.CreatedBy).Not.Nullable();
            Map(u => u.DateLastModified).Nullable();
            Map(u => u.ModifiedBy).Nullable();
            Map(u => u.DeleteDate).Nullable();
            Map(u => u.DeletedBy).Nullable();
            Map(u => u.IsDeleted).Not.Nullable();
            References(c => c.RutaRecoleccion, "RutaRecoleccionId").LazyLoad().Cascade.None();
        }
    }
}
