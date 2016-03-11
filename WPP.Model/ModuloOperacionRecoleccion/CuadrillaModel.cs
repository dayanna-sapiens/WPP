using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;

namespace WPP.Model.ModuloOperacionRecoleccion
{
    public class CuadrillaModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Por favor introduzca la descripcion")]
        public String Descripcion { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor seleccione el estado")]
        public String Estado { get; set; }

        public long Compania { get; set; }

        public String ChoferDescripcion { get; set; }

        [Required(ErrorMessage = "Por favor seleccione el chofer")]
        [Range(1, long.MaxValue, ErrorMessage = "Por favor seleccione el chofer")]public long Chofer { get; set; }
        public List<EmpleadoRecoleccion> ListaEmpleados { set; get; }

        public String EmpleadosId { set; get; }

    }
}
