using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;
using WPP.Entities.Objects.Generales;

namespace WPP.Entities.Objects.ModuloContratos
{
    public class Cliente : Entity
    {
        public virtual Catalogo Grupo { get; set; }
        public virtual long? Numero { get; set; }
        public virtual String Nombre { get; set; }
        public virtual String Cedula { get; set; }
        public virtual Catalogo Tipo { get; set; }
        public virtual String NombreComercial { get; set; }
        public virtual String NombreCorto { get; set; }
        public virtual String Email { get; set; }
        public virtual String Telefono1 { get; set; }
        public virtual String Telefono2 { get; set; }
        public virtual String Fax { get; set; }
        public virtual DateTime? FechaDesactivacion { get; set; }
        public virtual String RepresentanteLegal { get; set; }
        public virtual String Direccion { get; set; }
        public virtual Region Provincia { get; set; }
        public virtual Region Canton { get; set; }
        public virtual Region Distrito { get; set; }
        public virtual long CompaniaId { get; set; }

        public virtual IList<ContactoCliente> Contactos { get; set; }

        public virtual IList<UbicacionCliente> Ubicaciones { get; set; }
        
        public virtual IList<Contrato> Contratos { get; set; }
    }
}
