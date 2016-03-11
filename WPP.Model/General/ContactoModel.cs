using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPP.Model.General
{
    public class ContactoModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Por favor introduzca el nombre")]
        public String Nombre { get; set; }

        [Required(ErrorMessage = "Por favor introduzca el teléfono")]
        public String Telefono1 { get; set; }
        public String Ext1 { get; set; }
        public String Telefono2 { get; set; }
        public String Ext2 { get; set; }
        [Required(ErrorMessage = "Por favor introduzca la cédula")]
        [RegularExpression("^([a-zA-Z0-9]+)$", ErrorMessage = "No se permiten caracteres especiales -/.*!?")]
        public String Cedula { get; set; }

        [Required(ErrorMessage = "Por favor introduzca el correo")]
        [EmailAddress(ErrorMessage = "Por favor introduzca un correo válido")]
        public  String Email { get; set; }

        public String Observaciones { get; set; } 
        
        public String Horario { get; set; }

        public long ParentId { get; set; }
    }
}
