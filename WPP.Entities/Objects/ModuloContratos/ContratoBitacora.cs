using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;

namespace WPP.Entities.Objects.ModuloContratos
{
    public class ContratoBitacora : Entity
    {
        public virtual string Campo { set; get; }
        public virtual string valorAnterior { set; get; }
        public virtual string valorNuevo { set; get; }
        public virtual Contrato Contrato { set; get; }
    }
}
