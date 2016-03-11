using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPP.Model.ModuloContratos
{
    public class ProductoContratoModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Por favor introduzca la descripción")]
        public String Descripcion { get; set; }
        
        [Required(ErrorMessage = "Por favor seleccione el producto")]
        [Range(1, long.MaxValue, ErrorMessage = "Por favor seleccione el producto")]
        public long Producto { get; set; }

        public String ProductoDescripcion { get; set; }

        [Required(ErrorMessage = "Por favor seleccione el servicio")]
        [Range(1, long.MaxValue, ErrorMessage = "Por favor seleccione el servicio")]
        public long Servicio { get; set; }

        public String ServicioDescripcion { get; set; }


        public long? EsquemaRelevancia { get; set; }

        public String EsquemaRelevanciaDescripcion { get; set; }

        [Required(ErrorMessage = "Por favor seleccione la ubicación")]
        [Range(1, long.MaxValue, ErrorMessage = "Por favor seleccione la ubicación")]
        public long Ubicacion { get; set; }

        public String UbicacionDescripcion { get; set; }

        [Required(ErrorMessage = "Por favor seleccione la fecha de inicio")]
        public DateTime FechaInicial { get; set; }
        
        [Required(ErrorMessage = "Por favor seleccione la fecha final")]
        public DateTime FechaFinal { get; set; }

        [Required(ErrorMessage = "Por favor seleccione la fecha de cambio")]
        public DateTime FechaEstado { get; set; }
        
        public String Ruta { get; set; }

        [Required(ErrorMessage = "Por favor seleccione la moneda")]
        public String Moneda { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor seleccione el estado")]
        public String Estado { get; set; }

        [Required(ErrorMessage = "Por favor seleccione la cantidad")]
        [Range(1, long.MaxValue, ErrorMessage = "Por favor seleccione la cantidad")]
        public int Cantidad { get; set; }

        public bool? CobrarProductoCliente { get; set; }


        public long? Frecuecia { get; set; }

        public String FrecueciaDescripcion { get; set; }

        [Required(ErrorMessage = "Por favor seleccione la cuenta contable de crédito")]
        public String CuentaContableCredito { get; set; }

        [Required(ErrorMessage = "Por favor seleccione el monto")]
        [Range(1, long.MaxValue, ErrorMessage = "Por favor seleccione el monto")]
        public double Monto { get; set; }

        public double? Descuento { get; set; }

        public double? Total { get; set; }
        
        public long? RutaRecoleccion { get; set; }

        public String RutaRecoleccionDescripcion { get; set; }

        [Required(ErrorMessage = "Por favor seleccione el contrato")]
        [Range(1, long.MaxValue, ErrorMessage = "Por favor seleccione el contrato")]
        public long Contrato { get; set; }

        public String ContratoDescripcion { get; set; }

        public String SubItem { get; set; }

        public String UnidadCobro { get; set; }

        public String TipoEquipo { get; set; }

        public String ProcesoCobro { get; set; }

        public String DiasSemana { get; set; }

        public long? ProductoFosa { get; set; }

        public String ProductoFosaDescripcion { get; set; }

        public double? PrecioFosa { get; set; }

        public long Proyecto { get; set; }

        public String ProyectoDescripcion { get; set; }

        public bool? LigadoRecoleccion { get; set; }

        public long? Recoleccion { get; set; }

        public string RecoleccionDescripcion { get; set; }

    }
}
