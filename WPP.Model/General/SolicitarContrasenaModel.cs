using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPP.Model.General
{
    public class SolicitarContrasenaModel
    {
        [Required(ErrorMessage = "Por favor introduzca su email")]
        [EmailAddress(ErrorMessage = "Por favor introduzca un email válido")]
        [StringLength(80, ErrorMessage = "El email no puede exceder de 80 caracteres")]
        public String Email { get; set; }

        [Required(ErrorMessage = "Por favor introduzca su cédula")]
        public String Cedula{ get; set; }
    }
}
