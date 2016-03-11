using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPP.Model.ModuloContratos
{
    public class UbicacionClienteModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Por favor introduzca la descripción")]
        public String Descripción { get; set; }

        public String Telefono { get; set; }

        public String Contacto { get; set; }

        public String Direccion { get; set; }
        
        [EmailAddress(ErrorMessage = "Por favor introduzca un correo válido")]
        public String Email { get; set; }

        public long Cliente { get; set; }

        [Required(ErrorMessage = "Por favor seleccione el estado")]
        [Range(1, long.MaxValue, ErrorMessage = "Por favor seleccione el estado")]
        public long Estado { get; set; }

        public string DescripcionEstado { get; set; }
    }
}
