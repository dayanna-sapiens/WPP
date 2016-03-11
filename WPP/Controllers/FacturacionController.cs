using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using WPP.Entities.Objects.Generales;
using WPP.Entities.Objects.ModuloBascula;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Entities.Objects.ModuloFacturacion;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;
using WPP.Helpers;
using WPP.Mapper;
using WPP.Mapper.ModuloFacturacion;
using WPP.Model.ModuloFacturacion;
using WPP.Security;
using WPP.Service.Generales;
using WPP.Service.ModuloBascula;
using WPP.Service.ModuloBoletaManual;
using WPP.Service.ModuloContratos;
using WPP.Service.ModuloDetalleFacturacion;
using WPP.Service.ModuloFacturacion;
using WPP.Service.ModuloOperacionRecoleccion;
using CrystalDecisions.CrystalReports.Engine;
using WPP.Datos.Facturacion;
using CrystalDecisions.Shared;
using WPP.Models.Facturacion;
using Newtonsoft.Json;

namespace WPP.Controllers
{
    public class FacturacionController : BaseController
    {

        ICompaniaService companiaService;
        IFacturacionService facturacionService;
        IConsecutivoFacturacionService consecutivoService;
        IContratoService contratoService;
        IBasculaService basculaService;
        IOTRService otrService;
        ITipoCambioService tipoCambioService;
        IDetalleFacturacionService detalleService;
        CompaniaMapper companiaMapper;
        FacturacionMapper facturacionMapper;
        IBoletaManualService boletaManualService;
        IProductoContratoService productoContratoService;
        IReversionService reversionService;
        private String mensaje = "";

        public FacturacionController( ICompaniaService compania, IFacturacionService facturacion, IConsecutivoFacturacionService consecutivo, IContratoService contrato, IBasculaService bascula, IOTRService otr, ITipoCambioService tipocambio, IDetalleFacturacionService  detalle, IBoletaManualService boletaManual, IProductoContratoService productoContrato, IReversionService reversion)
        {
            try
            {
                this.companiaService = compania;
                this.facturacionService = facturacion;
                this.consecutivoService = consecutivo;
                this.contratoService = contrato;
                this.basculaService = bascula;
                this.otrService = otr;
                this.tipoCambioService = tipocambio;
                this.detalleService = detalle;
                this.boletaManualService = boletaManual;
                this.productoContratoService = productoContrato;
                this.reversionService = reversion;
                facturacionMapper = new FacturacionMapper();
                companiaMapper = new CompaniaMapper();
                mensaje = ActualizarTipoCambio() == true ? String.Empty : "Es necesario registrar el tipo de cambio de hoy";
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }


        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO)]
        public ActionResult Index()
        {
            if (mensaje != String.Empty)
            {
                TempData["alertMessage"] = mensaje;
            }
            return View();
        }

        #region PREFACTURACION

        /// <summary>
        /// Este método carga la información necesaria que se necesita despegar por pantalla, 
        /// tal como el tipo de cambio y el ultimo consec utivo de facturacion que fue impreso
        /// </summary>
        /// <returns>La vista PreFacturacion con los ViewBag que contiene la informacion necesaria </returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_PREFACTURACION)]
        public ActionResult PreFacturacion()
        {
            try
            {
                if (mensaje != String.Empty)
                {
                    TempData["alertMessage"] = mensaje;
                }

                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Fecha", DateTime.Now.ToString("dd/MM/yyyy"));
                var cambios = tipoCambioService.GetAll(criteria);
                ViewBag.TipoCambioVenta = cambios.Where(s => s.Tipo == "Venta").FirstOrDefault();

                Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                criteria = new Dictionary<string, object>();
                criteria.Add("Compania", compania);
                ConsecutivoFacturacion consecutivo = consecutivoService.Get(criteria);
                ViewBag.Consecutivo = consecutivo.Secuencia - 1;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }

            return View();
        }

        /// <summary>
        /// Este método se encarga de crear la prefactura que se indico para un contrato especifico en un periodo determinado
        /// </summary>
        /// <returns>un JSON con </returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_PREFACTURACION)]
        public JsonResult GenerarPrefactura(long contrato, string desde, string hasta)
        {
            try
            {
                DateTime fechaDesde = Convert.ToDateTime(desde);
                DateTime fechaHasta = Convert.ToDateTime(hasta);
                long compania = Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0;
                Contrato contratoActual = contratoService.Get(contrato);

                // Se guarda los detalles de la faturacion y se obtiene la nueva informacion producto de esta consulta
                var listaPrefacturacion = ObtenerDetalleFacturacion(contratoActual, desde, hasta, compania);

                return Json(listaPrefacturacion);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(null);
            }
        }


