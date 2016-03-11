using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPP.Model.ModuloContratos
{
    public class ServicioModel
    {
        public long Id { get; set; }
        
        [Required(ErrorMessage = "Por favor introduzca el nombre")]
        public virtual String Nombre { get; set; }

        public virtual bool Activo { get; set; }

        public long Compania { get; set; }
    }
}
