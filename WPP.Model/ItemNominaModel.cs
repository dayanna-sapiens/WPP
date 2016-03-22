using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPP.Model
{
    public class ItemNominaModel
    {
        public long Id { set; get; }
        public String Entrada { set; get; }
        public String Salida { set; get; }
        public String Dia { set; get; }
        public Double TotalHoras { set; get; }
        public Double HorasOrdinarias { set; get; }
        public Double HorasExtra { set; get; }
        public Double MontoOrdinario { set; get; }
        public Double MontoExtra { set; get; }
        public Double Total { set; get; }
        public Double Toneladas { set; get; }
        public Double Compensacion { set; get; }
        public long Empleado { set; get; }
        public String NombreEmpleado { set; get; }
        public String CodigoEmpleado { set; get; }
        public long Nomina  { set; get; }
        public String DescripcionNomina { set; get; }
        public String Fecha { set; get; }
    }
}
