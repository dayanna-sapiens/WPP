using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;

namespace WPP.Entities.Objects.ModuloNomina
{
    public class DiasFestivos : Entity
    {
        public virtual int Dia { set; get; }
        public virtual int Mes { set; get; }
        public virtual string Descripcion { set; get; }

    }
}
