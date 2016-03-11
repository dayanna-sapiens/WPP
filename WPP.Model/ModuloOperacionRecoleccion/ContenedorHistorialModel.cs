using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPP.Model.ModuloOperacionRecoleccion
{
    public class ContenedorHistorialModel
    {
        public long Id { set; get; }
        public string Cliente { set; get; }
        public string Ubicacion { set; get; }
        public DateTime Fecha { set; get; }
        public long OTR { set; get; }
        public long ConsecutivoOTR { set; get; }
        public long Contendedor { set; get; }
        public long CodigoContenedor { set; get; }

    }
}
