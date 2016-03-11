
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Entities.Objects.Generales
{
    public class Usuario : Entity
    {
        public virtual String Nombre { get; set; }
        public virtual String Apellido1 { get; set; }
        public virtual String Apellido2 { get; set; }
        public virtual DateTime FechaNac { get; set; }
        public virtual String Cedula { get; set; }
        public virtual String Telefono { get; set; }
        public virtual String Email { get; set; }
        public virtual String Password { get; set; }
        public virtual String Roles { get; set; }
        public virtual String NumeroEmpleado { get; set; }
        public virtual bool PasswordActivo { get; set; }
        public virtual IList<Compania> Companias { get; set; }
    }
}
