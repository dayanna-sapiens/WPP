using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPP.Model
{
    public class UsuarioModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Por favor introduzca el nombre")]
        public String Nombre { get; set; }

        [Required(ErrorMessage = "Por favor introduzca el apellido 1")]
        public String Apellido1 { get; set; }

        [Required(ErrorMessage = "Por favor introduzca el apellido 2")]
        public String Apellido2 { get; set; }

        [Required(ErrorMessage = "Por favor introduzca la fecha de nacimiento")]
        public DateTime FechaNac { get; set; }

        [Required(ErrorMessage = "Por favor introduzca el cédula")]
        [RegularExpression("^([a-zA-Z0-9]+)$", ErrorMessage = "No se permiten caracteres especiales -/.*!?")]
        public String Cedula { get; set; }
        
        [Required(ErrorMessage = "Por favor introduzca el número telefónico")]    
        [Phone(ErrorMessage = "Por favor introduzca un número telefónico válido")]
        public String Telefono { get; set; }

        [Required(ErrorMessage = "Por favor introduzca al menos un rol")]
        public String Roles { get; set; }

        [Required(ErrorMessage = "Por favor introduzca al menos una compañía")]
        public String Companias { get; set; }


        [Required(ErrorMessage = "Por favor introduzca el correo")]
        [EmailAddress(ErrorMessage = "Por favor introduzca un correo válido")]
        [StringLength(80, ErrorMessage = "El correo no puede ser mayor a 80 caracteres")]
        public String Email { get; set; }

        [Required(ErrorMessage = "Por favor introduzca la contraseña")]
        [StringLength(50, ErrorMessage = "La contraseña no puede ser mayor a 50 caracteres")]
        [DataType(DataType.Password)]
        public String Password { get; set; }

        [Required(ErrorMessage = "Por favor confirme la contraseña")]
        [StringLength(255, ErrorMessage = "La contraseña debe tener al menos 5 caracteres", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public string NumeroEmpleado { get; set; }

        public String NombreCompleto { get; set; }

        public int Edad
        {
            get
            {
                DateTime today = DateTime.Today;
                int edad = today.Year - FechaNac.Year;
                if (FechaNac > today.AddYears(-edad)) edad--;
                return edad;

            }
        }

    }
}