        /// <summary>
        /// Este método se encarga de obtener todos los servicios brindados a este contrato en el tiempo indicado para que sean facturados
        /// (Disposición, Recolección, Alquiler, etc)
        /// </summary>
        /// <returns>la factura con todos el detalle de los servicios a cobrar </returns>
        private IList<Object> ObtenerDetalleFacturacion(Contrato contrato, String desde, String hasta, long compania)
        {

            DateTime fechaDesde = Convert.ToDateTime(desde);
            DateTime fechaHasta = Convert.ToDateTime(hasta);
            Double Total = 0;
            Compania companiaActual = companiaService.Get(compania);
            IList<Prefacturacion> ListaDetalleFacturacion = new List<Prefacturacion>();

            foreach (ProductoContrato servicio in contrato.Productos)
            {
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                switch (servicio.Producto.Categoria.Tipo)
                {
                    case "Recoleccion": // servicios de Recolección (Carga Trasera / Roll-Off), se obtiene la información por medio de OTRs
                        {
                            criteria = new Dictionary<string, object>();
                            criteria.Add("Compania", servicio.Proyecto);
                            criteria.Add("IsDeleted", false);
                            criteria.Add("Facturada", false);

                            IList<OTR> ListaOTR = otrService.GetAll(criteria);
                            ListaOTR = ListaOTR.Where(s => s.Fecha < fechaHasta.AddDays(-1) && s.Fecha >= fechaDesde.AddDays(1) && s.Estado != "Anulada" && s.Estado != "Activo").ToList();//
                            IList<OTR> ListaOTRFiltrada = new List<OTR>();

                            foreach (var otr in ListaOTR)
                            {
                                IList<ProductoContrato> listaRutas = otr.RutaRecoleccion.Rutas;
                                int cantServicio = listaRutas.Where(s => s.Producto.Id == servicio.Producto.Id).ToList().Count;
                                if (cantServicio > 0)
                                {
                                    if (ListaOTRFiltrada.Where(s => s.Id == otr.Id).ToList().Count == 0)
                                    {
                                        ListaOTRFiltrada.Add(otr);
                                    }
                                }
                            }

                            foreach (var itemOTR in ListaOTRFiltrada)
                            {
                                //Se crea el detalle de la factura correspondiente a este servicio
                                Prefacturacion detalle = new Prefacturacion();                             
                                detalle.Producto = servicio.Id;
                                detalle.OTR = itemOTR.Id;
                                detalle.ConsecutivoOTR = itemOTR.Consecutivo;
                                detalle.Periodo = "Del " + desde + " al " + hasta;
                                detalle.Moneda = contrato.Moneda;
                                detalle.Descripcion = servicio.Descripcion;
                                detalle.Fecha = itemOTR.Fecha;
                                detalle.Unidad = servicio.Producto.UnidadCobro.Nombre;
                                double Monto = 0;
                                double Peso = 0;

                                // Se evalua el tipo de OTR para verificar el monto que se debe cobrar segun el servicio brindado
                                switch (itemOTR.Tipo)
                                {
                                    case "Municipal":
                                        {
                                            IDictionary<string, object> criteriaBascula = new Dictionary<string, object>();
                                            criteriaBascula.Add("OTR", itemOTR);
                                            criteriaBascula.Add("Estado", "Activo");
                                            criteriaBascula.Add("IsDeleted", false);
                                            criteriaBascula.Add("Facturada", false);

                                            // Se obtienen todas las boletas con las condiciones necesarias en un rango de fechas dado
                                            Bascula BoletaOTR = basculaService.Get(criteriaBascula);
                                            if (BoletaOTR.Id == 0) // Si la boleta de bascula de esta OTR se realizo de manera manual
                                            {
                                                BoletaManual boletaManual = boletaManualService.Get(criteriaBascula);
                                                detalle.BoletaManual = boletaManual.Id;
                                                detalle.ConsecutivoBoleta = boletaManual.NumeroBoleta;
                                                detalle.Cantidad = boletaManual.PesoNeto;
                                                Peso = (boletaManual.PesoNeto / 1000);
                                                Monto = (Peso > 1 ? Peso : 1) * servicio.Monto;
                                                
                                            }
                                            else
                                            {
                                                detalle.Bascula = BoletaOTR.Id;
                                                detalle.ConsecutivoBoleta = BoletaOTR.Boleta.ToString();
                                                detalle.Cantidad = BoletaOTR.PesoNeto;
                                                Peso = (BoletaOTR.PesoNeto / 1000);
                                                Monto = (Peso > 1 ? Peso : 1) * servicio.Monto;
                                            }
                                        }

                                        break;

                                    case "Comercial":
                                        {
                                            int diasxCobrar = 0;

                                            // Se toma en cuenta el estado para concer la cantidad de dias que deben ser facturados
                                            if (servicio.Estado == "Activo")
                                            {
                                                TimeSpan diferencia = fechaHasta - servicio.FechaEstado;
                                                diasxCobrar = diferencia.Days > 0 ? diferencia.Days : 0;
                                            }
                                            else
                                            {
                                                TimeSpan diferencia = servicio.FechaEstado - fechaDesde;
                                                diasxCobrar = diferencia.Days > 0 ? diferencia.Days : 0;
                                            }

                                            // Se obtiene el monto por dia y el precio por los dias de servicio brindado
                                            double precioxDia = servicio.Monto / 30;
                                            double meses = diasxCobrar / 30;
                                            meses = meses <= 1 ? 1 : meses; 

                                            if (diasxCobrar <= 31 && diasxCobrar >= 28)
                                            {

                                                Monto = meses * servicio.Monto;
                                            }
                                            else
                                            {
                                                Monto = diasxCobrar * precioxDia;
                                            }

                                           
                                            detalle.Cantidad = meses;
                                            IDictionary<string, object> criteriaBascula = new Dictionary<string, object>();
                                            criteriaBascula.Add("OTR", itemOTR);
                                            criteriaBascula.Add("Estado", "Activo");
                                            criteriaBascula.Add("IsDeleted", false);
                                            criteriaBascula.Add("Facturada", false);

                                            Bascula BoletaOTR = basculaService.Get(criteriaBascula);

                                            if (BoletaOTR == null) // Si la boleta de bascula de esta OTR se realizo de manera manual
                                            {
                                                BoletaManual boletaManual = boletaManualService.Get(criteriaBascula);
                                                if (boletaManual != null)
                                                {
                                                    detalle.BoletaManual = boletaManual.Id;
                                                    detalle.ConsecutivoBoleta = boletaManual.NumeroBoleta;
                                                }
                                            }
                                            else
                                            {
                                                detalle.Bascula = BoletaOTR.Id;
                                                detalle.ConsecutivoBoleta = BoletaOTR.Boleta.ToString();
                                            }
                                        }
                                        break;

                                    case "Roll-Off":
                                        {
                                            IDictionary<string, object> criteriaBascula = new Dictionary<string, object>();
                                            criteriaBascula.Add("OTR", itemOTR);
                                            criteriaBascula.Add("Estado", "Activo");
                                            criteriaBascula.Add("IsDeleted", false);
                                            criteriaBascula.Add("Facturada", false);

                                            // Se obtienen todas las boletas con las condiciones necesarias en un rango de fechas dado
                                            Bascula BoletaOTR = basculaService.Get(criteriaBascula);

                                            if (BoletaOTR == null) // Si la boleta de bascula de esta OTR se realizo de manera manual
                                            {
                                                BoletaManual boletaManual = boletaManualService.Get(criteriaBascula);
                                                if (boletaManual != null)
                                                {
                                                    detalle.BoletaManual = boletaManual.Id;
                                                    detalle.ConsecutivoBoleta = boletaManual.NumeroBoleta;
                                                    Peso = (boletaManual.PesoNeto / 1000);
                                                    Monto = (Peso > 1 ? Peso : 1) * servicio.Monto;
                                                    detalle.Cantidad = boletaManual.PesoNeto;
                                                }
                                            }
                                            else
                                            {
                                                detalle.Bascula = BoletaOTR.Id;
                                                detalle.ConsecutivoBoleta = BoletaOTR.Boleta.ToString();
                                                Peso = (BoletaOTR.PesoNeto / 1000);
                                                Monto = (Peso > 1 ? Peso : 1) * servicio.Monto;
                                                detalle.Cantidad = BoletaOTR.PesoNeto;
                                            }
                                        }
                                        break;
                                }
                                if (contrato.Moneda == "Colones")
                                {
                                    detalle.Precio = Monto;
                                }
                                else
                                {
                                    // Si el contrato esta en dólares pero se desea facturar en colones, 
                                    // se debe realizar la conversión de moneda con el tipo de cambio actual
                                    if ((bool)contrato.FacturarColones)
                                    {
                                        TipoCambio tipoCambio = new TipoCambio();
                                        IDictionary<string, object> criteriaTipoCambio = new Dictionary<string, object>();
                                        criteriaTipoCambio.Add("Fecha", DateTime.Now.ToString("dd/MM/yyyy"));
                                        criteriaTipoCambio.Add("Tipo", "Venta");
                                        tipoCambio = tipoCambioService.Get(criteriaTipoCambio);
                                        detalle.Precio = (Monto * tipoCambio.Valor);
                                        detalle.Moneda = "Colones";
                                    }
                                    else
                                    {
                                        detalle.Precio = Monto;
                                    }
                                }
                                                              
                                ListaDetalleFacturacion.Add(detalle);
                                Total += detalle.Precio;
                            }
                        }
                        break;

                    case "Ninguno": // Servicios como Alquiler, que se deben cobrar de manera Mensual
                        {
                            int diasxCobrar = 0;

                            // Se toma en cuenta el estado para concer la cantidad de dias que deben ser facturados
                            if (servicio.Estado == "Activo")
                            {
                                TimeSpan diferencia = fechaHasta - servicio.FechaEstado;
                                diasxCobrar = diferencia.Days > 0 ? diferencia.Days : 0;
                            }
                            else
                            {
                                TimeSpan diferencia = servicio.FechaEstado - fechaDesde;
                                diasxCobrar = diferencia.Days > 0 ? diferencia.Days : 0;
                            }

                            // Se obtiene el monto por dia y el precio por los dias de servicio brindado
                            double precioxDia = servicio.Monto / 30;
                            double Monto = 0;
                            double meses = diasxCobrar / 30;
                            meses = meses <= 1 ? 1 : meses;

                            if (diasxCobrar <= 31 && diasxCobrar >= 28)
                            {
                                Monto = meses * servicio.Monto;
                            }
                            else
                            {
                                Monto = diasxCobrar * precioxDia;
                            }

                            //Se crea el detalle de la factura correspondiente a este servicio
                            Prefacturacion detalle = new Prefacturacion();
                            detalle.Producto = servicio.Id;
                            detalle.Periodo = "Del " + desde + " al " + hasta;
                            detalle.Moneda = contrato.Moneda;
                          
                            detalle.Cantidad = meses;                     
                            detalle.Descripcion = servicio.Descripcion;
                            detalle.Fecha = DateTime.Now;
                            detalle.Unidad = servicio.Producto.UnidadCobro.Nombre;
                            if (contrato.Moneda == "Colones")
                            {
                                detalle.Precio = Monto;
                            }
                            else
                            {
                                // Si el contrato esta en dólares pero se desea facturar en colones, 
                                // se debe realizar la conversión de moneda con el tipo de cambio actual
                                if ((bool)contrato.FacturarColones)
                                {
                                    TipoCambio tipoCambio = new TipoCambio();
                                    IDictionary<string, object> criteriaTipoCambio = new Dictionary<string, object>();
                                    criteriaTipoCambio.Add("Fecha", DateTime.Now.ToString("dd/MM/yyyy"));
                                    criteriaTipoCambio.Add("Tipo", "Venta");
                                    tipoCambio = tipoCambioService.Get(criteriaTipoCambio);
                                    detalle.Precio = (Monto * tipoCambio.Valor);
                                    detalle.Moneda = "Colones";
                                }
                                else
                                {
                                    detalle.Precio = Monto;
                                }
                            }

                            ListaDetalleFacturacion.Add(detalle);
                            Total += detalle.Precio;
                        }
                        break;

                    default: // Servicios de Fosas, Destrucción y Disposición Final, se obtienen con báscula

                        criteria = new Dictionary<string, object>();
                        criteria.Add("Producto", servicio);
                        criteria.Add("Contrato", contrato);
                        criteria.Add("IsDeleted", false);
                        criteria.Add("Facturada", false);

                        // Se obtienen todas las boletas con las condiciones necesarias en un rango de fechas dado
                        IList<Bascula> listaBoletas = basculaService.GetAll(criteria);
                        listaBoletas = listaBoletas.Where(s => s.Fecha > fechaDesde.AddDays(-1) && s.Fecha < fechaHasta.AddDays(1) && s.Estado != "Anulada" && s.Estado != "Pendiente").ToList();

                        //Si el servicio deseado no esta ligado a una OTR
                        if (listaBoletas.Count > 0)
                        {
                            // Se recorre una a una las boletas para ser tomadas en cuenta en la prefacturacion
                            foreach (Bascula boleta in listaBoletas)
                            {
                                //Se crea el detalle de la factura correspondiente a este servicio
                                Prefacturacion detalle = new Prefacturacion();
                                detalle.Bascula = boleta.Id;
                                detalle.ConsecutivoBoleta = boleta.Boleta.ToString();
                                detalle.Producto = servicio.Id;
                                detalle.Descripcion = servicio.Producto.Descripcion;
                                detalle.Unidad = servicio.Producto.UnidadCobro.Nombre;
                                detalle.Periodo = "Del " + desde + " al " + hasta;
                                detalle.Moneda = contrato.Moneda;
                                detalle.Cantidad = boleta.PesoNeto;
                                detalle.Fecha = boleta.Fecha;
                                if (contrato.Moneda == "Colones")
                                {
                                    detalle.Precio = boleta.Total;
                                }
                                else
                                {
                                    // Si el contrato esta en dólares pero se desea facturar en colones, 
                                    // se debe realizar la conversión de moneda con el tipo de cambio actual
                                    if ((bool)contrato.FacturarColones)
                                    {
                                        TipoCambio tipoCambio = new TipoCambio();
                                        IDictionary<string, object> criteriaTipoCambio = new Dictionary<string, object>();
                                        criteriaTipoCambio.Add("Fecha", DateTime.Now.ToString("dd/MM/yyyy"));
                                        criteriaTipoCambio.Add("Tipo", "Venta");
                                        tipoCambio = tipoCambioService.Get(criteriaTipoCambio);
                                        detalle.Precio = (boleta.Total * tipoCambio.Valor);
                                        detalle.Moneda = "Colones";
                                    }
                                    else
                                    {
                                        detalle.Precio = boleta.Total;
                                    }
                                }

                                ListaDetalleFacturacion.Add(detalle);
                                Total += detalle.Precio;
                            }
                        }
                        else
                        { //En caso que este servicio de Disposición este ligado a una OTR 

                            if (servicio.LigadoRecoleccion)
                            {
                                criteria = new Dictionary<string, object>();
                                criteria.Add("Compania", servicio.Proyecto);
                                criteria.Add("IsDeleted", false);
                                criteria.Add("Facturada", false);

                                IList<OTR> ListaOTR = otrService.GetAll(criteria);
                                ListaOTR = ListaOTR.Where(s => s.Fecha < fechaHasta.AddDays(-1) && s.Fecha > fechaDesde.AddDays(1) && s.Estado != "Anulada" && s.Estado != "Activo").ToList();
                                IList<OTR> ListaOTRFiltrada = new List<OTR>();

                                foreach (var otr in ListaOTR)
                                {
                                    IList<ProductoContrato> listaRutas = otr.RutaRecoleccion.Rutas;
                                    int cantServicio = listaRutas.Where(s => s.Id == servicio.Recoleccion.Id).ToList().Count;
                                    if (cantServicio > 0)
                                    {
                                        if (ListaOTRFiltrada.Where(s => s.Id == otr.Id).ToList().Count == 0)
                                        {
                                            ListaOTRFiltrada.Add(otr);
                                        }
                                    }
                                }

                                foreach (var itemOTR in ListaOTRFiltrada)
                                {
                                    //Se crea el detalle de la factura correspondiente a este servicio
                                    Prefacturacion detalle = new Prefacturacion();                                   
                                    detalle.Producto = servicio.Id;
                                    detalle.OTR = itemOTR.Id;
                                    detalle.Periodo = "Del " + desde + " al " + hasta;
                                    detalle.Moneda = contrato.Moneda;
                                    detalle.Fecha = itemOTR.Fecha;
                                    detalle.ConsecutivoOTR = itemOTR.Consecutivo;
                                    detalle.Descripcion = servicio.Producto.Descripcion;
                                    detalle.Unidad = servicio.Producto.UnidadCobro.Nombre;
                                    double Monto = 0;
                                    double Peso = 0;

                                    IDictionary<string, object> criteriaBascula = new Dictionary<string, object>();
                                    criteriaBascula.Add("OTR", itemOTR);
                                    criteriaBascula.Add("Estado", "Activo");
                                    criteriaBascula.Add("IsDeleted", false);
                                    criteriaBascula.Add("Facturada", false);

                                    // Se obtienen todas las boletas con las condiciones necesarias en un rango de fechas dado
                                    Bascula BoletaOTR = basculaService.Get(criteriaBascula);
                                    if (BoletaOTR == null) // Si la boleta de bascula de esta OTR se realizo de manera manual
                                    {
                                        BoletaManual boletaManual = boletaManualService.Get(criteriaBascula);
                                        detalle.BoletaManual = boletaManual.Id;
                                        detalle.ConsecutivoBoleta = boletaManual.NumeroBoleta;
                                        Peso = (boletaManual.PesoNeto / 1000);
                                        Monto = (Peso > 1 ? Peso : 1) * servicio.Monto;
                                        detalle.Cantidad = boletaManual.PesoNeto;
                                    }
                                    else
                                    {
                                        detalle.Bascula = BoletaOTR.Id;
                                        detalle.ConsecutivoBoleta = BoletaOTR.Boleta.ToString();
                                        Peso = (BoletaOTR.PesoNeto / 1000);
                                        Monto = (Peso > 1 ? Peso : 1) * servicio.Monto;
                                        detalle.Cantidad = BoletaOTR.PesoNeto;
                                    }
                                    
                                    if (contrato.Moneda == "Colones")
                                    {
                                        detalle.Precio = Monto;
                                    }
                                    else
                                    {
                                        // Si el contrato esta en dólares pero se desea facturar en colones, 
                                        // se debe realizar la conversión de moneda con el tipo de cambio actual
                                        if ((bool)contrato.FacturarColones)
                                        {
                                            TipoCambio tipoCambio = new TipoCambio();
                                            IDictionary<string, object> criteriaTipoCambio = new Dictionary<string, object>();
                                            criteriaTipoCambio.Add("Fecha", DateTime.Now.ToString("dd/MM/yyyy"));
                                            criteriaTipoCambio.Add("Tipo", "Venta");
                                            tipoCambio = tipoCambioService.Get(criteriaTipoCambio);
                                            detalle.Precio = (Monto * tipoCambio.Valor);
                                            detalle.Moneda = "Colones";
                                        }
                                        else
                                        {
                                            detalle.Precio = Monto;
                                        }
                                    }

                                    ListaDetalleFacturacion.Add(detalle);
                                    Total += detalle.Precio;
                                }
                            }
                        }
                        break;
                }
            }

            IList<Object> ListaPrefacturacion = new List<Object>();
            ListaPrefacturacion.Add(ListaDetalleFacturacion);
            ListaPrefacturacion.Add(Total);
            return ListaPrefacturacion;
        }



