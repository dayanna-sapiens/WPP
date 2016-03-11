using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPP.Model.ModuloNomina
{
    public class DiasFestivosModel
    {
        [Required(ErrorMessage = "Por favor seleccione el día")]
        [Range(1, long.MaxValue, ErrorMessage = "Por favor seleccione el día")]
        public int Dia { set; get; }

        [Required(ErrorMessage = "Por favor seleccione el mes")]
        [Range(1, long.MaxValue, ErrorMessage = "Por favor seleccione el mes")]
        public int Mes { set; get; }

        [Required(ErrorMessage = "Por favor introduzca el nombre")]
        public string Descripcion { set; get; }

        public long Id { set; get; }
    }
}
