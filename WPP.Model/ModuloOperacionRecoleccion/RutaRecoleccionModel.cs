using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Model.ModuloOperacionRecoleccion
{
    public class RutaRecoleccionModel
    {
        public long Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor ingrese la descripción")]
        public string Descripcion { get; set; }

        public long Compania { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor seleccione el estado")]
        public string Estado { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor seleccione el tipo")]
        public string Tipo { get; set; }

        public string ListaRutas { get; set; }

        public List<ProductoContrato> Rutas { get; set; }
    }
}
