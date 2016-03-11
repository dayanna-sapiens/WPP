using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;

namespace WPP.Entities.Mappings.Generales
{
    public class CombustibleMapping: ClassMap<Combustible>
    {
        public CombustibleMapping()
        {
            Table("Combustible");
           // Id(x => x.Id).Column("ID").GeneratedBy.Guid();
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_Combustible");
            Version(u => u.Version);
            Map(u => u.CreateDate).Not.Nullable();
            Map(u => u.CreatedBy).Not.Nullable();
            Map(u => u.DateLastModified).Nullable();
            Map(u => u.ModifiedBy).Nullable();
            Map(u => u.DeleteDate).Nullable();
            Map(u => u.DeletedBy).Nullable();
            Map(u => u.IsDeleted).Not.Nullable();
            Map(r => r.Diesel).Not.Nullable();
            Map(r => r.Gasolina).Not.Nullable();
            Map(r => r.FechaDesde).Not.Nullable();
            Map(r => r.FechaHasta).Not.Nullable();
        }
    }
}
