using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPP.Model.General
{
    public class CostoHoraModel
    {    
        public long Id { set; get; }

        [Range(1, long.MaxValue, ErrorMessage = "Por favor ingrese el precio de costo por hora")]    
        public Double Monto { set; get; }

        [Required(ErrorMessage = "Por favor ingresar la fecha desde")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaInicio { set; get; }

        [Required(ErrorMessage = "Por favor ingresar la fecha fin")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaFin { set; get; }

        [Required(ErrorMessage = "Por favor ingresar el tipo")]
        public String Tipo { set; get; }
    }
}
