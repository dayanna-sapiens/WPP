using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloBascula;

namespace WPP.Entities.Mappings.ModuloBascula
{
    public class BasculaMapping : ClassMap<Bascula>
    {
        public BasculaMapping() 
        {
            Table("Bascula");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_Bascula");
            Version(u => u.Version);
            Map(u => u.CreateDate).Not.Nullable();
            Map(u => u.CreatedBy).Not.Nullable();
            Map(u => u.DateLastModified).Nullable();
            Map(u => u.ModifiedBy).Nullable();
            Map(u => u.DeleteDate).Nullable();
            Map(u => u.DeletedBy).Nullable();
            Map(u => u.IsDeleted).Not.Nullable();
            
            Map(r => r.EquipoWPP).Nullable();
            Map(r => r.NombreCliente).Nullable();
            Map(r => r.PesoBruto).Nullable();
            Map(r => r.PesoNeto).Nullable();
            Map(r => r.PesoTara).Nullable();
            Map(r => r.Eje1).Nullable();
            Map(r => r.Eje2).Nullable();
            Map(r => r.Eje3).Nullable();
            Map(r => r.Estado).Nullable();
            Map(r => r.Total).Nullable();
            Map(r => r.Boleta).Nullable();
            Map(r => r.Fecha).Not.Nullable();
            Map(r => r.NumeroRecibo).Nullable();
            Map(r => r.CierreCredito).Not.Nullable();
            Map(r => r.Facturada).Not.Nullable();


            References(c => c.Contrato, "ContratoId").LazyLoad().Cascade.None();
            References(c => c.OTR, "OTRId").LazyLoad().Cascade.None();
            References(c => c.Producto, "ProductoContratoId").LazyLoad().Cascade.None();
            References(c => c.Compania, "CompaniaId").LazyLoad().Cascade.None();
            References(c => c.Equipo, "EquipoId").LazyLoad().Cascade.None();

            HasMany(p => p.ListaPagos)
               .KeyColumn("BoletaId")
               .Inverse()
               .LazyLoad()
               .Cascade.AllDeleteOrphan()
               .AsBag();

            References(c => c.CierreCaja, "CierreCajaId").LazyLoad().Cascade.None();
        }
    }
}
