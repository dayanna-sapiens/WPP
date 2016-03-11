using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;

namespace WPP.Entities.Objects.Generales
{
    public class Anuncio : Entity
    {
        public virtual String Imagen1 { get; set; }
        public virtual String Imagen2 { get; set; }
        public virtual String Imagen3 { get; set; }
        public virtual String Imagen4 { get; set; }
    }
}
