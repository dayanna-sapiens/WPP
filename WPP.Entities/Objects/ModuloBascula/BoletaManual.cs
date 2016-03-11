using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;

namespace WPP.Entities.Objects.ModuloBascula
{
    public class BoletaManual : Entity
    {
        public virtual OTR OTR { set; get; }
        public virtual Compania Compania { set; get; }
        public virtual DateTime Fecha { set; get; }
        public virtual String Hora { set; get; }
        public virtual String NumeroBoleta { set; get; }
        public virtual double PesoBruto { set; get; }
        public virtual double PesoTara { set; get; }
        public virtual double PesoNeto { set; get; }
        public virtual string Observaciones { set; get; }
        public virtual RellenoSanitario Sitio { set; get; }
        public virtual string Estado { set; get; }
        public virtual string DescripcionCliente { set; get; }
        public virtual bool Facturada { set; get; }
    }
}
