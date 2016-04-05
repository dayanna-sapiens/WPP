using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;

namespace WPP.Models.Nomina
{
    public class NominaRecolector
    {
        public String Empleado { set; get; }
        public DateTime Fecha { set; get; }
        public String Entrada { set; get; }
        public String Salida { set; get; }
        public Double TotalHoras { set; get; }
        public RutaRecoleccion RutaRecoleccion { set; get; }
        public Double ToneladasXViaje { set; get; }
        public Double ToneladasXPersona { set; get; }
        public Double Monto { set; get; }
    }
}