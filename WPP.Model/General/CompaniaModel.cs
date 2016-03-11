using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;

namespace WPP.Model
{
    public class CompaniaModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Por favor introduzca el nombre")]
        public virtual String Nombre { get; set; }
        
        [Required(ErrorMessage = "Por favor seleccione el grupo")]
        [Range(1, long.MaxValue, ErrorMessage = "Por favor seleccione el grupo")]
        public virtual long Grupo { get; set; }

        [Required(ErrorMessage = "Por favor seleccione el tipo")]
        [Range(1, long.MaxValue, ErrorMessage = "Por favor seleccione el tipo")]
        public virtual long Tipo { get; set; }
        
        [Required(ErrorMessage = "Por favor introduzca el nombre corto de la compañía")]
        public virtual String NombreCorto { get; set; }

        [EmailAddress(ErrorMessage = "Por favor introduzca un correo válido")]
        [StringLength(80, ErrorMessage = "El correo no puede ser mayor a 80 caracteres")]
        [Required(ErrorMessage = "Por favor introduzca el correo")]
        public String Email { get; set; }

        [Required(ErrorMessage = "Por favor introduzca el teléfono")]
        public String Telefono { get; set; }

        [Required(ErrorMessage = "Por favor introduzca el represntante legal")]
        public String RepresentanteLegal { get; set; }

        [Required(ErrorMessage = "Por favor introduzca la cédula")]
        [RegularExpression("^([a-zA-Z0-9]+)$", ErrorMessage = "No se permiten caracteres especiales -/.*!?")]
        public String Cedula { get; set; }

        public String TipoNombre { get; set; }

        public String GrupoNombre { get; set; }

        public virtual long ClienteId { get; set; }

    }
}
