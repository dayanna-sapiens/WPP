using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloNomina;

namespace WPP.Entities.Mappings.ModuloNomina
{
    public class ItemNominaMapping : ClassMap<ItemNomina>
    {
        public ItemNominaMapping()
        {
            Table("ItemNomina");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_ItemNomina");
            Map(u => u.CreateDate).Not.Nullable();
            Map(u => u.CreatedBy).Not.Nullable();
            Map(u => u.Version).Not.Nullable();
            Map(u => u.DateLastModified).Nullable();
            Map(u => u.IsDeleted).Not.Nullable();
            Map(u => u.ModifiedBy).Nullable();
            Map(u => u.DeleteDate).Nullable();
            Map(u => u.DeletedBy).Nullable();

            Map(u => u.Entrada).Nullable();
            Map(u => u.Salida).Nullable();
            Map(u => u.Fecha).Nullable();
            Map(u => u.TotalHoras).Nullable();
            Map(u => u.HorasOrdinarias).Nullable();
            Map(u => u.MontoExtra).Nullable();
            Map(u => u.MontoOrdinario).Nullable();
            Map(u => u.Total).Nullable();
            Map(u => u.Toneladas).Nullable();
            Map(u => u.Compensacion).Nullable();

            References(c => c.Empleado, "EmpleadoId").LazyLoad().Cascade.None();
            References(c => c.Nomina, "PlanillaId").LazyLoad().Cascade.None();
        }

    }
}
