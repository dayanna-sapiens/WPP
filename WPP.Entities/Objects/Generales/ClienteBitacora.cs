using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Entities.Objects.Generales
{
    public class ClienteBitacora : Entity
    {
        public virtual string Campo { set; get; }
        public virtual string valorAnterior { set; get; }
        public virtual string valorNuevo { set; get; }
        public virtual Cliente Cliente { set; get; }
    }
}
