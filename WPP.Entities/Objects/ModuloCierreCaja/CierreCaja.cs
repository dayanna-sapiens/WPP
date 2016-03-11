using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;
using WPP.Entities.Objects.ModuloBascula;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Entities.Objects.ModuloCierreCaja
{
    public class CierreCaja: Entity
    {
        public virtual Compania Compania { set; get; }
        public virtual String Moneda { set; get; }
        public virtual Double BalanceApertura { set; get; }     
        public virtual Double Efectivo { set; get; }
        public virtual Double Tarjetas { set; get; }   
        public virtual Double Transferencia { set; get; }
        public virtual Double BalanceCierre { set; get; }
        public virtual Double AjusteCaja { set; get; }
        public virtual Double ConteoTotal { set; get; }
        public virtual Double Diferencia { set; get; }
        public virtual IList<EfectivoCierreCaja> ListaPagoEfectivo { set; get; }
        public virtual IList<PagoBascula> Pagos{ set; get; }
        public virtual IList<Bascula> Creditos { set; get; }
        public virtual long Consecutivo { set; get; }
    }
}
