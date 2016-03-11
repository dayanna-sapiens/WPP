using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPP.Model.ModuloFacturacion
{
    public class ReversionModel
    {
        public long Id { set; get; }

        public long Facturacion { set; get; }

        public long ConsecutivoFacturacion { set; get; }

        public long Compania { set; get; }

        public string Consecutivo { set; get; }

        [Required(ErrorMessage = "Por favor introduzca las observaciones")]
        public string Observaciones { set; get; }
    }
}
