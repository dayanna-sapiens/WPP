using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPP.Model.General
{
    public class TipoCambioModel
    {

        [Required(ErrorMessage = "Por favor introduzca el tipo de cambio de compra")]
        [Range(1, double.MaxValue, ErrorMessage = "Por favor introduzca el tipo de cambio de compra")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "El tipo de cambio debe ser un valor númerico")]
        public double Compra { set; get; }


        [Required(ErrorMessage = "Por favor introduzca el tipo de cambio de venta")]
        [Range(1, long.MaxValue, ErrorMessage = "Por favor introduzca el tipo de cambio de venta")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "El tipo de cambio debe ser un valor númerico")]
        public double Venta { set; get; }
    }
}
