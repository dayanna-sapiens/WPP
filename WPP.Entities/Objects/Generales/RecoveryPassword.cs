using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;

namespace WPP.Entities.Objects.Generales
{
    public class RecoveryPassword : Entity
    {
        public virtual String Token { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
