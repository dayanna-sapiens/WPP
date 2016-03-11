using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPP.Model.ModuloBascula
{
    public class ContenedorModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Por favor introduzca la descripción")]
        public String Descripcion { get; set; }

        [Required(ErrorMessage = "Por favor introduzca el código del contenedor")]
        public String Codigo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor seleccione el estado")]
        public String Estado { get; set; }

        public double? Peso { get; set; }

        public double? Eje1 { get; set; }

        public double? Eje2 { get; set; }

        public double? Eje3 { get; set; }
    }
}
