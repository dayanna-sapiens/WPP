using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;

namespace WPP.Entities.Objects.Generales
{
    public class CostoHora : Entity
    {
        public virtual Double Monto {set;get;}
        public virtual DateTime FechaInicio {set;get;}
        public virtual DateTime FechaFin { set; get; }
        public virtual String Tipo { set; get; }
    }
}
