using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPP.Model.ModuloOperacionRecoleccion
{
    public class EmpleadoRecoleccionModel
    {
        public long Id { get; set; }

        public virtual String Nombre { get; set; }

        public virtual String Estado { get; set; }

        public virtual String Puesto { get; set; }

        public virtual String Cedula { get; set; }

        public virtual String Codigo { get; set; }

        public virtual long Compania { get; set; }

        [Required(ErrorMessage = "Por favor seleccione la jornada laboral")]
        [Range(1, long.MaxValue, ErrorMessage = "Por favor seleccione la jornada laboral")]
        public virtual long Jornada { get; set; }
        public virtual string JornadaDescripcion { get; set; }
    }
}
