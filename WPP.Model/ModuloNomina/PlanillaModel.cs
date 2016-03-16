using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloNomina;

namespace WPP.Model.ModuloNomina
{
    public class PlanillaModel
    {
        public virtual long Id { set; get; }
        public virtual long Consecutivo { set; get; }
        public virtual string Descripcion { set; get; }
        public virtual long Compania { set; get; }
        public virtual string Estado { set; get; }

        public virtual IList<ItemNomina> ListaDetalles { set; get; }
    }
}
