using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPP.Model.ModuloCierreCaja
{
    public class CierreCajaModel
    {
        public long Id { get; set; }

        public long Compania { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor seleccione la moneda")]
        public String Moneda { get; set; }

        [Required(ErrorMessage = "Por favor indique el balance de apertura")]
        [Range(0, long.MaxValue, ErrorMessage = "Por favor indique el avance de apertura")]
        public Double BalanceApertura { set; get; }

        public Double? Efectivo { set; get; }

        public Double? Tarjetas { set; get; }

        public Double? Transferencia { set; get; }

        public Double? BalanceCierre { set; get; }

        public Double? AjusteCaja { set; get; }

        public Double? ConteoTotal { set; get; }

        public Double? Diferencia { set; get; }

        public String PagoEfectivo { set; get; }

        public String PagosBascula { set; get; }

        public String ListaPagos { set; get; }

        public String ListaCreditos { set; get; }

        public long Consecutivo { set; get; }
    }
}