        /// <summary>
        /// Este método se encarga de crear la prefactura que se indico para un contrato especifico en un periodo determinado
        /// </summary>
        /// <returns>un JSON con </returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_PREFACTURACION)]
        public JsonResult PreFacturar(PrefacturaModel model)
        {
            try
            {
                long compania = Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0;
                Contrato contratoActual = contratoService.Get(model.Contrato);

                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Contrato", contratoActual);
                criteria.Add("Compania", contratoActual.Compania);
                criteria.Add("IsDeleted", false);
                criteria.Add("Estado", "Prefacturación");
                Facturacion factAnterior = facturacionService.Get(criteria);
                if (factAnterior != null)
                {
                    factAnterior.IsDeleted = true;
                    factAnterior.DeleteDate = DateTime.Now;
                    factAnterior.DeletedBy = ObtenerUsuarioActual().Nombre;
                    facturacionService.Update(factAnterior);
                }

                // Se crea la prefactura
                Facturacion facturacion = new Facturacion();
                facturacion.Compania = companiaService.Get(compania);
                facturacion.Version = 1;
                facturacion.CreateDate = DateTime.Now;
                facturacion.CreatedBy = ObtenerUsuarioActual().Nombre;
                facturacion.Estado = "Prefacturación";
                facturacion.Descripcion = "Facturación del " + model.Desde.ToString("dd/MM/yyyy") + " al " + model.Hasta.ToString("dd/MM/yyyy") + ". Contrato: " + contratoActual.Numero + " - " + contratoActual.DescripcionContrato;
                facturacion.FechaDesde = model.Desde;
                facturacion.FechaHasta = model.Hasta;
                facturacion.Contrato = contratoActual;
                facturacion.Moneda = contratoActual.FacturarColones == true ? "Colones" : contratoActual.Moneda;
                facturacion.ListaDetalleFacturacion = new List<DetalleFacturacion>();

                facturacion = facturacionService.Create(facturacion);

                //facturacion = facturacionService.Create(facturacion);
                List<Prefacturacion> ListaDetalles = JsonConvert.DeserializeObject<List<Prefacturacion>>(Request.Form["ListaDetalle"]);
                
                // Se guarda los detalles de la faturacion y se obtiene la nueva informacion producto de esta consulta
                facturacion = GuadarDetalleFacturacion(facturacion, contratoActual, compania, ListaDetalles);

                //Se actualiza la factura con la nueva informacion
              //  facturacionService.Update(facturacion);
          
                return Json(facturacion.Id);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(null);
            }
        }

        /// <summary>
        /// Este método se encarga de obtener todos los servicios brindados a este contrato en el tiempo indicado para que sean facturados
        /// (Disposición, Recolección, Alquiler, etc)
        /// </summary>
        /// <returns>la factura con todos el detalle de los servicios a cobrar </returns>
        private Facturacion GuadarDetalleFacturacion(Facturacion facturacion, Contrato contrato, long compania, IList<Prefacturacion> ListaDetalle)
        {
            Double Total = 0;
            Compania companiaActual = companiaService.Get(compania);
            IList<DetalleFacturacion> ListaDetalleFacturacion = new List<DetalleFacturacion>();
            String usuario = ObtenerUsuarioActual().Nombre;

            foreach (Prefacturacion item in ListaDetalle)
            {
                DetalleFacturacion detalle = new DetalleFacturacion();
                detalle.CreateDate = DateTime.Now;
                detalle.CreatedBy = usuario;
                detalle.Version = 1;
                detalle.IsDeleted = false;
                detalle.Periodo = item.Periodo;
                detalle.Moneda = item.Moneda;
                detalle.Monto = item.Precio;
                detalle.Cantidad = item.Cantidad;
                
                if (item.Bascula != 0){
                    var bascula = basculaService.Get(item.Bascula);
                    detalle.Bascula = bascula;
                }

                if (item.BoletaManual != 0){
                    var boletaManual = boletaManualService.Get(item.BoletaManual);
                    detalle.BoletaManual = boletaManual;
                }

                if (item.OTR != 0){
                    var otr = otrService.Get(item.OTR);
                    detalle.OTR = otr;
                }
                
                detalle.Producto = item.Producto != 0 ? productoContratoService.Get(item.Producto) : new ProductoContrato();
                detalle.Facturacion = facturacion;

                detalleService.Create(detalle);
                ListaDetalleFacturacion.Add(detalle);
                Total+= detalle.Monto;                
            }

            facturacion.ListaDetalleFacturacion = ListaDetalleFacturacion;
            facturacion.Monto = Total;
            return facturacion;
        }


