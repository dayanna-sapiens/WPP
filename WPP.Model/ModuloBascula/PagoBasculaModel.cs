using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPP.Model.ModuloBascula
{
    public class PagoBasculaModel
    {
        public long Id { set; get; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor seleccione la forma de pago")]
        public String FormaPago { set; get; }

        public long Bacula { set; get; }

        [Required(ErrorMessage = "Por favor ingrese el monto del pago")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "El monto deben ser un valor númerico")]
        public double Monto { set; get; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor seleccione la moneda")]
        public string Moneda { set; get; }

        public double TipoCambio { set; get; }

        public string NumeroTarjeta { set; get; }

        public string NumeroAprobacion { set; get; }

        public string NumeroTransferencia { set; get; }

        public string DetalleTransferencia { set; get; }

        public long? Banco { set; get; }

        public string BancoDescripcion { set; get; }

        public bool CierreCaja { set; get; }

        public long? Boleta { set; get; }

        public DateTime? Fecha { set; get; }

        public long Cierre { set; get; }
    }
}