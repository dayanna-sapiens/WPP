using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Entities.Objects.ModuloOperacionRecoleccion
{
    public class Cuadrilla : Entity
    {
        public virtual String Estado { set; get; }
        public virtual IList<EmpleadoRecoleccion>  ListaEmpleados { set; get; }
        public virtual String Descripcion { set; get; }
        public virtual Compania Compania { set; get; }


    }
}
