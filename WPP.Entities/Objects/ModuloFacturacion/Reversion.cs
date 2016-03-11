using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Entities.Objects.ModuloFacturacion
{
    public class Reversion : Entity
    {
        public virtual String Consecutivo { set; get; }
        public virtual Compania Compania { set; get; }
        public virtual Facturacion Facturacion { set; get; }
        public virtual String Observaciones { set; get; }
    }
}
