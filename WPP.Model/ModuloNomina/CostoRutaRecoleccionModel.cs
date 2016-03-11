using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPP.Model.ModuloNomina
{
    public class CostoRutaRecoleccionModel
    {
        public long Id { set; get; }

        public long RutaRecoleccion { set; get; }

        public string RutaRecoleccionDescripcion { set; get; }

        public DateTime FechaDesde { set; get; }

        public DateTime FechaHasta { set; get; }

        [Required(ErrorMessage = "Por favor seleccione el costo")]
        [Range(1, long.MaxValue, ErrorMessage = "Por favor seleccione el costo")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "{0} debe ser un número.")]
        public double Costo { set; get; }

    }
}
