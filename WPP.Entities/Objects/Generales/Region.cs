using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;

namespace WPP.Entities.Objects
{
    public class Region : Entity
    {
        public virtual String Nombre { get; set; }
        public virtual long NodoPadre { get; set; }
    }
}
