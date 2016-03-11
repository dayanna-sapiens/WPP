using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Model.General
{
    public class PuntoFacturacionModel
    {
        public long Id { get; set; }
        public String Nombre { get; set; }
        public String IdCompanias { get; set; }
        public IList<Compania> Companias { get; set; }
    }
}
