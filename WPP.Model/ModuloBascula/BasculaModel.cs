using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloBascula;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Model.ModuloBascula
{
    public class BasculaModel
    {
        public long Id { get; set; }
        
        [Required(ErrorMessage = "Por favor introduzca la placa del equipo")]
        public String PlacaEquipo { get; set; }

        public bool EquipoWPP { get; set; }

        [Required(ErrorMessage = "Por favor introduzca el cliente")]
        public String Cliente { get; set; }

        public long NumeroCliente { get; set; }

        [Required(ErrorMessage = "Por favor seleccione el contrato")]
        [Range(1, long.MaxValue, ErrorMessage = "Por favor seleccione el contrato")]
        public long Contrato { get; set; }

        public String NombreCliente { get; set; }

        public double PesoBruto { get; set; }

        public double? PesoTara { get; set; }

        public double? PesoNeto { get; set; } 
                
        public long Boleta { get; set; }

        public long SecuenciaBoleta { get; set; }
        
        public long Compania { get; set; }

        public String ContratoDescripcion { get; set; }

        public double? Eje1 { get; set; }

        public double? Eje2 { get; set; }

        public double? Eje3 { get; set; }
        
        public string Estado { get; set; }

        public long Producto { get; set; }

        public string DescripcionProducto { get; set; }

        public double Total { get; set; }

        public string Moneda { get; set; }

        public string ListaProductos { get; set; }

        public long Equipo { get; set; }

        public double? Eje1Tara { get; set; }

        public double? Eje2Tara { get; set; }

        public double? Eje3Tara { get; set; }

        public String ListaPagos { get; set; }

        public DateTime Fecha { get; set; }

        public long OTR { get; set; }

        public long ConsecutivoOTR { set; get; }

        public String NumeroRecibo { get; set; }

        public bool CierreCredito { get; set; }

        public long CierreCaja { get; set; }

        public bool Facturada { set; get; }

    }
}
