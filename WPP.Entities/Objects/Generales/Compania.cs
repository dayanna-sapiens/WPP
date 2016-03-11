using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;
using WPP.Entities.Objects.Generales;

namespace WPP.Entities.Objects.ModuloContratos
{
    public class Compania: Entity
    {
        public virtual String Nombre { get; set; }
        public virtual Catalogo Grupo { get; set; }
        public virtual Catalogo Tipo { get; set; }
        public virtual String NombreCorto { get; set; }

        public virtual String Telefono { get; set; }

        public virtual String Cedula { get; set; }

        public virtual String Email{ get; set; }

        public virtual String RepresentanteLegal { get; set; }

        public virtual long ClienteId { get; set; }

        public virtual IList<Usuario> Usuarios { get; set; }
        

    }
}
