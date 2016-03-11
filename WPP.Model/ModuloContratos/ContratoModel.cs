using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Model.CustomValidations;

namespace WPP.Model.ModuloContratos
{
    public class ContratoModel
    {
        public long Id { get; set; }

        //[Required(ErrorMessage = "Por favor introduzca el número de contrato")]
        //[RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "El número del contrato debe ser númerico")]
        public long? Numero { get; set; }
        
        [Required(ErrorMessage = "Por favor introduzca el cliente")]
        public long NumeroCliente { get; set; }

        [Required(ErrorMessage = "Por favor introduzca la descripción del cliente")]
        public String DescripcionCliente { get; set; }

        [Required(ErrorMessage = "Por favor introduzca la descripción del contrato")]
        [StringLength(255, ErrorMessage = "La descripción del contrato no puede ser mayor a los 255 caracteres")]
        public String DescripcionContrato { get; set; }

        public int? MyProperty { get; set; }

        [Required(ErrorMessage = "Por favor introduzca la fecha de inicio")]
        public DateTime FechaInicio { get; set; }

       
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor seleccione el estado")]
        public String Estado { get; set; }

        public String EstadoDescripcion { get; set; }

        [Required(ErrorMessage = "Por favor seleccione el modo de facturación")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Por favor seleccione el modo de facturación")]
        public String ModoFacturacion { get; set; }

        public bool DiaCorteEsMes { get; set; }

        [RequiredIfAttributeValue("DiaCorteMes", "ModoFacturacion", "Día específico del mes", ErrorMessage = "Por favor introduzca el día de corte del mes")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "El día específico del mes deben ser un númerico.")]
        public int? DiaCorteMes { get; set; }
        
        [RequiredIfAttributeValue("DiaCorteSemana", "ModoFacturacion", "Día específico de la semana", ErrorMessage = "Por favor introduzca el día de corte de la semana")]
        public long? DiaCorteSemana { get; set; }

        public String DiaCorteSemanaDescripcion { get; set; }
        
        [Required(ErrorMessage = "Por favor seleccione el punto de facturación")]
        [Range(1, long.MaxValue, ErrorMessage = "Por favor seleccione el punto de facturación")]
        public long PuntoFacturacion { get; set; }

        public String PuntoFacturacionDescripcion { get; set; }

        [Required(ErrorMessage = "Por favor seleccione el modo de facturación")]
        [Range(0, long.MaxValue, ErrorMessage = "Por favor seleccione el modo de facturación")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Los días de aviso deben ser un númerico")]
        public int? DiasAvisoPrevioVencimiento { get; set; }

        [Required(ErrorMessage = "Por favor seleccione el repesaje")]
        [Range(1, long.MaxValue, ErrorMessage = "Por favor seleccione el repesaje")]
        public long Repesaje { get; set; }

         public string RepesajeDescripcion { get; set; }

        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "El número de formulario deben ser un númerico")]
        public int? NumeroFormulario { get; set; }

        [StringLength(255, ErrorMessage = "Las observaciones no pueden ser mayor a los 255 caracteres")]
        public String Observaciones { get; set; }

        [StringLength(255, ErrorMessage = "La observaciones de la factura no pueden ser mayor a los 255 caracteres")]
        public String ObservacionesFactura { get; set; }

        public long Compania { get; set; }

        public IList<ProductoContrato> Productos { get; set; }

        public ProductoContratoModel Producto { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor seleccione la moneda")]
        public String Moneda { get; set; }

        public bool? FacturarColones { get; set; }

        public bool? PagoContado { get; set; }

        [Required(ErrorMessage = "Por favor seleccione el ejecutivo")]
        [Range(1, long.MaxValue, ErrorMessage = "Por favor seleccione el ejecutivo")]
        public long Ejecutivo { set; get; }

        public string EjecutivoNombre { set; get; }
    }
}
