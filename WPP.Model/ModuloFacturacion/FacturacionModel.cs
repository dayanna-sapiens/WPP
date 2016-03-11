using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPP.Model.ModuloFacturacion
{
    public class FacturacionModel
    {
        public long Id { get; set; }

        public long Compania { get; set; }

        public string Descripcion { get; set; }

        public DateTime FechaDesde { get; set; }

        public DateTime FechaHasta { get; set; }

        public double Monto { get; set; }

        public long Contrato{ get; set; }

        public string ContratoDescripcion { get; set; }

        public long? Cliente { get; set; }

        public string ClienteDescripcion { get; set; }

        public string Moneda { get; set; }

        public string Observaciones { get; set; }

        public long Consecutivo { get; set; }

        public string Estado { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreateDate { get; set; }

        public string DetalleFacturacion { get; set; }
    }
}
