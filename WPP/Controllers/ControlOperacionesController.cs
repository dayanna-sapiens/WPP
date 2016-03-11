using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WPP.Datos.Administracion;
using WPP.Datos.Bascula;
using WPP.Datos.Contratos;
using WPP.Datos.ControlOperaciones;
using WPP.Datos.Facturacion;
using WPP.Datos.OperacionRecoleccion;
using WPP.Entities.Objects.Generales;
using WPP.Entities.Objects.ModuloBascula;
using WPP.Entities.Objects.ModuloCierreCaja;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Entities.Objects.ModuloFacturacion;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;
using WPP.Helpers;
using WPP.Mapper;
using WPP.Mapper.General;
using WPP.Mapper.ModuloContratos;
using WPP.Mapper.ModuloOperacionRecoleccion;
using WPP.Model;
using WPP.Model.ModuloOperacionRecoleccion;
using WPP.Security;
using WPP.Service.Generales;
using WPP.Service.ModuloBascula;
using WPP.Service.ModuloBoletaManual;
using WPP.Service.ModuloCierreCaja;
using WPP.Service.ModuloContratos;
using WPP.Service.ModuloFacturacion;
using WPP.Service.ModuloOperacionRecoleccion;

namespace WPP.Controllers
{
    public class ControlOperacionesController : BaseController
    {
        private ICompaniaService companiaService;
        private IContratoService contratoService;
        private IFacturacionService facturacionService;
        private ICierreCajaService cierreService;
        private IOTRService otrService;
        private IBasculaService basculaService;
        private IBoletaManualService boletaService;
        private IUsuarioService usuarioService;
        private ICuadrillaService cuadrillaService;
        private ITipoCambioService tipoCambioService;
        private IEquipoService equipoService;
        private IContenedorService contenedorService;
        private IViajeOTRService viajeOTRServie;
        private ICatalogoService catalogoService;
        private IClienteService clienteService;
        private IRutaRecoleccionService rutaRecoleccionService;
        private ICategoriaProductoService categoriaProductoService;
        private IContratoBitacoraService contratoBitacoraService;
        private IClienteBitacoraService clienteBitacoraService;
        private IProductoService productoService;
        private ICombustibleService combustibleService;
        private ICostoCamionService costoHoraService;
        private CompaniaMapper companiaMapper;
        private CuadrillaMapper cuadrillaMapper;
        private CatalogoMapper catalogoMapper;
        private CategoriaProductoMapper categoriaMapper;
        private UsuarioMapper usuarioMapper;

