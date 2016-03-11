using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;

namespace WPP.Entities.Objects.ModuloNomina
{
    public class Jornada : Entity
    {
        public virtual String Descripcion { set; get; }
        public virtual String Tipo { set; get; }
        public virtual String HoraInicio { set; get; }
        public virtual String HoraFin { set; get; }
    }
}
