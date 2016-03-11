using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPP.Model.ModuloFacturacion
{
    public class DetalleFacturacionModel
    {
        public long Id { get; set; }            

        public long Facturacion { get; set; }

        public long Producto { get; set; }

        public String ProductoDescripcion { get; set; }

        public long? OTR { get; set; }

        public double Cantidad { get; set; }

        public long? Bascula { get; set; }

        public long Monto { get; set; }

        public string Periodo { get; set; }

        public string Moneda { get; set; }

        public long? BoletaManual { get; set; }

    }
}
