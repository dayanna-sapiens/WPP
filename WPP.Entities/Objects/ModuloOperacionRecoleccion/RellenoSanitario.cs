using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;

namespace WPP.Entities.Objects.ModuloOperacionRecoleccion
{
    public class RellenoSanitario:Entity
    {
        public virtual string Nombre { set; get; }
        public virtual bool Estado { set; get; }

    }
}
