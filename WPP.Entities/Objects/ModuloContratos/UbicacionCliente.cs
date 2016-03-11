using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;
using WPP.Entities.Objects.Generales;

namespace WPP.Entities.Objects.ModuloContratos
{
    public class UbicacionCliente : Entity
    {
        public virtual Cliente Cliente { get; set; }
        public virtual string Descripcion { get; set; }
        public virtual string Direccion { get; set; }
        public virtual string Contacto { get; set; }
        public virtual string Telefono { get; set; }
        public virtual string Email { get; set; }
        public virtual Catalogo Estado { get; set; }

    }
}
