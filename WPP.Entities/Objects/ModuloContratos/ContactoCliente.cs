using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;

namespace WPP.Entities.Objects.ModuloContratos
{
    public class ContactoCliente : ContactoBase
    {
        public virtual Cliente Cliente { get; set; }
    }
}
