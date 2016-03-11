using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;

namespace WPP.Entities.Objects.ModuloBascula
{
    public class ConfiguracionPuerto : Entity
    {
        public virtual String NombrePC { set; get; }
        public virtual String Puerto { set; get; }

    }
}
