using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Model.CustomValidations;

namespace WPP.Model.ModuloOperacionRecoleccion
{
    public class OTRModel
    {
        public long Id { get; set; }

        public long Consecutivo { get; set; }

        public long Compania { get; set; }
        
        [Required(ErrorMessage = "Por favor introduzca el equipo")]
        public String NombreEquipo { get; set; }

        public long Equipo { set; get; }

        [Required(ErrorMessage = "Por favor seleccione la fecha")]
        public DateTime Fecha { get; set; }
                
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor seleccione el estado")]
        public string Estado { get; set; }

        //[Required(ErrorMessage = "Por favor seleccione la ruta de recolección")]
        //[Range(1, long.MaxValue, ErrorMessage = "Por favor seleccione la ruta de recolección")]
        public long? RutaRecoleccion { get; set; }

        public string RutaRecoleccionDescripcion { set; get; }


        //[Required(ErrorMessage = "Por favor seleccione la ruta")]
        //[Range(1, long.MaxValue, ErrorMessage = "Por favor seleccione la ruta")]
        //public long Ruta { get; set; }

        //public string RutaDescripcion { set; get; }

        //public String NombreCliente { get; set; }

        //public long NumeroCliente { get; set; }

        //public String DescripcionContrato { get; set; }

        //public long NumeroContrato { get; set; }

        public long Origen { get; set; }

        public string OrigenDescripcion { set; get; }

        public long Destino { get; set; }

        public string DestinoDescripcion { set; get; }

        public long Relleno { get; set; }

        public string RellenoDescripcion { set; get; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor seleccione el tipo")]
        public string Tipo { get; set; }

        public string Observaciones { get; set; }

        //public bool OTRMadre { get; set; }

        //public string OTRHijas { get; set; }
       
        [Required(ErrorMessage = "Por favor seleccione la cuadrilla")]
        [Range(1, long.MaxValue, ErrorMessage = "Por favor seleccione la cuadrilla")]
        public long Cuadrilla { get; set; }

        public string CuadrillaDescripcion { set; get; }

        [Required(ErrorMessage = "Por favor seleccione el cliente")]
        [Range(1, long.MaxValue, ErrorMessage = "Por favor seleccione el cliente")]
        public long Cliente { get; set; }

        public string DescripcionCliente { get; set; }

        [Required(ErrorMessage = "Por favor seleccione el contrato")]
        [Range(1, long.MaxValue, ErrorMessage = "Por favor seleccione el contrato")]
        public long Contrato { get; set; }

        public string DescripcionContrato { get; set; }

        public string DesechosRecibidos { set; get; }

        public string ObservacionesTrabajo { set; get; }

        public string ObservacionesMecanicas {set; get; }


        [RegularExpression(@"^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "{0} debe tener el siguiente formato HH:MM")]
        public String SalidaRelleno { set; get; }


        [RegularExpression(@"^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "{0} debe tener el siguiente formato HH:MM")]
        public String FinalizaOTR { set; get; }


       [RegularExpression(@"^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "{0} debe tener el siguiente formato HH:MM")]
        public String IniciaRecoleccion { set; get; }


       [RegularExpression(@"^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "{0} debe tener el siguiente formato HH:MM")]
        public String FinalizaRecoleccion { set; get; }


       [RegularExpression(@"^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "{0} debe tener el siguiente formato HH:MM")]
        public String InicioDemora { set; get; }


        [RegularExpression(@"^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "{0} debe tener el siguiente formato HH:MM")]
        public String FinalizaDemora { set; get; }


       [RegularExpression(@"^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "{0} debe tener el siguiente formato HH:MM")]
        public String IngresoRelleno { set; get; }

       [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "{0} debe ser un número.")]
       public Double KilometrajeInicio { set; get; }

       [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "{0} debe ser un número.")]
       public Double KilometrajeFin { set; get; }

       [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "{0} debe ser un número.")]
       public Double Combustible { set; get; }

       [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "{0} debe ser un número.")]
       public Double HorimetroInicio { set; get; }

       [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "{0} debe ser un número.")]
       public Double HorimetroFin { set; get; }

       public String ListaRutas { set; get; }

       public bool Facturada { set; get; }

    }
}
