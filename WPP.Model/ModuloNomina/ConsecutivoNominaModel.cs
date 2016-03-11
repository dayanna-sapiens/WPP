using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPP.Model.ModuloNomina
{
    public class ConsecutivoNominaModel
    {
        [Required(ErrorMessage = "Por favor introduzca el nuevo consecutivo")]
        [Range(1, long.MaxValue, ErrorMessage = "Por favor introduzca el nuevo consecutivo")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "El consecutivo debe ser un valor númerico")]
        public long Consecutivo { set; get; }

        public long Compania { set; get; }

        public long Id { set; get; }
    }
}
