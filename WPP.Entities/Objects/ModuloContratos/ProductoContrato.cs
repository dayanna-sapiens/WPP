using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;
using WPP.Entities.Objects.Generales;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;

namespace WPP.Entities.Objects.ModuloContratos
{
    public class ProductoContrato : Entity
    {
        public virtual string Descripcion { get; set; }
        public virtual Producto Producto { get; set; }
        public virtual Servicio Servicio { get; set; }
        public virtual Catalogo EsquemaRelevancia { get; set; }
        public virtual UbicacionCliente Ubicacion { get; set; }
        public virtual DateTime FechaInicial { get; set; }
        public virtual DateTime FechaFinal { get; set; }
        public virtual string Ruta { get; set; }
        public virtual int Cantidad { get; set; }
        public virtual bool CobrarProductoCliente { get; set; }
        public virtual Catalogo Frecuecia { get; set; }
        public virtual string CuentaContableCredito { get; set; }
        public virtual double Monto { get; set; }
        public virtual double? Descuento { get; set; }
        public virtual double Total { get; set; }
        public virtual RutaRecoleccion RutaRecoleccion { get; set; }
        public virtual Contrato Contrato { get; set; }
        public virtual  string Estado { get; set; }
        public virtual string SubItem { get; set; }
        public virtual string DiasSemana { get; set; }
        public virtual Producto ProductoFosa { get; set; }
        public virtual double PrecioFosa { get; set; }
        public virtual DateTime FechaEstado { get; set; }
        public virtual Compania Proyecto { get; set; }
        public virtual bool LigadoRecoleccion { get; set; }
        public virtual ProductoContrato Recoleccion { get; set; }

    }
}
