using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Entities.Objects.ModuloBascula
{
    public class Contenedor: Entity
    {
        public virtual string Codigo { set; get; }
        public virtual string Descripcion { set; get; }
        public virtual double Peso { set; get; }
        public virtual double Eje1 { set; get; }
        public virtual double Eje2 { set; get; }
        public virtual double Eje3 { set; get; }
        public virtual string Estado { set; get; }
       
    }
}
