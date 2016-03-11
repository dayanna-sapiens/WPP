using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;

namespace WPP.Entities.Objects.Generales
{
    public class Catalogo : Entity
    {
        public virtual String Nombre { get; set; }
        public virtual String Tipo { get; set; }

    }
}
