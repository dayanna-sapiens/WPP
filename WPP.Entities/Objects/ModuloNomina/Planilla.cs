using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Entities.Objects.ModuloNomina
{
    public class Planilla : Entity
    {
        public virtual string Descripcion { set; get; }
        public virtual Compania Compania { set; get; }
        public virtual string Estado { set; get; }
        public virtual IList<ItemNomina> DetallesNomina { set; get; }
    }
}
