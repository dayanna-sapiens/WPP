using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;

namespace WPP.Entities.Objects.ModuloContratos
{
    public class Servicio : Entity
    {
         public virtual String Nombre { get; set; }
         public virtual bool Activo { get; set; }
         public virtual Compania Compania { get; set; }
    }
}
