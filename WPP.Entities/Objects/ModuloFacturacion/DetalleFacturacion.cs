using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;
using WPP.Entities.Objects.Generales;
using WPP.Entities.Objects.ModuloBascula;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;

namespace WPP.Entities.Objects.ModuloFacturacion
{
    public class DetalleFacturacion : Entity
    {
        public virtual Facturacion Facturacion { set; get; }
        public virtual ProductoContrato Producto { set; get; }
        public virtual OTR OTR { set; get; }
        public virtual double Cantidad { set; get; }
        public virtual Bascula Bascula { set; get; }
        public virtual double Monto { set; get; }
        public virtual string Moneda { set; get; }
        public virtual string Periodo { set; get; }
        public virtual BoletaManual BoletaManual { set; get; }

    }
}
