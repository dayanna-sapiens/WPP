using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Entities.Objects.ModuloBascula
{
    public class ConsecutivoReciboPago : Entity
    {
        public virtual Compania Compania { get; set; }
        public virtual long Secuencia { get; set; }
    }
}
