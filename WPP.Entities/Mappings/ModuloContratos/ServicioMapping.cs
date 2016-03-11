using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Entities.Mappings.ModuloContratos
{
    class ServicioMapping : ClassMap<Servicio>
    {
        public ServicioMapping()
        {
            Table("Servicio");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_Servicio");
            Version(u => u.Version);
            Map(u => u.CreateDate).Not.Nullable();
            Map(u => u.CreatedBy).Not.Nullable();
            Map(u => u.DateLastModified).Nullable();
            Map(u => u.ModifiedBy).Nullable();
            Map(u => u.DeleteDate).Nullable();
            Map(u => u.DeletedBy).Nullable();
            Map(u => u.IsDeleted).Not.Nullable();
            Map(u => u.Nombre).Not.Nullable();
            Map(u => u.Activo).Not.Nullable();

            References(c => c.Compania, "CompaniaId").LazyLoad().Cascade.None();
           
        }
    }
}
