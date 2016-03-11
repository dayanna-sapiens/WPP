using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Entities.Objects.ModuloOperacionRecoleccion
{
    public class RutaRecoleccion : Entity
    {
        public virtual string Descripcion { get; set; }
        public virtual String Estado { get; set; }
        public virtual Compania Compania { get; set; }
        public virtual IList<ProductoContrato> Rutas { get; set; }
        public virtual string Tipo { get; set; }
    }
}
