using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;

namespace WPP.Entities.Mappings.Generales
{
    public class AnuncioMapping: ClassMap<Anuncio>
    {
        public AnuncioMapping()
        {
            Table("Anuncio");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_Anuncio");
            Version(u => u.Version);
            Map(u => u.CreateDate).Not.Nullable();
            Map(u => u.CreatedBy).Not.Nullable();
            Map(u => u.DateLastModified).Nullable();
            Map(u => u.ModifiedBy).Nullable();
            Map(u => u.DeleteDate).Nullable();
            Map(u => u.DeletedBy).Nullable();
            Map(u => u.IsDeleted).Not.Nullable();
            Map(r => r.Imagen1).Not.Nullable();
            Map(r => r.Imagen2).Not.Nullable();
            Map(r => r.Imagen3).Not.Nullable();
            Map(r => r.Imagen4).Not.Nullable();
        }
    }
}
