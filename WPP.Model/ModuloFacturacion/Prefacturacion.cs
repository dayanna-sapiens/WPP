using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPP.Model.ModuloFacturacion
{
    public class Prefacturacion
    {
        public long OTR { set; get; }
        public long Bascula { set; get; }
        public long BoletaManual { set; get; }
        public string ConsecutivoBoleta { set; get; }
        public long ConsecutivoOTR { set; get; }
        public long Producto { set; get; }
        public DateTime Fecha { set; get; }
        public String Descripcion { set; get; }
        public Double Cantidad { set; get; }
        public String Unidad { set; get; }
        public Double Precio { set; get; }
        public String Moneda { set; get; }
        public String Periodo { set; get; }
    }
}
