using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;
using WPP.Entities.Objects.Generales;
using WPP.Entities.Objects.ModuloBascula;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Entities.Objects.ModuloOperacionRecoleccion
{
    public class ViajeOTR : Entity
    {
        public virtual Compania Compania { set; get; }
        public virtual OTR OTR { set; get; }
        public virtual String Observaciones { set; get; }
        public virtual Catalogo Accion { set; get; }
        public virtual Catalogo TipoEquipo { set; get; }
        public virtual Catalogo Tamano { set; get; }
        public virtual Contenedor Contenedor { set; get; }
        public virtual ProductoContrato Viaje { set; get; }

    }
}
