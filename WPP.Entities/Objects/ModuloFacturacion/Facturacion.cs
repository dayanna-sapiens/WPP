using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Entities.Objects.ModuloFacturacion
{
    public class Facturacion : Entity
    {
        public virtual Compania Compania{ get; set; }
        public virtual Contrato Contrato { get; set; }
        public virtual DateTime FechaDesde { set; get; }
        public virtual DateTime FechaHasta { set; get; }
        public virtual Double Monto { set; get; }
        public virtual string Descripcion { set; get; }
        public virtual string Moneda { set; get; }
        public virtual string Estado { set; get; }
        public virtual long Consecutivo { set; get; }
        public virtual string Observaciones { set; get; }
        public virtual IList<DetalleFacturacion> ListaDetalleFacturacion { set; get; }
    }
}
