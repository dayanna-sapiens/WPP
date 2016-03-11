using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using WPP.Entities.Objects.ModuloBascula;
using WPP.Entities.Objects.ModuloCierreCaja;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;
using WPP.Helpers;
using WPP.Mapper.ModuloBascula;
using WPP.Mapper.ModuloContratos;
using WPP.Model.ModuloBascula;
using WPP.Model.ModuloContratos;
using WPP.Security;
using WPP.Service.Generales;
using WPP.Service.ModuloBascula;
using WPP.Service.ModuloContratos;
using WPP.Service.ModuloOperacionRecoleccion;

namespace WPP.Controllers
{
    public class BasculaController : BaseController
    {
        //
        // GET: /BoletaBascula/
        private IContratoService contratoService;
        private ProductoMapper productoMapper;
        private ContratoMapper contratoMapper;
        private ServicioMapper servicioMapper;
        private ClienteMapper clienteMapper;
        private ProductoContratoMapper productoContratoMapper;
        private PagoBaculaMapper pagoMapper;
        private IProductoService productoService;
        private BasculaMapper basculaMapper;
        private IClienteService clienteService;
        private ICompaniaService companiaService;
        private IProductoContratoService productoContratoService;
        private IBasculaService basculaService;
        private IConsecutivoBoletaService consecutivoService;
        private IEquipoService equipoService;
        private ICatalogoService catalogoService;
        private IPagoBasculaService pagoService;
        private ITipoCambioService tipoCambioService;
        private IOTRService otrService;
        private IConsecutivoReciboPagoService reciboService;
        private IConfiguracionPuertoService puertoService;

