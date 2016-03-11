using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;
using WPP.Entities.Objects.ModuloBascula;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Entities.Objects.ModuloOperacionRecoleccion
{
    public class ContenedorHistorial : Entity
    {
        public virtual Contenedor Contenedor { set; get; }
        public virtual OTR OTR { set; get; }
        public virtual String Ubicacion { set; get; }
        public virtual DateTime Fecha { set; get; }
        public virtual String Cliente { set; get; }   

    }
}
