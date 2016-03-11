using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Entities.Mappings.ModuloContratos
{
    public class ClientesMapping : ClassMap<Cliente>
    {
        public ClientesMapping() 
        {
            Table("Cliente");
            Id(x => x.Id).GeneratedBy.Sequence("SEQ_Cliente");
            Version(u => u.Version);
            Map(u => u.CreateDate).Not.Nullable();
            Map(u => u.CreatedBy).Not.Nullable();
            Map(u => u.DateLastModified).Nullable();
            Map(u => u.ModifiedBy).Nullable();
            Map(u => u.DeleteDate).Nullable();
            Map(u => u.DeletedBy).Nullable();
            Map(u => u.IsDeleted).Not.Nullable();

         
            Map(r => r.Numero).Nullable();
            Map(r => r.Nombre).Not.Nullable();
            Map(r => r.Cedula).Nullable();
            Map(r => r.NombreComercial).Nullable();
            Map(r => r.NombreCorto).Not.Nullable();
            Map(r => r.Email).Nullable();
            Map(r => r.Telefono1).Nullable();
            Map(r => r.Telefono2).Nullable();
            Map(r => r.Fax).Nullable();
            Map(r => r.FechaDesactivacion).Nullable();
            Map(r => r.RepresentanteLegal).Nullable();
            Map(r => r.Direccion).Nullable();
            Map(r => r.CompaniaId).Nullable();

            References(c => c.Tipo, "TipoId").LazyLoad().Cascade.None();
            References(c => c.Grupo, "GrupoId").LazyLoad().Cascade.None();

            References(c => c.Provincia, "ProvinciaId").LazyLoad().Cascade.None();
            References(c => c.Canton, "CantonId").LazyLoad().Cascade.None();
            References(c => c.Distrito, "DistritoId").LazyLoad().Cascade.None();

            HasMany(p => p.Contactos)
           .KeyColumn("IdCliente")
           .Inverse()
           .LazyLoad()
           .Cascade.AllDeleteOrphan()
           .AsBag();

            HasMany(p => p.Ubicaciones)
             .KeyColumn("IdCliente")
             .Inverse()
             .LazyLoad()
             .Cascade.AllDeleteOrphan()
             .AsBag();

            HasMany(p => p.Contratos)
            .KeyColumn("IdCliente")
            .Inverse()
            .LazyLoad()
            .Cascade.AllDeleteOrphan()
            .AsBag();
        }
    }
}
