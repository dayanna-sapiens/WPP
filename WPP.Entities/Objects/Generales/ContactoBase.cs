using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;

namespace WPP.Entities.Objects.Generales
{
    public abstract class ContactoBase : Entity
    {
        public virtual String Nombre { get; set; }
        public virtual String Telefono1 { get; set; }
        public virtual String Ext1 { get; set; }
        public virtual String Telefono2 { get; set; }
        public virtual String Ext2 { get; set; }
        public virtual String Cedula { get; set; }
        public virtual String Email { get; set; }
        public virtual String Observaciones { get; set; }
        public virtual String Horario { get; set; }
    }
}
