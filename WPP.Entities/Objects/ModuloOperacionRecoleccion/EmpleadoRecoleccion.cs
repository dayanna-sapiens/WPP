
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Entities.Objects.ModuloNomina;

namespace WPP.Entities.Objects.ModuloOperacionRecoleccion
{
    public class EmpleadoRecoleccion : Entity
    {
        public virtual String Nombre { set; get; }
        public virtual String Puesto { set; get; }
        public virtual String Cedula { set; get; }
        public virtual String Estado { set; get; }
        public virtual Compania Compania { set; get; }
        public virtual String Codigo { set; get; }
        public virtual Jornada Jornada { set; get; }
    }
}
