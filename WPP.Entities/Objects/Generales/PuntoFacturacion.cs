using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Entities.Objects.Generales
{
    public class PuntoFacturacion : Entity
    {
        public virtual String Nombre { get; set; }
        public virtual IList<Compania> Companias { get; set; }
    }
}
