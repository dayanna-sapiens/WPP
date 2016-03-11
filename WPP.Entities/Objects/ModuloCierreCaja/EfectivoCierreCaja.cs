using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;
using WPP.Entities.Objects.Generales;

namespace WPP.Entities.Objects.ModuloCierreCaja
{
    public class EfectivoCierreCaja : Entity
    {
        public virtual int Cantidad { set; get; }
        public virtual Catalogo Denominacion { set; get; }
        public virtual CierreCaja CierreCaja { set; get; }
    }
}
