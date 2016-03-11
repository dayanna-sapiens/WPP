using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPP.Model.ModuloBascula
{
    public class BoletaManualModel
    {
        public long Id { set; get; }

        public long OTR { set; get; }

        [Required(ErrorMessage = "Por favor introduzca la OTR")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "{0} debe ser un número.")]
        public long OTRConsecutivo { set; get; }

        public long Compania { set; get; }

        [Required(ErrorMessage = "Por favor introduzca la fecha")]
        public DateTime Fecha { set; get; }
        
        [RegularExpression(@"^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "{0} debe tener el siguiente formato HH:MM")]
        public String Hora { set; get; }

        [Required(ErrorMessage = "Por favor introduzca el número de boleta")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "{0} debe ser un número.")]
        public String NumeroBoleta { set; get; }

        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "{0} debe ser un número.")]
        public double PesoBruto { set; get; }

        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "{0} debe ser un número.")]
        public double PesoTara { set; get; }

        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "{0} debe ser un número.")]
        public double PesoNeto { set; get; }

        public string Observaciones { set; get; }

        public long Sitio { set; get; }

        public string SitioDescripcion { set; get; }

        public string Estado { set; get; }

        public string DescripcionCliente { set; get; }
        
        public bool Facturada { set; get; }
    }
}
