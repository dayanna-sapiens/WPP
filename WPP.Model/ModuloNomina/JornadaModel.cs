
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPP.Model.ModuloNomina
{
    public class JornadaModel
    {
        public long Id { set; get; }

        [Required(ErrorMessage = "Por favor ingrese la descripción")]
        public string Descripcion { set; get; }

        [Required(ErrorMessage = "Por favor seleccione el tipo de jornada laboral")]
        public string Tipo { set; get; }

        [Required(ErrorMessage = "Por favor seleccione la hora de inicio de la jornada")]
        public string HoraInicio { set; get; }

        [Required(ErrorMessage = "Por favor seleccione la hora final de la jornada")]
        public string HoraFin { set; get; }
    }
}
