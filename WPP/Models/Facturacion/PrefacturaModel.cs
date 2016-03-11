using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WPP.Model.ModuloFacturacion;

namespace WPP.Models.Facturacion
{
    public class PrefacturaModel
    {
        public long Contrato { set; get; }
        public DateTime Desde { set; get; }
        public DateTime Hasta{ set; get; }
        public IList<Prefacturacion> ListaDetalle { set; get; }
    }
}