        /// <summary>
        /// Este método se encarga de obtener todos los servicios brindados a este contrato en el tiempo indicado para que sean facturados
        /// (Disposición, Recolección, Alquiler, etc)
        /// </summary>
        /// <returns>la factura con todos el detalle de los servicios a cobrar </returns>
        private Facturacion GuadarDetalleFacturacionAnterior(Facturacion facturacion, Contrato contrato, String desde, String hasta, long compania)
        {

            DateTime fechaDesde = Convert.ToDateTime(desde);
            DateTime fechaHasta = Convert.ToDateTime(hasta);
            Double Total = 0;
            Compania companiaActual = companiaService.Get(compania);
            IList<DetalleFacturacion> ListaDetalleFacturacion = new List<DetalleFacturacion>();

            foreach (ProductoContrato servicio in contrato.Productos)
            {
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                switch (servicio.Producto.Categoria.Tipo)
                {
                    case "Recoleccion": // servicios de Recolección (Carga Trasera / Roll-Off), se obtiene la información por medio de OTRs
                        {
                            criteria = new Dictionary<string, object>();
                            criteria.Add("Compania", servicio.Proyecto);
                            criteria.Add("IsDeleted", false);
                            criteria.Add("Facturada", false);

                            IList<OTR> ListaOTR = otrService.GetAll(criteria);
                            ListaOTR = ListaOTR.Where(s => s.Fecha < fechaHasta.AddDays(-1) && s.Fecha >= fechaDesde.AddDays(1) && s.Estado != "Anulada" && s.Estado != "Activo").ToList();//
                            IList<OTR> ListaOTRFiltrada = new List<OTR>();
                            
                            foreach (var otr in ListaOTR)
                            {
                                IList<ProductoContrato> listaRutas = otr.RutaRecoleccion.Rutas;
                                int cantServicio = listaRutas.Where(s => s.Producto.Id == servicio.Producto.Id).ToList().Count;
                                if (cantServicio > 0)
                                {
                                    if (ListaOTRFiltrada.Where(s=> s.Id == otr.Id).ToList().Count == 0)
                                    {
                                        ListaOTRFiltrada.Add(otr);
                                        otr.Facturada = true;
                                        otrService.Update(otr);
                                    }
                                }
                            }

                            foreach (var itemOTR in ListaOTRFiltrada)
                            {
                                //Se crea el detalle de la factura correspondiente a este servicio
                                DetalleFacturacion detalle = new DetalleFacturacion();
                                detalle.Version = 1;
                                detalle.CreateDate = DateTime.Now;
                                detalle.CreatedBy = ObtenerUsuarioActual().Nombre;
                                detalle.Facturacion = facturacion;
                                detalle.Producto = servicio;
                                detalle.OTR = itemOTR;
                                detalle.Periodo = "Del " + desde + " al " + hasta;
                                detalle.Moneda = contrato.Moneda;
                                double Monto = 0;
                                double Peso = 0;

                                // Se evalua el tipo de OTR para verificar el monto que se debe cobrar segun el servicio brindado
                                switch (itemOTR.Tipo)
                                {
                                    case "Municipal":
                                        {
                                            IDictionary<string, object> criteriaBascula = new Dictionary<string, object>();
                                            criteriaBascula.Add("OTR", itemOTR);
                                            criteriaBascula.Add("Estado", "Activo");
                                            criteriaBascula.Add("IsDeleted", false);
                                            criteriaBascula.Add("Facturada", false);

                                            // Se obtienen todas las boletas con las condiciones necesarias en un rango de fechas dado
                                            Bascula BoletaOTR = basculaService.Get(criteriaBascula);
                                            if (BoletaOTR.Id == 0) // Si la boleta de bascula de esta OTR se realizo de manera manual
                                            {
                                                BoletaManual boletaManual = boletaManualService.Get(criteriaBascula);
                                                detalle.BoletaManual = boletaManual;
                                                detalle.Cantidad = boletaManual.PesoNeto;
                                                Peso = (boletaManual.PesoNeto / 1000);
                                                Monto = (Peso > 1 ? Peso : 1) * servicio.Monto;
                                                boletaManual.Facturada = false;
                                                boletaManualService.Update(boletaManual);
                                            }
                                            else
                                            {
                                                detalle.Bascula = BoletaOTR;
                                                detalle.Cantidad = BoletaOTR.PesoNeto;
                                                Peso = (BoletaOTR.PesoNeto / 1000);
                                                Monto = (Peso > 1 ? Peso : 1) * servicio.Monto;
                                                BoletaOTR.Facturada = false;
                                                basculaService.Update(BoletaOTR);
                                            }
                                        }

                                        break;

                                    case "Comercial":
                                        {
                                            int diasxCobrar = 0;

                                            // Se toma en cuenta el estado para concer la cantidad de dias que deben ser facturados
                                            if (servicio.Estado == "Activo")
                                            {
                                                TimeSpan diferencia = fechaHasta - servicio.FechaEstado;
                                                diasxCobrar = diferencia.Days > 0 ? diferencia.Days : 0;
                                            }
                                            else
                                            {
                                                TimeSpan diferencia = servicio.FechaEstado - fechaDesde;
                                                diasxCobrar = diferencia.Days > 0 ? diferencia.Days : 0; 
                                            }

                                            // Se obtiene el monto por dia y el precio por los dias de servicio brindado
                                            double precioxDia = servicio.Monto / 30;

                                            if (diasxCobrar <= 31 && diasxCobrar >= 28)
                                            {
                                                Monto = servicio.Monto;
                                            }
                                            else
                                            {
                                                Monto = diasxCobrar * precioxDia;
                                            }

                                            detalle.Cantidad = 1;
                                            IDictionary<string, object> criteriaBascula = new Dictionary<string, object>();
                                            criteriaBascula.Add("OTR", itemOTR);
                                            criteriaBascula.Add("Estado", "Activo");
                                            criteriaBascula.Add("IsDeleted", false);
                                            criteriaBascula.Add("Facturada", false);

                                            Bascula BoletaOTR = basculaService.Get(criteriaBascula);
                                            
                                            if (BoletaOTR == null) // Si la boleta de bascula de esta OTR se realizo de manera manual
                                            {
                                                BoletaManual boletaManual = boletaManualService.Get(criteriaBascula);
                                                if (boletaManual != null)
                                                {
                                                    detalle.BoletaManual = boletaManual;
                                                    boletaManual.Facturada = false;
                                                    boletaManualService.Update(boletaManual);
                                                }
                                            }
                                            else
                                            {
                                                detalle.Bascula = BoletaOTR;
                                                BoletaOTR.Facturada = false;
                                                basculaService.Update(BoletaOTR);
                                            }
                                        }
                                        break;

                                    case "Roll-Off":
                                        {
                                            IDictionary<string, object> criteriaBascula = new Dictionary<string, object>();
                                            criteriaBascula.Add("OTR", itemOTR);
                                            criteriaBascula.Add("Estado", "Activo");
                                            criteriaBascula.Add("IsDeleted", false);
                                            criteriaBascula.Add("Facturada", false);

                                            // Se obtienen todas las boletas con las condiciones necesarias en un rango de fechas dado
                                            Bascula BoletaOTR = basculaService.Get(criteriaBascula);
                                            
                                            if (BoletaOTR == null) // Si la boleta de bascula de esta OTR se realizo de manera manual
                                            {
                                                BoletaManual boletaManual = boletaManualService.Get(criteriaBascula);
                                                if (boletaManual != null)
                                                {
                                                    detalle.BoletaManual = boletaManual;
                                                    Peso = (boletaManual.PesoNeto / 1000);
                                                    Monto = (Peso > 1 ? Peso : 1) * servicio.Monto;
                                                    detalle.Cantidad = boletaManual.PesoNeto;
                                                    boletaManual.Facturada = false;
                                                    boletaManualService.Update(boletaManual);
                                                }
                                            }
                                            else
                                            {
                                                detalle.Bascula = BoletaOTR;
                                                Peso = (BoletaOTR.PesoNeto / 1000);
                                                Monto = (Peso > 1 ? Peso : 1) * servicio.Monto;
                                                detalle.Cantidad = BoletaOTR.PesoNeto;
                                                BoletaOTR.Facturada = false;
                                                basculaService.Update(BoletaOTR);
                                            }
                                        }
                                        break;
                                }
                                if (contrato.Moneda == "Colones")
                                {
                                    detalle.Monto = Monto;
                                }
                                else
                                {
                                    // Si el contrato esta en dólares pero se desea facturar en colones, 
                                    // se debe realizar la conversión de moneda con el tipo de cambio actual
                                    if ((bool)contrato.FacturarColones)
                                    {
                                        TipoCambio tipoCambio = new TipoCambio();
                                        IDictionary<string, object> criteriaTipoCambio = new Dictionary<string, object>();
                                        criteriaTipoCambio.Add("Fecha", DateTime.Now.ToString("dd/MM/yyyy"));
                                        criteriaTipoCambio.Add("Tipo", "Venta");
                                        tipoCambio = tipoCambioService.Get(criteriaTipoCambio);
                                        detalle.Monto = (Monto * tipoCambio.Valor);
                                        detalle.Moneda = "Colones";
                                    }
                                    else
                                    {
                                        detalle.Monto = Monto;
                                    }
                                }

                                detalleService.Create(detalle);
                                ListaDetalleFacturacion.Add(detalle);
                                Total += detalle.Monto;
                            }
                        }                        
                        break;

                    case "Ninguno": // Servicios como Alquiler, que se deben cobrar de manera Mensual
                        {
                            int diasxCobrar = 0;

                            // Se toma en cuenta el estado para concer la cantidad de dias que deben ser facturados
                            if (servicio.Estado == "Activo")
                            {
                                TimeSpan diferencia = fechaHasta - servicio.FechaEstado;
                                diasxCobrar = diferencia.Days > 0 ? diferencia.Days : 0;
                            }
                            else
                            {
                                TimeSpan diferencia = servicio.FechaEstado - fechaDesde;
                                diasxCobrar = diferencia.Days > 0 ? diferencia.Days : 0;
                            }

                            // Se obtiene el monto por dia y el precio por los dias de servicio brindado
                            double precioxDia = servicio.Monto / 30;
                            double Monto = 0;
                            
                            if(diasxCobrar <=  31 && diasxCobrar >= 28)
                            {
                                Monto = servicio.Monto;
                            }
                            else
                            {
                                Monto = diasxCobrar * precioxDia;
                            }

                            //Se crea el detalle de la factura correspondiente a este servicio
                            DetalleFacturacion detalle = new DetalleFacturacion();
                            detalle.Version = 1;
                            detalle.CreateDate = DateTime.Now;
                            detalle.CreatedBy = ObtenerUsuarioActual().Nombre;
                            detalle.Facturacion = facturacion;
                            detalle.Producto = servicio;
                            detalle.Periodo = "Del " + desde + " al " + hasta;
                            detalle.Moneda = contrato.Moneda;
                            detalle.Cantidad = 1;
                            if (contrato.Moneda == "Colones")
                            {
                                detalle.Monto = Monto;
                            }
                            else
                            {
                                // Si el contrato esta en dólares pero se desea facturar en colones, 
                                // se debe realizar la conversión de moneda con el tipo de cambio actual
                                if ((bool)contrato.FacturarColones)
                                {
                                    TipoCambio tipoCambio = new TipoCambio();
                                    IDictionary<string, object> criteriaTipoCambio = new Dictionary<string, object>();
                                    criteriaTipoCambio.Add("Fecha", DateTime.Now.ToString("dd/MM/yyyy"));
                                    criteriaTipoCambio.Add("Tipo", "Venta");
                                    tipoCambio = tipoCambioService.Get(criteriaTipoCambio);
                                    detalle.Monto = (Monto * tipoCambio.Valor);
                                    detalle.Moneda = "Colones";
                                }
                                else
                                {
                                    detalle.Monto = Monto;
                                }
                            }

                            detalleService.Create(detalle);
                            ListaDetalleFacturacion.Add(detalle);
                            Total += detalle.Monto;
                        }
                        break;

                    default: // Servicios de Fosas, Destrucción y Disposición Final, se obtienen con báscula

                        criteria = new Dictionary<string, object>();
                        criteria.Add("Producto", servicio);
                        criteria.Add("Contrato", contrato);
                        criteria.Add("IsDeleted", false);
                        criteria.Add("Facturada", false);

                        // Se obtienen todas las boletas con las condiciones necesarias en un rango de fechas dado
                        IList<Bascula> listaBoletas = basculaService.GetAll(criteria);
                        listaBoletas = listaBoletas.Where(s => s.Fecha > fechaDesde.AddDays(-1) && s.Fecha < fechaHasta.AddDays(1) && s.Estado != "Anulada" && s.Estado != "Pendiente").ToList();

                        //Si el servicio deseado no esta ligado a una OTR
                        if (listaBoletas.Count > 0) 
                        {
                            // Se recorre una a una las boletas para ser tomadas en cuenta en la prefacturacion
                            foreach (Bascula boleta in listaBoletas)
                            {

                                //Se crea el detalle de la factura correspondiente a este servicio
                                DetalleFacturacion detalle = new DetalleFacturacion();
                                detalle.Version = 1;
                                detalle.CreateDate = DateTime.Now;
                                detalle.CreatedBy = ObtenerUsuarioActual().Nombre;
                                detalle.Bascula = boleta;
                                detalle.Facturacion = facturacion;
                                detalle.Producto = servicio;
                                detalle.Periodo = "Del " + desde + " al " + hasta;
                                detalle.Moneda = contrato.Moneda;
                                detalle.Cantidad = boleta.PesoNeto;
                                if (contrato.Moneda == "Colones")
                                {
                                    detalle.Monto = boleta.Total;
                                }
                                else
                                {
                                    // Si el contrato esta en dólares pero se desea facturar en colones, 
                                    // se debe realizar la conversión de moneda con el tipo de cambio actual
                                    if ((bool)contrato.FacturarColones)
                                    {
                                        TipoCambio tipoCambio = new TipoCambio();
                                        IDictionary<string, object> criteriaTipoCambio = new Dictionary<string, object>();
                                        criteriaTipoCambio.Add("Fecha", DateTime.Now.ToString("dd/MM/yyyy"));
                                        criteriaTipoCambio.Add("Tipo", "Venta");
                                        tipoCambio = tipoCambioService.Get(criteriaTipoCambio);
                                        detalle.Monto = (boleta.Total * tipoCambio.Valor);
                                        detalle.Moneda = "Colones";
                                    }
                                    else
                                    {
                                        detalle.Monto = boleta.Total;
                                    }
                                }
                                
                                boleta.Facturada = false;
                                basculaService.Update(boleta);
                                detalleService.Create(detalle);
                                ListaDetalleFacturacion.Add(detalle);
                                Total += detalle.Monto;
                            }
                        }
                        else { //En caso que este servicio de Disposición este ligado a una OTR 

                            if(servicio.LigadoRecoleccion)
                            {
                                criteria = new Dictionary<string, object>();
                                criteria.Add("Compania", servicio.Proyecto);
                                criteria.Add("IsDeleted", false);
                                criteria.Add("Facturada", false);

                                IList<OTR> ListaOTR = otrService.GetAll(criteria);
                                ListaOTR = ListaOTR.Where(s => s.Fecha < fechaHasta.AddDays(-1) && s.Fecha > fechaDesde.AddDays(1) && s.Estado != "Anulada" && s.Estado != "Activo").ToList();
                                IList<OTR> ListaOTRFiltrada = new List<OTR>();

                                foreach (var otr in ListaOTR)
                                {
                                    IList<ProductoContrato> listaRutas = otr.RutaRecoleccion.Rutas;
                                    int cantServicio = listaRutas.Where(s => s.Id == servicio.Recoleccion.Id).ToList().Count;
                                    if (cantServicio > 0)
                                    {
                                        if (ListaOTRFiltrada.Where(s => s.Id == otr.Id).ToList().Count == 0)
                                        {
                                            ListaOTRFiltrada.Add(otr);
                                            otr.Facturada = false;
                                            otrService.Update(otr);
                                        }
                                    }
                                }

                                foreach (var itemOTR in ListaOTRFiltrada)
                                {
                                    //Se crea el detalle de la factura correspondiente a este servicio
                                    DetalleFacturacion detalle = new DetalleFacturacion();
                                    detalle.Version = 1;
                                    detalle.CreateDate = DateTime.Now;
                                    detalle.CreatedBy = ObtenerUsuarioActual().Nombre;
                                    detalle.Facturacion = facturacion;
                                    detalle.Producto = servicio;
                                    detalle.OTR = itemOTR;
                                    detalle.Periodo = "Del " + desde + " al " + hasta;
                                    detalle.Moneda = contrato.Moneda;
                                    double Monto = 0;
                                    double Peso = 0;

                                    IDictionary<string, object> criteriaBascula = new Dictionary<string, object>();
                                    criteriaBascula.Add("OTR", itemOTR);
                                    criteriaBascula.Add("Estado", "Activo");
                                    criteriaBascula.Add("IsDeleted", false);
                                    criteriaBascula.Add("Facturada", false);

                                    // Se obtienen todas las boletas con las condiciones necesarias en un rango de fechas dado
                                    Bascula BoletaOTR = basculaService.Get(criteriaBascula);
                                    if (BoletaOTR == null) // Si la boleta de bascula de esta OTR se realizo de manera manual
                                    {
                                        BoletaManual boletaManual = boletaManualService.Get(criteriaBascula);
                                        detalle.BoletaManual = boletaManual;
                                        Peso = (boletaManual.PesoNeto / 1000);
                                        Monto = (Peso > 1 ? Peso : 1) * servicio.Monto;
                                        detalle.Cantidad = boletaManual.PesoNeto;
                                        boletaManual.Facturada = false;
                                        boletaManualService.Update(boletaManual);
                                    }
                                    else
                                    {
                                        detalle.Bascula = BoletaOTR;
                                        Peso = (BoletaOTR.PesoNeto / 1000);
                                        Monto = (Peso > 1 ? Peso : 1) * servicio.Monto;
                                        detalle.Cantidad = BoletaOTR.PesoNeto;
                                        BoletaOTR.Facturada = false;
                                        basculaService.Update(BoletaOTR);
                                    }


                                    if (contrato.Moneda == "Colones")
                                    {
                                        detalle.Monto = Monto;
                                    }
                                    else
                                    {
                                        // Si el contrato esta en dólares pero se desea facturar en colones, 
                                        // se debe realizar la conversión de moneda con el tipo de cambio actual
                                        if ((bool)contrato.FacturarColones)
                                        {
                                            TipoCambio tipoCambio = new TipoCambio();
                                            IDictionary<string, object> criteriaTipoCambio = new Dictionary<string, object>();
                                            criteriaTipoCambio.Add("Fecha", DateTime.Now.ToString("dd/MM/yyyy"));
                                            criteriaTipoCambio.Add("Tipo", "Venta");
                                            tipoCambio = tipoCambioService.Get(criteriaTipoCambio);
                                            detalle.Monto = (Monto * tipoCambio.Valor);
                                            detalle.Moneda = "Colones";
                                        }
                                        else
                                        {
                                            detalle.Monto = Monto;
                                        }
                                    }

                                    detalleService.Create(detalle);
                                    ListaDetalleFacturacion.Add(detalle);
                                    Total += detalle.Monto;
                                }
                            }                            
                        }
                                                
                        break;                   
                }
            }                     

            facturacion.ListaDetalleFacturacion = ListaDetalleFacturacion;
            facturacion.Monto = Total;
            return facturacion;
        }


