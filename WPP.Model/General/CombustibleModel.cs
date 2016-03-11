using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPP.Model.General
{
    public class CombustibleModel
    {
        public long Id { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "Por favor ingrese el precio de la gasolina el monto")]
        public Double Gasolina { get; set; }
        
        [Range(1, long.MaxValue, ErrorMessage = "Por favor ingrese el precio del diesel el monto")]
        public Double Diesel { get; set; }
        
        public DateTime FechaDesde{ get; set; }
        
        public DateTime FechaHasta { get; set; }

    }
}
