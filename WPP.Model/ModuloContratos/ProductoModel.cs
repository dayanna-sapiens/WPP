using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPP.Model.ModuloContratos
{
    public class ProductoModel
    {
        public long Id { get; set; }

        public long Compania { get; set; }

        [Required(ErrorMessage = "Por favor introduzca la descripción")]
        [StringLength(255, ErrorMessage = "La descripción del producto no puede ser mayor a los 255 caracteres")]
        public virtual String Descripcion { get; set; }

        [Required(ErrorMessage = "Por favor seleccione el tipo de producto")]
        [Range(1, long.MaxValue, ErrorMessage = "Por favor seleccione el tipo de producto")]
        public virtual long Categoria { get; set; }

        [Required(ErrorMessage = "Por favor seleccione el tipo de equipo")]
        [Range(1, long.MaxValue, ErrorMessage = "Por favor seleccione el tipo de equipo")]
        public long TipoEquipo { get; set; }

        public long? UnidadCobro { get; set; }

        public long? ProcesoCobro { get; set; }

        [Required(ErrorMessage = "Por favor introduzca el estado")]
        [Range(1, long.MaxValue, ErrorMessage = "Por favor introduzca el estado")]
        public long Estado { get; set; }

        public bool RutasRecoleccion { get; set; }

        public String CategoriaNombre { get; set; }

        public String TipoEquipoNombre { get; set; }

        public String UnidadCobroNombre { get; set; }

        public String ProcesoCobroNombre { get; set; }

        public String EstadoNombre { get; set; }

        public long? Tamano { get; set; }

        public String TamanoNombre { get; set; }

        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "{0} debe ser un número.")]
        public double? Precio { get; set; }

        public string Moneda { get; set; }
      
    }
}
