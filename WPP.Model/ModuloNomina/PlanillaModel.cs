using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPP.Model.ModuloNomina
{
    public class PlanillaModel
    {
        public virtual long Id { set; get; }
        public virtual long Consecutivo { set; get; }
        public virtual string Descripcion { set; get; }
        public virtual long Compania { set; get; }
        public virtual string Estado { set; get; }

    }
}