        public void ReportePrefacturacion(long id)
        {
            Facturacion factura = new Facturacion();
            factura = facturacionService.Get(id);

            //ReportDocument reporte = new ReportDocument();
            ds_Prefacturacion dsFacturacion = new ds_Prefacturacion();
            ds_TotalUnidadesPrefacturacion ds_Totales = new ds_TotalUnidadesPrefacturacion();

            var dtReporte = dsFacturacion.Tables["Reporte"];
            var dtDatos = dsFacturacion.Tables["Datos"];

            var dtUnidades = ds_Totales.Tables["Datos"];

            // Rows DataTable Reporte
            DataRow row = dtReporte.NewRow();
            row["Compania"] = factura.Compania.Nombre;
            row["FechaActual"] = DateTime.Now;
            row["FechaProceso"] = factura.CreateDate;
            row["TipoFacturacion"] = "Prefacturación cliente (" + factura.Contrato.Cliente.Nombre + ")";
            row["MonedaProceso"] = factura.Moneda;
            row["Contrato"] = factura.Contrato.Numero;
            row["Cliente"] = factura.Contrato.Cliente.Numero + " - " + factura.Contrato.Cliente.Nombre;
            row["Moneda"] = factura.Moneda;
            dtReporte.Rows.Add(row);

            // Rows DataTable Datos
            foreach (DetalleFacturacion detalle in factura.ListaDetalleFacturacion)
            {
                DataRow dato = dtDatos.NewRow();
                DataRow datosTotales = dtUnidades.NewRow();
                dato["Ruta"] = detalle.OTR == null ? "" : detalle.OTR.Consecutivo.ToString();
                if(detalle.OTR != null) // Si el producto es de recoleccion
                {
                    dato["Fecha"] = detalle.OTR.Fecha;
                }
                else
                {
                    if (detalle.Bascula != null) // Si el producto es de dispocision
                    {
                        dato["Fecha"] = detalle.Bascula.Fecha;
                    }
                    else
                    {
                        if (detalle.BoletaManual != null)// Si el producto es de dispocision con boleta manual
                        {
                            dato["Fecha"] = detalle.BoletaManual.Fecha;
                        }
                        else // En caso de que no sea uno de los anteriores (Ejemplo: Alquiler)
                        {
                            dato["Fecha"] = detalle.CreateDate;
                        }
                    }
                }
                //dato["Fecha"] = detalle.;
                dato["Descripcion"] = detalle.Producto.Descripcion;
                if (detalle.OTR != null && detalle.BoletaManual == null && detalle.Bascula == null ) // Si el producto es de carga trasera
                {
                    dato["Cantidad"] = detalle.Cantidad;//1;
                    dato["Boleta"] = String.Empty;
                    dato["Placa"] = detalle.OTR.Equipo.Nombre;
                    datosTotales["Cantidad"] = detalle.Cantidad; //1;
                }
                else
                {
                    if (detalle.Bascula != null && detalle.OTR == null) // Si el producto es de carga trasera
                    {
                        //var cantidad = detalle.Bascula.PesoNeto / 1000;
                        dato["Cantidad"] = detalle.Cantidad;//cantidad < 1 ? 1 : cantidad;
                        dato["Boleta"] = detalle.Bascula.Boleta;
                        dato["Placa"] = detalle.Bascula.Equipo.Nombre;
                        datosTotales["Cantidad"] = detalle.Cantidad;//cantidad < 1 ? 1 : cantidad;
                    }
                    else
                    {
                        if (detalle.OTR != null && detalle.BoletaManual != null)
                        {
                           // var cantidad = detalle.BoletaManual.PesoNeto / 1000;
                            dato["Cantidad"] = detalle.Cantidad;///cantidad < 1 ? 1 : cantidad;
                            dato["Boleta"] = detalle.BoletaManual.NumeroBoleta;
                            dato["Placa"] = detalle.OTR.Equipo.Nombre;
                            datosTotales["Cantidad"] = detalle.Cantidad;//cantidad < 1 ? 1 : cantidad;
                        }
                        else
                        {
                            if (detalle.OTR == null && detalle.BoletaManual == null && detalle.Bascula == null) // Si el producto es de carga trasera
                            {
                                dato["Cantidad"] = detalle.Cantidad;// 1;
                                dato["Boleta"] = String.Empty;
                                dato["Placa"] = String.Empty;
                                datosTotales["Cantidad"] = detalle.Cantidad; //1;
                            }
                            else
                            {
                                if(detalle.Bascula != null && detalle.OTR != null)
                                {
                                    //var cantidad = detalle.Bascula.PesoNeto / 1000;
                                    dato["Cantidad"] = detalle.Cantidad; //cantidad < 1 ? 1 : cantidad;
                                    dato["Boleta"] = detalle.Bascula.Boleta;
                                    dato["Placa"] = detalle.OTR.Equipo.Nombre;
                                    datosTotales["Cantidad"] = detalle.Cantidad; //cantidad < 1 ? 1 : cantidad; 
                                }
                            }
                        }
                    }
                }

                dato["Unidad"] = detalle.Producto.Producto.UnidadCobro.Nombre;
                datosTotales["Unidad"] = detalle.Producto.Producto.UnidadCobro.Nombre;
                dato["PrecioUnidad"] = detalle.Producto.Total;
                dato["PrecioTotal"] = detalle.Monto;
                dtDatos.Rows.Add(dato);
                dtUnidades.Rows.Add(datosTotales);
            }
       
            ReportDocument rd = new ReportDocument();
            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//Facturacion//rpt_Prefacturacion.rpt";
            rd.Load(strRptPath);

            rd.SetDataSource(dsFacturacion);
            rd.Subreports[0].SetDataSource(ds_Totales);
            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Prefacturacion");
        }

