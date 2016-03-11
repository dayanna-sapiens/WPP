using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPP.Model.ModuloContratos
{
    public class CategoriaProductoModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Por favor introduzca la descripción")]
        public virtual String Nombre { get; set; }
                
        public String Tipo { get; set; }
               
            
        
    }
}
