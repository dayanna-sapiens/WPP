using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;
using WPP.Entities.Objects.Generales;
using WPP.Entities.Objects.ModuloCierreCaja;

namespace WPP.Entities.Objects.ModuloBascula
{
    public class PagoBascula : Entity
    {
        public virtual String FormaPago {set; get;}
        public virtual double Monto { set; get; }
        public virtual Bascula Boleta { set; get; }
        public virtual string Moneda { set; get; }
        public virtual double TipoCambio { set; get; }
        public virtual string NumeroTarjeta { set; get; }
        public virtual string NumeroAprobacion { set; get; }
        public virtual string DetalleTransferencia { set; get; }
        public virtual string NumeroTransferencia { set; get; }
        public virtual DateTime Fecha { set; get; }
        public virtual Catalogo Banco { set; get; }
        public virtual Boolean CierreCaja { set; get; }
        public virtual CierreCaja Cierre { set; get; }
    }
}
