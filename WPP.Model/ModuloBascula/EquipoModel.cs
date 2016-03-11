using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPP.Model.ModuloBascula
{
    public class EquipoModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Por favor introduzca el nombre")]
        public String Nombre { get; set; }

        [Required(ErrorMessage = "Por favor introduzca la placa del equipo")]
        public String Placa { get; set; }

        [Required(ErrorMessage = "Por favor introduzca la marca del equipo")]
        public String Marca { get; set; }

        public double? Peso { get; set; }

        [Required(ErrorMessage = "Por favor seleccione el tipo de equipo")]
        public String Tipo { get; set; }

        public double? Eje1 { get; set; }

        public double? Eje2 { get; set; }

        public double? Eje3 { get; set; }

        public String TipoCombustible {get;set;}

    }
}