        public ControlOperacionesController(ICompaniaService compania, IContratoService contrato, IFacturacionService facturacion, 
            ICierreCajaService cierre, IOTRService otr, IBasculaService bascula, IBoletaManualService boleta, IUsuarioService usuario, 
            ICuadrillaService cuadrilla, ITipoCambioService tipoCambio, IEquipoService equipo, IContenedorService contenedor, 
            IViajeOTRService viajeOTR, ICatalogoService catalogo, IClienteService cliente, IRutaRecoleccionService ruta, 
            ICategoriaProductoService categoria, IProductoService producto, ICombustibleService combustible, ICostoCamionService costo,
            IContratoBitacoraService contratoBitacora, IClienteBitacoraService clienteBitacora)
        {
            try
            {
                this.companiaService = compania;
                this.contratoService = contrato;
                this.facturacionService = facturacion;
                this.cierreService = cierre;
                this.otrService = otr;
                this.basculaService = bascula;
                this.boletaService = boleta;
                this.usuarioService = usuario;
                this.cuadrillaService = cuadrilla;
                this.tipoCambioService = tipoCambio;
                this.equipoService = equipo;
                this.contenedorService = contenedor;
                this.viajeOTRServie = viajeOTR;
                this.catalogoService = catalogo;
                this.clienteService = cliente;
                this.rutaRecoleccionService = ruta;
                this.categoriaProductoService = categoria;
                this.productoService = producto;
                this.combustibleService = combustible;
                this.costoHoraService = costo;
                this.contratoBitacoraService = contratoBitacora;
                this.clienteBitacoraService = clienteBitacora;
                companiaMapper = new CompaniaMapper();
                cuadrillaMapper = new CuadrillaMapper();
                catalogoMapper = new CatalogoMapper();
                categoriaMapper = new CategoriaProductoMapper();
                usuarioMapper = new UsuarioMapper();

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        #region FACTURACION

        /// <summary>
        /// Este método carga la información de las compañías existentes 
        /// para poder seleccionar de cual compañía en especifico se desea la facturación 
        /// </summary>
        /// <returns>La vista Facturacion</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_REP_FACTURACION + "," + WPPConstants.ROLES_REPORTES_GERENCIA)]
        public ActionResult Facturacion()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("IsDeleted", false);
            ViewBag.ListaCompania = companiaMapper.GetListaCompaniaModel(companiaService.GetAll(criteria));
            return View();
        }

        /// <summary>
        /// Este método se encarga de generar el reporte de facturación, 
        /// filtra la información según los datos brindados por el usuario: contrato, fecha desde, fecha hasta
        /// Y segun el tipo de reporte (General / Detallado) muestra los datos deseados.
        /// Formato (PDF / Excel)
        /// </summary>
        /// <returns>El reporte deseado</returns>
        public void ReporteFacturacion(long contrato, string desde, string hasta, bool agrupar, string tipo, string formato)
        {
            // Se establecen los filtros
            DateTime fechaHasta = Convert.ToDateTime(hasta);
            DateTime fechaDesde = Convert.ToDateTime(desde);
            Contrato contratoActual = contratoService.Get(contrato);
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Contrato", contratoActual);
            criteria.Add("Estado", "Facturación");
            criteria.Add("Compania", contratoActual.Compania);
            criteria.Add("IsDeleted", false);
       
            IList<Facturacion> ListaFacturacion = facturacionService.GetAll(criteria);
            ListaFacturacion = ListaFacturacion.Where(s => s.CreateDate > fechaDesde.AddDays(-1) && s.CreateDate <= fechaHasta.AddDays(1)).ToList();

            // Se identifica el tipo de reporte
           if(tipo == "General")
           {
               ReporteFacturacionGeneral(contratoActual, ListaFacturacion, agrupar, formato);
           }
           else 
           {
               ReporteFacturacionDetallado(contratoActual, ListaFacturacion, agrupar, formato);
           }
        }

        /// <summary>
        /// Este método se encarga de generar el reporte de facturación general, 
        /// recorre cada una de las facturas que cumplen con los filtros dados y sus detalles 
        /// con el fin de cargar el data set que mostrara la informacion correspondiente
        /// Formato (PDF / Excel)
        /// </summary>
        /// <returns>El reporte deseado</returns>
        public void ReporteFacturacionGeneral(Contrato contratoActual, IList<Facturacion> ListaFacturacion, bool agrupar, string formato)
        {
            ds_FacturacionGeneral dsfacturacion = new ds_FacturacionGeneral();
            var dtReporte = dsfacturacion.Tables["Reporte"];
            var dtDatos = dsfacturacion.Tables["Datos"];

            // Rows DataTable Reporte
            DataRow row = dtReporte.NewRow();
            row["Compania"] = contratoActual.Compania.Nombre;
            row["FechaActual"] = DateTime.Now;
            row["Contrato"] = contratoActual.Numero + " - " + contratoActual.DescripcionContrato;
            row["Cliente"] = contratoActual.Cliente.Numero + " - " + contratoActual.Cliente.Nombre;
            row["Moneda"] = contratoActual.Moneda;
            dtReporte.Rows.Add(row);

            foreach (var factura in ListaFacturacion)
            {
                foreach (var item in factura.ListaDetalleFacturacion)
                {
                    DataRow dato = dtDatos.NewRow();

                    // Rows DataTable Datos
                    dato["Descripcion"] = item.Producto.Descripcion;
                    dato["Precio"] = item.Monto;
                    dato["Factura"] = factura.Consecutivo;
                    dato["Periodo"] = item.Periodo;

                    if (item.OTR != null && item.BoletaManual == null && item.Bascula == null) // Si el producto es de carga trasera 
                    {
                        dato["Cantidad"] = 1;
                    }
                    else
                    {
                        if (item.Bascula != null && item.OTR == null) // Si el producto es de recoleccion
                        {
                            var cantidad = item.Bascula.PesoNeto / 1000;
                            dato["Cantidad"] = cantidad < 1 ? 1 : cantidad;
                        }
                        else
                        {
                            if (item.OTR != null && item.BoletaManual != null) // Si el producto es de carga trasera o roll off con una boleta manual
                            {
                                var cantidad = item.BoletaManual.PesoNeto / 1000;
                                dato["Cantidad"] = cantidad < 1 ? 1 : cantidad;
                            }
                            else
                            {
                                if (item.OTR == null && item.BoletaManual == null && item.Bascula == null) // Si el producto es de alquiler u otro
                                {
                                    dato["Cantidad"] = 1;
                                }
                                else
                                {
                                    if (item.Bascula != null && item.OTR != null)  // Si el producto es de roll-off
                                    {
                                        var cantidad = item.Bascula.PesoNeto / 1000;
                                        dato["Cantidad"] = cantidad < 1 ? 1 : cantidad;
                                    }
                                }
                            }
                        }
                    }

                    dtDatos.Rows.Add(dato);
                }
            }

            // Se genera el reporte deseado
            ReportDocument rd = new ReportDocument();
            string strRptPath = String.Empty;
            // se verifica si el reporte se desea agrupado o no para indicar que rpt le pertenece
            if (agrupar)
            {
                strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//ControlOperaciones//rpt_FacturacionGeneralAgrupado.rpt";
            }
            else
            {
                strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//ControlOperaciones//rpt_FacturacionGeneral.rpt";
            }
            rd.Load(strRptPath);

            rd.SetDataSource(dsfacturacion);// Se asigna el dataset al datasorce del reporte
            var tipoFormato = formato == "pdf" ? ExportFormatType.PortableDocFormat : ExportFormatType.Excel;
            rd.ExportToHttpResponse(tipoFormato, System.Web.HttpContext.Current.Response, false, "Facturación_ControlOperaciones_General");
        }

        /// <summary>
        /// Este método se encarga de generar el reporte de facturación detallado, 
        /// recorre cada una de las facturas que cumplen con los filtros dados y sus detalles 
        /// con el fin de cargar el data set que mostrara la informacion correspondiente 
        /// de la manera mas detallada posible
        /// Formato (PDF / Excel)
        /// </summary>
        /// <returns>El reporte deseado</returns>
        public void ReporteFacturacionDetallado(Contrato contratoActual, IList<Facturacion> ListaFacturacion, bool agrupar, string formato)
        {         
            ds_FacturacionDetallado dsfacturacion = new ds_FacturacionDetallado();
            var dtReporte = dsfacturacion.Tables["Reporte"];
            var dtDatos = dsfacturacion.Tables["Datos"];

            // Rows DataTable Reporte
            DataRow row = dtReporte.NewRow();
            row["Compania"] = contratoActual.Compania.Nombre;
            row["FechaActual"] = DateTime.Now;
            row["Contrato"] = contratoActual.Numero + " - " + contratoActual.DescripcionContrato;
            row["Cliente"] = contratoActual.Cliente.Numero + " - " + contratoActual.Cliente.Nombre;
            row["Moneda"] = contratoActual.Moneda;
            dtReporte.Rows.Add(row);

            foreach (var factura in ListaFacturacion)
            {
                foreach (var item in factura.ListaDetalleFacturacion)
                {
                     // Rows DataTable Datos
                    DataRow dato = dtDatos.NewRow();

                    dato["Descripcion"] = item.Producto.Descripcion;
                    dato["Factura"] = factura.Consecutivo;
                    dato["Periodo"] = item.Periodo;
                    dato["Ruta"] = item.OTR == null ? "" : item.OTR.Consecutivo.ToString();

                    if (item.OTR != null) // Si el producto es de recoleccion
                    {
                        dato["Fecha"] = item.OTR.Fecha;
                    }
                    else
                    {
                        if (item.Bascula != null) // Si el producto es de dispocision
                        {
                            dato["Fecha"] = item.Bascula.Fecha;
                        }
                        else
                        {
                            if (item.BoletaManual != null)// Si el producto es de dispocision con boleta manual
                            {
                                dato["Fecha"] = item.BoletaManual.Fecha;
                            }
                            else // En caso de que no sea uno de los anteriores (Ejemplo: Alquiler)
                            {
                                dato["Fecha"] = item.CreateDate;
                            }
                        }
                    }

                    if (item.OTR != null && item.BoletaManual == null && item.Bascula == null) // Si el producto es de carga trasera
                    {
                        dato["Cantidad"] = 1;
                        dato["Boleta"] = String.Empty;
                        dato["Placa"] = item.OTR.Equipo.Nombre;
                    }
                    else
                    {
                        if (item.Bascula != null && item.OTR == null) // Si el producto es de disposicion final
                        {
                            var cantidad = item.Bascula.PesoNeto / 1000;
                            dato["Cantidad"] = cantidad < 1 ? 1 : cantidad;
                            dato["Boleta"] = item.Bascula.Boleta;
                            dato["Placa"] = item.Bascula.Equipo.Nombre;
                        }
                        else
                        {
                            if (item.OTR != null && item.BoletaManual != null)// si el producto es de disposicion con boleta manual
                            {
                                var cantidad = item.BoletaManual.PesoNeto / 1000;
                                dato["Cantidad"] = cantidad < 1 ? 1 : cantidad;
                                dato["Boleta"] = item.BoletaManual.NumeroBoleta;
                                dato["Placa"] = item.OTR.Equipo.Nombre;
                            }
                            else
                            {
                                if (item.OTR == null && item.BoletaManual == null && item.Bascula == null) // Si el producto es de alquiler
                                {
                                    dato["Cantidad"] = 1;
                                    dato["Boleta"] = String.Empty;
                                    dato["Placa"] = String.Empty;
                                }
                                else
                                {
                                    if (item.Bascula != null && item.OTR != null )// si el producto es de roll-off
                                    {
                                        var cantidad = item.Bascula.PesoNeto / 1000;
                                        dato["Cantidad"] = cantidad < 1 ? 1 : cantidad;
                                        dato["Boleta"] = item.Bascula.Boleta;
                                        dato["Placa"] = item.OTR.Equipo.Nombre;
                                    }
                                }
                            }
                        }
                    }

                    dato["Unidad"] = item.Producto.Producto.UnidadCobro.Nombre;
                    dato["PrecioUnidad"] = item.Producto.Total;
                    dato["PrecioTotal"] = item.Monto;
                    dtDatos.Rows.Add(dato);
                }
            }

            // Se genera el reporte deseado
            ReportDocument rd = new ReportDocument();
            string strRptPath = String.Empty;
            //Se verifica si el reporte se desea agrupado o no con el fin de indicar a que rpt se debe asignar el dataset
            if (agrupar)
            {
                strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//ControlOperaciones//rpt_FacturacionDetalladoAgrupado.rpt";
            }
            else
            {
                strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//ControlOperaciones//rpt_FacturacionDetallado.rpt";
            }
            rd.Load(strRptPath);

            rd.SetDataSource(dsfacturacion);// Se asigna el dataset al datasorce del reporte
            var tipoFormato = formato == "pdf" ? ExportFormatType.PortableDocFormat : ExportFormatType.Excel;
            rd.ExportToHttpResponse(tipoFormato, System.Web.HttpContext.Current.Response, false, "Facturación_ControlOperaciones_Detalle");
        }

        /// <summary>
        /// Este método carga la información de las compañías existentes 
        /// para poder seleccionar de cual compañía en especifico se desea la lista de facturación 
        /// </summary>
        /// <returns>La vista ListaFacturas</returns>
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_REP_FACTURACION + "," + WPPConstants.ROLES_REPORTES_GERENCIA)]
        public ActionResult ListaFacturas()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("IsDeleted", false);
            ViewBag.ListaCompania = companiaMapper.GetListaCompaniaModel(companiaService.GetAll(criteria));
            return View();
        }

        /// <summary>
        /// Este método se encarga de generar el reporte de lista de facturas, 
        /// recorre cada una de las facturas que cumplen con los filtros dados y sus detalles 
        /// con el fin de cargar el data set que mostrara la informacion correspondiente 
        /// Formato (PDF / Excel)
        /// </summary>
        /// <returns>El reporte deseado</returns>
        public void ReporteListaFacturas(string desde, string hasta, string contrato, string moneda, string estado, string formato, string compania)
        {
            // Se establecen los filtros
            DateTime fechaHasta = Convert.ToDateTime(hasta);
            DateTime fechaDesde = Convert.ToDateTime(desde);
            Compania companiaActual = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
          
            IDictionary<string, object> criteria = new Dictionary<string, object>();

            criteria.Add("Moneda", moneda);
            if(estado !="Todos")
            {
                criteria.Add("Estado", estado);            
            }
            if (contrato != String.Empty)
            {
                Contrato contratoActual = contratoService.Get(Convert.ToInt64(contrato));
                criteria.Add("Contrato", contrato);
            }
           // criteria.Add("Compania", compania);
            if (compania != String.Empty)
            {
                if (compania != "Todos")
                {
                    Compania companiaSearch = companiaService.Get(Convert.ToInt64(compania));
                    criteria.Add("Compania", companiaSearch.Id);
                }
            }
            else
            {
                criteria.Add("Compania", companiaActual);
            }

            criteria.Add("IsDeleted", false);
            IList<Facturacion> ListaFacturas = facturacionService.GetAll(criteria);
            ListaFacturas = ListaFacturas.Where(s => s.CreateDate > fechaDesde.AddDays(-1) && s.CreateDate <= fechaHasta.AddDays(1)).ToList();
            if(estado == "Todos")
            {
                ListaFacturas = ListaFacturas.Where(s => s.Estado != "Prefacturación").ToList();
            }

            // Se indica el data set a utilizar
            ds_ListaFacturas dsfacturacion = new ds_ListaFacturas();
            ds_ItemsFactura dsItems = new ds_ItemsFactura();
            var dtReporte = dsfacturacion.Tables["Reporte"];
            var dtDatos = dsfacturacion.Tables["Datos"];
            var dtItems = dsItems.Tables["Datos"];

            // Rows DataTable Reporte
            DataRow row = dtReporte.NewRow();
            row["Compania"] = companiaActual.Nombre;
            row["FechaActual"] = DateTime.Now;
            dtReporte.Rows.Add(row);

            foreach (var factura in ListaFacturas)
            {
                //Rows DataTable Datos
                DataRow dato = dtDatos.NewRow();
                dato["Cliente"] = factura.Contrato.Cliente.Nombre;
                dato["Moneda"] = factura.Moneda;
                dato["Contrato"] = factura.Contrato.Numero;
                dato["IdFactura"] = factura.Id;
                dato["Factura"] = factura.Consecutivo;
                dato["Fecha"] = factura.CreateDate;
                dato["Monto"] = factura.Monto;
                dato["Saldo"] = 0;
                dato["Estado"] = factura.Estado;
                dato["Compania"] = factura.Compania.Nombre;
                dtDatos.Rows.Add(dato);

                foreach (var item in factura.ListaDetalleFacturacion)
                {
                    //Rows DataTable Datos (Subreporte)
                    DataRow datoItem = dtItems.NewRow();
                    datoItem["IdFactura"] = factura.Id;
                    datoItem["Unidad"] = item.Producto.Producto.UnidadCobro.Nombre;
                    datoItem["Monto"] = item.Monto;
                    datoItem["Cantidad"] = item.Cantidad;
                    datoItem["Descripcion"] = item.Producto.Descripcion + ". " + item.Periodo;
                    dtItems.Rows.Add(datoItem);
                }
            }

            // Se genera el reporte deseado
            ReportDocument rd = new ReportDocument();
            string strRptPath = String.Empty;
            strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//Facturacion//rpt_ListaFacturas.rpt";
            rd.Load(strRptPath);

            rd.SetDataSource(dsfacturacion);// Se asigna el dataset al datasorce del reporte
            rd.Subreports[0].SetDataSource(dsItems); // Se asigna el datasource al subreporte (dsItems)
            var tipoFormato = formato == "pdf" ? ExportFormatType.PortableDocFormat : ExportFormatType.Excel;
            rd.ExportToHttpResponse(tipoFormato, System.Web.HttpContext.Current.Response, false, "ControlOperaciones_ListaFacturas");
        }

        #endregion

        #region CIERRE CAJA

        /// <summary>
        /// Este método carga la información de las compañías existentes 
        /// para poder seleccionar de cual compañía en especifico se desea el cierre de caja
        /// </summary>
        /// <returns>La vista CierreCaja</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_REP_CIERRE_CAJA + "," + WPPConstants.ROLES_REPORTES_GERENCIA)]
        public ActionResult CierreCaja()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("IsDeleted", false);
            ViewBag.ListaCompania = companiaMapper.GetListaCompaniaModel(companiaService.GetAll(criteria));
            return View();
        }

        /// <summary>
        /// Este método se encarga de generar el reporte de lista de facturas, 
        /// recorre cada uno de los cierres de caja que cumplen con los filtros dados  
        /// con el fin de cargar el data set que mostrara la informacion correspondiente 
        /// Formato (PDF / Excel)
        /// </summary>
        /// <returns>El reporte deseado</returns>
        public void ReporteCierreCaja(string moneda, string desde, string hasta, bool agrupar, string tipo, string formato, string compania)
        {
            // Se establecen los filtros
            Compania companiaActual = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
            
            DateTime fechaHasta = DateTime.Now;
            if (desde != String.Empty)
            {
                fechaHasta = Convert.ToDateTime(hasta);            
            }
            DateTime fechaDesde = DateTime.Now;
            if (desde != String.Empty)
            {
                fechaDesde = Convert.ToDateTime(desde);
            }
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Moneda", moneda);
            criteria.Add("IsDeleted", false);
            if (compania != String.Empty)
            {
                if (compania != "Todos")
                {
                    Compania companiaSearch = companiaService.Get(Convert.ToInt64(compania));
                    criteria.Add("Compania", companiaSearch.Id);
                }
            }
            else
            {
                criteria.Add("Compania", companiaActual);
            }

            IList<CierreCaja> ListaCierreCaja = cierreService.GetAll(criteria);
            if (desde != String.Empty && hasta != String.Empty)
            {
                ListaCierreCaja = ListaCierreCaja.Where(s => s.CreateDate > fechaDesde.AddDays(-1) && s.CreateDate <= fechaHasta.AddDays(1)).ToList();
            }

            // Se indica el data set a utilizar
            ds_CierreCajaDetallado dsCierre = new ds_CierreCajaDetallado();
            var dtReporte = dsCierre.Tables["Reporte"];
            var dtDatos = dsCierre.Tables["Datos"];

            // Rows DataTable Reporte
            DataRow row = dtReporte.NewRow();
            row["Compania"] = companiaActual.Nombre;
            row["FechaActual"] = DateTime.Now;           
            dtReporte.Rows.Add(row);

            foreach (var cierre in ListaCierreCaja)
            {
                IList<PagoBascula> Pagos = new List<PagoBascula>();
                if (tipo != "Todos")
                {
                    Pagos = cierre.Pagos.Where(s => s.FormaPago == tipo).ToList();
                }
                else
                {
                    Pagos = cierre.Pagos;
                }

                foreach (var item in Pagos)
                {
                    // Rows DataTable Datos
                    DataRow dato = dtDatos.NewRow();
                    dato["Fecha"] = cierre.CreateDate;
                    dato["Boleta"] = item.Boleta.Boleta;
                    dato["Recibo"] = item.Boleta.NumeroRecibo;
                    dato["Monto"] = item.Monto;
                    dato["Nombre"] = item.Boleta.NombreCliente;
                    dato["TipoPago"] = item.FormaPago;
                    dato["Consecutivo"] = item.Cierre.Consecutivo;
                    dato["Cajero"] = item.Cierre.CreatedBy;
                    dato["Moneda"] = item.Cierre.Moneda;
                    dato["Compania"] = cierre.Compania.Nombre;
                    dato["Estado"] = item.Boleta.Estado;
                                        
                    dtDatos.Rows.Add(dato);
                }
            }

            // Se genera el reporte deseado
            ReportDocument rd = new ReportDocument();
            string strRptPath = String.Empty;
            //Se verifica si se desea agrupado o no para indicar que rpt debe usar
            if (agrupar)
            {
                strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//ControlOperaciones//rpt_CierreCajaAgrupado.rpt";
            }
            else
            {
                strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//ControlOperaciones//rpt_CierreCaja.rpt";
            }
            rd.Load(strRptPath);

            rd.SetDataSource(dsCierre);// Se asigna el dataset al datasorce del reporte
            var tipoFormato = formato == "pdf" ? ExportFormatType.PortableDocFormat : ExportFormatType.Excel;
            rd.ExportToHttpResponse(tipoFormato, System.Web.HttpContext.Current.Response, false, "CierreCaja_ControlOperaciones");
        }

        /// <summary>
        /// Este método carga la vista deseada para seleccionar los filtros para obtener el reporte de cierre de caja general
        /// </summary>
        /// <returns>La vista CierreCajaGeneral</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_REP_CIERRE_CREDITO_CONTADO + "," + WPPConstants.ROLES_REPORTES_GERENCIA)]
        public ActionResult CierreCajaGeneral()
        {
            return View();
        }

        /// <summary>
        /// Este método se encarga de generar el reporte de cierre de caja, 
        /// recorre cada uno de los cierres de caja que cumplen con los filtros dados  
        /// con el fin de cargar el data set que mostrara la informacion correspondiente
        /// Este reporte presenta tanto los registros de credito y de contado (Contador y Cuentas por Cobrar)
        /// Formato (PDF / Excel)
        /// </summary>
        /// <returns>El reporte deseado</returns>
        public void ReporteCierreCajaGeneral(string moneda, string desde, string hasta, string formato)
        {
            // Se establecen los filtros
            Compania companiaActual = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);

            DateTime fechaHasta = DateTime.Now;
            if (desde != String.Empty)
            {
                fechaHasta = Convert.ToDateTime(hasta);
            }
            DateTime fechaDesde = DateTime.Now;
            if (desde != String.Empty)
            {
                fechaDesde = Convert.ToDateTime(desde);
            }
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Moneda", moneda);
            criteria.Add("IsDeleted", false);          
            criteria.Add("Compania", companiaActual);
            
            IList<CierreCaja> ListaCierreCaja = cierreService.GetAll(criteria);
            if (desde != String.Empty && hasta != String.Empty)
            {
                ListaCierreCaja = ListaCierreCaja.Where(s => s.CreateDate > fechaDesde.AddDays(-1) && s.CreateDate <= fechaHasta.AddDays(1)).ToList();
            }

            // Se indica el data set a utilizar
            ds_CierreCajaFinal dsCierre = new ds_CierreCajaFinal();
            var dtReporte = dsCierre.Tables["Reporte"];
            var dtDatos = dsCierre.Tables["Datos"];

            // Rows DataTable Reporte
            DataRow row = dtReporte.NewRow();
            row["Compania"] = companiaActual.Nombre;
            row["FechaActual"] = DateTime.Now;
            dtReporte.Rows.Add(row);

            foreach (var cierre in ListaCierreCaja)
            {
                // Registros de Contado
                IList<PagoBascula> Pagos = new List<PagoBascula>();
                Pagos = cierre.Pagos;
                
                foreach (var item in Pagos)
                {
                    // Rows DataTable Datos
                    DataRow dato = dtDatos.NewRow();
                    dato["Fecha"] = cierre.CreateDate;
                    dato["Boleta"] = item.Boleta.Boleta;
                    dato["Recibo"] = item.Boleta.NumeroRecibo;
                    dato["Monto"] = item.Monto;
                    dato["Cliente"] = item.Boleta.NombreCliente;
                    dato["FormaPago"] = item.FormaPago;
                    dato["Cierre"] = item.Cierre.Consecutivo;
                    dato["Tipo"] = "Contado";
                    dato["Tonelaje"] = item.Boleta.PesoNeto;
                    dato["Estado"] = item.Boleta.Estado;

                    dtDatos.Rows.Add(dato);
                }

                // Registros de Crédito
                IList<Bascula> creditos = new List<Bascula>();
                creditos = cierre.Creditos;

                foreach (var item in creditos)
                {
                    // Rows DataTable Datos
                    DataRow dato = dtDatos.NewRow();
                    dato["Fecha"] = cierre.CreateDate;
                    dato["Boleta"] = item.Boleta;
                    dato["Recibo"] = item.NumeroRecibo;
                    dato["Monto"] = item.Total;
                    dato["Cliente"] = item.NombreCliente;
                    dato["FormaPago"] = "Crédito";
                    dato["Cierre"] = item.CierreCaja.Consecutivo;
                    dato["Tipo"] = "Crédito";
                    dato["Tonelaje"] = item.PesoNeto;
                    dato["Estado"] = item.Estado;

                    dtDatos.Rows.Add(dato);
                }
            }

            // Se genera el reporte deseado
            ReportDocument rd = new ReportDocument();
            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//CierreCaja//rpt_CierreCajaGeneral.rpt";
            
            rd.Load(strRptPath);

            rd.SetDataSource(dsCierre);// Se asigna el dataset al datasorce del reporte
            var tipoFormato = formato == "pdf" ? ExportFormatType.PortableDocFormat : ExportFormatType.Excel;
            rd.ExportToHttpResponse(tipoFormato, System.Web.HttpContext.Current.Response, false, "CierreCaja");
        }


        #endregion

        #region OPERACION DE RECOLECCION
        
        /// <summary>
        /// retorna la vista indicada para obtener los filtros deseados para generar el reporte
        /// </summary>
        /// <returns>La vista ComparativoHoras</returns>
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_REP_CONTROL_OPERACIONES_DIARIO + "," + WPPConstants.ROLES_REPORTES_GERENCIA)]
        public ActionResult ComparativoHoras()
        {
            return View();
        }

        /// <summary>
        /// retorna la vista indicada para obtener los filtros deseados para generar el reporte
        /// </summary>
        /// <returns>La vista DieselConsumido</returns>
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_REP_CONTROL_OPERACIONES_DIARIO + "," + WPPConstants.ROLES_REPORTES_GERENCIA)]
        public ActionResult DieselConsumido()
        {
            return View();
        }

        /// <summary>
        /// retorna la vista indicada para obtener los filtros deseados para generar el reporte
        /// </summary>
        /// <returns>La vista HorasEfectivas</returns>
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_REP_CONTROL_OPERACIONES_DIARIO + "," + WPPConstants.ROLES_REPORTES_GERENCIA)]
        public ActionResult HorasEfectivas()
        {
            return View();
        }

        /// <summary>
        /// retorna la vista indicada para obtener los filtros deseados para generar el reporte
        /// </summary>
        /// <returns>La vista Promedios</returns>
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_REP_CONTROL_OPERACIONES_DIARIO + "," + WPPConstants.ROLES_REPORTES_GERENCIA)]
        public ActionResult Promedios()
        {
            return View();
        }

        /// <summary>
        /// retorna la vista indicada para obtener los filtros deseados para generar el reporte
        /// </summary>
        /// <returns>La vista Resumen</returns>
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_REP_CONTROL_OPERACIONES_DIARIO + "," + WPPConstants.ROLES_REPORTES_GERENCIA)]
        public ActionResult Resumen()
        {
            return View();
        }

        /// <summary>
        /// retorna la vista indicada para obtener los filtros deseados para generar el reporte
        /// </summary>
        /// <returns>La vista TotalDiesel</returns>
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_REP_CONTROL_OPERACIONES_DIARIO + "," + WPPConstants.ROLES_REPORTES_GERENCIA)]
        public ActionResult TotalDiesel()
        {
            return View();
        }

        /// <summary>
        /// retorna la vista indicada para obtener los filtros deseados para generar el reporte
        /// </summary>
        /// <returns>La vista TotalToneladas</returns>
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_REP_CONTROL_OPERACIONES_DIARIO + "," + WPPConstants.ROLES_REPORTES_GERENCIA)]
        public ActionResult TotalToneladas()
        {
            return View();
        }

        /// <summary>
        /// retorna la vista indicada para obtener los filtros deseados para generar el reporte
        /// </summary>
        /// <returns>La vista TotalViajes</returns>
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_REP_CONTROL_OPERACIONES_DIARIO + "," + WPPConstants.ROLES_REPORTES_GERENCIA)]
        public ActionResult TotalViajes()
        {
            return View();
        }

        /// <summary>
        /// retorna la vista indicada para obtener los filtros deseados para generar el reporte
        /// </summary>
        /// <returns>La vista ControlRecoleccionDiario</returns>
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_REP_CONTROL_OPERACIONES_DIARIO + "," + WPPConstants.ROLES_REPORTES_GERENCIA)]
        public ActionResult ControlRecoleccionDiario()
        {
            //IDictionary<string, object> criteria = new Dictionary<string, object>();
            //criteria.Add("IsDeleted", false);
            //ViewBag.ListaCompania = companiaMapper.GetListaCompaniaModel(companiaService.GetAll(criteria));
            return View();
        }

        /// <summary>
        /// Este metodo recopila la informacion necesaria para generar el reporte de Control de recoleccion diaria,
        /// donde recorre todas las OTRs que cumplen con los filtros dados 
        /// </summary>
        /// <returns>La vista ComparativoHoras</returns>
        public void ReporteControlRecoleccionDiario(string desde, string hasta, string tipo, string formato, string reporte)
        {
            // Se establecen los filtros
            DateTime fechaHasta = Convert.ToDateTime(hasta);
            DateTime fechaDesde = Convert.ToDateTime(desde);
            Compania companiaActual = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
            
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("IsDeleted", false);
            criteria.Add("Estado", "Cerrado");
            if (tipo != "Todos")
            {
                criteria.Add("Tipo", tipo);
            }
            
            IList<OTR> ListaOTR = otrService.GetAll(criteria);
            ListaOTR = ListaOTR.Where(s => s.Fecha > fechaDesde.AddDays(-1) && s.Fecha <= fechaHasta.AddDays(1)).ToList();

            // Se indica el data set a utilizar
            ds_ControlRecoleccionDiario dsCierre = new ds_ControlRecoleccionDiario();
            var dtReporte = dsCierre.Tables["Reporte"];
            var dtDatos = dsCierre.Tables["Datos"];

            // Rows DataTable Reporte
            DataRow row = dtReporte.NewRow();
            row["Compania"] = companiaActual.Nombre;
            row["FechaActual"] = DateTime.Now;
            dtReporte.Rows.Add(row);

            foreach (var otr in ListaOTR)
            {      
                foreach (var item in otr.RutaRecoleccion.Rutas)
                {
                    // Rows DataTable Datos
                    DataRow dato = dtDatos.NewRow();
                    criteria = new Dictionary<string, object>();
                    criteria.Add("IsDeleted", false);
                    criteria.Add("OTR", otr);
                    Bascula bascula = basculaService.Get(criteria);

                    if (bascula != null)
                    {
                        dato["FechaBoleta"] = bascula.Fecha;
                        dato["Boleta"] = bascula.Boleta;
                        dato["Neto"] = bascula.PesoNeto;
                    }
                    else
                    {
                        criteria = new Dictionary<string, object>();
                        criteria.Add("IsDeleted", false);
                        criteria.Add("OTR", otr);

                        var boleta = boletaService.Get(criteria);
                        dato["FechaBoleta"] = boleta.Fecha;
                        dato["Boleta"] = boleta.NumeroBoleta;
                        dato["Neto"] = boleta.PesoNeto;
                    }

                    dato["Placa"] = otr.Equipo.Placa;
                    var chofer = otr.Cuadrilla.ListaEmpleados.Where(s=> s.Puesto == "Chofer").FirstOrDefault();
                    dato["Chofer"] = chofer.Nombre;
                    dato["Cliente"] = item.Contrato.Cliente.Nombre;
                    dato["OTR"] = otr.Consecutivo;
                    dato["HorimetroInicio"] = otr.HorimetroInicio;
                    dato["HorimetroFin"] = otr.HorimetroFin;
                    dato["KilometrajeInicio"] = otr.KilometrajeInicio;
                    dato["KilometrajeFin"] = otr.KilometrajeFin;
                    dato["Combustible"] = otr.Combustible == null ? 0 : Convert.ToInt64(otr.Combustible);
                    dato["TipoOTR"] = otr.Tipo;
                    dato["Compania"] = companiaActual.Nombre;

                    IList<Combustible> ListaCombustible = combustibleService.ListAll();
                    var ListaCombustibleFilter = ListaCombustible.Where(s => s.FechaDesde.AddDays(-1) <= otr.Fecha && s.FechaHasta.AddDays(1) >= otr.Fecha).ToList();
                    if (ListaCombustibleFilter.Count > 0) // si la lista filtrada por fechas no retorna nada, se toma el ultimo costo
                    {
                        var combustible = ListaCombustibleFilter.OrderByDescending(s => s.FechaHasta).ToList().FirstOrDefault();
                        dato["Gasolina"] = otr.Equipo.TipoCombustible == "Diesel" ? combustible.Diesel : combustible.Gasolina;
                        dato["Diesel"] = otr.Equipo.TipoCombustible == "Diesel" ? combustible.Diesel : combustible.Gasolina;
                    }
                    else
                    {
                        var combustible = ListaCombustible.OrderByDescending(s => s.FechaHasta).ToList();
                        if (combustible.Count > 0)
                        {
                            dato["Gasolina"] = otr.Equipo.TipoCombustible == "Diesel" ? combustible.FirstOrDefault().Diesel : combustible.FirstOrDefault().Gasolina;
                            dato["Diesel"] = otr.Equipo.TipoCombustible == "Diesel" ? combustible.FirstOrDefault().Diesel : combustible.FirstOrDefault().Gasolina;
                        }
                        else { // En caso de que no existan ningun registro, se pone 0
                            dato["Gasolina"] = 0;
                            dato["Diesel"] = 0;                      
                        }
                    }
                    

                    IList<CostoCamion> ListaCostoHora = costoHoraService.ListAll();
                    var ListaCostoHoraFilter = ListaCostoHora.Where(s => s.FechaDesde.AddDays(-1) <= otr.Fecha && s.FechaHasta.AddDays(1) >= otr.Fecha && s.Tipo == otr.Cliente.Tipo).ToList();

                    if (ListaCostoHoraFilter.Count > 0) // si la lista filtrada por fechas no retorna nada, se toma el ultimo costo
                    {
                        var costo = ListaCostoHoraFilter.OrderByDescending(s => s.FechaHasta).ToList().FirstOrDefault();
                        dato["CostoTrabajo"] = costo.Monto;
                    }
                    else
                    {
                        ListaCostoHora = ListaCostoHora.OrderByDescending(s=>s.FechaHasta).ToList();
                        if (ListaCostoHora.Count > 0)
                        {
                            var costo = ListaCostoHora.FirstOrDefault();
                            dato["CostoTrabajo"] = costo.Monto;
                        }
                        else{// En caso de que no existan ningun registro, se pone 0
                            dato["CostoTrabajo"] = 0;                        
                        }
                    }                   

                    dtDatos.Rows.Add(dato);
                }
            }

            // Se genera el reporte deseado
            ReportDocument rd = new ReportDocument();
            string strRptPath = String.Empty;
            //Se verifica cual reporte es para indicar que rpt debe ser generado
            switch (reporte)
            {
                case "ComparativoHoras":
                    strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//ControlOperaciones//rpt_ComparativoHorasControlDiario.rpt";
                    break;
                case "DieselConsumido":
                    strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//ControlOperaciones//rpt_DieselConsumidoControlDiario.rpt";
                    break;
                case "HorasEfectivas":
                    strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//ControlOperaciones//rpt_HorasEfectivasControlDiario.rpt";
                    break;
                case "Promedios":
                    strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//ControlOperaciones//rpt_PromediosControlDiario.rpt";
                    break;
                case "Resumen":
                    strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//ControlOperaciones//rpt_ResumenControlDiario.rpt";
                    break;
                case "TotalDiesel":
                    strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//ControlOperaciones//rpt_TotalDieselControlDiario.rpt";
                     break;
                case "TotalToneladas":
                     strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//ControlOperaciones//rpt_TotalToneladasControlDiario.rpt";
                    break;
                case "TotalViajes":
                    strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//ControlOperaciones//rpt_TotalViajesControlDiario.rpt";
                    break;
                default:
                    strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//ControlOperaciones//rpt_ControlOperacionDiario.rpt";
                    break;
            }
            rd.Load(strRptPath);

            rd.SetDataSource(dsCierre);// Se asigna el dataset al datasorce del reporte
            var tipoFormato = formato == "pdf" ? ExportFormatType.PortableDocFormat : ExportFormatType.Excel;
            rd.ExportToHttpResponse(tipoFormato, System.Web.HttpContext.Current.Response, false, "ControlOperacionesDiario");
        }

        /// <summary>
        /// Este método carga la información de las compañías existentes 
        /// para poder seleccionar de cual compañía en especifico se desea la otr 
        /// y retorna la vista indicada para obtener los filtros deseados para generar el reporte
        /// </summary>
        /// <returns>La vista OTR</returns>
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_REP_OTR + "," + WPPConstants.ROLES_REPORTES_GERENCIA)]
        public ActionResult OTR()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("IsDeleted", false);
            ViewBag.ListaCompania = companiaMapper.GetListaCompaniaModel(companiaService.GetAll(criteria));
            return View();
        }

        /// <summary>
        /// Este metodo recopila la informacion necesaria para generar el reporte de OTRs,
        /// donde recorre todas las OTRs que cumplen con los filtros dados 
        /// </summary>
        /// <returns>retorna el reporte deseado</returns>
        public void ReporteOTR(string desde, string hasta, string tipo, string estado, string formato, string compania)
        {
            // Se establecen los filtros
            DateTime fechaHasta = Convert.ToDateTime(hasta);
            DateTime fechaDesde = Convert.ToDateTime(desde);
            Compania companiaActual = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
           
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("IsDeleted", false);
            if (estado != "Todos")
            {
                criteria.Add("Estado", estado);
            }
            if (tipo != "Todos")
            {
                criteria.Add("Tipo", tipo);
            }
            if (compania != String.Empty)
            {
                if (compania != "Todos")
                {
                    companiaActual = companiaService.Get(Convert.ToInt64(compania));
                    criteria.Add("Compania", companiaActual);
                }
            }
            else
            {
                criteria.Add("Compania", companiaActual);
            }

            IList<OTR> ListaOTR = otrService.GetAll(criteria);
            ListaOTR = ListaOTR.Where(s => s.Fecha > fechaDesde.AddDays(-1) && s.Fecha <= fechaHasta.AddDays(1)).ToList();
            ListaOTR = ListaOTR.OrderBy(s => s.Fecha).ToList();

            // Se indica el data set a utilizar
            ds_OTR dsCierre = new ds_OTR();
            var dtReporte = dsCierre.Tables["Reporte"];
            var dtDatos = dsCierre.Tables["Datos"];

            // Rows DataTable Reporte
            DataRow row = dtReporte.NewRow();
            row["Compania"] = companiaActual.Nombre;
            row["FechaActual"] = DateTime.Now;
            dtReporte.Rows.Add(row);

            foreach (var otr in ListaOTR)
            {
                // Rows DataTable Datos
                DataRow dato = dtDatos.NewRow();
                criteria = new Dictionary<string, object>();
                criteria.Add("IsDeleted", false);
                criteria.Add("OTR", otr);
              
                dato["Equipo"] = otr.Equipo.Placa;
                var chofer = otr.Cuadrilla.ListaEmpleados.Where(s => s.Puesto == "Chofer").FirstOrDefault();
                dato["Chofer"] = chofer.Nombre;
                dato["Fecha"] = otr.Fecha;
                dato["OTR"] = otr.Consecutivo;
                dato["Estado"] = otr.Estado;
                dato["Ruta"] = otr.RutaRecoleccion.Descripcion;
                dato["Tipo"] = otr.Tipo;
                dato["Compania"] = otr.Compania.Nombre;

                dtDatos.Rows.Add(dato);  
            }

            // Se genera el reporte deseado
            ReportDocument rd = new ReportDocument();
            string strRptPath = String.Empty;
            strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//ControlOperaciones//rpt_OTR.rpt";
            rd.Load(strRptPath);

            rd.SetDataSource(dsCierre);// Se asigna el dataset al datasorce del reporte
            var tipoFormato = formato == "pdf" ? ExportFormatType.PortableDocFormat : ExportFormatType.Excel;
            rd.ExportToHttpResponse(tipoFormato, System.Web.HttpContext.Current.Response, false, "ControlOperaciones_OTR");

        }

        /// <summary>
        /// retorna la vista indicada para obtener los filtros deseados para generar el reporte
        /// </summary>
        /// <returns>La vista Cuadrilla</returns>
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_REP_CUADRILLAS + "," + WPPConstants.ROLES_REPORTES_GERENCIA)]
        public ActionResult Cuadrilla()
        {
            return View();
        }

        /// <summary>
        /// Este metodo recopila la informacion necesaria para generar el reporte de cuadrilla,
        /// donde recorre todas las cuadrillas que cumplen con los filtros dados 
        /// para obtener la informacion correspondiente
        /// </summary>
        /// <returns>el reporte deseado</returns>
        public void ReporteCuadrilla(string chofer, string descripcion, string formato)
        {
            // Se establecen los filtros
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("IsDeleted", false);

            IList<Cuadrilla> ListaCuadrilla = new List<Cuadrilla>();
            ListaCuadrilla = cuadrillaService.GetAll(criteria);   

            if(descripcion != String.Empty)
            {
                ListaCuadrilla = ListaCuadrilla.Where(s=> s.Descripcion.ToUpper().Contains(descripcion.ToUpper())).ToList();
            }
            if (chofer != String.Empty)
            {
                ListaCuadrilla = ListaCuadrilla.Where(s => s.Descripcion.ToUpper().Contains(descripcion.ToUpper())).ToList();
            }

            // Se indica el data set a utilizar
            ds_Cuadrilla dsCierre = new ds_Cuadrilla();
            var dtReporte = dsCierre.Tables["Reporte"];
            var dtDatos = dsCierre.Tables["Datos"];

            Compania companiaActual = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
           
            // Rows DataTable Reporte
            DataRow row = dtReporte.NewRow();
            row["Compania"] = companiaActual.Nombre;
            row["FechaActual"] = DateTime.Now;
            dtReporte.Rows.Add(row);

            foreach (var item in ListaCuadrilla)
            {
                var NombreChofer = item.ListaEmpleados.Where(s => s.Puesto == "Chofer").FirstOrDefault().Nombre;
                if (chofer == String.Empty || NombreChofer.ToUpper().Contains(chofer.ToUpper()))
                {
                    var empleados = item.ListaEmpleados.OrderBy(s => s.Puesto).ToList();
                    foreach (var empleado in empleados)
                    {
                        DataRow dato = dtDatos.NewRow();
                        dato["Cuadrilla"] = item.Descripcion;
                        dato["Puesto"] = empleado.Puesto;
                        dato["Nombre"] = empleado.Nombre;
                        dtDatos.Rows.Add(dato);
                    }
                }
            }

            // Se genera el reporte deseado
            ReportDocument rd = new ReportDocument();
            string strRptPath = String.Empty;
            strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//Recoleccion//rpt_Cuadrilla.rpt";
            rd.Load(strRptPath);

            rd.SetDataSource(dsCierre);// Se asigna el dataset al datasorce del reporte
            var tipoFormato = formato == "pdf" ? ExportFormatType.PortableDocFormat : ExportFormatType.Excel;
            rd.ExportToHttpResponse(tipoFormato, System.Web.HttpContext.Current.Response, false, "ControlOperaciones_Cuadrilla");
        
        }

        /// <summary>
        /// Este método carga la información de las compañías existentes 
        /// para poder seleccionar de cual compañía en especifico se desea obtener las toneladas por cuadrilla
        /// y retorna la vista indicada para obtener los filtros deseados para generar el reporte
        /// </summary>
        /// <returns>La vista ToneladasXCuadrilla</returns>
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_REP_CUADRILLAS + "," + WPPConstants.ROLES_REPORTES_GERENCIA)]
        public ActionResult ToneladasXCuadrilla()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("IsDeleted", false);
            ViewBag.ListaCuadrilla = cuadrillaMapper.GetListaModel(cuadrillaService.GetAll(criteria));
            return View();
        }

        /// <summary>
        /// Este metodo recopila la informacion necesaria para generar el reporte de cuadrilla,
        /// donde recorre todas las cuadrillas que cumplen con los filtros dados 
        /// para obtener la informacion correspondiente
        /// </summary>
        /// <returns>el reporte deseado</returns>
        public void ReporteToneladasXCuadrilla(string desde, string hasta, string otr, string cuadrilla, string formato)
        {
            // Se establecen los filtros
            DateTime fechaHasta = Convert.ToDateTime(hasta);
            DateTime fechaDesde = Convert.ToDateTime(desde);
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("IsDeleted", false);
            criteria.Add("Estado", "Cerrado");
            if (cuadrilla != "Todos")
            {
                Cuadrilla cuadrillaActual = cuadrillaService.Get(Convert.ToInt64(cuadrilla));
                criteria.Add("Cuadrilla", cuadrillaActual);
            }
            if (otr != "Todos")
            {
                criteria.Add("Id", Convert.ToInt64(otr));
            }

            IList<OTR> ListaOTR = otrService.GetAll(criteria);
            ListaOTR = ListaOTR.Where(s => s.Fecha > fechaDesde.AddDays(-1) && s.Fecha <= fechaHasta.AddDays(1)).ToList();
            ListaOTR = ListaOTR.OrderBy(s => s.Fecha).ToList();

            // Se indica el data set a utilizar
            ds_CuadrillaPorTonelada dsCierre = new ds_CuadrillaPorTonelada();
            var dtReporte = dsCierre.Tables["Reporte"];
            var dtDatos = dsCierre.Tables["Datos"];

            Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
            // Rows DataTable Reporte
            DataRow row = dtReporte.NewRow();
            row["Compania"] = compania.Nombre;
            row["FechaActual"] = DateTime.Now;
            dtReporte.Rows.Add(row);

            foreach (var item in ListaOTR)
            {
                // Rows DataTable Datos
                DataRow dato = dtDatos.NewRow();
                criteria = new Dictionary<string, object>();
                criteria.Add("IsDeleted", false);
                criteria.Add("OTR", item);
                Bascula bascula = basculaService.Get(criteria);

                if (bascula != null)
                {
                    dato["Boleta"] = bascula.Boleta;
                    dato["Toneladas"] = bascula.PesoNeto;
                }
                else
                {
                    criteria = new Dictionary<string, object>();
                    criteria.Add("IsDeleted", false);
                    criteria.Add("OTR", item);

                    var boleta = boletaService.Get(criteria);
                    dato["Boleta"] = boleta.NumeroBoleta;
                    dato["Toneladas"] = boleta.PesoNeto;
                }

             //   dato["Equipo"] = item.Equipo.Placa;
                var chofer = item.Cuadrilla.ListaEmpleados.Where(s => s.Puesto == "Chofer").FirstOrDefault();
                dato["Chofer"] = chofer.Nombre;
                dato["OTR"] = item.Consecutivo;
                dato["Cuadrilla"] = item.Cuadrilla.Descripcion;
                dato["Ruta"] = item.RutaRecoleccion.Descripcion;
                dato["Fecha"] = item.Fecha;
                var peones = item.Cuadrilla.ListaEmpleados.Where(s => s.Puesto != "Chofer");
                String ListaPeones = String.Empty;
                foreach (var peon in peones)
                {
                    ListaPeones += ListaPeones == String.Empty ? peon.Nombre : ", " + peon.Nombre;
                }
                dato["Peones"] = ListaPeones;

                dtDatos.Rows.Add(dato);
            }

            // Se genera el reporte deseado
            ReportDocument rd = new ReportDocument();
            string strRptPath = String.Empty;
            strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//Recoleccion//rpt_CuadrillaXTonelada.rpt";
            rd.Load(strRptPath);

            rd.SetDataSource(dsCierre);// Se asigna el dataset al datasorce del reporte
            var tipoFormato = formato == "pdf" ? ExportFormatType.PortableDocFormat : ExportFormatType.Excel;
            rd.ExportToHttpResponse(tipoFormato, System.Web.HttpContext.Current.Response, false, "ControlOperaciones_OTR");

        }

        /// <summary>
        /// Este método carga la información de las compañías existentes 
        /// para poder seleccionar de cual compañía en especifico se desea la ruta de recoleccion
        /// y retorna la vista indicada para obtener los filtros deseados para generar el reporte
        /// </summary>
        /// <returns>La vista RutaRecoleccion</returns>
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_REP_RUTAS_RECOLECCION + "," + WPPConstants.ROLES_REPORTES_GERENCIA)]
        public ActionResult RutaRecoleccion()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("IsDeleted", false);
            ViewBag.ListaCompania = companiaMapper.GetListaCompaniaModel(companiaService.GetAll(criteria));
            return View();
        }

        /// <summary>
        /// Este metodo recopila la informacion necesaria para generar el reporte de rutas de recoleccioin
        /// para obtener la informacion correspondiente
        /// </summary>
        /// <returns>el reporte deseado</returns>
        public void ReporteRutaRecoleccion(string descripcion, string tipo, bool agrupar, string formato, string compania)
        {
            Compania companiaActual = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
            
            // Se establecen los filtros
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("IsDeleted", false);
            //criteria.Add("Compania", companiaActual);
            if (tipo != "Todos")
            {
                criteria.Add("Tipo",tipo);
            }
            if (compania != String.Empty)
            {
                if (compania != "Todos")
                {
                    companiaActual = companiaService.Get(Convert.ToInt64(compania));
                    criteria.Add("Compania", companiaActual);
                }
            }
            else
            {
                criteria.Add("Compania", companiaActual);
            }
            IList<RutaRecoleccion> ListaRutas = rutaRecoleccionService.GetAll(criteria);
            if(descripcion != String.Empty)
            {
                ListaRutas = ListaRutas.Where(s => s.Descripcion.ToUpper().Contains(descripcion.ToUpper())).ToList();
            }

            // Se indica el data set a utilizar
            ds_RutaRecoleccion dsRuta = new ds_RutaRecoleccion();
            var dtReporte = dsRuta.Tables["Reporte"];
            var dtDatos = dsRuta.Tables["Datos"];

            // Rows DataTable Reporte
            DataRow row = dtReporte.NewRow();
            row["Compania"] = companiaActual.Nombre;
            row["FechaActual"] = DateTime.Now;
            dtReporte.Rows.Add(row);

            foreach (var ruta in ListaRutas)
            {
                if(agrupar)
                {
                    foreach (var item in ruta.Rutas)
                    {
                        // Rows DataTable Datps
                        DataRow dato = dtDatos.NewRow();
                        dato["Tipo"] = ruta.Tipo;
                        dato["Descripcion"] = ruta.Descripcion;
                        dato["Producto"] = item.Descripcion;
                        dato["Cliente"] = item.Contrato.Cliente.Numero + " - " + item.Contrato.Cliente.Nombre;
                        dato["Contrato"] = item.Contrato.Numero + " - " + item.Contrato.DescripcionContrato;
                        dato["Ubicacion"] = item.Ubicacion == null ? String.Empty : item.Ubicacion.Descripcion;
                        dato["Compania"] = ruta.Compania.Nombre;
                        dtDatos.Rows.Add(dato);                        
                    }
                }
                else
                {
                    DataRow dato = dtDatos.NewRow();
                    dato["Tipo"] = ruta.Tipo;
                    dato["Descripcion"] = ruta.Descripcion;
                    dato["Compania"] = ruta.Compania.Nombre;
                    dtDatos.Rows.Add(dato);
                }
            }

            // Se genera el reporte deseado
            ReportDocument rd = new ReportDocument();
            string strRptPath = String.Empty;
            if (agrupar)
            {
                strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//Recoleccion//rpt_RutaRecoleccionDetalle.rpt";            
            }
            else
            {
                strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//Recoleccion//rpt_RutaRecoleccionXTipo.rpt";    
            }
            rd.Load(strRptPath);

            rd.SetDataSource(dsRuta); // Se asigna el dataset al datasorce del reporte
            var tipoFormato = formato == "pdf" ? ExportFormatType.PortableDocFormat : ExportFormatType.Excel;
            rd.ExportToHttpResponse(tipoFormato, System.Web.HttpContext.Current.Response, false, "ControlOperaciones_RutaRecoleccion");

        }

        /// <summary>
        /// retorna la vista indicada para obtener los filtros deseados para generar el reporte
        /// </summary>
        /// <returns>La vista CategoriaProductos</returns>
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_REP_CATEGORIA_PRODUCTOS + "," + WPPConstants.ROLES_REPORTES_GERENCIA)]
        public ActionResult CategoriaProductos()
        {
            return View();
        }

        /// <summary>
        /// Este metodo recopila la informacion necesaria para generar el reporte de cateogrias de productos
        /// para obtener la informacion correspondiente
        /// </summary>
        /// <returns>el reporte deseado</returns>
        public void ReporteCategoriaProducto(string descripcion, string tipo, string formato)
        {
            Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
            
            // Se establecen los filtros
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("IsDeleted", false);
            //criteria.Add("Compania", compania);
            if (tipo != "Todos")
            {
                criteria.Add("Tipo", tipo);
            }

            IList<CategoriaProducto> ListaCategorias = categoriaProductoService.GetAll(criteria);
            if (descripcion != String.Empty)
            {
                ListaCategorias = ListaCategorias.Where(s => s.Nombre.ToUpper().Contains(descripcion.ToUpper())).ToList();
            }

            // Se indica el data set aa utilizar
            ds_RutaRecoleccion dsRuta = new ds_RutaRecoleccion();
            var dtReporte = dsRuta.Tables["Reporte"];
            var dtDatos = dsRuta.Tables["Datos"];

            // Rows DataTable Reporte
            DataRow row = dtReporte.NewRow();
            row["Compania"] = compania.Nombre;
            row["FechaActual"] = DateTime.Now;
            dtReporte.Rows.Add(row);

            foreach (var ruta in ListaCategorias)
            {
                // Rows DataTable Datos
                DataRow dato = dtDatos.NewRow();
                dato["Tipo"] = ruta.Tipo;
                dato["Descripcion"] = ruta.Nombre;
                dtDatos.Rows.Add(dato);                
            }

            // Se genera el reporte deseado
            ReportDocument rd = new ReportDocument();
            string strRptPath = String.Empty;
            strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//Contratos//rpt_CategoriaProductos.rpt";
            
            rd.Load(strRptPath);

            rd.SetDataSource(dsRuta);// Se asigna el dataset al datasorce del reporte
            var tipoFormato = formato == "pdf" ? ExportFormatType.PortableDocFormat : ExportFormatType.Excel;
            rd.ExportToHttpResponse(tipoFormato, System.Web.HttpContext.Current.Response, false, "ControlOperaciones_CategoriaProductos");
        }
        
        #endregion

        #region ADMINISTRACION

        /// <summary>
        /// retorna la vista indicada para obtener los filtros deseados para generar el reporte
        /// </summary>
        /// <returns>La vista Usuarios</returns>
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_REP_USUARIOS + "," + WPPConstants.ROLES_REPORTES_GERENCIA)]
        public ActionResult Usuarios()
        {
            //IDictionary<string, object> criteria = new Dictionary<string, object>();
            //criteria.Add("IsDeleted", false);
            //ViewBag.ListaCompania =  companiaMapper.GetListaCompaniaModel(companiaService.GetAll(criteria));
            return View();
        }

        /// <summary>
        /// Este metodo recopila la informacion necesaria para generar el reporte de usuarios
        /// para obtener la informacion correspondiente
        /// </summary>
        /// <returns>el reporte deseado</returns>
        public void ReporteUsuarios(string email, string roles, string formato)
        {
            // Se establecen los filtros
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("IsDeleted", false);

            IList<Usuario> ListaUsuario = new List<Usuario>();
            ListaUsuario = usuarioService.GetAll(criteria);
            if(email != String.Empty)
            {
                ListaUsuario = ListaUsuario.Where(s => s.Email.ToUpper() == email.ToUpper()).ToList();
            }
            if (roles != "Todos")
            {
                ListaUsuario = ListaUsuario.Where(s => s.Roles.ToUpper().Contains(roles.ToUpper())).ToList();
            }

            // Se indica el data set a utilizar
            ds_Usuarios dsCierre = new ds_Usuarios();
            var dtReporte = dsCierre.Tables["Reporte"];
            var dtDatos = dsCierre.Tables["Datos"];

            Compania companiaActual = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
            
            // Rows DataTable Reporte
            DataRow row = dtReporte.NewRow();
            row["Compania"] = companiaActual.Nombre;
            row["FechaActual"] = DateTime.Now;
            dtReporte.Rows.Add(row);

            foreach (var usuario in ListaUsuario)
            {
                // Rows DataTable Datos
                DataRow dato = dtDatos.NewRow();
               
                dato["Nombre"] = usuario.Nombre;
                dato["Email"] = usuario.Email;
                dato["Identificacion"] = usuario.Cedula;
                dato["Telefono"] = usuario.Telefono;
                dato["NumeroEmpleado"] = usuario.NumeroEmpleado;

                String companias = String.Empty;                
                foreach (var item in usuario.Companias)
                {                     
                    companias += companias == String.Empty ?  item.Nombre : ", " + item.Nombre;                    
                }
                dato["Companias"] = companias;
                dato["Rol"] = usuario.Roles.Replace(",", ", ");     

                dtDatos.Rows.Add(dato);  
            }

            // Se genera el reporte deseado
            ReportDocument rd = new ReportDocument();
            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//Administracion//rpt_Usuarios.rpt";
            rd.Load(strRptPath);

            rd.SetDataSource(dsCierre);// Se asigna el dataset al datasorce del reporte
            var tipoFormato = formato == "pdf" ? ExportFormatType.PortableDocFormat : ExportFormatType.Excel;
            rd.ExportToHttpResponse(tipoFormato, System.Web.HttpContext.Current.Response, false, "ControlOperaciones_Usuarios");
        }

        /// <summary>
        /// retorna la vista indicada para obtener los filtros deseados para generar el reporte
        /// </summary>
        /// <returns>La vista ContratoDetalle</returns>
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_REP_BITACORA_CLIENTE + "," + WPPConstants.ROLES_REPORTES_GERENCIA)]
        public ActionResult ClienteBitacora()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("IsDeleted", false);
            criteria.Add("Tipo", "TipoCompania");
            ViewBag.ListaTipo = catalogoMapper.GetListaCatalogoModel(catalogoService.GetAll(criteria));
            return View();
        }

        /// <summary>
        /// Este metodo recopila la informacion necesaria para generar el reporte de rutas de clientes
        /// para obtener la informacion correspondiente
        /// </summary>
        /// <returns>el reporte deseado</returns>
        public void ReporteClienteBitacora(string nombre, string numero, string tipo, string tipoReporte, string formato)
        {
            try
            {
                // Se establecen los filtros
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                if (numero != String.Empty)
                {
                    criteria.Add("Numero", Convert.ToInt64(numero));
                }
                if (tipo != "Todos")
                {
                    Catalogo catalogo = catalogoService.Get(Convert.ToInt64(tipo));
                    criteria.Add("Tipo", catalogo);
                }
                criteria.Add("IsDeleted", false);

                IList<Cliente> ListaCliente = clienteService.GetAll(criteria);
                if (nombre != String.Empty)
                {
                    ListaCliente = ListaCliente.Where(s => s.Nombre.ToUpper().Contains(nombre.ToUpper())).ToList();
                }

                ds_ClienteBitacora dscliente = new ds_ClienteBitacora();
                var dtReporte = dscliente.Tables["Reporte"];
                var dtDatos = dscliente.Tables["Datos"];

                Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);

                // Rows DataTable Reporte
                DataRow row = dtReporte.NewRow();
                row["Compania"] = compania.Nombre;
                row["Fecha"] = DateTime.Now;

                dtReporte.Rows.Add(row);

                foreach (var cliente in ListaCliente)
                {
                    // Rows DataTable Datos

                    criteria = new Dictionary<string, object>();
                    criteria.Add("Cliente", cliente);
                    var bitacora = clienteBitacoraService.GetAll(criteria);
                    foreach (var item in bitacora)
                    {
                        DataRow dato = dtDatos.NewRow();
                        dato["FechaCambio"] = item.CreateDate; 
                        dato["IdCliente"] = item.Cliente.Id;
                        dato["Cliente"] = item.Cliente.Numero + " - " + item.Cliente.Nombre;
                        dato["Campo"] = item.Campo;
                        dato["ValorAnterior"] = item.valorAnterior == null || item.valorAnterior == String.Empty ? "" : item.valorAnterior;
                        dato["ValorActual"] = item.valorNuevo == null || item.valorNuevo == String.Empty ? "" : item.valorNuevo;
                        dato["Responsable"] = item.CreatedBy;

                        dtDatos.Rows.Add(dato);
                    }
                }

                // Se genera el reporte deseado
                ReportDocument rd = new ReportDocument();
                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//Administracion//rpt_ClienteBitacora.rpt";

                rd.Load(strRptPath);

                rd.SetDataSource(dscliente);// Se asigna el dataset al datasorce del reporte
                var tipoFormato = formato == "pdf" ? ExportFormatType.PortableDocFormat : ExportFormatType.Excel;
                rd.ExportToHttpResponse(tipoFormato, System.Web.HttpContext.Current.Response, false, "ControlOperaciones_BitacoraClientes");
            }
            catch (Exception ex)
            {
                string msj = ex.Message;
            }
        }

        /// <summary>
        /// retorna la vista indicada para obtener los filtros deseados para generar el reporte
        /// </summary>
        /// <returns>La vista Cliente</returns>
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_REP_CLIENTES + "," + WPPConstants.ROLES_REPORTES_GERENCIA)]
        public ActionResult Cliente()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("IsDeleted", false);
            criteria.Add("Tipo", "TipoCompania");
            ViewBag.ListaTipo =  catalogoMapper.GetListaCatalogoModel(catalogoService.GetAll(criteria));
            return View();
        }

        /// <summary>
        /// Este metodo recopila la informacion necesaria para generar el reporte de rutas de clientes
        /// para obtener la informacion correspondiente
        /// </summary>
        /// <returns>el reporte deseado</returns>
        public void ReporteCliente(string nombre, string numero, string tipo, string tipoReporte, string formato)
        {
            // Se establecen los filtros
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            if(numero != String.Empty)
            {
                criteria.Add("Numero", Convert.ToInt64(numero));
            }
            if (tipo != "Todos")
            {
                Catalogo catalogo = catalogoService.Get(Convert.ToInt64(tipo));
                criteria.Add("Tipo", catalogo);
            }
            criteria.Add("IsDeleted", false);

            IList<Cliente> ListaCliente = clienteService.GetAll(criteria);
            if(nombre != String.Empty)
            {
                ListaCliente = ListaCliente.Where(s => s.Nombre.ToUpper().Contains(nombre.ToUpper())).ToList();
            }
           
            // Se identifica el tipo de reporte (General / Detallado)
            if (tipoReporte == "General")
            {
                ReporteClienteGeneral(ListaCliente, formato);
            }
            else
            {
                ReporteClienteDetallado(ListaCliente, formato);
            }
        }

        /// <summary>
        /// Este metodo recopila la informacion necesaria para generar el reporte de clientes General
        /// para obtener la informacion correspondiente
        /// </summary>
        /// <returns>el reporte deseado</returns>
        public void ReporteClienteGeneral(IList<Cliente> ListaCliente, string formato)
        {
            // Se indica el data set a utilizar
            ds_ClienteGeneral dscliente = new ds_ClienteGeneral();
            var dtReporte = dscliente.Tables["Reporte"];
            var dtDatos = dscliente.Tables["Datos"];

            Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);

            // Rows DataTable Reporte
            DataRow row = dtReporte.NewRow();
            row["Compania"] = compania.Nombre;
            row["FechaActual"] = DateTime.Now;
           
            dtReporte.Rows.Add(row);

            foreach (var cliente in ListaCliente)
            {
                // Rows DataTable Datos
                DataRow dato = dtDatos.NewRow();

                dato["Nombre"] = cliente.Nombre;
                dato["NombreCorto"] = cliente.NombreCorto;
                dato["Numero"] = cliente.Numero == null ? 0 : cliente.Numero;
                dato["Cedula"] = cliente.Cedula;
                dato["Tipo"] = cliente.Tipo.Nombre;
                dato["Email"] = cliente.Email;
                dato["Telefono"] = cliente.Telefono1;        
                
                dtDatos.Rows.Add(dato);
            }

            // Se genera el reporte deseado
            ReportDocument rd = new ReportDocument();
            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//Administracion//rpt_ClienteGeneral.rpt";
          
            rd.Load(strRptPath);

            rd.SetDataSource(dscliente);// Se asigna el dataset al datasorce del reporte
            var tipoFormato = formato == "pdf" ? ExportFormatType.PortableDocFormat : ExportFormatType.Excel;
            rd.ExportToHttpResponse(tipoFormato, System.Web.HttpContext.Current.Response, false, "ControlOperaciones_Clientes");
        }

        /// <summary>
        /// Este metodo recopila la informacion necesaria para generar el reporte de clientes Detallado
        /// para obtener la informacion correspondiente
        /// </summary>
        /// <returns>el reporte deseado</returns>
        public void ReporteClienteDetallado(IList<Cliente> ListaCliente, string formato)
        {
            //Se indica el data set a utilizar
            ds_ClienteDetalle dsCliente = new ds_ClienteDetalle();
            ds_Contactos dsContactos = new ds_Contactos();
            ds_Ubicacion dsUbicaciones = new ds_Ubicacion();

            var dtReporte = dsCliente.Tables["Reporte"];
            var dtDatos = dsCliente.Tables["Datos"];

            var dtContactos = dsContactos.Tables["Datos"];
            var dtUbicaciones = dsUbicaciones.Tables["Datos"];

            Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);

            // Rows DataTable Reporte
            DataRow row = dtReporte.NewRow();
            row["Compania"] = compania.Nombre;
            row["FechaActual"] = DateTime.Now;          
            dtReporte.Rows.Add(row);

            // Rows DataTable Datos
            foreach (Cliente cliente in ListaCliente)
            {
                DataRow dato = dtDatos.NewRow();
                dato["Nombre"] = cliente.Nombre;
                dato["NombreCorto"] = cliente.NombreCorto;
                dato["Numero"] = cliente.Numero == null ? 0 : cliente.Numero;
                dato["Cedula"] = cliente.Cedula;
                dato["Email"] = cliente.Email;
                dato["Fax"] = cliente.Fax;
                dato["Telefono1"] = cliente.Telefono1;
                dato["Telefono2"] = cliente.Telefono2;
                dato["Tipo"] = cliente.Tipo.Nombre;
                dato["RepresentanteLegal"] = cliente.RepresentanteLegal;
                dato["Direccion"] = cliente.Direccion;
                dato["Provincia"] = cliente.Provincia == null ? "" : cliente.Provincia.Nombre;
                dato["Canton"] = cliente.Canton == null ? "" : cliente.Canton.Nombre;
                dato["Distrito"] = cliente.Distrito == null ? "" : cliente.Distrito.Nombre;
                dato["IdCliente"] = cliente.Id;       
                dtDatos.Rows.Add(dato);

                foreach (var contacto in cliente.Contactos)
                {
                    // Rows DataTable Datos (SubReporte Contactos)
                    DataRow datosContactos = dtContactos.NewRow();
                    datosContactos["Nombre"] = contacto.Nombre;
                    datosContactos["Email"] = contacto.Email;
                    datosContactos["Telefono1"] = contacto.Telefono1;
                    datosContactos["Ext1"] = contacto.Ext1;
                    datosContactos["Telefono2"] = contacto.Telefono2;
                    datosContactos["Ext2"] = contacto.Ext2;
                    datosContactos["Cedula"] = contacto.Cedula;
                    datosContactos["IdCliente"] = cliente.Id;       
                    dtContactos.Rows.Add(datosContactos);                    
                }

                foreach (var ubicacion in cliente.Ubicaciones)
                {
                    // Rows DataTable Datos (SubReporte Ubicaciones)
                    DataRow datosUbicaciones = dtUbicaciones.NewRow();
                    datosUbicaciones["Descripcion"] = ubicacion.Descripcion;
                    datosUbicaciones["Contacto"] = ubicacion.Contacto;
                    datosUbicaciones["Telefono"] = ubicacion.Telefono;
                    datosUbicaciones["Email"] = ubicacion.Email;
                    datosUbicaciones["Direccion"] = ubicacion.Direccion;
                    datosUbicaciones["IdCliente"] = cliente.Id;       
                    dtUbicaciones.Rows.Add(datosUbicaciones);
                }
                
            }

            // Se genera el reporte deseado
            ReportDocument rd = new ReportDocument();
            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//Administracion//rpt_ClienteDetalle.rpt";
            rd.Load(strRptPath);

            rd.SetDataSource(dsCliente);// Se asigna el dataset al datasorce del reporte
            rd.Subreports[0].SetDataSource(dsContactos); // Se asignan los respectivos dataset a los subreportes
            rd.Subreports[1].SetDataSource(dsUbicaciones);
            var tipoFormato = formato == "pdf" ? ExportFormatType.PortableDocFormat : ExportFormatType.Excel;
            rd.ExportToHttpResponse(tipoFormato, System.Web.HttpContext.Current.Response, false, "ControlOperaciones_ClientesDetallado");

        }

        /// <summary>
        /// retorna la vista indicada para obtener los filtros deseados para generar el reporte
        /// </summary>
        /// <returns>La vista Compania</returns>
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_REP_COMPANIAS + "," + WPPConstants.ROLES_REPORTES_GERENCIA)]
        public ActionResult Compania()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("IsDeleted", false);
            criteria.Add("Tipo", "TipoCompania");
            ViewBag.ListaTipo = catalogoMapper.GetListaCatalogoModel(catalogoService.GetAll(criteria));
            return View();
        }

        /// <summary>
        /// Este metodo recopila la informacion necesaria para generar el reporte de companias
        /// para obtener la informacion correspondiente
        /// </summary>
        /// <returns>el reporte deseado</returns>
        public void ReporteCompania(string nombre, string tipo, string tipoReporte, string formato)
        {
            // Se establecen los filtros
            IDictionary<string, object> criteria = new Dictionary<string, object>();          
            if (tipo != "Todos")
            {
                Catalogo catalogo = catalogoService.Get(Convert.ToInt64(tipo));
                criteria.Add("Tipo", catalogo);
            }
            criteria.Add("IsDeleted", false);

            IList<Compania> ListaCompania = companiaService.GetAll(criteria);
            if (nombre != String.Empty)
            {
                ListaCompania = ListaCompania.Where(s => s.Nombre.ToUpper().Contains(nombre.ToUpper())).ToList();
            }

            // Se indica el data set a utilizar
            ds_Compania dscompania = new ds_Compania();
            var dtReporte = dscompania.Tables["Reporte"];
            var dtDatos = dscompania.Tables["Datos"];

            Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);

            // Rows DataTable Reporte
            DataRow row = dtReporte.NewRow();
            row["Compania"] = compania.Nombre;
            row["FechaActual"] = DateTime.Now;

            dtReporte.Rows.Add(row);

            foreach (var item in ListaCompania)
            {
                // Rows DataTable Datos
                DataRow dato = dtDatos.NewRow();

                dato["Nombre"] = item.Nombre;
                dato["NombreCorto"] = item.NombreCorto;
                dato["Cedula"] = item.Cedula;
                dato["Tipo"] = item.Tipo.Nombre;
                dato["Email"] = item.Email;
                dato["Telefono"] = item.Telefono;
                dato["RepresentanteLegal"] = item.RepresentanteLegal;

                dtDatos.Rows.Add(dato);
            }

            // Se genera el reporte deseado
            ReportDocument rd = new ReportDocument();
            string strRptPath = String.Empty;
            if (tipoReporte == "General")
            {
                strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//Administracion//rpt_CompaniaGeneral.rpt";
            }
            else
            {
                strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//Administracion//rpt_CompaniaDetalle.rpt";
            }

            rd.Load(strRptPath);

            rd.SetDataSource(dscompania);// Se asigna el dataset al datasorce del reporte
            var tipoFormato = formato == "pdf" ? ExportFormatType.PortableDocFormat : ExportFormatType.Excel;
            rd.ExportToHttpResponse(tipoFormato, System.Web.HttpContext.Current.Response, false, "ControlOperaciones_Compañías");
           
        }

        #endregion

        #region BASCULA

        /// <summary>
        /// retorna la vista indicada para obtener los filtros deseados para generar el reporte
        /// </summary>
        /// <returns>La vista ToneladasXBoleta</returns>
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_REP_TONELADAS_BOLETA + "," + WPPConstants.ROLES_REPORTES_GERENCIA)]
        public ActionResult ToneladasXBoleta()
        {
            return View();
        }

        /// <summary>
        /// Este metodo recopila la informacion necesaria para generar el reporte de toneladas por boleta
        /// para obtener la informacion correspondiente
        /// </summary>
        /// <returns>el reporte deseado</returns>
        public void ReporteToneladasXBoleta(string contrato, string desde, string hasta,  string formato)
        {
            // Se establecen los filtros
            DateTime fechaHasta = Convert.ToDateTime(hasta);
            DateTime fechaDesde = Convert.ToDateTime(desde);
            
            long compania = Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0;
            Compania companiaActual = companiaService.Get(compania);
            IList<DetalleFacturacion> ListaDetalleFacturacion = new List<DetalleFacturacion>();
            Contrato contratoActual = contratoService.Get(Convert.ToInt64(contrato));

            // Se indica el data set a utilizar
            ds_ToneladasXBoleta dsCierre = new ds_ToneladasXBoleta();
            var dtReporte = dsCierre.Tables["Reporte"];
            var dtDatos = dsCierre.Tables["Datos"];

             // Rows DataTable Reporte
            DataRow row = dtReporte.NewRow();
            row["Compania"] = companiaActual.Nombre;
            row["FechaActual"] = DateTime.Now;
            row["Cliente"] = contratoActual.Cliente.Numero + " - " + contratoActual.Cliente.Nombre;
            row["Contrato"] = contratoActual.Numero + " - " + contratoActual.DescripcionContrato;
            row["Moneda"] = contratoActual.Moneda;
            dtReporte.Rows.Add(row);

            foreach (ProductoContrato servicio in contratoActual.Productos)
            {
                // Rows DataTable Datos
                DataRow datos = dtDatos.NewRow();
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                switch (servicio.Producto.Categoria.Tipo)
                {
                    case "Recoleccion": // servicios de Recolección (Carga Trasera / Roll-Off), se obtiene la información por medio de OTRs
                        {
                            criteria = new Dictionary<string, object>();
                            criteria.Add("Compania", servicio.Proyecto);
                            criteria.Add("IsDeleted", false);

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
                               
                                //detalle.Producto = servicio;
                                //dtDatos["OTR"] = itemOTR;
                                //detalle.Periodo = "Del " + desde + " al " + hasta;
                                datos = dtDatos.NewRow();
                                datos["OTR"] = itemOTR.Consecutivo;
                                datos["Descripcion"] = servicio.Descripcion;
                                datos["Fecha"] = itemOTR.Fecha;
                                datos["Placa"] = itemOTR.Equipo.Nombre;
                                datos["PrecioUnidad"] = servicio.Total;
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

                                            // Se obtienen todas las boletas con las condiciones necesarias en un rango de fechas dado
                                            Bascula BoletaOTR = basculaService.Get(criteriaBascula);
                                            if (BoletaOTR.Id == 0) // Si la boleta de bascula de esta OTR se realizo de manera manual
                                            {
                                                BoletaManual boletaManual = boletaService.Get(criteriaBascula);
                                                datos["Boleta"] = boletaManual.NumeroBoleta;
                                                datos["PesoBruto"] = boletaManual.PesoBruto;
                                                datos["PesoTara"] = boletaManual.PesoTara;
                                                datos["PesoNeto"] = boletaManual.PesoNeto;
                                                Peso = (boletaManual.PesoNeto / 1000);
                                                Monto = (Peso > 1 ? Peso : 1) * servicio.Monto;
                                            }
                                            else
                                            {
                                                datos["Boleta"] = BoletaOTR.Boleta;
                                                datos["PesoBruto"] = BoletaOTR.PesoBruto;
                                                datos["PesoTara"] = BoletaOTR.PesoTara;
                                                datos["PesoNeto"] = BoletaOTR.PesoNeto;
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

                                            if (diasxCobrar <= 31 && diasxCobrar >= 28)
                                            {
                                                Monto = servicio.Monto;
                                            }
                                            else
                                            {
                                                Monto = diasxCobrar * precioxDia;
                                            }

                                            IDictionary<string, object> criteriaBascula = new Dictionary<string, object>();
                                            criteriaBascula.Add("OTR", itemOTR);
                                            criteriaBascula.Add("Estado", "Activo");
                                            criteriaBascula.Add("IsDeleted", false);

                                            Bascula BoletaOTR = basculaService.Get(criteriaBascula);

                                            if (BoletaOTR == null) // Si la boleta de bascula de esta OTR se realizo de manera manual
                                            {
                                                BoletaManual boletaManual = boletaService.Get(criteriaBascula);
                                                if (boletaManual != null)
                                                {
                                                    datos["Boleta"] = boletaManual.NumeroBoleta;
                                                    datos["PesoBruto"] = boletaManual.PesoBruto;
                                                    datos["PesoTara"] = boletaManual.PesoTara;
                                                    datos["PesoNeto"] = boletaManual.PesoNeto;
                                                }
                                            }
                                            else
                                            {
                                                datos["Boleta"] = BoletaOTR.Boleta;
                                                datos["PesoBruto"] = BoletaOTR.PesoBruto;
                                                datos["PesoTara"] = BoletaOTR.PesoTara;
                                                datos["PesoNeto"] = BoletaOTR.PesoNeto;
                                            }
                                        }
                                        break;

                                    case "Roll-Off":
                                        {
                                            IDictionary<string, object> criteriaBascula = new Dictionary<string, object>();
                                            criteriaBascula.Add("OTR", itemOTR);
                                            criteriaBascula.Add("Estado", "Activo");
                                            criteriaBascula.Add("IsDeleted", false);

                                            // Se obtienen todas las boletas con las condiciones necesarias en un rango de fechas dado
                                            Bascula BoletaOTR = basculaService.Get(criteriaBascula);

                                            if (BoletaOTR == null) // Si la boleta de bascula de esta OTR se realizo de manera manual
                                            {
                                                BoletaManual boletaManual = boletaService.Get(criteriaBascula);
                                                if (boletaManual != null)
                                                {
                                                    datos["Boleta"] = boletaManual.NumeroBoleta;
                                                    datos["PesoBruto"] = boletaManual.PesoBruto;
                                                    datos["PesoTara"] = boletaManual.PesoTara;
                                                    datos["PesoNeto"] = boletaManual.PesoNeto;
                                                    Peso = (boletaManual.PesoNeto / 1000);
                                                    Monto = (Peso > 1 ? Peso : 1) * servicio.Monto;
                                                }
                                            }
                                            else
                                            {
                                                datos["Boleta"] = BoletaOTR.Boleta;
                                                datos["PesoBruto"] = BoletaOTR.PesoBruto;
                                                datos["PesoTara"] = BoletaOTR.PesoTara;
                                                datos["PesoNeto"] = BoletaOTR.PesoNeto;
                                                Peso = (BoletaOTR.PesoNeto / 1000);
                                                Monto = (Peso > 1 ? Peso : 1) * servicio.Monto;
                                            }
                                        }
                                        break;
                                }
                                if (contratoActual.Moneda == "Colones")
                                {
                                    datos["PrecioTotal"] = Monto;
                                }
                                else
                                {
                                    // Si el contrato esta en dólares pero se desea facturar en colones, 
                                    // se debe realizar la conversión de moneda con el tipo de cambio actual
                                    if ((bool)contratoActual.FacturarColones)
                                    {
                                        TipoCambio tipoCambio = new TipoCambio();
                                        IDictionary<string, object> criteriaTipoCambio = new Dictionary<string, object>();
                                        criteriaTipoCambio.Add("Fecha", DateTime.Now.ToString("dd/MM/yyyy"));
                                        criteriaTipoCambio.Add("Tipo", "Venta");
                                        tipoCambio = tipoCambioService.Get(criteriaTipoCambio);
                                        datos["PrecioTotal"] = (Monto * tipoCambio.Valor);
                                       // detalle.Moneda = "Colones";
                                    }
                                    else
                                    {
                                        datos["PrecioTotal"] = Monto;
                                    }
                                }

                                dtDatos.Rows.Add(datos);
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

                            if (diasxCobrar <= 31 && diasxCobrar >= 28)
                            {
                                Monto = servicio.Monto;
                            }
                            else
                            {
                                Monto = diasxCobrar * precioxDia;
                            }

                            //Se crea el detalle de la factura correspondiente a este servicio
                            datos = dtDatos.NewRow();
                            datos["Descripcion"] = servicio.Descripcion;
                            datos["PrecioUnidad"] = servicio.Total;
                          //  detalle.Periodo = "Del " + desde + " al " + hasta;
                         //   detalle.Moneda = contrato.Moneda;
                            if (contratoActual.Moneda == "Colones")
                            {
                                datos["PrecioTotal"] = Monto;
                            }
                            else
                            {
                                // Si el contrato esta en dólares pero se desea facturar en colones, 
                                // se debe realizar la conversión de moneda con el tipo de cambio actual
                                if ((bool)contratoActual.FacturarColones)
                                {
                                    TipoCambio tipoCambio = new TipoCambio();
                                    IDictionary<string, object> criteriaTipoCambio = new Dictionary<string, object>();
                                    criteriaTipoCambio.Add("Fecha", DateTime.Now.ToString("dd/MM/yyyy"));
                                    criteriaTipoCambio.Add("Tipo", "Venta");
                                    tipoCambio = tipoCambioService.Get(criteriaTipoCambio);
                                     datos["PrecioTotal"] = (Monto * tipoCambio.Valor);
                                    //detalle.Moneda = "Colones";
                                }
                                else
                                {
                                    datos["PrecioTotal"] = Monto;
                                }
                            }

                            dtDatos.Rows.Add(datos);
                        }
                        break;

                    default: // Servicios de Fosas, Destrucción y Disposición Final, se obtienen con báscula

                        criteria = new Dictionary<string, object>();
                        criteria.Add("Producto", servicio);
                        criteria.Add("Contrato", contratoActual);
                        criteria.Add("IsDeleted", false);

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
                                datos = dtDatos.NewRow();
                                datos["Boleta"] = boleta.Boleta;
                                datos["Fecha"] = boleta.Fecha;
                                datos["Descripcion"] = servicio.Descripcion;
                                datos["PesoBruto"] = boleta.PesoBruto;
                                datos["PesoNeto"] = boleta.PesoNeto;
                                datos["PesoTara"] = boleta.PesoTara;
                                datos["Placa"] = boleta.Equipo.Nombre;
                                datos["PrecioUnidad"] = servicio.Total;
                                //detalle.Periodo = "Del " + desde + " al " + hasta;
                                //detalle.Moneda = contrato.Moneda;
                                if (contratoActual.Moneda == "Colones")
                                {
                                    datos["PrecioTotal"] = boleta.Total;
                                }
                                else
                                {
                                    // Si el contrato esta en dólares pero se desea facturar en colones, 
                                    // se debe realizar la conversión de moneda con el tipo de cambio actual
                                    if ((bool)contratoActual.FacturarColones)
                                    {
                                        TipoCambio tipoCambio = new TipoCambio();
                                        IDictionary<string, object> criteriaTipoCambio = new Dictionary<string, object>();
                                        criteriaTipoCambio.Add("Fecha", DateTime.Now.ToString("dd/MM/yyyy"));
                                        criteriaTipoCambio.Add("Tipo", "Venta");
                                        tipoCambio = tipoCambioService.Get(criteriaTipoCambio);
                                        datos["PrecioTotal"] = (boleta.Total * tipoCambio.Valor);
                                        //detalle.Moneda = "Colones";
                                    }
                                    else
                                    {
                                        datos["PrecioTotal"] = boleta.Total;
                                    }
                                }

                                dtDatos.Rows.Add(datos);
                            }
                        }
                        else
                        { //En caso que este servicio de Disposición este ligado a una OTR 

                            if (servicio.LigadoRecoleccion)
                            {
                                criteria = new Dictionary<string, object>();
                                criteria.Add("Compania", servicio.Proyecto);
                                criteria.Add("IsDeleted", false);

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
                                    datos = dtDatos.NewRow();
                                    datos["Descripcion"] = servicio.Descripcion;
                                    datos["OTR"] = itemOTR.Consecutivo;
                                    datos["Fecha"] = itemOTR.Fecha;
                                    datos["PrecioUnidad"] = servicio.Total;
                                    datos["Placa"] = itemOTR.Equipo.Nombre;
                                   // detalle.Periodo = "Del " + desde + " al " + hasta;
                                   // detalle.Moneda = contrato.Moneda;
                                    double Monto = 0;
                                    double Peso = 0;

                                    IDictionary<string, object> criteriaBascula = new Dictionary<string, object>();
                                    criteriaBascula.Add("OTR", itemOTR);
                                    criteriaBascula.Add("Estado", "Activo");
                                    criteriaBascula.Add("IsDeleted", false);

                                    // Se obtienen todas las boletas con las condiciones necesarias en un rango de fechas dado
                                    Bascula BoletaOTR = basculaService.Get(criteriaBascula);
                                    if (BoletaOTR == null) // Si la boleta de bascula de esta OTR se realizo de manera manual
                                    {
                                        BoletaManual boletaManual = boletaService.Get(criteriaBascula);
                                        datos["Boleta"] = boletaManual.NumeroBoleta;
                                        datos["PesoTara"] = boletaManual.PesoTara;
                                        datos["PesoNeto"] = boletaManual.PesoNeto;
                                        datos["PesoBruto"] = boletaManual.PesoBruto;
                                        Peso = (boletaManual.PesoNeto / 1000);
                                        Monto = (Peso > 1 ? Peso : 1) * servicio.Monto;
                                    }
                                    else
                                    {
                                        datos["Boleta"] = BoletaOTR.Boleta;
                                        datos["PesoTara"] = BoletaOTR.PesoTara;
                                        datos["PesoNeto"] = BoletaOTR.PesoNeto;
                                        datos["PesoBruto"] = BoletaOTR.PesoBruto;
                                        Peso = (BoletaOTR.PesoNeto / 1000);
                                        Monto = (Peso > 1 ? Peso : 1) * servicio.Monto;
                                    }

                                    if (contratoActual.Moneda == "Colones")
                                    {
                                         datos["PrecioTotal"] = Monto;
                                    }
                                    else
                                    {
                                        // Si el contrato esta en dólares pero se desea facturar en colones, 
                                        // se debe realizar la conversión de moneda con el tipo de cambio actual
                                        if ((bool)contratoActual.FacturarColones)
                                        {
                                            TipoCambio tipoCambio = new TipoCambio();
                                            IDictionary<string, object> criteriaTipoCambio = new Dictionary<string, object>();
                                            criteriaTipoCambio.Add("Fecha", DateTime.Now.ToString("dd/MM/yyyy"));
                                            criteriaTipoCambio.Add("Tipo", "Venta");
                                            tipoCambio = tipoCambioService.Get(criteriaTipoCambio);
                                            datos["PrecioTotal"] = (Monto * tipoCambio.Valor);
                                            //detalle.Moneda = "Colones";
                                        }
                                        else
                                        {
                                            datos["PrecioTotal"] = Monto;
                                        }
                                    }

                                    dtDatos.Rows.Add(datos);
                                }
                            }
                        }

                        break;
                }
            }

            // Se genera el reporte deseado
            ReportDocument rd = new ReportDocument();
            string strRptPath = strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//Bascula//rpt_ToneladasXBoleta.rpt";
            
            rd.Load(strRptPath);

            rd.SetDataSource(dsCierre);// Se asigna el dataset al datasorce del reporte
            var tipoFormato = formato == "pdf" ? ExportFormatType.PortableDocFormat : ExportFormatType.Excel;
            rd.ExportToHttpResponse(tipoFormato, System.Web.HttpContext.Current.Response, false, "ToneladasPorBoleta");

        }

        /// <summary>
        /// retorna la vista indicada para obtener los filtros deseados para generar el reporte
        /// </summary>
        /// <returns>La vista Equipos</returns>
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_REP_EQUIPO + "," + WPPConstants.ROLES_REPORTES_GERENCIA)]
        public ActionResult Equipos()
        {
            return View();
        }

        /// <summary>
        /// Este metodo recopila la informacion necesaria para generar el reporte de equipos
        /// para obtener la informacion correspondiente
        /// </summary>
        /// <returns>el reporte deseado</returns>
        public void ReporteEquipos(string placa, string tipo, string formato)
        {
            // Se establecen los filtros
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("IsDeleted", false);

            IList<Equipo> ListaEquipo = new List<Equipo>();
            ListaEquipo = equipoService.GetAll(criteria);
            if (placa != String.Empty)
            {
                ListaEquipo = ListaEquipo.Where(s => s.Placa.ToUpper() == placa.ToUpper()).ToList();
            }
            if (tipo != "Todos")
            {
                ListaEquipo = ListaEquipo.Where(s => s.Tipo.ToUpper().Contains(tipo.ToUpper())).ToList();
            }

            // Se indica el data set a utilizar
            ds_Equipo dsCierre = new ds_Equipo();
            var dtReporte = dsCierre.Tables["Reporte"];
            var dtDatos = dsCierre.Tables["Datos"];

            Compania companiaActual = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
           
            // Rows DataTable Reporte
            DataRow row = dtReporte.NewRow();
            row["Compania"] = companiaActual.Nombre;
            row["FechaActual"] = DateTime.Now;
            dtReporte.Rows.Add(row);

            foreach (var usuario in ListaEquipo)
            {
                // Rows DataTable Datos
                DataRow dato = dtDatos.NewRow();

                dato["Descripcion"] = usuario.Nombre;
                dato["Placa"] = usuario.Placa;
                dato["Peso"] = usuario.Peso;
                dato["Tipo"] = usuario.Tipo;
                dato["Marca"] = usuario.Marca;

                dtDatos.Rows.Add(dato);
            }

            // Se genera el reporte deseado
            ReportDocument rd = new ReportDocument();
            string strRptPath = String.Empty;
            strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//Bascula//rpt_Equipo.rpt";
            rd.Load(strRptPath);

            rd.SetDataSource(dsCierre);// Se asigna el dataset al datasorce del reporte
            var tipoFormato = formato == "pdf" ? ExportFormatType.PortableDocFormat : ExportFormatType.Excel;
            rd.ExportToHttpResponse(tipoFormato, System.Web.HttpContext.Current.Response, false, "ControlOperaciones_Equipos");
        }

        /// <summary>
        /// retorna la vista indicada para obtener los filtros deseados para generar el reporte
        /// </summary>
        /// <returns>La vista Contenedor</returns>
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_REP_CONTENEDOR + "," + WPPConstants.ROLES_REPORTES_GERENCIA)]
        public ActionResult Contenedor()
        {
            return View();
        }

        /// <summary>
        /// Este metodo recopila la informacion necesaria para generar el reporte de contenedores
        /// para obtener la informacion correspondiente
        /// </summary>
        /// <returns>el reporte deseado</returns>
        public void ReporteContenedor(string desde, string hasta, string codigo, string tipo, string formato)
        {
            // Se establecen los filtros
            DateTime fechaHasta = DateTime.Now;
            DateTime fechaDesde = DateTime.Now;
            Compania companiaActual = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
           
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("IsDeleted", false);

            IList<Contenedor> ListaContenedor = new List<Contenedor>();
            ListaContenedor = contenedorService.GetAll(criteria);
            if (codigo != String.Empty)
            {
                ListaContenedor = ListaContenedor.Where(s => s.Codigo.ToUpper().Contains(codigo.ToUpper())).ToList();
            }
            if (desde != String.Empty && hasta != String.Empty)
            {
                fechaHasta = Convert.ToDateTime(hasta);
                fechaDesde = Convert.ToDateTime(desde);
                //ListaContenedor = ListaContenedor.Where(s => s.CreateDate > fechaDesde.AddDays(-1) && s.CreateDate <= fechaHasta.AddDays(1)).ToList();
            }

            // Se indica el data set a utilizar
            ds_Contenedor dsCierre = new ds_Contenedor();
            var dtReporte = dsCierre.Tables["Reporte"];
            var dtDatos = dsCierre.Tables["Datos"];

             // Rows DataTable Reporte
            DataRow row = dtReporte.NewRow();
            row["Compania"] = companiaActual.Nombre;
            row["FechaActual"] = DateTime.Now;
            dtReporte.Rows.Add(row);

            if(tipo == "General")
            {
                foreach (var contenedor in ListaContenedor)
                {
                    // Rows DataTable Datos
                    DataRow dato = dtDatos.NewRow();

                    dato["Descripcion"] = contenedor.Descripcion;
                    dato["Codigo"] = contenedor.Codigo;
                    dato["Peso"] = contenedor.Peso;

                    dtDatos.Rows.Add(dato);
                }
            }
            else
            {
                foreach (var contenedor in ListaContenedor)
                {
                    criteria = new Dictionary<string, object>();
                    criteria.Add("Contenedor", contenedor);
                    var Lista = viajeOTRServie.GetAll(criteria).OrderByDescending(s => s.OTR.Fecha).ToList();
                    if(desde != String.Empty)
                    {
                        Lista = Lista.Where(s => s.OTR.Fecha > fechaDesde.AddDays(-1)).ToList();           
                    }
                    if (hasta != String.Empty)
                    {
                        Lista = Lista.Where(s => s.OTR.Fecha <= fechaHasta.AddDays(1)).ToList();
                    }

                    List<OTRModel> listaModel = new List<OTRModel>();
                    int index = 0;
                    while (index < Lista.Count)
                    {
                        var item = Lista[index];
                        if (item.OTR.Estado == "Procesada" || item.OTR.Estado == "Cerrada")
                        {
                            DataRow dato = dtDatos.NewRow();

                            dato["Descripcion"] = contenedor.Descripcion;
                            dato["Codigo"] = contenedor.Codigo;
                            dato["Peso"] = contenedor.Peso;                           
                            dato["Contrato"] = item.OTR.Contrato.Numero + " - " + item.OTR.Contrato.DescripcionContrato;
                            dato["Cliente"] = item.OTR.Cliente.Numero + " - " + item.OTR.Cliente.Nombre;
                            dato["Fecha"] = item.OTR.Fecha;
                            dato["OTR"] = item.OTR.Consecutivo;
                            dato["Ubicacion"] = item.Viaje.Ubicacion != null ? item.Viaje.Ubicacion.Descripcion : String.Empty;                            

                            dtDatos.Rows.Add(dato);
                        }
                        index++;
                    }
                }
            }

            // Se genera el reporte deseado
            ReportDocument rd = new ReportDocument();
            string strRptPath = String.Empty;

            // Se identifica el tipo de reporte
            if(tipo == "General")
            {
                strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//Bascula//rpt_ContenedorGeneral.rpt";
            }
            else
            {
                strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//Bascula//rpt_ContenedorDetalle.rpt";
            }
            rd.Load(strRptPath);

            rd.SetDataSource(dsCierre);// Se asigna el dataset al datasorce del reporte
            var tipoFormato = formato == "pdf" ? ExportFormatType.PortableDocFormat : ExportFormatType.Excel;
            rd.ExportToHttpResponse(tipoFormato, System.Web.HttpContext.Current.Response, false, "ControlOperaciones_Contenedores");
        }

        /// <summary>
        /// retorna la vista indicada para obtener los filtros deseados para generar el reporte
        /// </summary>
        /// <returns>La vista ToneladasXCompania</returns>
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_REP_TONELADAS_BOLETA + "," + WPPConstants.ROLES_REPORTES_GERENCIA)]
        public ActionResult ToneladasXCompania()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("IsDeleted", false);
            ViewBag.ListaCompania = companiaMapper.GetListaCompaniaModel(companiaService.GetAll(criteria));
            return View();
        }

        /// <summary>
        /// Este metodo recopila la informacion necesaria para generar el reporte de toneladas por companias
        /// para obtener la informacion correspondiente
        /// </summary>
        /// <returns>el reporte deseado</returns>
        public void ReporteToneladasXCompania(string desde, string hasta, string formato, string compania)
        {
            // Se establecen los filtros
            DateTime fechaHasta = DateTime.Now;
            DateTime fechaDesde = DateTime.Now;
            Compania companiaActual = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("IsDeleted", false);
            if (compania != String.Empty)
            {
                if(compania!= "Todos")
                {
                    companiaActual = companiaService.Get(Convert.ToInt64(compania));
                    criteria.Add("Compania", companiaActual);
                }
            }
            else
            {
                criteria.Add("Compania", companiaActual);
            }

            IList<Bascula> ListaBascula = new List<Bascula>();
            ListaBascula = basculaService.GetAll(criteria);
            if (desde != String.Empty && hasta != String.Empty)
            {
                fechaHasta = Convert.ToDateTime(hasta);
                fechaDesde = Convert.ToDateTime(desde);
                ListaBascula = ListaBascula.Where(s => s.Fecha > fechaDesde.AddDays(-1) && s.Fecha <= fechaHasta.AddDays(1)).ToList();
            }
            ListaBascula = ListaBascula.OrderBy(s => s.Fecha).ToList();

            // Se indica el data set a utilizar
            ds_ToneladasXCompania dsToneladas = new ds_ToneladasXCompania();
            var dtReporte = dsToneladas.Tables["Reporte"];
            var dtDatos = dsToneladas.Tables["Datos"];

             // Rows DataTable Reporte
            DataRow row = dtReporte.NewRow();
            row["Compania"] = companiaActual.Nombre;           
            row["FechaActual"] = DateTime.Now;
            dtReporte.Rows.Add(row);
            
            foreach (var boleta in ListaBascula)
            {
                // Rows DataTable Datos
                DataRow dato = dtDatos.NewRow();

                dato["Placa"] = boleta.Equipo.Placa;
                dato["Boleta"] = boleta.Boleta;
                dato["PesoBruto"] = boleta.PesoBruto;
                dato["PesoTara"] = boleta.PesoTara;
                dato["Fecha"] = boleta.Fecha;
                dato["PesoNeto"] = boleta.PesoNeto;
                dato["PrecioTotal"] = boleta.Total;
                dato["Compania"] = boleta.Compania.Nombre;

                dtDatos.Rows.Add(dato);            
            }

            // Se genera el reporte deseado
            ReportDocument rd = new ReportDocument();
            string strRptPath = String.Empty;
            strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//ControlOperaciones//rpt_ToneladasXCompania.rpt";
            rd.Load(strRptPath);

            rd.SetDataSource(dsToneladas);// Se asigna el dataset al datasorce del reporte
            var tipoFormato = formato == "pdf" ? ExportFormatType.PortableDocFormat : ExportFormatType.Excel;
            rd.ExportToHttpResponse(tipoFormato, System.Web.HttpContext.Current.Response, false, "ControlOperaciones_Equipos");
        }
       
        #endregion

        #region CONTRATOS

        /// <summary>
        /// retorna la vista indicada para obtener los filtros deseados para generar el reporte
        /// </summary>
        /// <returns>La vista Contrato</returns>
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_REP_CONTRATO + "," + WPPConstants.ROLES_REPORTES_GERENCIA)]
        public ActionResult Contrato()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("IsDeleted", false);
            ViewBag.ListaCompania = companiaMapper.GetListaCompaniaModel(companiaService.GetAll(criteria));
            return View();
        }

        /// <summary>
        /// Este metodo recopila la informacion necesaria para generar el reporte de contratos
        /// para obtener la informacion correspondiente
        /// </summary>
        /// <returns>el reporte deseado</returns>
        public void ReporteContrato(string desde, string hasta, string cliente, string moneda, string estado, bool agrupar, string formato, string compania)
        {
            // Se establecen los filtros
            DateTime fechaHasta = DateTime.Now;
            if (desde != String.Empty)
            {
                fechaHasta = Convert.ToDateTime(hasta);
            }
            DateTime fechaDesde = DateTime.Now;
            if (desde != String.Empty)
            {
                fechaDesde = Convert.ToDateTime(desde);
            }

            Compania companiaActual = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
          
            IDictionary<string, object> criteria = new Dictionary<string, object>();     
            if(estado != "Todos")
            {
                criteria.Add("Estado", estado);
            }
            if (moneda != "Todos")
            {
                criteria.Add("Moneda", moneda);
            }
           
            if (compania != String.Empty)
            {
                if (compania != "Todos")
                {
                    Compania companiaSearch = companiaService.Get(Convert.ToInt64(compania));
                    criteria.Add("Compania", companiaSearch.Id);
                }
            }
            else
            {
                criteria.Add("Compania", companiaActual);
            }
            criteria.Add("IsDeleted", false);

            // Se indica el data set a utilizar
            ds_Contratos dsContrato = new ds_Contratos();
            var dtReporte = dsContrato.Tables["Reporte"];
            var dtDatos = dsContrato.Tables["Datos"];

            // Rows DataTable Reporte
            DataRow row = dtReporte.NewRow();
            row["Compania"] = companiaActual.Nombre;
            row["FechaActual"] = DateTime.Now;
            dtReporte.Rows.Add(row);

            IList<Contrato> ListaContrato = contratoService.GetAll(criteria);
            if (cliente != String.Empty)
            {
                ListaContrato = ListaContrato.Where(s => s.Cliente.Nombre.ToUpper().Contains(cliente.ToUpper())).ToList();
            }

            foreach (var item in ListaContrato)
            {
                IList<ProductoContrato> Productos = new List<ProductoContrato>();
                if(desde != String.Empty && hasta != String.Empty)
                {
                    Productos = item.Productos.Where(s => s.FechaFinal > fechaDesde.AddDays(-1) && s.FechaFinal <= fechaHasta.AddDays(1)).ToList();                
                }
                else
                {
                    Productos = item.Productos;
                }
                
                if(Productos.Count > 0)
                {
                    // Rows DataTable Datos
                    DateTime fechaVencimiento = Productos.Min(s => s.FechaFinal);
                    DataRow dato = dtDatos.NewRow();
                    dato["Numero"] = item.Numero;
                    dato["Descripcion"] = item.DescripcionContrato;
                    dato["Estado"] = item.Estado;
                    dato["FechaVencimiento"] = fechaVencimiento;
                    dato["Cliente"] = item.Cliente.Nombre;
                    dato["Moneda"] = item.Moneda;
                    dato["Compania"] = item.Compania.Nombre;
                    dtDatos.Rows.Add(dato);
                }
            }

            // Se genera el reporte deseado
            ReportDocument rd = new ReportDocument();
            string strRptPath = String.Empty;
            // Se identifica el tipo de reporte
            if (agrupar)
            {
                strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//Contratos//rpt_ContratosXCliente.rpt";
            }
            else
            {
                strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//Contratos//rpt_Contratos.rpt";
            }
            rd.Load(strRptPath);

            rd.SetDataSource(dsContrato);// Se asigna el dataset al datasorce del reporte
            var tipoFormato = formato == "pdf" ? ExportFormatType.PortableDocFormat : ExportFormatType.Excel;
            rd.ExportToHttpResponse(tipoFormato, System.Web.HttpContext.Current.Response, false, "ControlOperaciones_Contrato");
        }


        /// <summary>
        /// retorna la vista indicada para obtener los filtros deseados para generar el reporte
        /// </summary>
        /// <returns>La vista ContratoDetalle</returns>
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_REP_BITACORA_CONTRATO + "," + WPPConstants.ROLES_REPORTES_GERENCIA)]
        public ActionResult ContratoBitacora()
        {
            return View();
        }

        /// <summary>
        /// Este metodo recopila la informacion necesaria para generar el reporte contratos
        /// para obtener la informacion correspondiente
        /// </summary>
        /// <returns>el reporte deseado</returns>
        public void ReporteContratoBitacora(string cliente, string moneda, string estado, string numero, string contrato, string formato)
        {
            // Se establecen los filtros
            Compania companiaActual = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);

            IDictionary<string, object> criteria = new Dictionary<string, object>();
            if (estado != "Todos")
            {
                criteria.Add("Estado", estado);
            }
            if (moneda != "Todos")
            {
                criteria.Add("Moneda", moneda);
            }
            if (numero != String.Empty)
            {
                criteria.Add("Numero", Convert.ToInt64(numero));
            }
            criteria.Add("Compania", companiaActual);
            criteria.Add("IsDeleted", false);

            // Se indica el data set a utilizar
            ds_ContratoBitacora dsContrato = new ds_ContratoBitacora();
            var dtReporte = dsContrato.Tables["Reporte"];
            var dtDatos = dsContrato.Tables["Datos"];

            // Rows DataTable Reporte
            DataRow row = dtReporte.NewRow();
            row["Compania"] = companiaActual.Nombre;
            row["Fecha"] = DateTime.Now;
            dtReporte.Rows.Add(row);

            IList<Contrato> ListaContrato = contratoService.GetAll(criteria);
            if (cliente != String.Empty)
            {
                ListaContrato = ListaContrato.Where(s => s.Cliente.Nombre.ToUpper().Contains(cliente.ToUpper())).ToList();
            }
            if (contrato != String.Empty)
            {
                ListaContrato = ListaContrato.Where(s => s.DescripcionContrato.ToUpper().Contains(contrato.ToUpper())).ToList();
            }

            foreach (var contratoItem in ListaContrato)
            {
                criteria = new Dictionary<string, object>();
                criteria.Add("Contrato", contratoItem);
                var bitacora = contratoBitacoraService.GetAll(criteria);
                foreach (var item in bitacora)
                {
                    // Rows DataTable Datos
                    DataRow dato = dtDatos.NewRow();
                    dato["Contrato"] = item.Contrato.Numero + " - " + item.Contrato.DescripcionContrato;
                    dato["Responsable"] = item.CreatedBy;
                    dato["FechaCambio"] = item.CreateDate;
                    dato["ValorAnterior"] = item.valorAnterior;
                    dato["ValorActual"] = item.valorNuevo;
                    dato["Campo"] = item.Campo;
                    dtDatos.Rows.Add(dato);                    
                }
            }

            // Se genera el reporte deseado
            ReportDocument rd = new ReportDocument();
            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//Contratos//rpt_ContratoBitacora.rpt";

            rd.Load(strRptPath);

            rd.SetDataSource(dsContrato);// Se asigna el dataset al datasorce del reporte
            var tipoFormato = formato == "pdf" ? ExportFormatType.PortableDocFormat : ExportFormatType.Excel;
            rd.ExportToHttpResponse(tipoFormato, System.Web.HttpContext.Current.Response, false, "ControlOperaciones_BitacoraContrato");
        }

        /// <summary>
        /// retorna la vista indicada para obtener los filtros deseados para generar el reporte
        /// </summary>
        /// <returns>La vista ContratoDetalle</returns>
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_REP_CONTRATO + "," + WPPConstants.ROLES_REPORTES_GERENCIA)]
        public ActionResult ContratoDetalle()
        {
            return View();
        }

        /// <summary>
        /// Este metodo recopila la informacion necesaria para generar el reporte contratos
        /// para obtener la informacion correspondiente
        /// </summary>
        /// <returns>el reporte deseado</returns>
        public void ReporteContratoDetalle(string cliente, string moneda, string estado, string numero, string contrato, string formato)
        {
            // Se establecen los filtros
            Compania companiaActual = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);

            IDictionary<string, object> criteria = new Dictionary<string, object>();
            if (estado != "Todos")
            {
                criteria.Add("Estado", estado);
            }
            if (moneda != "Todos")
            {
                criteria.Add("Moneda", moneda);
            }
            if (numero != String.Empty)
            {
                criteria.Add("Numero", Convert.ToInt64(numero));
            }
            criteria.Add("Compania", companiaActual);           
            criteria.Add("IsDeleted", false);

            // Se indica el data set a utilizar
            ds_ContratosDetalle dsContrato = new ds_ContratosDetalle();
            ds_ProductosContrato dsProductos = new ds_ProductosContrato();
            var dtReporte = dsContrato.Tables["Reporte"];
            var dtDatos = dsContrato.Tables["Datos"];
            var dtProductos = dsProductos.Tables["Datos"];

            // Rows DataTable Reporte
            DataRow row = dtReporte.NewRow();
            row["Compania"] = companiaActual.Nombre;
            row["FechaActual"] = DateTime.Now;
            dtReporte.Rows.Add(row);

            IList<Contrato> ListaContrato = contratoService.GetAll(criteria);
            if (cliente != String.Empty)
            {
                ListaContrato = ListaContrato.Where(s => s.Cliente.Nombre.ToUpper().Contains(cliente.ToUpper())).ToList();
            }
            if (contrato != String.Empty)
            {
                ListaContrato = ListaContrato.Where(s => s.DescripcionContrato.ToUpper().Contains(contrato.ToUpper())).ToList();
            }

            foreach (var item in ListaContrato)
            {
                // Rows DataTable Datos
                DataRow dato = dtDatos.NewRow();
                dato["Numero"] = item.Numero;
                dato["Descripcion"] = item.DescripcionContrato;
                dato["Estado"] = item.Estado;
                dato["FechaInicio"] = item.FechaInicio;
                dato["Cliente"] = item.Cliente.Nombre;
                dato["Moneda"] = item.Moneda;
                dato["ModoFacturacion"] = item.ModoFacturacion;
                dato["PuntoFacturacion"] = item.PuntoFacturacion.Nombre;
                dato["PagoContado"] = item.PagoContado != null ? (item.PagoContado == true ? "SI" : "NO") : String.Empty;
                dato["Repesaje"] = item.Repesaje != null ? item.Repesaje.Nombre : String.Empty;
                dato["Observaciones"] = item.Observaciones;
                dato["NumeroFormulario"] = item.NumeroFormulario;
                dato["IdContrato"] = item.Id;
                dato["Ejecutivo"] = item.Ejecutivo.Nombre + " " + item.Ejecutivo.Apellido1 + " " + item.Ejecutivo.Apellido2; 
                dtDatos.Rows.Add(dato);

                foreach (var producto in item.Productos)
                {
                    // Rows DataTable Datos (Subreporte)
                    DataRow datoProductos = dtProductos.NewRow();
                    datoProductos["IdContrato"] = item.Id;
                    datoProductos["Descripcion"] = producto.Descripcion;
                    datoProductos["Servicio"] = producto.Servicio.Nombre;
                    datoProductos["Producto"] = producto.Producto.Descripcion;
                    datoProductos["Estado"] = producto.Estado;
                    datoProductos["CuentaContable"] = producto.CuentaContableCredito;
                    datoProductos["Precio"] = producto.Total;
                    datoProductos["Cantidad"] = producto.Cantidad;
                    datoProductos["FechaInicio"] = producto.FechaInicial;
                    datoProductos["FechaFin"] = producto.FechaFinal;                   
                    dtProductos.Rows.Add(datoProductos);
                }
            }

            // Se genera el reporte deseado
            ReportDocument rd = new ReportDocument();
            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//Contratos//rpt_ContratosDetalle.rpt";
            
            rd.Load(strRptPath);

            rd.SetDataSource(dsContrato);// Se asigna el dataset al datasorce del reporte
            rd.Subreports[0].SetDataSource(dsProductos); // Se asigna el dataset al datasorce del subreporte
            var tipoFormato = formato == "pdf" ? ExportFormatType.PortableDocFormat : ExportFormatType.Excel;
            rd.ExportToHttpResponse(tipoFormato, System.Web.HttpContext.Current.Response, false, "ControlOperaciones_Contrato");
        }


        /// <summary>
        /// retorna la vista indicada para obtener los filtros deseados para generar el reporte
        /// </summary>
        /// <returns>La vista Productos</returns>
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_REP_PRODUCTOS + "," + WPPConstants.ROLES_REPORTES_GERENCIA)]
        public ActionResult Productos()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("IsDeleted", false);
            ViewBag.ListaCategoria = categoriaMapper.GetListaCategoriaProductoModel(categoriaProductoService.GetAll(criteria));
            ViewBag.ListaCompania = companiaMapper.GetListaCompaniaModel(companiaService.GetAll(criteria));
            return View();
        }

        /// <summary>
        /// Este metodo recopila la informacion necesaria para generar el reporte de productos
        /// para obtener la informacion correspondiente
        /// </summary>
        /// <returns>el reporte deseado</returns>
        public void ReporteProductos(string categoria, string descripcion, bool agrupar, string formato, string compania) 
        {
            // Se establecen los filtros
            Compania companiaActual = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);

            IDictionary<string, object> criteria = new Dictionary<string, object>();
            if (categoria != "Todos")
            {
                CategoriaProducto categoriaProducto = categoriaProductoService.Get(Convert.ToInt64(categoria));
                criteria.Add("Categoria", categoriaProducto);
            }
            if (compania != String.Empty)
            {
                if (compania != "Todos")
                {
                    Compania companiaSearch = companiaService.Get(Convert.ToInt64(compania));
                    criteria.Add("Compania", companiaSearch.Id);
                }
            }
            else
            {
                criteria.Add("Compania", companiaActual.Id);
            }
            criteria.Add("IsDeleted", false);

            // Se indica el data set a utilizar
            ds_Productos dsProductos = new ds_Productos();
            var dtReporte = dsProductos.Tables["Reporte"];
            var dtDatos = dsProductos.Tables["Datos"];

            // Rows DataTable Reporte
            DataRow row = dtReporte.NewRow();
            
            row["Compania"] = companiaActual.Nombre;
            
            row["FechaActual"] = DateTime.Now;
            dtReporte.Rows.Add(row);

            IList<Producto> ListaProducto = productoService.GetAll(criteria);
            if (descripcion != String.Empty)
            {
                ListaProducto = ListaProducto.Where(s => s.Descripcion.ToUpper().Contains(descripcion.ToUpper())).ToList();
            }

            foreach (var item in ListaProducto)
            {
                // Rows DataTable Datos
                DataRow dato = dtDatos.NewRow();
                dato["Categoria"] = item.Categoria.Nombre;
                dato["Descripcion"] = item.Descripcion;
                dato["UnidadCobro"] = item.UnidadCobro != null ? item.UnidadCobro.Nombre : String.Empty;
                dato["TipoEquipo"] = item.TipoEquipo != null ? item.TipoEquipo.Nombre : String.Empty;
                dato["Tamano"] = item.Tamano != null ? item.Tamano.Nombre : String.Empty;
                Compania ComProducto = companiaService.Get(item.Compania);
                dato["Compania"] = ComProducto.Nombre;
                dtDatos.Rows.Add(dato);                
            }

            // Se genera el reporte deseado
            ReportDocument rd = new ReportDocument();
            string strRptPath = String.Empty;
            //Se identifica el tipo de reporte
            if (agrupar)
            {
                strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//Contratos//rpt_ProductosXCategoria.rpt";
            }
            else
            {
                strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//Contratos//rpt_Productos.rpt";
            }

            rd.Load(strRptPath);

            rd.SetDataSource(dsProductos);// Se asigna el dataset al datasorce del reporte
            var tipoFormato = formato == "pdf" ? ExportFormatType.PortableDocFormat : ExportFormatType.Excel;
            rd.ExportToHttpResponse(tipoFormato, System.Web.HttpContext.Current.Response, false, "ControlOperaciones_Productos");
        }

        /// <summary>
        /// retorna la vista indicada para obtener los filtros deseados para generar el reporte
        /// </summary>
        /// <returns>La vista Contrato</returns>
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_REP_CONTRATO + "," + WPPConstants.ROLES_REPORTES_GERENCIA)]
        public ActionResult ContratoXEjecutivo()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("IsDeleted", false);
            ViewBag.ListaCompania = companiaMapper.GetListaCompaniaModel(companiaService.GetAll(criteria));
            criteria = new Dictionary<string, object>();
            criteria.Add("IsDeleted", false);
            ViewBag.ListaEjecutivos = usuarioMapper.GetListaUsuarioModel(usuarioService.GetAll(criteria));

            return View();
        }

        /// <summary>
        /// Este metodo recopila la informacion necesaria para generar el reporte de contratos
        /// para obtener la informacion correspondiente
        /// </summary>
        /// <returns>el reporte deseado</returns>
        public void ReporteContratosXEjecutivo(string desde, string hasta, string cliente, string ejecutivo, string estado, bool contratosFacturados, string formato, string compania)
        {
            // Se establecen los filtros
            DateTime fechaHasta = DateTime.Now;
            if (desde != String.Empty)
            {
                fechaHasta = Convert.ToDateTime(hasta);
            }
            DateTime fechaDesde = DateTime.Now;
            if (desde != String.Empty)
            {
                fechaDesde = Convert.ToDateTime(desde);
            }

            IList<Contrato> ListaContrato = new List<Contrato>();
            Compania companiaActual = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);


            if (contratosFacturados)
            {
                ListaContrato = facturacionService.FiltrarContratosXEmpleadoFacturados(desde, hasta, cliente, ejecutivo, estado, compania);
            }
            else
            {
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                if (estado != "Todos")
                {
                    criteria.Add("Estado", estado);
                }

                if (ejecutivo != "Todos")
                {
                    Usuario ejecutivoSearch = usuarioService.Get(Convert.ToInt64(ejecutivo));
                    criteria.Add("Ejecutivo", ejecutivoSearch);
                }

                if (compania != String.Empty)
                {
                    if (compania != "Todos")
                    {
                        Compania companiaSearch = companiaService.Get(Convert.ToInt64(compania));
                        criteria.Add("Compania", companiaSearch.Id);
                    }
                }
                else
                {
                    criteria.Add("Compania", companiaActual);
                }
                criteria.Add("IsDeleted", false);

                ListaContrato = contratoService.GetAll(criteria);
                if (cliente != String.Empty)
                {
                    ListaContrato = ListaContrato.Where(s => s.Cliente.Nombre.ToUpper().Contains(cliente.ToUpper())).ToList();
                }
            }

            // Se indica el data set a utilizar
            ds_ContratosXEjecutivo dsContrato = new ds_ContratosXEjecutivo();
            var dtReporte = dsContrato.Tables["Reporte"];
            var dtDatos = dsContrato.Tables["Datos"];

            // Rows DataTable Reporte
            DataRow row = dtReporte.NewRow();
            row["Compania"] = companiaActual.Nombre;
            row["FechaActual"] = DateTime.Now;
            dtReporte.Rows.Add(row);

            foreach (var item in ListaContrato)
            {
                IList<ProductoContrato> Productos = new List<ProductoContrato>();
                if (desde != String.Empty && hasta != String.Empty)
                {
                    Productos = item.Productos.Where(s => s.FechaFinal > fechaDesde.AddDays(-1) && s.FechaFinal <= fechaHasta.AddDays(1)).ToList();
                }
                else
                {
                    Productos = item.Productos;
                }

                if (Productos.Count > 0)
                {
                    // Rows DataTable Datos
                    DateTime fechaVencimiento = Productos.Min(s => s.FechaFinal);
                    DataRow dato = dtDatos.NewRow();
                    dato["Numero"] = item.Numero;
                    dato["Descripcion"] = item.DescripcionContrato;
                    dato["Estado"] = item.Estado;
                    dato["FechaVencimiento"] = fechaVencimiento;
                    dato["Cliente"] = item.Cliente.Nombre;
                    dato["Moneda"] = item.Moneda;
                    dato["Compania"] = item.Compania.Nombre;
                    dato["Ejecutivo"] = item.Ejecutivo.Nombre + " " + item.Ejecutivo.Apellido1 + " " + item.Ejecutivo.Apellido2;
                    dtDatos.Rows.Add(dato);
                }
            }

            // Se genera el reporte deseado
            ReportDocument rd = new ReportDocument();
            string strRptPath = strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//Contratos//rpt_ContratosXEjecutivo.rpt";
           
            rd.Load(strRptPath);

            rd.SetDataSource(dsContrato);// Se asigna el dataset al datasorce del reporte
            var tipoFormato = formato == "pdf" ? ExportFormatType.PortableDocFormat : ExportFormatType.Excel;
            rd.ExportToHttpResponse(tipoFormato, System.Web.HttpContext.Current.Response, false, "ControlOperaciones_ContratoPorEjecutivo");
        }


        #endregion
    }
}
