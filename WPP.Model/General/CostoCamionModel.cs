using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPP.Model.General
{
    public class CostoCamionModel
    {
        public long Id { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "Por favor ingrese el precio de costo por tonelada")]
        public Double Monto { get; set; }

        [Required(ErrorMessage = "Por favor ingresar la fecha desde")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaDesde { get; set; }

        [Required(ErrorMessage = "Por favor ingresar la fecha hasta")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaHasta { get; set; }

        [Required(ErrorMessage = "Por favor seleccione el tipo")]
        [Range(1, long.MaxValue, ErrorMessage = "Por favor seleccione el tipo")]
        public long Tipo { get; set; }

        public String DescripcionTipo { set; get; }
    }
}
