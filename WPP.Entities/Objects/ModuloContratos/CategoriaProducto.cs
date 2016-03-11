using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;

namespace WPP.Entities.Objects.Generales
{
    public class CategoriaProducto: Entity
    {
        public virtual string Nombre { get; set; }
        public virtual string Tipo { get; set; }


    }
}