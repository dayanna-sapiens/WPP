using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPP.Model.ModuloCierreCaja
{
    public class EfectivoCierreCajaModel
    {
        public long Id { get; set; }
        
        public String DenominacionDescripcion { get; set; }

        [Required(ErrorMessage = "Por favor indique la denominación")]
        [Range(1, long.MaxValue, ErrorMessage = "Por favor indique la denominación")]
        public long Denominacion { set; get; }

        [Required(ErrorMessage = "Por favor indique la cantidad")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "La cantidad debe ser un valor númerico")]
        public int Cantidad { set; get; }
        
        public long CierreCaja { set; get; }
    }
}
