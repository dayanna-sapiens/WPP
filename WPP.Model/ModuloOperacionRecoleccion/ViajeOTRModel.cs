using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPP.Model.ModuloOperacionRecoleccion
{
    public class ViajeOTRModel
    {
        public long Id { get; set; }
        
        public long Compania { get; set; }

        public String Observaciones { get; set; }

        public long OTR { get; set; }

        public String AccionDescripcion { get; set; }

        public long Accion { get; set; }

        public String TamanoDescripcion { get; set; }

        public long Tamano { get; set; }

        public String TipoEquipoDescripcion { get; set; }

        public long TipoEquipo { get; set; }

        public string ContenedorDescripcion { get; set; }

        public long Contenedor { get; set; }

        public String ViajeDescripcion { get; set; }

        public long Viaje { get; set; }

        
   

    }
}
