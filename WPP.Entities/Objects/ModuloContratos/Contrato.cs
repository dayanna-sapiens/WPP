using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;
using WPP.Entities.Objects.Generales;

namespace WPP.Entities.Objects.ModuloContratos
{
    public class Contrato : Entity
    {
        public virtual long? Numero { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual String DescripcionContrato { get; set; }
        public virtual int? MyProperty { get; set; }
        public virtual DateTime FechaInicio { get; set; }
        public virtual String Estado { get; set; }
        public virtual String ModoFacturacion { get; set; }
        public virtual bool DiaCorteEsMes { get; set; }
        public virtual int? DiaCorteMes { get; set; }
        public virtual Catalogo DiaCorteSemana { get; set; }
        public virtual PuntoFacturacion PuntoFacturacion { get; set; }
        public virtual int? DiasAvisoPrevioVencimiento { get; set; }
        public virtual Catalogo Repesaje { get; set; }
        public virtual int? NumeroFormulario { get; set; }
        public virtual String Observaciones { get; set; }
        public virtual String ObservacionesFactura { get; set; }
        public virtual Compania Compania { get; set; }
        public virtual String Moneda { get; set; }
        public virtual bool? FacturarColones{ get; set; }
        public virtual IList<ProductoContrato> Productos { get; set; }
        public virtual bool? PagoContado { get; set; }

        public virtual Usuario Ejecutivo { get; set; }
    }
}