        private String mensaje = "";
        public BasculaController(IContratoService service, IProductoService producto, IClienteService cliente, ICompaniaService compania, 
            IProductoContratoService productoContrato, IBasculaService boleta, IConsecutivoBoletaService consecutivo, IEquipoService equipo, 
            ICatalogoService catalogo, IPagoBasculaService pago, ITipoCambioService tipocambio, IOTRService otr, IConsecutivoReciboPagoService recibo, 
            IConfiguracionPuertoService puerto)
        {
            try
            {
                this.contratoService = service;
                this.productoService = producto;
                this.clienteService = cliente;
                this.companiaService = compania;
                this.productoContratoService = productoContrato;
                this.basculaService = boleta;
                this.equipoService = equipo;
                this.consecutivoService = consecutivo;
                this.catalogoService = catalogo;
                this.pagoService = pago;
                this.tipoCambioService = tipocambio;
                this.otrService = otr;
                this.reciboService = recibo;
                this.puertoService = puerto;
                servicioMapper = new ServicioMapper();
                contratoMapper = new ContratoMapper();
                productoMapper = new ProductoMapper();
                clienteMapper = new ClienteMapper();
                productoContratoMapper = new ProductoContratoMapper();
                basculaMapper = new BasculaMapper();
                pagoMapper = new PagoBaculaMapper();
                mensaje = ActualizarTipoCambio() == true ? String.Empty : "Es necesario registrar el tipo de cambio de hoy";
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private ActionResult RedirectURL(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("BoletaSinOTR", "BoletaBascula");
            }
        }

        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_CONS_BOLETAS + "," + WPPConstants.ROLES_ADMIN_BOLETAS + "," + WPPConstants.ROLES_EDIT_BOLETAS)]
        public ActionResult Index()
        {
            if (mensaje != String.Empty)
            {
                TempData["alertMessage"] = mensaje;
            }
            return Buscar(null, null, null, null, null,null,null,null,null,null,null);
        }

        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_CONS_BOLETAS + "," + WPPConstants.ROLES_ADMIN_BOLETAS + "," + WPPConstants.ROLES_EDIT_BOLETAS)]
        public ActionResult DetallesBoleta(long idBoleta)
        {
            Bascula bascula = basculaService.Get(idBoleta);
            BasculaModel model = basculaMapper.GetBoletaBasculaModel(bascula);
            model.SecuenciaBoleta = model.Boleta;
            model.Moneda = bascula.Contrato.Moneda;

            return View(model);
        }

     
        
        #region BOLETAS SIN OTRO

        /// <summary>
        /// Este método se encarga de cargar los ViewBags para los listados necesarios (contratos, productos),
        /// así como el consecutivo de la boleta para crear una boleta de bascula
        /// </summary>
        /// <returns>La vista BoletaSinOTR</returns>
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_BOLETAS + "," + WPPConstants.ROLES_EDIT_BOLETAS)]
      
        public ActionResult BoletaSinOTR()
        {
            Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Compania", compania);
            ConsecutivoBoleta consecutivo = consecutivoService.Get(criteria);
            
            ViewBag.BoletaSecuencia = consecutivo.Secuencia;
            ViewBag.BoletaId = consecutivo.Id;
            ViewBag.ListaContrato = new List<ContratoModel>();
            ViewBag.ListaProductos = new List<ProductoContratoModel>();
            
            //Se actualiza el consecutivo de la boleta
            consecutivo.Secuencia++;
            consecutivoService.Update(consecutivo);

            return View();
        }

        /// <summary>
        /// Este método guarda la información ingresada en el formulario de BoletaSinOTR
        /// </summary>
        /// <returns>La vista que se desea, si es para repesaje retorna el Index si es para cobrar retornará la vista de RecaudacionCaja</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_BOLETAS + "," + WPPConstants.ROLES_EDIT_BOLETAS)]
        public ActionResult GuardarBoletaSinOTR(BasculaModel model)
        {
            if (ModelState.IsValid)
            {
                Bascula boleta = new Bascula();
                boleta = basculaMapper.GetBoletaBascula(model, boleta);
                boleta.Version = 1;
                boleta.CreateDate = DateTime.Now;
                boleta.DateLastModified = DateTime.Now;
                boleta.CreatedBy = NombreUsuarioActual();
                boleta.Fecha = DateTime.Now;
                boleta.Compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                boleta.Facturada = false;

                if (model.Contrato > 0)
                {
                    boleta.Contrato = contratoService.Get(model.Contrato);
                    if (!boleta.Contrato.PagoContado.Value && boleta.Estado != "Repesaje")
                    {
                        boleta.Estado = "Activo";
                    }
                }

                if (model.Producto > 0)
                {
                    boleta.Producto = productoContratoService.Get(model.Producto);
                }

                if (model.Equipo > 0)
                {
                    boleta.Equipo = equipoService.Get(model.Equipo);
                }

                boleta.ListaPagos = new List<PagoBascula>();

                boleta.CierreCredito = false;

                boleta.Boleta = model.SecuenciaBoleta;                
                              
                // Se crea la boleta
                basculaService.Create(boleta);
                
                if (boleta.Estado == "Repesaje" || boleta.Estado == "Activo")
                {
                    return RedirectToAction("Index", "Bascula");
                }
                //Se envia al cobro
                return RedirectToAction("RecaudacionCaja", new { IdBoleta = boleta.Id });
                //return RecaudacionCaja(boleta);
            }
            else            
            {
                Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Compania", compania);
                criteria.Add("IsDeleted", false);
                ConsecutivoBoleta consecutivo = consecutivoService.Get(criteria);
                ViewBag.BoletaSecuencia = consecutivo.Secuencia;
                ViewBag.BoletaId = consecutivo.Id;
                ViewBag.ListaContrato = new List<ContratoModel>();
                ViewBag.ListaProductos = new List<ProductoContratoModel>();
                return RedirectToAction("BoletaSinOTR", "Bascula");
                //return View("BoletaSinOTR");
            }            
        }

        /// <summary>
        /// Este método carga la información de la boleta pendiente por repesar, esto según el idBoleta
        /// </summary>
        /// <returns>La vista RepesarBoletaSinOTR con el modelo de la boleta deseada</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_BOLETAS + "," + WPPConstants.ROLES_EDIT_BOLETAS)]
        public ActionResult RepesarBoletaSinOTR(long idBoleta)
        {
            Bascula bascula = basculaService.Get(idBoleta);
            BasculaModel model = basculaMapper.GetBoletaBasculaModel(bascula);
            model.SecuenciaBoleta = model.Boleta;
            ViewBag.PrecioProducto = bascula.Producto.Total;
            model.Moneda = bascula.Contrato.Moneda;
            ViewBag.Fosa = bascula.Producto.Producto.Categoria.Tipo == "Fosas" ? true : false;
            ViewBag.PrecioFosa = bascula.Producto.PrecioFosa;
            ViewBag.TipoPago = bascula.Contrato.PagoContado == true ? "Contado" : "Crédito";
            ViewBag.TipoCliente = bascula.Contrato.Cliente.Tipo.Nombre;

            IDictionary<string, object> criteriaProductos = new Dictionary<string, object>();
            criteriaProductos.Add("Contrato", bascula.Contrato);
            criteriaProductos.Add("IsDeleted", false);

            List<ProductoContratoModel> productos = new List<ProductoContratoModel>();
            var listaProductosContratos = productoContratoService.GetAll(criteriaProductos);

            foreach (var item in listaProductosContratos)
            {
                if (item.Producto.Categoria.Tipo == "Fosas" || item.Producto.Categoria.Tipo == "DisposicionFinal")
                {
                    productos.Add(
                        new ProductoContratoModel
                        {
                            Id = item.Id,
                            Descripcion = item.Descripcion,
                            Producto = item.Producto.Id,
                            ProductoDescripcion = item.Producto.Descripcion,
                            Total = item.Total,
                            Moneda = item.Contrato.Moneda
                        }
                     );
                }
            }

            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            model.ListaProductos = oSerializer.Serialize(productos);
            return View(model);
        }

        /// <summary>
        /// Este método guarda la información de la boleta ingresada una vez que el camión fue repesado y actualiza el peso de los ejes en el equipo
        /// </summary>
        /// <returns>La vista de RecaudacionCaja</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_BOLETAS + "," + WPPConstants.ROLES_EDIT_BOLETAS)]
        public ActionResult GuardarRepesajeBoletaSinOTR(BasculaModel model)
        {
            if (ModelState.IsValid)
            {
                Bascula boleta = basculaService.Get(model.Id);
                boleta = basculaMapper.GetBoletaBascula(model, boleta);
                boleta.Version++;
                boleta.DateLastModified = DateTime.Now;
                boleta.ModifiedBy = NombreUsuarioActual();
                boleta.Fecha = DateTime.Now;
                boleta.Compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);

                if (model.Contrato > 0)
                {
                    boleta.Contrato = contratoService.Get(model.Contrato);
                    if(!boleta.Contrato.PagoContado.Value)
                    {
                        boleta.Estado = "Activo";
                    }
                }

                if (model.Producto > 0)
                {
                    boleta.Producto = productoContratoService.Get(model.Producto);
                }

                Equipo equipo = new Equipo();
                if (model.Equipo > 0)
                {
                    equipo = equipoService.Get(model.Equipo);
                    boleta.Equipo = equipo;
                }

                boleta.Boleta = model.SecuenciaBoleta;
                
                // Se crea la boleta
                boleta = basculaService.Update(boleta);

                //Se actualiza la información del equipo según el repesaje
                if (model.Eje1Tara.HasValue)
                {
                    equipo.Eje1 = model.Eje1Tara.Value;
                }
                if (model.Eje2Tara.HasValue)
                {
                    equipo.Eje2 = model.Eje2Tara.Value;
                }
                if (model.Eje3Tara.HasValue)
                {
                    equipo.Eje3 = model.Eje3Tara.Value;
                }
                equipo.Peso = equipo.Eje1 + equipo.Eje2 + equipo.Eje3;
                equipo.Version++;
                equipo.DateLastModified = DateTime.Now;
                equipo.ModifiedBy = NombreUsuarioActual();

                equipo = equipoService.Update(equipo);

                if (boleta.Estado == "Activo")
                {
                    return RedirectToAction("Index", "Bascula");
                }
                //Se envia al cobro

                return RedirectToAction("RecaudacionCaja", new { IdBoleta = boleta.Id });
                //return RecaudacionCaja(boleta);
            }
            else
            {
                Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Compania", compania);

                ConsecutivoBoleta consecutivo = consecutivoService.Get(criteria);
                ViewBag.BoletaSecuencia = consecutivo.Secuencia;
                ViewBag.BoletaId = consecutivo.Id;
                ViewBag.ListaContrato = new List<ContratoModel>();
                ViewBag.ListaProductos = new List<ProductoContratoModel>();
                return RedirectToAction("BoletaSinOTR", "Bascula");
               // return View("BoletaSinOTR");
            }
        }

        /// <summary>
        /// Este método carga la información de la boleta pendiente de editar (esto según el idBoleta), una vez que la boleta original ha sido anulada
        /// </summary>
        /// <returns>La vista EditarBoletaSinOTR con el modelo de la boleta a editar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + "," + WPPConstants.ROLES_ADMIN_BOLETAS + "," + WPPConstants.ROLES_EDIT_BOLETAS)]
        public ActionResult EditarBoletaSinOTR(long idBoleta)
        {
            Bascula bascula = basculaService.Get(idBoleta);
            BasculaModel model = basculaMapper.GetBoletaBasculaModel(bascula);
            model.SecuenciaBoleta = model.Boleta;
            ViewBag.PrecioProducto = bascula.Producto.Total;
            model.Moneda = bascula.Contrato.Moneda;

            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Cliente", bascula.Contrato.Cliente);

            List<ContratoModel> contratos = new List<ContratoModel>();
            var listaContratos = contratoMapper.GetListaClienteModel(contratoService.GetAll(criteria));

            foreach (var item in listaContratos)
            {
                contratos.Add(
                    new ContratoModel
                    {
                        Id = item.Id,
                        DescripcionContrato = item.DescripcionContrato
                    }
                 );
            }

            ViewBag.ListaContrato = contratos;

            IDictionary<string, object> criteriaProductos = new Dictionary<string, object>();
            criteriaProductos.Add("Contrato", bascula.Contrato);

            List<ProductoContratoModel> productos = new List<ProductoContratoModel>();
            var listaProductosContratos = productoContratoService.GetAll(criteriaProductos);

            foreach (var item in listaProductosContratos)
            {
                if (item.Producto.Categoria.Tipo == "Fosas" || item.Producto.Categoria.Tipo == "DisposicionFinal")
                {
                    productos.Add(
                        new ProductoContratoModel
                        {
                            Id = item.Id,
                            Descripcion = item.Descripcion,
                            Producto = item.Producto.Id,
                            ProductoDescripcion = item.Producto.Descripcion,
                            Total = item.Total,
                            Moneda = item.Contrato.Moneda
                        }
                     );
                }
            }

            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            model.ListaProductos = oSerializer.Serialize(productos);

            return View(model);
        }

        /// <summary>
        /// Este método actualiza la información de la boleta seleccionada, según lo datos ingresados
        /// </summary>
        /// <returns>La vista de RecaudacionCaja</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_BOLETAS + "," + WPPConstants.ROLES_EDIT_BOLETAS)]
        public ActionResult EditarBoletaSinOTR(BasculaModel model)
        {
            if (ModelState.IsValid)
            {
                Bascula boleta = new Bascula();
                boleta = basculaService.Get(model.Id);
                boleta = basculaMapper.GetBoletaBascula(model, boleta);
                boleta.Version++;
                boleta.DateLastModified = DateTime.Now;
                boleta.ModifiedBy = NombreUsuarioActual();
                boleta.Compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);

                if (model.Contrato > 0)
                {
                    boleta.Contrato = contratoService.Get(model.Contrato);
                    if (!boleta.Contrato.PagoContado.Value)
                    {
                        boleta.Estado = "Activo";
                    }
                }

                if (model.Producto > 0)
                {
                    boleta.Producto = productoContratoService.Get(model.Producto);
                }

                if (model.Equipo > 0)
                {
                    boleta.Equipo = equipoService.Get(model.Equipo);
                }

                // Se crea la boleta
                basculaService.Update(boleta);


                if (boleta.Estado == "Activo")
                {
                    return RedirectToAction("Index", "Bascula");
                }

                //Se envia al cobro
                return RedirectToAction("RecaudacionCaja", new { IdBoleta = boleta.Id });
                //return RecaudacionCaja(boleta);
            }
            else
            {
                Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Compania", compania);

                ConsecutivoBoleta consecutivo = consecutivoService.Get(criteria);
                ViewBag.BoletaSecuencia = consecutivo.Secuencia;
                ViewBag.BoletaId = consecutivo.Id;
                ViewBag.ListaContrato = new List<ContratoModel>();
                ViewBag.ListaProductos = new List<ProductoContratoModel>();
                return RedirectToAction("BoletaSinOTR", "Bascula");
                //return View("BoletaSinOTR");
            }
        }

        /// <summary>
        /// Este método carga el model del cual se desea mostrar un detalle de la boleta 
        /// </summary>
        /// <returns>La vista de DetallesBoletaSinOTR con el modelo de boleta que se desea consultar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_BOLETAS + "," + WPPConstants.ROLES_EDIT_BOLETAS)]
        public ActionResult DetallesBoletaSinOTR(long idBoleta)
        {
            Bascula bascula = basculaService.Get(idBoleta);
            BasculaModel model = basculaMapper.GetBoletaBasculaModel(bascula);
            model.SecuenciaBoleta = model.Boleta;
            model.Moneda = bascula.Contrato.Moneda;

            ViewBag.TipoPago = bascula.Contrato.PagoContado == true ? "Contado" : "Crédito";
            ViewBag.TipoCliente = bascula.Contrato.Cliente.Tipo.Nombre == "Comerciales"? "Comercial" : "Municipal";

            return View(model);
        }

        /// <summary>
        /// Este método carga la información de la boleta que se desea eliminar, esto según el idBoleta
        /// </summary>
        /// <returns>La vista EliminarBoletaSinOTR con el modelo de la boleta a eliminar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_BOLETAS)]
        public ActionResult EliminarBoletaSinOTR(long idBoleta)
        {
            Bascula bascula = basculaService.Get(idBoleta);
            BasculaModel model = basculaMapper.GetBoletaBasculaModel(bascula);
            model.SecuenciaBoleta = model.Boleta;
            model.Moneda = bascula.Contrato.Moneda;

            return View(model);
        }

        /// <summary>
        /// Este método actualiza el estado IsDeleted a true, con el fin de indicar que esta boleta fue eliminada
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_BOLETAS)]
        public ActionResult EliminarBoletaSinOTR(BasculaModel model)
        {
            Bascula bascula = basculaService.Get(model.Id);
            bascula.IsDeleted = true;
            bascula.DateLastModified = DateTime.Now;
            bascula.Version++;
            bascula.DeletedBy = NombreUsuarioActual();
            bascula.DeleteDate = DateTime.Now;

            basculaService.Update(bascula);

            return RedirectToAction("Index", "Bascula");
        }

        /// <summary>
        /// Este método carga la información necesaria (FormaPago, Banco, Pagos, TipoCambio) según la báscula recibida, 
        /// con el fin de poder llevar a cabo el pago de la boleta de bascula generada 
        /// </summary>
        /// <returns>La vista RecaudacionCaja</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_BOLETAS + "," + WPPConstants.ROLES_EDIT_BOLETAS)]
        public ActionResult RecaudacionCaja(long IdBoleta)
        {
            Bascula bascula = basculaService.Get(IdBoleta);
            ViewBag.Bascula = bascula;
           // ViewBag.ListaFormaPago = catalogoService.GetByType("FormaPago");
            ViewBag.ListaBanco = catalogoService.GetByType("Banco");
            ViewBag.ListaPagos = bascula.ListaPagos.Where(s => s.IsDeleted == false);
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Fecha", DateTime.Now.ToString("dd/MM/yyyy"));
            var cambios = tipoCambioService.GetAll(criteria);
           // ViewBag.TipoCambioCompra = cambios.Where(s => s.Tipo == "Compra").FirstOrDefault();
            ViewBag.TipoCambioVenta = cambios.Where(s => s.Tipo == "Venta").FirstOrDefault();
            ViewBag.Usuario = NombreUsuarioActual();

            return View("RecaudacionCaja");
        }

        /// <summary>
        /// Este método guarda cada uno de los pagos realizados a la boleta de báscula
        /// </summary>
        /// <returns>un Json con el modelo del pago realizado</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_BOLETAS + "," + WPPConstants.ROLES_EDIT_BOLETAS)]
        public JsonResult GuardarPagoBascula(PagoBasculaModel model)
        {
            try
            {
                PagoBascula pago = new PagoBascula();
                pago = pagoMapper.GetPagoBascula(model, pago);
                pago.CreatedBy = NombreUsuarioActual();
                pago.CreateDate = DateTime.Now;
                pago.Version = 1;
                pago.CierreCaja = false;

                if (model.Banco.HasValue)
                {
                    pago.Banco= catalogoService.Get(model.Banco.Value);
                }
                
                Bascula boleta = new Bascula();
                if (model.Bacula > 0)
                {

                    boleta = basculaService.Get(model.Bacula);
                    pago.Boleta = boleta;
                }
                
                // Se crea la boleta
                pago = pagoService.Create(pago);

                boleta.ListaPagos.Add(pago);
                basculaService.Update(boleta);

                PagoBasculaModel pagoModel = pagoMapper.GetPagoBasculaModel(pago);
                return Json(pagoModel, JsonRequestBehavior.AllowGet);     
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(null);
            }                
        }

        /// <summary>
        /// Este método da por finalizada la boleta, realiza un cambio de estado a Activo a la boleta indicada (idBoleta)
        /// </summary>
        /// <returns>un Json con true o false si se efectuo el cambio deseado</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_BOLETAS + "," + WPPConstants.ROLES_EDIT_BOLETAS)]
        public JsonResult AplicarPagoBascula(long idBoleta)
        {
            try 
            {
                Bascula boleta = basculaService.Get(idBoleta);
                boleta.Estado = "Activo";
                boleta.DateLastModified = DateTime.Now;
                boleta.ModifiedBy = NombreUsuarioActual();
                boleta.Version++;

                Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Compania", compania);
                ConsecutivoReciboPago recibo = reciboService.Get(criteria);

                boleta.NumeroRecibo = "BA-" + recibo.Secuencia;

                basculaService.Update(boleta);

                recibo.Secuencia++;
                reciboService.Update(recibo);

                return Json(boleta.NumeroRecibo, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
                return Json(null);
            }                
        }

        /// <summary>
        /// Este método efectua la anulació de una boleta de báscula, lo cual consiste en: cambiar a estado Anulado la boleta original (idBoleta) 
        /// y crear una copia exacta a esta boleta con un estado Pendiente y con un número de consecutivo diferente,
        /// el cual vaya acorde al consecutivo de la boletas físicas, esto para permitir editar la información ingresada en la boleta original
        /// </summary>
        /// <returns>un Json con el id de la nueva boleta</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_BOLETAS)]
        public JsonResult AnulaBoletaSinOTR(long idBoleta)
        {
            try
            {
                Bascula actualizaBoleta = AnularBoletaSinOTR(idBoleta, "Pendiente");

                return Json(actualizaBoleta.Id, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(null);
            }
        }

        /// <summary>
        /// Este método efectua la anulació de una boleta de báscula, lo cual consiste en: cambiar a estado Anulado la boleta original (idBoleta) 
        /// y crear una copia exacta a esta boleta con un estado indicado y con un número de consecutivo diferente
        /// </summary>
        /// <returns>la nueva boleta</returns>
        private Bascula AnularBoletaSinOTR(long idBoleta, string estado)
        {
            try
            {
                Bascula boleta = basculaService.Get(idBoleta);
                Bascula nuevaBoleta = new Bascula();

                // Se anula la boleta original
                boleta.Estado = "Anulada";
                boleta.DateLastModified = DateTime.Now;
                boleta.ModifiedBy = NombreUsuarioActual();
                boleta.Version++;

                basculaService.Update(boleta);

                // Se crea una nueva boleta a partir de la boleta original

                Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Compania", compania);

                ConsecutivoBoleta consecutivo = consecutivoService.Get(criteria);
                nuevaBoleta.Compania = boleta.Compania;
                nuevaBoleta.Contrato = boleta.Contrato;
                nuevaBoleta.Eje1 = boleta.Eje1;
                nuevaBoleta.Eje2 = boleta.Eje2;
                nuevaBoleta.Eje3 = boleta.Eje3;
                nuevaBoleta.Equipo = boleta.Equipo;
                nuevaBoleta.EquipoWPP = boleta.EquipoWPP;
                nuevaBoleta.NombreCliente = boleta.NombreCliente;
                nuevaBoleta.PesoBruto = boleta.PesoBruto;
                nuevaBoleta.PesoNeto = boleta.PesoNeto;
                nuevaBoleta.PesoTara = boleta.PesoTara;
                nuevaBoleta.Producto = boleta.Producto;
                nuevaBoleta.OTR = boleta.OTR;
                nuevaBoleta.Total = boleta.Total;
                nuevaBoleta.NumeroRecibo = boleta.NumeroRecibo;
                nuevaBoleta.CreateDate = DateTime.Now;
                nuevaBoleta.CreatedBy = NombreUsuarioActual();
                nuevaBoleta.Version = 1;
                nuevaBoleta.Estado = estado;
                nuevaBoleta.Boleta = consecutivo.Secuencia;
                nuevaBoleta.ListaPagos = new List<PagoBascula>();
                basculaService.Create(nuevaBoleta);

                //Se actualiza el consecutivo de la boleta
                consecutivo.Secuencia++;
                consecutivoService.Update(consecutivo);

                // Se crea cada uno de los pagos asociados
                foreach (var item in boleta.ListaPagos)
                {
                    PagoBascula pago = new PagoBascula();
                    pago.Banco = item.Banco;
                    pago.Boleta = nuevaBoleta;
                    pago.CreateDate = DateTime.Now;
                    pago.CreatedBy = NombreUsuarioActual();
                    pago.FormaPago = item.FormaPago;
                    pago.IsDeleted = false;
                    pago.Moneda = item.Moneda;
                    pago.Monto = item.Monto;
                    pago.NumeroAprobacion = item.NumeroAprobacion;
                    pago.NumeroTarjeta = item.NumeroTarjeta;
                    pago.TipoCambio = item.TipoCambio;
                    pago.Version = 1;
                    pago.CierreCaja = false;
                    pago.Cierre = new CierreCaja();

                    pagoService.Create(pago);

                    nuevaBoleta.ListaPagos.Add(pago);
                }

                basculaService.Update(nuevaBoleta);
                return nuevaBoleta;
            }
            catch (Exception)
            {
                return null;
            }
        
        }

        /// <summary>
        /// Este método efectua la reimpresión de una boleta de báscula, lo cual consiste en: cambiar a estado Anulado la boleta original (idBoleta) 
        /// y crear una copia exacta a esta boleta con un estado Activo y con un número de consecutivo diferente,
        /// el cual vaya acorde al consecutivo de la boletas físicas
        /// </summary>
        /// <returns>un Json con true o false si se efectuo el cambio deseado</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_BOLETAS)]
        public JsonResult ReimprimirBoletaSinOTR(long idBoleta)
        {
            try
            {
                Bascula actualizaBoleta = AnularBoletaSinOTR(idBoleta, "Activo");

                return Json(actualizaBoleta != null ? true : false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(null);
            }

        }

        /// <summary>
        /// Este método actualiza el estado IsDeleted a true, con el fin de indicar que este pago fue eliminado
        /// </summary>
        /// <returns>un Json con true o false si se efectuo el cambio deseado</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_BOLETAS + "," + WPPConstants.ROLES_EDIT_BOLETAS)]
        public JsonResult EliminarPagoBascula(long idPago)
        {
            try
            {
                PagoBascula pago = pagoService.Get(idPago);
                pago.DeletedBy = NombreUsuarioActual();
                pago.DeleteDate = DateTime.Now;
                pago.IsDeleted = true;
                pago.Version ++;

                // Se elimina la boleta
                pago = pagoService.Update(pago);

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(null);
            }
        }
        

        #endregion


        #region BOLETAS CON OTR

        /// <summary>
        /// Este método se encarga de cargar los ViewBags para los listados necesarios (contratos, productos),
        /// así como el consecutivo de la boleta para crear una boleta de bascula
        /// </summary>
        /// <returns>La vista BoletaConOTR</returns>
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_BOLETAS + "," + WPPConstants.ROLES_EDIT_BOLETAS)]
        public ActionResult BoletaConOTR()
        {
            Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Compania", compania);

            ConsecutivoBoleta consecutivo = consecutivoService.Get(criteria);
            ViewBag.BoletaSecuencia = consecutivo.Secuencia;
            ViewBag.BoletaId = consecutivo.Id;
            ViewBag.ListaContrato = new List<ContratoModel>();

            consecutivo.Secuencia++;
            consecutivoService.Update(consecutivo);

            return View();
        }

        /// <summary>
        /// Este método guarda la información ingresada en el formulario de BoletaConOTR
        /// </summary>
        /// <returns>La vista de Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_BOLETAS + "," + WPPConstants.ROLES_EDIT_BOLETAS)]
        public ActionResult GuardarBoletaConOTR(BasculaModel model)
        {
            if (ModelState.IsValid)
            {
                ConsecutivoBoleta consecutivo = new ConsecutivoBoleta();
                Bascula boleta = new Bascula();
                boleta = basculaMapper.GetBoletaBascula(model, boleta);
                boleta.Version = 1;
                boleta.CreateDate = DateTime.Now;
                boleta.CreatedBy = NombreUsuarioActual();
                boleta.Compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                boleta.Facturada = false;

                if (model.Contrato > 0)
                {
                    boleta.Contrato = contratoService.Get(model.Contrato);
                }

                if (model.Equipo > 0)
                {
                    boleta.Equipo = equipoService.Get(model.Equipo);
                }

                if (model.OTR > 0)
                {
                    boleta.OTR = otrService.Get(model.OTR);
                }

                if (model.Estado != "Repesaje")
                {
                    model.Estado = "Activo";
                }

                boleta.ListaPagos = new List<PagoBascula>();

                boleta.CierreCredito = false;

                boleta.Boleta = model.SecuenciaBoleta;

                // Se crea la boleta
                basculaService.Create(boleta);

                // Se actualiza el estado de la OTR indicandi que ya fue procesada (ya paso por báscula)
                boleta.OTR.Estado = "Procesada";

                //if (boleta.OTR.OTRMadre)
                //{
                //    IList<OTR> lista = boleta.OTR.ListaOTRHijas;

                //    foreach (var item in lista)
                //    {
                //        item.Estado = "Procesada";
                //        otrService.Update(item);
                //    }
                //}
                otrService.Update(boleta.OTR);

                return RedirectToAction("Index", "Bascula");
            }
            else
            {
                Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Compania", compania);

                ConsecutivoBoleta consecutivo = consecutivoService.Get(criteria);
                ViewBag.BoletaSecuencia = consecutivo.Secuencia;
                ViewBag.BoletaId = consecutivo.Id;
                ViewBag.ListaContrato = new List<ContratoModel>();

                return RedirectToAction("BoletaConOTR", "Bascula");
                //return View("BoletaConOTR");
            }
        }

        /// <summary>
        /// Este método carga la información de la boleta pendiente por repesar, esto según el idBoleta
        /// </summary>
        /// <returns>La vista RepesarBoletaConOTR con el modelo de la boleta deseada</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_BOLETAS + "," + WPPConstants.ROLES_EDIT_BOLETAS)]
        public ActionResult RepesarBoletaConOTR(long idBoleta)
        {
            Bascula bascula = basculaService.Get(idBoleta);
            BasculaModel model = basculaMapper.GetBoletaBasculaModel(bascula);
            model.SecuenciaBoleta = model.Boleta;
            model.Moneda = bascula.Contrato.Moneda;
           
            return View(model);
        }

        /// <summary>
        /// Este método guarda la información de la boleta ingresada una vez que el camión fue repesado y actualiza el peso de los ejes en el equipo
        /// </summary>
        /// <returns>La vista de Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_BOLETAS + "," + WPPConstants.ROLES_EDIT_BOLETAS)]
        public ActionResult GuardarRepesajeBoletaConOTR(BasculaModel model)
        {
            if (ModelState.IsValid)
            {
                Bascula boleta = basculaService.Get(model.Id);
                boleta = basculaMapper.GetBoletaBascula(model, boleta);
                boleta.Version++;
                boleta.DateLastModified = DateTime.Now;
                boleta.ModifiedBy = NombreUsuarioActual();
                boleta.Compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                boleta.Estado = "Activo";

                if (model.Contrato > 0)
                {
                    boleta.Contrato = contratoService.Get(model.Contrato);
                }

                if (model.OTR > 0)
                {
                    boleta.OTR = otrService.Get(model.OTR);
                }

                if (model.Producto > 0)
                {
                    boleta.Producto = productoContratoService.Get(model.Producto);
                }

                Equipo equipo = new Equipo();
                if (model.Equipo > 0)
                {
                    equipo = equipoService.Get(model.Equipo);
                    boleta.Equipo = equipo;
                }

                boleta.Boleta = model.SecuenciaBoleta;
                
                // Se crea la boleta
                boleta = basculaService.Update(boleta);

                //Se actualiza la información del equipo según el repesaje
                if (model.Eje1Tara.HasValue)
                {
                    equipo.Eje1 = model.Eje1Tara.Value;
                }
                if (model.Eje2Tara.HasValue)
                {
                    equipo.Eje2 = model.Eje2Tara.Value;
                }
                if (model.Eje3Tara.HasValue)
                {
                    equipo.Eje3 = model.Eje3Tara.Value;
                }
                equipo.Peso = equipo.Eje1 + equipo.Eje2 + equipo.Eje3;
                equipo.Version++;
                equipo.DateLastModified = DateTime.Now;
                equipo.ModifiedBy = NombreUsuarioActual();

                equipo = equipoService.Update(equipo);
                
                return RedirectToAction("Index", "Bascula");
            }
            else
            {
                Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Compania", compania);

                ConsecutivoBoleta consecutivo = consecutivoService.Get(criteria);
                ViewBag.BoletaSecuencia = consecutivo.Secuencia;
                ViewBag.BoletaId = consecutivo.Id;
                ViewBag.ListaContrato = new List<ContratoModel>();

                return RedirectToAction("BoletaSinOTR", "Bascula");
            }
        }

        /// <summary>
        /// Este método carga el model del cual se desea mostrar un detalle de la boleta 
        /// </summary>
        /// <returns>La vista de DetallesBoletaConOTR con el modelo de boleta que se desea consultar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_BOLETAS + "," + WPPConstants.ROLES_EDIT_BOLETAS + "," + WPPConstants.ROLES_CONS_BOLETAS)]
        public ActionResult DetallesBoletaConOTR(long idBoleta)
        {
            Bascula bascula = basculaService.Get(idBoleta);
            BasculaModel model = basculaMapper.GetBoletaBasculaModel(bascula);
            model.SecuenciaBoleta = model.Boleta;
            model.Moneda = bascula.Contrato.Moneda;

            ViewBag.TipoCliente = bascula.Contrato.Cliente.Tipo.Nombre == "Comerciales" ? "Comercial" : "Municipal";

            return View(model);
        }

        /// <summary>
        /// Este método efectua la anulació de una boleta de báscula, lo cual consiste en: cambiar a estado Anulado la boleta original (idBoleta) 
        /// y crear una copia exacta a esta boleta con un estado Pendiente y con un número de consecutivo diferente,
        /// el cual vaya acorde al consecutivo de la boletas físicas, esto para permitir editar la información ingresada en la boleta original
        /// </summary>
        /// <returns>un Json con el id de la nueva boleta</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_BOLETAS )]
        public JsonResult AnulaBoletaConOTR(long idBoleta)
        {
            try
            {
                Bascula actualizaBoleta = AnularBoletaSinOTR(idBoleta, "Pendiente");

                return Json(actualizaBoleta.Id, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(null);
            }
        }

        /// <summary>
        /// Este método efectua la anulació de una boleta de báscula, lo cual consiste en: cambiar a estado Anulado la boleta original (idBoleta) 
        /// y crear una copia exacta a esta boleta con un estado indicado y con un número de consecutivo diferente
        /// </summary>
        /// <returns>la nueva boleta</returns>
        private Bascula AnularBoletaConOTR(long idBoleta, string estado)
        {
            try
            {
                Bascula boleta = basculaService.Get(idBoleta);
                Bascula nuevaBoleta = new Bascula();

                // Se anula la boleta original
                boleta.Estado = "Anulada";
                boleta.DateLastModified = DateTime.Now;
                boleta.ModifiedBy = NombreUsuarioActual();
                boleta.Version++;

                basculaService.Update(boleta);

                // Se crea una nueva boleta a partir de la boleta original

                Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Compania", compania);

                ConsecutivoBoleta consecutivo = consecutivoService.Get(criteria);
                nuevaBoleta.Compania = boleta.Compania;
                nuevaBoleta.Contrato = boleta.Contrato;
                nuevaBoleta.Eje1 = boleta.Eje1;
                nuevaBoleta.Eje2 = boleta.Eje2;
                nuevaBoleta.Eje3 = boleta.Eje3;
                nuevaBoleta.Equipo = boleta.Equipo;
                nuevaBoleta.EquipoWPP = boleta.EquipoWPP;
                nuevaBoleta.NombreCliente = boleta.NombreCliente;
                nuevaBoleta.PesoBruto = boleta.PesoBruto;
                nuevaBoleta.PesoNeto = boleta.PesoNeto;
                nuevaBoleta.PesoTara = boleta.PesoTara;
                nuevaBoleta.Producto = boleta.Producto;
                nuevaBoleta.OTR = boleta.OTR;
                nuevaBoleta.Total = boleta.Total;
                nuevaBoleta.CreateDate = DateTime.Now;
                nuevaBoleta.CreatedBy = NombreUsuarioActual();
                nuevaBoleta.Version = 1;
                nuevaBoleta.Estado = estado;
                nuevaBoleta.Facturada = false;
                nuevaBoleta.Boleta = consecutivo.Secuencia;
                nuevaBoleta.ListaPagos = new List<PagoBascula>();
                basculaService.Create(nuevaBoleta);

                //Se actualiza el consecutivo de la boleta
                consecutivo.Secuencia++;
                consecutivoService.Update(consecutivo);

                basculaService.Update(nuevaBoleta);
                return nuevaBoleta;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Este método efectua la reimpresión de una boleta de báscula, lo cual consiste en: cambiar a estado Anulado la boleta original (idBoleta) 
        /// y crear una copia exacta a esta boleta con un estado Activo y con un número de consecutivo diferente,
        /// el cual vaya acorde al consecutivo de la boletas físicas
        /// </summary>
        /// <returns>un Json con true o false si se efectuo el cambio deseado</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_BOLETAS )]
        public JsonResult ReimprimirBoletaConOTR(long idBoleta)
        {
            try
            {
                Bascula actualizaBoleta = AnularBoletaSinOTR(idBoleta, "Activo");

                return Json(actualizaBoleta != null ? true : false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(null);
            }
        }

        /// <summary>
        /// Este método carga la información de la boleta que se desea eliminar, esto según el idBoleta
        /// </summary>
        /// <returns>La vista EliminarBoletaConOTR con el modelo de la boleta a eliminar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_BOLETAS )]
        public ActionResult EliminarBoletaConOTR(long idBoleta)
        {
            Bascula bascula = basculaService.Get(idBoleta);
            BasculaModel model = basculaMapper.GetBoletaBasculaModel(bascula);
            model.SecuenciaBoleta = model.Boleta;
            model.Moneda = bascula.Contrato.Moneda;

            return View(model);
        }

        /// <summary>
        /// Este método actualiza el estado IsDeleted a true, con el fin de indicar que esta boleta fue eliminada
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_BOLETAS)]
        public ActionResult EliminarBoletaConOTR(BasculaModel model)
        {
            Bascula bascula = basculaService.Get(model.Id);
            bascula.IsDeleted = true;
            bascula.DateLastModified = DateTime.Now;
            bascula.Version++;
            bascula.DeletedBy = NombreUsuarioActual();
            bascula.DeleteDate = DateTime.Now;

            basculaService.Update(bascula);

            return RedirectToAction("Index", "Bascula"); 
        }


        /// <summary>
        /// Este método carga la información de la boleta pendiente de editar (esto según el idBoleta), una vez que la boleta original ha sido anulada
        /// </summary>
        /// <returns>La vista EditarBoletaConOTR con el modelo de la boleta a editar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_BOLETAS + "," + WPPConstants.ROLES_EDIT_BOLETAS)]
        public ActionResult EditarBoletaConOTR(long idBoleta)
        {
            Bascula bascula = basculaService.Get(idBoleta);
            BasculaModel model = basculaMapper.GetBoletaBasculaModel(bascula);
            model.SecuenciaBoleta = model.Boleta;
            model.Moneda = bascula.Contrato.Moneda;
                               
            return View(model);
        }

        /// <summary>
        /// Este método actualiza la información de la boleta seleccionada, según lo datos ingresados
        /// </summary>
        /// <returns>La vista de RecaudacionCaja</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_BOLETAS + "," + WPPConstants.ROLES_EDIT_BOLETAS)]
        public ActionResult EditarBoletaConOTR(BasculaModel model)
        {
            if (ModelState.IsValid)
            {
                Bascula boleta = new Bascula();
                boleta = basculaService.Get(model.Id);
                boleta = basculaMapper.GetBoletaBascula(model, boleta);
                boleta.Version++;
                boleta.DateLastModified = DateTime.Now;
                boleta.ModifiedBy = NombreUsuarioActual();
                boleta.Compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                boleta.Estado = "Activo";
                
                if (model.Contrato > 0)
                {
                    boleta.Contrato = contratoService.Get(model.Contrato);
                }

                if (model.Producto > 0)
                {
                    boleta.Producto = productoContratoService.Get(model.Producto);
                }

                if (model.Equipo > 0)
                {
                    boleta.Equipo = equipoService.Get(model.Equipo);
                }

                if (model.OTR > 0)
                {
                    boleta.OTR = otrService.Get(model.OTR);
                }

                // Se crea la boleta
                basculaService.Update(boleta);
                
                return RedirectToAction("Index", "Bascula"); 
            }
            else
            {
                Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Compania", compania);

                ConsecutivoBoleta consecutivo = consecutivoService.Get(criteria);
                ViewBag.BoletaSecuencia = consecutivo.Secuencia;
                ViewBag.BoletaId = consecutivo.Id;
                consecutivo.Secuencia++;
                consecutivoService.Update(consecutivo);
                ViewBag.ListaContrato = new List<ContratoModel>();
                ViewBag.ListaProductos = new List<ProductoContratoModel>();

                return RedirectToAction("BoletaConOTR", "Bascula"); 
                //return View("BoletaConOTR");
            }
        }

        /// <summary>
        /// Este método verifica que no exista una boleta ya registrada con la misma OTR 
        /// </summary>
        /// <returns>un Json indicando true si puede continuar o false para indicar que no es valida la otr</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_BOLETAS + "," + WPPConstants.ROLES_EDIT_BOLETAS)]
        public JsonResult ValidarOTRBascula(long otr)
        {
            try
            {
                bool continuar = false;
                OTR OTR = otrService.Get(otr);
                Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Compania", compania);
                criteria.Add("IsDeleted", false);
                criteria.Add("OTR", OTR);

                Bascula bascula = basculaService.Get(criteria);

                if (bascula == null)
                {
                    continuar = true;
                }

                return Json(continuar, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(null);
            }
        }

        #endregion


        #region CONEXION BASCULA

        SerialPort serialPort;
        bool _continue;

        [HttpPost]
        public JsonResult ObtenerPeso()
        {
            // Create a new SerialPort object with default settings.
            serialPort = new SerialPort();
            //Thread readThread = new Thread(Read);
            string peso = String.Empty;
            string nombre = Environment.MachineName;
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("NombrePC", nombre);
            ConfiguracionPuerto puerto = puertoService.Get(criteria);
            // Allow the user to set the appropriate properties.
            if (puerto != null)
            {
                serialPort.PortName = puerto.Puerto;
                serialPort.BaudRate = 9600; //9600 300 600  1200  2400  4800  110
                serialPort.Parity = System.IO.Ports.Parity.None;
                serialPort.DataBits = 7;
                serialPort.StopBits = System.IO.Ports.StopBits.One;

                // Set the read/write timeouts
                serialPort.ReadTimeout = 500;
                serialPort.WriteTimeout = 500;

                serialPort.Open();
                _continue = true;
               // readThread.Start();
                //Console.Write("Name: ");
                //name = Console.ReadLine();

                //Console.WriteLine("Type QUIT to exit");

                while (_continue)
                {
                    string mensaje = serialPort.ReadLine();
                    if(mensaje.Length >= 13)
                    {
                        _continue = false;
                        peso = mensaje.Substring(1, 9).Trim();
                    }
                    //if (stringComparer.Equals("quit", message))
                    //{
                    //    _continue = false;
                    //}
                    //else
                    //{
                    //    serialPort.WriteLine(
                    //        String.Format("<{0}>: {1}", name, message));
                    //}
                }

               // readThread.Join();
                serialPort.Close();
            }
            else
            {
                return Json(null);
            }

            return Json(peso);
        }

        public void Read()
        {
            while (_continue)
            {
                try
                {
                    string message = serialPort.ReadLine();
                    Console.WriteLine(message);
                }
                catch (TimeoutException) { }
            }
        }

    #endregion



        /// <summary>
        /// Este método lista y filtra el listado de registros de boletas de báscula mostrados en el index
        /// </summary>
        /// <returns>Vista Index con el modelo IPageList(Bascula)</returns>
        public ActionResult Buscar(string sortOrder, string currentBoleta, string currentCliente, string currentEquipo, string currentEstado,
                 string searchStringBoleta, string searchStringCliente, string searchStringEquipo, string searchStringEstado, int? searchStringFilas, int? page)
        {
            ViewBag.CurrentSort = sortOrder;

            if (sortOrder == "BoletaAsc")
                ViewBag.BoletaSort = "BoletaDesc";
            else
                ViewBag.BoletaSort = "BoletaAsc";

            if (sortOrder == "ClienteAsc")
                ViewBag.ClienteSort = "ClienteDesc";
            else
                ViewBag.ClienteSort = "ClienteAsc";

            if (sortOrder == "EquipoAsc")
                ViewBag.EquipoSort = "EquipoDesc";
            else
                ViewBag.EquipoSort = "EquipoAsc";

            if (sortOrder == "EstadoAsc")
                ViewBag.EstadoSort = "EstadoDesc";
            else
                ViewBag.EstadoSort = "EstadoAsc";
            

            if (searchStringBoleta != null || searchStringCliente != null || searchStringEquipo != null || searchStringEstado != null)
                page = 1;
            else
            {
                searchStringBoleta = currentBoleta;
                searchStringCliente = currentCliente;
                searchStringEquipo = currentEquipo;
                searchStringEstado = currentEstado;
            }

            ViewBag.CurrentBoleta = searchStringBoleta;
            ViewBag.CurrentCliente = searchStringCliente;
            ViewBag.CurrentEquipo = searchStringEquipo;
            ViewBag.CurrentEstado = searchStringEstado;

            var compania = Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0;          
            var filas = searchStringFilas == null ? 10 : Convert.ToInt32(searchStringFilas);
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var pagina = basculaService.PagingSearch(searchStringBoleta, searchStringCliente,searchStringEquipo,searchStringEstado, compania, pageNumber, filas, sortOrder);

            return View("Index", pagina);
        }

    
    }
}
