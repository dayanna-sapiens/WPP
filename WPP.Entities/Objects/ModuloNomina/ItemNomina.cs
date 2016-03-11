using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;

namespace WPP.Entities.Objects.ModuloNomina
{
    public class ItemNomina:Entity
    {
        public virtual EmpleadoRecoleccion Empleado { set; get; }
        public virtual String Entrada { set; get; }
        public virtual String Salida { set; get; }
        public virtual DateTime Fecha { set; get; }
        public virtual Double TotalHoras { set; get; }
        public virtual Double HorasOrdinarias { set; get; }
        public virtual Double HorasExtra { set; get; }
        public virtual Double MontoOrdinario { set; get; }
        public virtual Double MontoExtra { set; get; }
        public virtual Double Total { set; get; }
        public virtual Double Toneladas { set; get; }
        public virtual Double Compensacion { set; get; }
        public virtual Planilla Nomina { set; get; } 

    }
}
