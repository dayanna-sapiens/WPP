using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;

namespace WPP.Entities.Objects.ModuloNomina
{
    public class CostoRutaRecoleccion: Entity
    {        
        public virtual RutaRecoleccion RutaRecoleccion { set; get; }

        public virtual DateTime FechaDesde { set; get; }

        public virtual DateTime FechaHasta { set; get; }

        public virtual double Costo { set; get; }

    }
}
