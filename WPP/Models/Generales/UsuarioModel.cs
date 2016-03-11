using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WPP.Models.Generales
{
    public class UsuarioModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Por favor introdusca el nombre")]
        public String Nombre { get; set; }

        [Required(ErrorMessage = "Por favor introdusca el apellido")]
        public String Apellido { get; set; }

        [Required(ErrorMessage = "Por favor introdusca la fecha de nacimiento")]
        public DateTime FechaNac { get; set; }

        [Required(ErrorMessage = "Por favor introdusca al menos un rol")]
        public String Roles { get; set; }

        [Required(ErrorMessage = "Por favor introdusca el correo")]
        [EmailAddress(ErrorMessage = "Por favor introdusca un correo válido")]
        [StringLength(80, ErrorMessage = "El correo no puede ser mayor a 80 caracteres")]
        public String Email { get; set; }

        [Required(ErrorMessage = "Por favor introdusca la contraseña")]
        [StringLength(50, ErrorMessage = "Password can't be more than 50 digits long")]
        [DataType(DataType.Password)]
        public String Password { get; set; }

        [Required(ErrorMessage = "Por favor confirme la contraseña")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

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