using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;
using WPP.Entities.Objects.Generales;

namespace WPP.Entities.Objects.ModuloContratos
{
    public class Producto : Entity
    {
        public virtual Catalogo UnidadCobro { get; set; }
        public virtual Catalogo ProcesoCobro { get; set; }
        public virtual Catalogo TipoEquipo { get; set; }
        public virtual Catalogo Tamano { get; set; }
        public virtual Catalogo Estado { get; set; }
        public virtual CategoriaProducto Categoria { get; set; }
        public virtual bool RutasRecoleccion{ get; set; }
        public virtual string Descripcion { get; set; }
        public virtual long Compania { get; set; }
        public virtual double Precio { get; set; }

        public virtual string Moneda { get; set; }
    }
}
