using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;
using WPP.Entities.Objects.Generales;
using WPP.Entities.Objects.ModuloBascula;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Entities.Objects.ModuloOperacionRecoleccion
{
    public class OTR : Entity
    {
        public virtual Compania Compania { set; get; }
        public virtual long Consecutivo { set; get; }
        public virtual Equipo Equipo { set; get; }
        public virtual RellenoSanitario Origen { set; get; }
        public virtual RellenoSanitario Destino { set; get; }
        public virtual RellenoSanitario Relleno { set; get; }
        public virtual DateTime Fecha { set; get; }
       // public virtual ProductoContrato Ruta { set; get; }
        public virtual RutaRecoleccion RutaRecoleccion { set; get; }
        public virtual Cliente Cliente { set; get; }
        public virtual Contrato Contrato { set; get; }
        public virtual String Estado { set; get; }
        public virtual String Tipo { set; get; }
        public virtual String Observaciones { set; get; }
       // public virtual bool OTRMadre { set; get; }
      //  public virtual String OTRHijas { set; get; }       
        public virtual Cuadrilla Cuadrilla { set; get; }
        public virtual String DesechosRecibidos { set; get; }
        public virtual String ObservacionesTrabajo { set; get; }
        public virtual String ObservacionesMecanicas { set; get; }
        public virtual String SalidaRelleno { set; get; }
        public virtual String FinalizaOTR { set; get; }
        public virtual String IniciaRecoleccion { set; get; }
        public virtual String FinalizaRecoleccion { set; get; }
        public virtual String InicioDemora { set; get; }
        public virtual String FinalizaDemora { set; get; }
        public virtual String IngresoRelleno { set; get; }
        public virtual Double KilometrajeInicio { set; get; }
        public virtual Double KilometrajeFin { set; get; }
        public virtual Double Combustible { set; get; }
        public virtual Double HorimetroInicio { set; get; }
        public virtual Double HorimetroFin { set; get; }

       // public virtual IList<OTR> ListaOTRHijas { set; get; }
        public virtual IList<ViajeOTR> ListaViajesOTR { set; get; }

        public virtual bool Facturada { set; get; }

    }
}
