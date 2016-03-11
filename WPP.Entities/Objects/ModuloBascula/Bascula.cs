using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;
using WPP.Entities.Objects.ModuloCierreCaja;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;

namespace WPP.Entities.Objects.ModuloBascula
{
    public class Bascula : Entity
    {
        public virtual Compania Compania { get; set; }
        public virtual bool EquipoWPP { get; set; }
        public virtual Contrato Contrato { get; set; }
        public virtual string NombreCliente { get; set; }
        public virtual double PesoBruto{ get; set; }
        public virtual double PesoTara { get; set; }
        public virtual double PesoNeto { get; set; }
        public virtual long Boleta { get; set; }
        public virtual double Eje1 { get; set; }
        public virtual double Eje2 { get; set; }
        public virtual double Eje3 { get; set; }
        public virtual string Estado { get; set; }
        public virtual OTR OTR { get; set; }
        public virtual double Total { get; set; }
        public virtual ProductoContrato Producto { get; set; }
        public virtual Equipo Equipo { get; set; }
        public virtual DateTime Fecha { get; set; }
        public virtual IList<PagoBascula> ListaPagos { get; set; }
        public virtual string NumeroRecibo { get; set; }
        public virtual bool CierreCredito { get; set; }

        public virtual CierreCaja CierreCaja { get; set; }

        public virtual bool Facturada { set; get; }
    }
}