        public void ReporteConsultaDetalleRecibo(long factura)
        { 
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Consecutivo", factura);
            criteria.Add("Compania", companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0));
            Facturacion facturacion = facturacionService.Get(criteria);
            ReporteDetalleRecibos(facturacion.Id);
        }

        public void ReporteDetalleRecibos(long id)
        {
            Facturacion factura = new Facturacion();
            factura = facturacionService.Get(id);

            //ReportDocument reporte = new ReportDocument();
            ds_RecibosxServicio dsFacturacion = new ds_RecibosxServicio();
            var dtReporte = dsFacturacion.Tables["Reporte"];
            var dtDatos = dsFacturacion.Tables["Datos"];

            // Rows DataTable Reporte
            DataRow row = dtReporte.NewRow();
            row["Compania"] = factura.Compania.Nombre;
            row["FechaActual"] = DateTime.Now;
            row["Contrato"] = factura.Contrato.Numero;
            row["Cliente"] = factura.Contrato.Cliente.Numero + " - " + factura.Contrato.Cliente.Nombre;
            row["Moneda"] = factura.Moneda;
            row["Periodo"] = factura.Descripcion;
            dtReporte.Rows.Add(row);

            // Rows DataTable Datos
            foreach (DetalleFacturacion detalle in factura.ListaDetalleFacturacion)
            {
                DataRow dato = dtDatos.NewRow();
                if (detalle.OTR != null) // Si el producto es de recoleccion
                {
                    dato["Fecha"] = detalle.OTR.Fecha;
                }
                else
                {
                    if (detalle.Bascula != null) // Si el producto es de dispocision
                    {
                        dato["Fecha"] = detalle.Bascula.Fecha;
                    }
                    else
                    {
                        if (detalle.BoletaManual != null)// Si el producto es de dispocision con boleta manual
                        {
                            dato["Fecha"] = detalle.BoletaManual.Fecha;
                        }
                        else // En caso de que no sea uno de los anteriores (Ejemplo: Alquiler)
                        {
                            dato["Fecha"] = detalle.CreateDate;
                        }
                    }
                }

                if (detalle.OTR != null && detalle.BoletaManual == null && detalle.Bascula == null) // Si el producto es de carga trasera
                {
                    dato["PesoTara"] = 0;
                    dato["PesoBruto"] = 0;
                    dato["PesoNeto"] = 0;
                    dato["Unidad"] = String.Empty;
                    dato["Recibo"] = String.Empty;
                }
                else
                {
                    if (detalle.Bascula != null && detalle.OTR == null) // Si el producto es de carga trasera
                    {
                        dato["PesoTara"] = detalle.Bascula.PesoTara;
                        dato["PesoBruto"] = detalle.Bascula.PesoBruto;
                        dato["PesoNeto"] = detalle.Bascula.PesoNeto;
                        dato["Unidad"] = detalle.Bascula.Equipo.Nombre;
                        dato["Recibo"] = detalle.Bascula.Boleta;                    
                    }
                    else
                    {
                        if (detalle.OTR != null && detalle.BoletaManual != null)
                        {
                            dato["PesoTara"] = detalle.BoletaManual.PesoTara;
                            dato["PesoBruto"] = detalle.BoletaManual.PesoBruto;
                            dato["PesoNeto"] = detalle.BoletaManual.PesoNeto;
                            dato["Unidad"] = detalle.OTR.Equipo.Nombre;
                            dato["Recibo"] = detalle.BoletaManual.NumeroBoleta;    
                        }
                        else
                        {
                            if (detalle.OTR == null && detalle.BoletaManual == null && detalle.Bascula == null) // Si el producto es de carga trasera
                            {

                                dato["PesoTara"] = 0;
                                dato["PesoBruto"] = 0;
                                dato["PesoNeto"] = 0;
                                dato["Unidad"] = String.Empty;
                                dato["Recibo"] = String.Empty;
                            }
                            else
                            {
                                if (detalle.Bascula != null && detalle.OTR != null)
                                {
                                    dato["PesoTara"] = detalle.Bascula.PesoTara;
                                    dato["PesoBruto"] = detalle.Bascula.PesoBruto;
                                    dato["PesoNeto"] = detalle.Bascula.PesoNeto;
                                    dato["Unidad"] = detalle.Bascula.Equipo.Nombre;
                                    dato["Recibo"] = detalle.Bascula.Boleta;      
                                }
                            }
                        }
                    }
                }

                dato["Monto"] = detalle.Monto;
                dato["Servicio"] = detalle.Producto.Descripcion;
                dtDatos.Rows.Add(dato);
            }

            ReportDocument rd = new ReportDocument();
            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//Facturacion//rpt_RecibosxServicio.rpt";
            rd.Load(strRptPath);

            rd.SetDataSource(dsFacturacion);
            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Detalle_Recibos");
        }


        /// <summary>
        ///Obtiene todas las facturas con estado de prefactura de un cliente es especifico
        /// </summary>
        /// <returns>un json con la lista de todas las prefacturas encontradas </returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_PREFACTURACION)]
        public JsonResult CargarPreFacturas(long cliente)
        {
            try
            {
                long compania = Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0;

                IList<Facturacion> ListaPrefacturacion = facturacionService.PrefacturaSearch(cliente, compania);
                IList<FacturacionModel> ListaModel = new List<FacturacionModel>();
                foreach (var item in ListaPrefacturacion)
                {
                    FacturacionModel model = new FacturacionModel();
                    model.Id = item.Id;
                    model.Descripcion = item.Descripcion;
                    ListaModel.Add(model);
                }
                
                return Json(ListaModel);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(null);
            }
        }
        #endregion

        #region CONSULTAR FACTURA

        /// <summary>
        /// Este método se encarga de cargar la vista de ConsultaFactura
        /// </summary>
        /// <returns>vista ConsultaFactura</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CONSULTA_FACTURACION)]
        public ActionResult ConsultaFactura()
        {
            try
            {
                if (mensaje != String.Empty)
                {
                    TempData["alertMessage"] = mensaje;
                }

                Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Compania", compania);
                ConsecutivoFacturacion consecutivo = consecutivoService.Get(criteria);
                ViewBag.Consecutivo = consecutivo.Secuencia - 1;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }

            return View();
        }


        /// <summary>
        /// Este método se encarga de cargar la una factura en especifica que se desea consultar
        /// </summary>
        /// <returns>JSON con la informacion de la factura deseada</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CONSULTA_FACTURACION)]
        public JsonResult CargarFactura(long factura)
        {
            try
            {
                Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Consecutivo", factura);
                criteria.Add("Compania", compania);
                criteria.Add("Estado", "Facturación");

                Facturacion facturacion = new Facturacion();
                facturacion = facturacionService.Get(criteria);
                FacturacionModel model = new FacturacionModel();//facturacionMapper.GetModel(facturacion);
                model.Compania = facturacion.Compania.Id;
                model.Consecutivo = facturacion.Consecutivo;
                model.CreateDate = facturacion.CreateDate;
                model.CreatedBy = facturacion.CreatedBy;
                model.Moneda = facturacion.Moneda;
                model.Monto = facturacion.Monto;
                model.ClienteDescripcion = facturacion.Contrato.Cliente.Numero + " - " + facturacion.Contrato.Cliente.Nombre;
                model.Cliente = facturacion.Contrato.Cliente.Id;
                model.Contrato = facturacion.Contrato.Id;
                model.ContratoDescripcion = facturacion.Contrato.Numero + " - " + facturacion.Contrato.DescripcionContrato;
                model.Estado = facturacion.Estado;
                model.Descripcion = facturacion.Descripcion;
                model.Observaciones = facturacion.Observaciones;
                model.Id = facturacion.Id;
              
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(null);
            }
        }

        /// <summary>
        /// Este método se encarga de anular la factura indicada
        /// </summary>
        /// <returns>JSON indicando si se realizo la accion</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CONSULTA_FACTURACION)]
        public JsonResult AnularFactura(long factura)
        {
            try{
                Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Compania", compania);
                criteria.Add("Consecutivo", factura);
                criteria.Add("Estado", "Facturación");

                Facturacion facturacion = facturacionService.Get(criteria);
                facturacion.Estado = "Anulada";
                facturacion.ModifiedBy = ObtenerUsuarioActual().Nombre;
                facturacion.DateLastModified = DateTime.Now;

                facturacionService.Update(facturacion);

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(null);
            }
        }

        /// <summary>
        /// Este método se encarga de anular la factura indicada
        /// </summary>
        /// <returns>JSON con la lista de prefacturas</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CONSULTA_FACTURACION)]
        public JsonResult ReimprimirFactura(long factura)
        {
            try
            {
                Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Compania", compania);
                criteria.Add("Consecutivo", factura);
                criteria.Add("Estado", "Facturación");
                
                Facturacion facturacion = facturacionService.Get(criteria);
                Facturacion facturaCopia = new Facturacion();

                // Se anula la factura original
                facturacion.Estado = "Anulada";
                facturacion.ModifiedBy = ObtenerUsuarioActual().Nombre;
                facturacion.DateLastModified = DateTime.Now;
                facturacionService.Update(facturacion);

                // Se crea una copia de la factura original
                facturaCopia.Compania = facturacion.Compania;
                facturaCopia.Contrato = facturacion.Contrato;
                facturaCopia.Descripcion = facturacion.Descripcion;
                facturaCopia.FechaDesde = facturacion.FechaDesde;
                facturaCopia.FechaHasta = facturacion.FechaHasta;
                facturaCopia.Moneda= facturacion.Moneda;
                facturaCopia.Monto = facturacion.Monto;
                facturaCopia.Observaciones = facturacion.Observaciones;
                facturaCopia.Version = facturacion.Version;
                facturaCopia.ListaDetalleFacturacion = new List<DetalleFacturacion>();
                facturaCopia.CreateDate = DateTime.Now;
                facturaCopia.DateLastModified = DateTime.Now;
                facturaCopia.CreatedBy = ObtenerUsuarioActual().Nombre;
                facturaCopia.ModifiedBy = ObtenerUsuarioActual().Nombre;
                facturaCopia.Estado = "Facturación";

                 //Se cambia el consecutivo de la copia de la factura
                criteria = new Dictionary<string, object>();
                criteria.Add("Compania",compania);
                ConsecutivoFacturacion consecutivo = consecutivoService.Get(criteria);
                facturaCopia.Consecutivo = consecutivo.Secuencia;

                consecutivo.Secuencia++;
                consecutivoService.Update(consecutivo);

                // Se guarda la factura copia
                facturacionService.Create(facturaCopia);
               
                foreach (DetalleFacturacion item in facturacion.ListaDetalleFacturacion)
                {
                    DetalleFacturacion detalle = new DetalleFacturacion();
                    detalle.Bascula = item.Bascula;
                    detalle.BoletaManual = item.BoletaManual;
                    detalle.Cantidad = item.Cantidad;
                    detalle.CreateDate = DateTime.Now;
                    detalle.CreatedBy = ObtenerUsuarioActual().Nombre;
                    detalle.Facturacion = facturaCopia;
                    detalle.Moneda = item.Moneda;
                    detalle.Monto = item.Monto;
                    detalle.OTR = item.OTR;
                    detalle.Periodo = item.Periodo;
                    detalle.Producto = item.Producto;
                    detalle.Version = item.Version;
                    facturaCopia.ListaDetalleFacturacion.Add(detalle);
                    detalleService.Create(detalle);
                }
                
                return Json(facturaCopia.Id, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(null);
            }
        }
        #endregion

        #region CONTROL DE PREFACTURACION

        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO)]
        public ActionResult Control()
        {
            try
            {
                if (mensaje != String.Empty)
                {
                    TempData["alertMessage"] = mensaje;
                }

                ViewBag.ListaPrefacturas = new List<FacturacionModel>();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }

            return View();
        }
        #endregion

        #region DOCUMENTOS
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO)]
        public ActionResult Documentos()
        {
            try
            {
                if (mensaje != String.Empty)
                {
                    TempData["alertMessage"] = mensaje;
                }

                Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Compania", compania);
                ConsecutivoFacturacion consecutivo = consecutivoService.Get(criteria);
                ViewBag.Consecutivo = consecutivo.Secuencia - 1;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }

            return View();
        }

        public ActionResult BuscarDocumentos(string searchStringDesde, string searchStringHasta, string searchStringNumeroCliente, string searchStringCliente,
            string searchStringNumeroContrato, string searchStringContrato)
        {

            var compania = Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0;
            IList<Facturacion> listaDocuemntos = facturacionService.DocumentosSearch(searchStringDesde, searchStringHasta, searchStringNumeroCliente, searchStringCliente, searchStringNumeroContrato, searchStringContrato, compania);

            return View("Documentos", listaDocuemntos);
        }

        #endregion

        #region EXCLUIR
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO)]
        public ActionResult Excluir()
        {
            try
            {
                if (mensaje != String.Empty)
                {
                    TempData["alertMessage"] = mensaje;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }

            return View();
        }
        #endregion

        #region FACTURACION

        /// <summary>
        /// Este método se encarga de cargar la vista de Facturacion con la informacion inicial 
        /// (Tipoo de cambio y el consecutivo de la ultima factura impresa)
        /// </summary>
        /// <returns>vista Facturacion</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_FACTURACION)]
        public ActionResult Facturacion()
        {
            try
            {
                if (mensaje != String.Empty)
                {
                    TempData["alertMessage"] = mensaje;
                }

                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Fecha", DateTime.Now.ToString("dd/MM/yyyy"));
                var cambios = tipoCambioService.GetAll(criteria);
                ViewBag.TipoCambioVenta = cambios.Where(s => s.Tipo == "Venta").FirstOrDefault();

                Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                criteria = new Dictionary<string, object>();
                criteria.Add("Compania", compania);
                ConsecutivoFacturacion consecutivo = consecutivoService.Get(criteria);
                ViewBag.Consecutivo = consecutivo.Secuencia - 1;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            return View();
        }

        /// <summary>
        /// Este método se encarga de cargar la lista de prefacturas pendientes por facturar
        /// </summary>
        /// <returns>JSON con la lista de prefacturas</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_FACTURACION)]
        public JsonResult Facturar(string factura)
        {
            try
            {
                Facturacion facturacion = facturacionService.Get(Convert.ToInt64(factura));
                facturacion.Estado = "Facturación";
                facturacion.DateLastModified = DateTime.Now;
                facturacion.ModifiedBy = ObtenerUsuarioActual().Nombre;
                
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Compania", facturacion.Compania);
                ConsecutivoFacturacion secuencia = consecutivoService.Get(criteria);
                facturacion.Consecutivo = secuencia.Secuencia;
                
                facturacionService.Update(facturacion);

                foreach (DetalleFacturacion item in facturacion.ListaDetalleFacturacion)
                {

                    if (item.Bascula != null)
                    {
                        item.Bascula.Facturada = true;
                        basculaService.Update(item.Bascula);
                    }

                    if (item.BoletaManual != null)
                    {
                        item.BoletaManual.Facturada = true;
                        boletaManualService.Update(item.BoletaManual);
                    }

                    if (item.OTR != null)
                    {
                        item.OTR.Facturada = true;
                        otrService.Update(item.OTR);
                    }
                }

                secuencia.Secuencia++;
                consecutivoService.Update(secuencia);

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(null);
            }
        }

        /// <summary>
        /// Este método se encarga de cargar la lista de prefacturas pendientes por facturar
        /// </summary>
        /// <returns>JSON con la lista de prefacturas</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_REV_FACTURACION)]
        public JsonResult ReversarFacturar(string factura, string observaciones)
        {
            try
            {
                // Se actualiza la factura que se reverso
                Facturacion facturacion = facturacionService.Get(Convert.ToInt64(factura));
                facturacion.Estado = "Reversada";
                facturacion.DateLastModified = DateTime.Now;
                facturacion.ModifiedBy = ObtenerUsuarioActual().Nombre;

                facturacionService.Update(facturacion);

                // Se liberan cada uno de los item de la factura
                foreach (DetalleFacturacion item in facturacion.ListaDetalleFacturacion)
                {
                    if (item.Bascula != null)
                    {
                        item.Bascula.Facturada = true;
                        basculaService.Update(item.Bascula);
                    }

                    if (item.BoletaManual != null)
                    {
                        item.BoletaManual.Facturada = true;
                        boletaManualService.Update(item.BoletaManual);
                    }

                    if (item.OTR != null)
                    {
                        item.OTR.Facturada = true;
                        otrService.Update(item.OTR);
                    }
                }

                long compania = Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0;
                long contador = reversionService.ReversionSearch(Convert.ToInt64(factura), compania);
             
                //se crea la reversion
                Reversion reversion = new Reversion();
                reversion.Facturacion = facturacion;
                reversion.CreateDate = DateTime.Now;
                reversion.CreatedBy = ObtenerUsuarioActual().Nombre;
                reversion.Observaciones = observaciones;
                reversion.Consecutivo = "DOC-NC-" + (contador+ 1)  + "-" + DateTime.Now.Year;
                reversion.Version = 1;
                reversion.IsDeleted = false;
                reversion.Compania = companiaService.Get(compania);

                reversion = reversionService.Create(reversion);

                return Json(reversion.Id, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(null);
            }
        }

        public void ReporteReversion(long IdReversion)
        {
            Reversion reversion = reversionService.Get(IdReversion);
            //ReportDocument reporte = new ReportDocument();
            ds_Reversion dsReversion = new ds_Reversion();

            var dtReporte = dsReversion.Tables["Reporte"];

            // Rows DataTable Reporte
            DataRow row = dtReporte.NewRow();
            row["Fecha"] = DateTime.Now;
            row["Cliente"] = reversion.Facturacion.Contrato.Cliente.Nombre;
            row["NumCliente"] = reversion.Facturacion.Contrato.Cliente.Numero;
            row["Consecutivo"] = reversion.Consecutivo;
            row["Factura"] = reversion.Facturacion.Consecutivo;
            row["Compania"] = reversion.Facturacion.Compania.Nombre;
            row["Monto"] = reversion.Facturacion.Monto;
            row["Justificacion"] = reversion.Observaciones;
            row["Periodo"] = reversion.Facturacion.FechaDesde.ToString("dd/MM/yyyy") + " al " + reversion.Facturacion.FechaHasta.ToString("dd/MM/yyyy");
            var listaServicios = reversion.Facturacion.Contrato.Productos;
            String servicios = String.Empty;
            foreach (var item in listaServicios)
            {
                if (servicios == String.Empty)
                {
                    servicios += item.Descripcion;
                }
                else
                {
                    servicios += "," + item.Descripcion;
                }
            }
            row["Servicios"] = servicios;
            dtReporte.Rows.Add(row);

            ReportDocument rd = new ReportDocument();
            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//Facturacion//rpt_Reversion.rpt";
            rd.Load(strRptPath);

            rd.SetDataSource(dsReversion);
            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Reversion");
        }

        /// <summary>
        /// Este método se encarga de cargar la lista de prefacturas pendientes por facturar
        /// </summary>
        /// <returns>JSON con la lista de prefacturas</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_FACTURACION)]
        public JsonResult BusquedaPrefacturacion(string desde, string hasta, string cliente, string numCliente, string contrato, string numContrato)
        {
            try
            {
                long compania = Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0;
                IList<Facturacion> Lista = facturacionService.FacturacionSearch(cliente, numCliente, contrato, numContrato, compania);

                if (!String.IsNullOrEmpty(desde))
                {
                    DateTime fechaDesde = Convert.ToDateTime(desde).AddDays(-1);
                    Lista = Lista.Where(u => u.FechaDesde < fechaDesde).ToList();
                }

                if (!String.IsNullOrEmpty(hasta))
                {
                    DateTime fechaHasta = Convert.ToDateTime(hasta).AddDays(1);
                    Lista = Lista.Where(u => u.FechaHasta < fechaHasta).ToList();
                }

                IList<FacturacionModel> ListaModel = facturacionMapper.GetListaModel(Lista);

                return Json(ListaModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(null);
            }
        }

        public void ReporteFacturacion(long id)
        {
            Facturacion factura = new Facturacion();
            factura = facturacionService.Get(id);

            //ReportDocument reporte = new ReportDocument();
            ds_Facturacion dsFacturacion = new ds_Facturacion();

            var dtReporte = dsFacturacion.Tables["Reporte"];
            var dtDatos = dsFacturacion.Tables["Datos"];
            
            // Rows DataTable Reporte
            DataRow row = dtReporte.NewRow();
            row["FechaActual"] = DateTime.Now;
            row["Cliente"] = factura.Contrato.Cliente.Nombre;
            row["Direccion"] = factura.Contrato.Cliente.Direccion;
            row["Telefono"] = factura.Contrato.Cliente.Telefono1;
            row["Total"] = factura.Monto;
            dtReporte.Rows.Add(row);

            // Rows DataTable Datos
            foreach (DetalleFacturacion detalle in factura.ListaDetalleFacturacion)
            {
                DataRow dato = dtDatos.NewRow();
             
                dato["Precio"] = detalle.Monto;
                dato["Descripcion"] = detalle.Periodo + ". " + detalle.Producto.Descripcion;

                if (detalle.OTR != null && detalle.BoletaManual == null && detalle.Bascula == null) // Si el producto es de carga trasera
                {
                    dato["Cantidad"] = 1;
                }
                else
                {
                    if (detalle.Bascula != null && detalle.OTR == null) // Si el producto es de carga trasera
                    {
                        var cantidad = detalle.Bascula.PesoNeto / 1000;
                        dato["Cantidad"] = cantidad < 1 ? 1 : cantidad;
                    }
                    else
                    {
                        if (detalle.OTR != null && detalle.BoletaManual != null)
                        {
                            var cantidad = detalle.BoletaManual.PesoNeto / 1000;
                            dato["Cantidad"] = cantidad < 1 ? 1 : cantidad;
                        }
                        else
                        {
                            if (detalle.OTR == null && detalle.BoletaManual == null && detalle.Bascula == null) // Si el producto es de carga trasera
                            {
                                dato["Cantidad"] = 1;
                            }
                            else
                            {
                                if (detalle.Bascula != null && detalle.OTR != null)
                                {
                                    var cantidad = detalle.Bascula.PesoNeto / 1000;
                                    dato["Cantidad"] = cantidad < 1 ? 1 : cantidad;
                                }
                            }
                        }
                    }
                }

                dtDatos.Rows.Add(dato);
            }

            ReportDocument rd = new ReportDocument();
            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//Facturacion//rpt_Facturacion.rpt";
            rd.Load(strRptPath);

            rd.SetDataSource(dsFacturacion);
            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Facturacion");
        }

        #endregion


    }
}
