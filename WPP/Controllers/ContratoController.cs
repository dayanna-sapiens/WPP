using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WPP.Entities.Objects.Generales;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Helpers;
using WPP.Mapper;
using WPP.Mapper.General;
using WPP.Mapper.ModuloContratos;
using WPP.Mapper.ModuloOperacionRecoleccion;
using WPP.Model.General;
using WPP.Model.ModuloContratos;
using WPP.Security;
using WPP.Service.Generales;
using WPP.Service.ModuloContratos;
using WPP.Service.ModuloOperacionRecoleccion;

namespace WPP.Controllers
{
    public class ContratoController : BaseController
    {
        private IContratoService contratoService;
        private ProductoMapper productoMapper;
        private CatalogoMapper catalogoMapper;
        private ContratoMapper contratoMapper;
        private ServicioMapper servicioMapper;
        private RutaRecoleccionMapper rutaMapper;
        private UbicacionClienteMapper ubicacionMapper;
        private ClienteMapper clienteMapper;
        private CategoriaProductoMapper categoriaProductoMapper;
        private ProductoContratoMapper productoContratoMapper;
        private PuntoFacturacionMapper puntoFacturacionMapper;
        private CompaniaMapper companiaMapper;
        private IProductoService productoService;
        private IClienteService clienteService;
        private IPuntoFacturacionService puntoFacturacionService;
        private IServicioService servicioService;
        private IUbicacionClienteService ubicacionService;
        private IRutaRecoleccionService rutaService;
        private ICompaniaService companiaService;
        private IProductoContratoService productoContratoService;
        private ICategoriaProductoService categoriaProductoService;
        private ICatalogoService catalogoService;
        private IConsecutivoContratoService consecutivoService;
        private IUsuarioService usuarioService;
        private UsuarioMapper usuarioMapper;
        private IContratoBitacoraService bitacoraService;

        public ContratoController(IContratoService service, IProductoService producto, IClienteService cliente, IPuntoFacturacionService puntoFacturacion, 
            IServicioService servicio, IUbicacionClienteService ubicacion, IRutaRecoleccionService ruta, ICompaniaService compania, 
            IProductoContratoService productoContrato, ICategoriaProductoService categoriaProducto, ICatalogoService catalogo, 
            IConsecutivoContratoService consecutivo, IUsuarioService usuario, IContratoBitacoraService bitacora )
        {
            try
            {
                this.contratoService = service;
                this.productoService = producto;
                this.clienteService = cliente;
                this.puntoFacturacionService = puntoFacturacion;
                this.servicioService = servicio;
                this.ubicacionService = ubicacion;
                this.rutaService = ruta;
                this.companiaService = compania;
                this.productoContratoService = productoContrato;
                this.categoriaProductoService = categoriaProducto;
                this.catalogoService = catalogo;
                this.consecutivoService = consecutivo;
                this.usuarioService = usuario;
                this.bitacoraService = bitacora;
                categoriaProductoMapper = new CategoriaProductoMapper();
                servicioMapper = new ServicioMapper();
                contratoMapper = new ContratoMapper();
                productoMapper = new ProductoMapper();
                catalogoMapper = new CatalogoMapper();
                clienteMapper = new ClienteMapper();
                companiaMapper = new CompaniaMapper();
                puntoFacturacionMapper = new PuntoFacturacionMapper();
                ubicacionMapper = new UbicacionClienteMapper();
                rutaMapper = new RutaRecoleccionMapper();
                productoContratoMapper = new ProductoContratoMapper();
                usuarioMapper = new UsuarioMapper();
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
                return RedirectToAction("Index", "Producto");
            }
        }

        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROLES_ADMINISTRACION + "," + WPPConstants.ROLES_ADMIN_CONTRATOS + "," + WPPConstants.ROLES_EDIT_CONTRATOS + "," + WPPConstants.ROLES_CONS_CONTRATOS)]
        public ActionResult Index()
        {
            RevisarContratosActivos();
            return Buscar(null, null, null, null, null, null, null, null, null, null, null, null, null,null,null);
        }

        #region CONTRATO

        private void RevisarContratosActivos()
        {
            Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Compania", compania);
            criteria.Add("IsDeleted", false);
            criteria.Add("Estado", "Activo");

            IList<Contrato> Contratos = contratoService.GetAll(criteria);

            foreach (Contrato item in Contratos)
            {
                foreach (ProductoContrato itemProducto in item.Productos)
                {
                    if (itemProducto.FechaFinal < DateTime.Now)
                    {
                        item.Estado = "Vencido";

                        contratoService.Update(item);

                        //itemProducto.Estado = "Vencido";

                        //productoContratoService.Update(itemProducto);
                    }
                }
            }
        }

        /// <summary>
        /// Este método se encarga de retornar la  vista que permite crear contratos y
        /// cargar los viewbags necesarios(Estado, Repesaje, CorteSemana, Punto Facturacion) 
        /// </summary>
        /// <returns>La vista Crear</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CONTRATOS + "," + WPPConstants.ROLES_EDIT_CONTRATOS)]
        public ActionResult Crear()
        {

            ContratoModel model = new ContratoModel();
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("IsDeleted", false);
            ViewBag.ListaEjecutivos = usuarioMapper.GetListaUsuarioModel(usuarioService.GetAll(criteria));

           // ViewBag.ListaEstado = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("EstadoContrato"));
            ViewBag.ListaRepesaje = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("Repesaje"));
            ViewBag.ListaCorteSemana = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("DiaSemana")).OrderBy(s => s.Id);
            IList<PuntoFacturacionModel> puntosFacturacion = puntoFacturacionMapper.GetListaPuntoFacturacionModel(puntoFacturacionService.ListAll()); // Filtrar por compania
            ViewBag.ListaPuntoFacturacion = ObtenerListaPuntoFacturacion(puntosFacturacion);
            model.FechaInicio = DateTime.Now;
           
            return View(model);
        }

        /// <summary>
        /// Este método filtra los puntos de facturación que son permitidos para la compañía que se encuentra en sesion
        /// </summary>
        /// <returns>Lista de puntos de facturacion filtrada</returns>
        public List<PuntoFacturacionModel> ObtenerListaPuntoFacturacion(IList<PuntoFacturacionModel> puntosFacturacion)
        {
            List<PuntoFacturacionModel> puntoFacturacionFiltrada = new List<PuntoFacturacionModel>();
            var compania = Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0;
            foreach (var item in puntosFacturacion)
            {
                var companias = item.Companias;
                int i = 0;
                bool listo = false;
                while (i < companias.Count && listo == false)
                {
                    if (companias[i].Id == compania)
                    {
                        puntoFacturacionFiltrada.Add(item);
                        listo = true;
                    }
                    i++;
                }
            }

            return puntoFacturacionFiltrada;
        }

        /// <summary>
        /// Este método guarda la información del modelo del contrato (encabezado) ingresada en el formulario de Crear
        /// </summary>
        /// <returns>La vista Detalles con el id del nuevo contrato</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CONTRATOS + "," + WPPConstants.ROLES_EDIT_CONTRATOS)]
        public ActionResult Crear(ContratoModel contrato)
        {
            if (ModelState.IsValid)
            {                
                Contrato nuevaContrato = new Contrato();
                nuevaContrato = contratoMapper.GetContrato(contrato, nuevaContrato);
                nuevaContrato.Version = 1;
                nuevaContrato.CreateDate = DateTime.Now;
                nuevaContrato.DateLastModified = DateTime.Now;
                nuevaContrato.CreatedBy = NombreUsuarioActual();
                nuevaContrato.Compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);

                //if (contrato.Estado > 0)
                //{
                //    nuevaContrato.Estado = CatalogoService.Get(contrato.Estado);
                //}

                if (contrato.NumeroCliente > 0)
                {
                    nuevaContrato.Cliente = clienteService.Get(contrato.NumeroCliente);
                }

                if (contrato.DiaCorteSemana.HasValue)
                {
                    nuevaContrato.DiaCorteSemana = CatalogoService.Get(contrato.DiaCorteSemana.Value);
                }
                
                if (contrato.PuntoFacturacion > 0)
                {
                    nuevaContrato.PuntoFacturacion = puntoFacturacionService.Get(contrato.PuntoFacturacion);
                }

                if (contrato.Repesaje >0)
                {
                    nuevaContrato.Repesaje = CatalogoService.Get(contrato.Repesaje);
                }
                if (contrato.Ejecutivo > 0)
                {
                    nuevaContrato.Ejecutivo = usuarioService.Get(contrato.Ejecutivo);
                }


                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Compania", nuevaContrato.Compania);

                ConsecutivoContrato consecutivo = consecutivoService.Get(criteria);
                nuevaContrato.Numero = consecutivo.Secuencia;

                //Se actualiza el consecutivo de la boleta
                consecutivo.Secuencia++;
                consecutivoService.Update(consecutivo);

                // Se crea el producto
                nuevaContrato = contratoService.Create(nuevaContrato);

                // Se incluye el contrato al cliente
                var Cliente = nuevaContrato.Cliente;
                Cliente.Contratos.Add(nuevaContrato);
                clienteService.Update(Cliente);

                ViewBag.Mensaje = "Se ha creado el nuevo contrato";

                //return Detalles(nuevaContrato.Id);    
                return RedirectToAction("Detalles", new { idContrato = nuevaContrato.Id });
            }
            else
            {
                ViewBag.ListaEstado = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("EstadoContrato"));
                ViewBag.ListaRepesaje = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("Repesaje"));
                ViewBag.ListaCorteSemana = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("DiaSemana")).OrderBy(s => s.Id);
                IList<PuntoFacturacionModel> puntosFacturacion = puntoFacturacionMapper.GetListaPuntoFacturacionModel(puntoFacturacionService.ListAll()); // Filtrar por compania
                ViewBag.ListaPuntoFacturacion = ObtenerListaPuntoFacturacion(puntosFacturacion);
               
                return View();
            }
        }

        /// <summary>
        /// Este método carga la información del contrato que se desea editar, por medio de su idy
        /// cargar los viewbags necesarios(Estado, Repesaje, CorteSemana, Punto Facturacion) 
        /// </summary>
        /// <returns>La vista Editar con el modelo del contenedor a editar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CONTRATOS + "," + WPPConstants.ROLES_EDIT_CONTRATOS)]
        public ActionResult Editar(long idContrato)
        {
            ContratoModel contrato = contratoMapper.GetContratoModel(contratoService.Get(idContrato));
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("IsDeleted", false);
            ViewBag.ListaEjecutivos = usuarioMapper.GetListaUsuarioModel(usuarioService.GetAll(criteria));
           // ViewBag.ListaEstado = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("EstadoContrato"));
            ViewBag.ListaRepesaje = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("Repesaje"));
            ViewBag.ListaCorteSemana = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("DiaSemana")).OrderBy(s => s.Id);
            IList<PuntoFacturacionModel> puntosFacturacion = puntoFacturacionMapper.GetListaPuntoFacturacionModel(puntoFacturacionService.ListAll()); // Filtrar por compania
            ViewBag.ListaPuntoFacturacion = ObtenerListaPuntoFacturacion(puntosFacturacion);

            return View(contrato);
        }

        /// <summary>
        /// Este método actualiza la información del contrato seleccionado, según lo datos del modelo
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CONTRATOS + "," + WPPConstants.ROLES_EDIT_CONTRATOS)]
        public ActionResult Editar(ContratoModel contratoModel)
        {
            Contrato contrato = contratoService.Get(contratoModel.Id);
            GuardarBitacora(contrato, contratoModel);
            Compania compania = contrato.Compania;
            contrato = contratoMapper.GetContrato(contratoModel, contrato);
            contrato.DateLastModified = DateTime.Now;
            contrato.ModifiedBy = NombreUsuarioActual();
            contrato.Version++;
            contrato.Compania = compania;

            //if (contratoModel.Estado > 0)
            //{
            //    contrato.Estado = CatalogoService.Get(contratoModel.Estado);
            //}

            if (contratoModel.NumeroCliente > 0)
            {
                contrato.Cliente = clienteService.Get(contratoModel.NumeroCliente);
            }

            if (contratoModel.DiaCorteSemana.HasValue)
            {
                contrato.DiaCorteSemana = CatalogoService.Get(contratoModel.DiaCorteSemana.Value);
            }

            if (contratoModel.PuntoFacturacion > 0)
            {
                contrato.PuntoFacturacion = puntoFacturacionService.Get(contratoModel.PuntoFacturacion);
            }

            if (contratoModel.Repesaje > 0)
            {
                contrato.Repesaje = CatalogoService.Get(contratoModel.Repesaje);
            }

            if (contratoModel.Ejecutivo > 0)
            {
                contrato.Ejecutivo = usuarioService.Get(contratoModel.Ejecutivo);
            }

            // Se actualiza la información del contrato
            contrato = contratoService.Update(contrato);

            //return Detalles(contrato.Id);
            return RedirectToAction("Detalles", new { idContrato = contrato.Id });
        }


        public void GuardarBitacora(Contrato contrato, ContratoModel model)
        {
            ContratoBitacora bitacora = new ContratoBitacora();
            string usuario = ObtenerUsuarioActual().Nombre;

            if (contrato.Numero != model.Numero)
            {
                bitacora = new ContratoBitacora();
                bitacora.CreateDate = DateTime.Now;
                bitacora.CreatedBy = usuario;
                bitacora.Campo = "Número Contrato";
                bitacora.valorAnterior = contrato.Numero != null ? contrato.Numero.ToString() : String.Empty;
                bitacora.valorNuevo = model.Numero != null ? model.Numero.ToString() : String.Empty;
                bitacora.Contrato = contrato;
                bitacoraService.Create(bitacora);
            }

            if (contrato.DescripcionContrato != model.DescripcionContrato)
            {
                bitacora = new ContratoBitacora();
                bitacora.CreateDate = DateTime.Now;
                bitacora.CreatedBy = usuario;
                bitacora.Campo = "Descripción Contrato";
                bitacora.valorAnterior = contrato.DescripcionContrato;
                bitacora.valorNuevo = model.DescripcionContrato;
                bitacora.Contrato = contrato;
                bitacoraService.Create(bitacora);
            }
            if (contrato.Estado != model.Estado)
            {
                bitacora = new ContratoBitacora();
                bitacora.CreateDate = DateTime.Now;
                bitacora.CreatedBy = usuario;
                bitacora.Campo = "Estado";
                bitacora.valorAnterior = contrato.Estado;
                bitacora.valorNuevo = model.Estado;
                bitacora.Contrato = contrato;
                bitacoraService.Create(bitacora);
            }
            if (contrato.ModoFacturacion != model.ModoFacturacion)
            {
                bitacora = new ContratoBitacora();
                bitacora.CreateDate = DateTime.Now;
                bitacora.CreatedBy = usuario;
                bitacora.Campo = "Modo de Facturación";
                bitacora.valorAnterior = contrato.ModoFacturacion;
                bitacora.valorNuevo = model.ModoFacturacion;
                bitacora.Contrato = contrato;
                bitacoraService.Create(bitacora);
            }
            if (contrato.Observaciones != model.Observaciones)
            {
                bitacora = new ContratoBitacora();
                bitacora.CreateDate = DateTime.Now;
                bitacora.CreatedBy = usuario;
                bitacora.Campo = "Observaciones";
                bitacora.valorAnterior = contrato.Observaciones;
                bitacora.valorNuevo = model.Observaciones;
                bitacora.Contrato = contrato;
                bitacoraService.Create(bitacora);
            }

            if (contrato.ObservacionesFactura != model.ObservacionesFactura)
            {
                bitacora = new ContratoBitacora();
                bitacora.CreateDate = DateTime.Now;
                bitacora.CreatedBy = usuario;
                bitacora.Campo = "Observaciones Factura";
                bitacora.valorAnterior = contrato.ObservacionesFactura;
                bitacora.valorNuevo = model.ObservacionesFactura;
                bitacora.Contrato = contrato;
                bitacoraService.Create(bitacora);
            }
            if (contrato.DiaCorteMes != model.DiaCorteMes)
            {
                bitacora = new ContratoBitacora();
                bitacora.CreateDate = DateTime.Now;
                bitacora.CreatedBy = usuario;
                bitacora.Campo = "Día del Corte Mes";
                bitacora.valorAnterior = contrato.DiaCorteMes != null ? contrato.DiaCorteMes.ToString() : String.Empty;
                bitacora.valorNuevo = model.DiaCorteMes != null ? model.DiaCorteMes.ToString() : String.Empty;
                bitacora.Contrato = contrato;
                bitacoraService.Create(bitacora);
            }
            if (contrato.DiasAvisoPrevioVencimiento != model.DiasAvisoPrevioVencimiento)
            {
                bitacora = new ContratoBitacora();
                bitacora.CreateDate = DateTime.Now;
                bitacora.CreatedBy = usuario;
                bitacora.Campo = "Días de Aviso Previo al Vencimiento ";
                bitacora.valorAnterior = contrato.DiasAvisoPrevioVencimiento != null ? contrato.DiasAvisoPrevioVencimiento.ToString() : String.Empty;
                bitacora.valorNuevo = model.DiasAvisoPrevioVencimiento != null ? model.DiasAvisoPrevioVencimiento.ToString() : String.Empty;
                bitacora.Contrato = contrato;
                bitacoraService.Create(bitacora);
            }
            if (contrato.NumeroFormulario != model.NumeroFormulario)
            {
                bitacora = new ContratoBitacora();
                bitacora.CreateDate = DateTime.Now;
                bitacora.CreatedBy = usuario;
                bitacora.Campo = "Número Formulario";
                bitacora.valorAnterior = contrato.NumeroFormulario != null ? contrato.NumeroFormulario.ToString() : String.Empty;
                bitacora.valorNuevo = model.NumeroFormulario != null ? model.NumeroFormulario.ToString() : String.Empty;
                bitacora.Contrato = contrato;
                bitacoraService.Create(bitacora);
            }
            if (contrato.Numero != model.Numero)
            {
                bitacora = new ContratoBitacora();
                bitacora.CreateDate = DateTime.Now;
                bitacora.CreatedBy = usuario;
                bitacora.Campo = "Número Contrato";
                bitacora.valorAnterior = contrato.Numero != null ? contrato.Numero.ToString() : String.Empty;
                bitacora.valorNuevo = model.Numero != null ? model.Numero.ToString() : String.Empty;
                bitacora.Contrato = contrato;
                bitacoraService.Create(bitacora);
            }
            if (contrato.FechaInicio != model.FechaInicio)
            {
                bitacora = new ContratoBitacora();
                bitacora.CreateDate = DateTime.Now;
                bitacora.CreatedBy = usuario;
                bitacora.Campo = "Fecha Inicio";
                bitacora.valorAnterior = contrato.FechaInicio != null ? contrato.FechaInicio.ToString() : String.Empty;
                bitacora.valorNuevo = model.FechaInicio != null ? model.FechaInicio.ToString() : String.Empty;
                bitacora.Contrato = contrato;
                bitacoraService.Create(bitacora);

            }
            if (contrato.Moneda != model.Moneda)
            {
                bitacora = new ContratoBitacora();
                bitacora.CreateDate = DateTime.Now;
                bitacora.CreatedBy = usuario;
                bitacora.Campo = "Moneda";
                bitacora.valorAnterior = contrato.Moneda;
                bitacora.valorNuevo = model.Moneda;
                bitacora.Contrato = contrato;
                bitacoraService.Create(bitacora);
            }
            if (contrato.PagoContado != model.PagoContado)
            {
                bitacora = new ContratoBitacora();
                bitacora.CreateDate = DateTime.Now;
                bitacora.CreatedBy = usuario;
                bitacora.Campo = "Pago Contado";
                bitacora.valorAnterior = contrato.PagoContado != null ? contrato.PagoContado == true ? "Si" : "No" : String.Empty;
                bitacora.valorNuevo = model.PagoContado != null ? model.PagoContado == true ? "Si" : "No" : String.Empty;
                bitacora.Contrato = contrato;
                bitacoraService.Create(bitacora);
            }

            if (contrato.FacturarColones != model.FacturarColones)
            {
                bitacora = new ContratoBitacora();
                bitacora.CreateDate = DateTime.Now;
                bitacora.CreatedBy = usuario;
                bitacora.Campo = "Facturar en Colones";
                bitacora.valorAnterior = contrato.FacturarColones != null ? contrato.FacturarColones == true ? "Si" : "No" : String.Empty;
                bitacora.valorNuevo = model.FacturarColones != null ? model.FacturarColones == true ? "Si" : "No" : String.Empty;
                bitacora.Contrato = contrato;
                bitacoraService.Create(bitacora);
            }

            if (contrato.DiaCorteEsMes != model.DiaCorteEsMes)
            {
                bitacora = new ContratoBitacora();
                bitacora.CreateDate = DateTime.Now;
                bitacora.CreatedBy = usuario;
                bitacora.Campo = "Día de corte es mes";
                bitacora.valorAnterior = contrato.DiaCorteEsMes != null ? contrato.DiaCorteEsMes == true ? "Si" : "No" : String.Empty;
                bitacora.valorNuevo = model.DiaCorteEsMes != null ? model.DiaCorteEsMes == true ? "Si" : "No" : String.Empty;
                bitacora.Contrato = contrato;
                bitacoraService.Create(bitacora);
            }

            if (contrato.DiaCorteSemana != null)
            {
                if (contrato.DiaCorteSemana.Id != model.DiaCorteSemana)
                {
                    bitacora = new ContratoBitacora();
                    bitacora.CreateDate = DateTime.Now;
                    bitacora.CreatedBy = usuario;
                    bitacora.Campo = "DiaCorteSemana"; 
                    Catalogo diaActual = catalogoService.Get(model.DiaCorteSemana.Value);
                    bitacora.valorAnterior = contrato.DiaCorteSemana != null ? contrato.DiaCorteSemana.Nombre.ToString() : String.Empty;
                    bitacora.valorNuevo = diaActual != null ? diaActual.ToString() : String.Empty;
                    bitacora.Contrato = contrato;
                    bitacoraService.Create(bitacora);
                }
            }

            if (contrato.Cliente != null)
            {
                if (contrato.Cliente.Id != model.NumeroCliente)
                {
                    bitacora = new ContratoBitacora();
                    bitacora.CreateDate = DateTime.Now;
                    bitacora.CreatedBy = usuario;
                    bitacora.Campo = "Cliente";
                    Cliente clienteActual = clienteService.Get(model.NumeroCliente);
                    bitacora.valorAnterior = contrato.Cliente != null ? contrato.Cliente.Nombre.ToString() : String.Empty;
                    bitacora.valorNuevo = clienteActual != null ? clienteActual.Nombre.ToString() : String.Empty;
                    bitacora.Contrato = contrato;
                    bitacoraService.Create(bitacora);
                }
            }

            if (contrato.PuntoFacturacion != null)
            {
                if (contrato.PuntoFacturacion.Id != model.PuntoFacturacion)
                {
                    bitacora = new ContratoBitacora();
                    bitacora.CreateDate = DateTime.Now;
                    bitacora.CreatedBy = usuario;
                    bitacora.Campo = "Punto de Facturación";
                    PuntoFacturacion puntoActual = puntoFacturacionService.Get(model.PuntoFacturacion);
                    bitacora.valorAnterior = contrato.PuntoFacturacion != null ? contrato.PuntoFacturacion.Nombre.ToString() : String.Empty;
                    bitacora.valorNuevo = puntoActual != null ? puntoActual.Nombre.ToString() : String.Empty;
                    bitacora.Contrato = contrato;
                    bitacoraService.Create(bitacora);
                }
            }

            if (contrato.Repesaje != null)
            {
                if (contrato.Repesaje.Id != model.Repesaje)
                {
                    bitacora = new ContratoBitacora();
                    bitacora.CreateDate = DateTime.Now;
                    bitacora.CreatedBy = usuario;
                    bitacora.Campo = "Repesaje";
                    Catalogo repesajeActual = catalogoService.Get(model.Repesaje);
                    bitacora.valorAnterior = contrato.Repesaje != null ? contrato.Repesaje.Nombre.ToString() : String.Empty;
                    bitacora.valorNuevo = repesajeActual != null ? repesajeActual.Nombre.ToString() : String.Empty;
                    bitacora.Contrato = contrato;
                    bitacoraService.Create(bitacora);
                }
            }

            if (contrato.Ejecutivo != null)
            {
                if (contrato.Ejecutivo.Id != model.Ejecutivo)
                {
                    bitacora = new ContratoBitacora();
                    bitacora.CreateDate = DateTime.Now;
                    bitacora.CreatedBy = usuario;
                    bitacora.Campo = "Ejecutivo";
                    Usuario usuarioActual = usuarioService.Get(model.Ejecutivo);
                    bitacora.valorAnterior = contrato.Ejecutivo != null ? contrato.Ejecutivo.Nombre.ToString() : String.Empty;
                    bitacora.valorNuevo = usuarioActual != null ? usuarioActual.Nombre.ToString() : String.Empty;
                    bitacora.Contrato = contrato;
                    bitacoraService.Create(bitacora);
                }
            }
        }


        /// <summary>
        /// Este método carga la información del contrato que se desea eliminar, esto según el id
        /// </summary>
        /// <returns>La vista Eliminar con el modelo del contrato a eliminar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CONTRATOS)]
        public ActionResult Eliminar(long idContrato)
        {
            Contrato contrato = contratoService.Get(idContrato);
            ContratoModel contratoModel = contratoMapper.GetContratoModel(contrato);

            return View(contratoModel);
        }

        /// <summary>
        /// Este método actualiza el estado IsDeleted a true, con el fin de indicar que este contrato fue eliminado
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CONTRATOS)]
        public ActionResult Eliminar(ContratoModel contratoModel)
        {
            Contrato contrato = contratoService.Get(contratoModel.Id);
            contrato.IsDeleted = true;
            contrato.DateLastModified = DateTime.Now;
            contrato.Version++;
            contrato.DeletedBy = NombreUsuarioActual();
            contrato.DeleteDate = DateTime.Now;

            contratoService.Update(contrato);

            //return Index();
            return RedirectToAction("Index", "Contrato"); 
        }

        /// <summary>
        /// Este método carga el modelo del contrato que se desea obtener el detalle
        /// </summary>
        /// <returns>La vista de Detalles con el modelo de contrato que se desea consultar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CONTRATOS + "," + WPPConstants.ROLES_EDIT_CONTRATOS + "," + WPPConstants.ROLES_CONS_CONTRATOS)]
        public ActionResult Detalles(long idContrato)
        {
            Contrato contrato = contratoService.Get(idContrato);
            ContratoModel contratoModel = contratoMapper.GetContratoModel(contrato);
            return View("Detalles", contratoModel);
        }

        /// <summary>
        /// Este método lista y filtra el listado de registros de contratos mostrados en el index
        /// </summary>
        /// <returns>Vista Index con el modelo IPageList(Contrato)</returns>
        public ActionResult Buscar(string sortOrder, string currentFilterClienteNumero, string currentFilterClienteDescripcion, string currentFilterContrato, string currentFilterDescripcionContrato, string currentFilterPuntoFacturacion, string currentFilterEstado,
                   string searchStringClienteNumero, string searchStringClienteDescripcion, string searchStringContrato, string searchStringDescripcionContrato, string searchStringPuntoFacturacion, string searchStringEstado, string searchStringFilas, int? page)
        {
            ViewBag.CurrentSort = sortOrder;

            if (sortOrder == "ClienteDescripcionAsc")
                ViewBag.ClienteDescripcionSortParam = "ClienteDescripcionDesc";
            else
                ViewBag.ClienteDescripcionSortParam = "ClienteDescripcionAsc";

            if (sortOrder == "ClienteNumeroAsc")
                ViewBag.ClienteNumeroSortParam = "ClienteNumeroDesc";
            else
                ViewBag.ClienteNumeroSortParam = "ClienteNumeroAsc";

            if (sortOrder == "ContratoAsc")
                ViewBag.ContratoSortParam = "ContratoDesc";
            else
                ViewBag.ContratoSortParam = "ContratoAsc";

            if (sortOrder == "PuntoFacturacionAsc")
                ViewBag.PuntoFacturacionSortParam = "PuntoFacturacionDesc";
            else
                ViewBag.PuntoFacturacionSortParam = "PuntoFacturacionAsc";

            if (sortOrder == "EstadoAsc")
                ViewBag.EstadoSortParam = "EstadoDesc";
            else
                ViewBag.EstadoSortParam = "EstadoAsc";

            if (sortOrder == "DescripcionContratoAsc")
                ViewBag.DescripcionContratoSortParam = "DescripcionContratoDesc";
            else
                ViewBag.DescripcionContratoSortParam = "DescripcionContratoAsc";


            if (searchStringClienteDescripcion != null || searchStringClienteNumero != null || searchStringEstado != null || searchStringContrato != null || searchStringPuntoFacturacion != null || searchStringDescripcionContrato != null)
                page = 1;
            else
            {
                searchStringClienteDescripcion = currentFilterClienteDescripcion;
                searchStringEstado = currentFilterEstado;
                searchStringClienteNumero = currentFilterClienteNumero;
                searchStringContrato = currentFilterContrato;
                searchStringPuntoFacturacion = currentFilterPuntoFacturacion;
                searchStringDescripcionContrato = currentFilterDescripcionContrato;
            }

            ViewBag.CurrentClienteDescripcion = searchStringClienteDescripcion;
            ViewBag.CurrentEstado = searchStringEstado;
            ViewBag.CurrentClienteNumero = searchStringClienteNumero;
            ViewBag.CurrentContrato = searchStringContrato;
            ViewBag.CurrentPuntoFacturacion = searchStringPuntoFacturacion;
            ViewBag.CurrentFormulario = searchStringDescripcionContrato;

            var filas = searchStringFilas == null ? 10 : Convert.ToInt32(searchStringFilas);
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var compania = Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0;
            var paginaContrato = contratoService.PagingSearch(searchStringClienteNumero, searchStringClienteDescripcion, searchStringEstado, searchStringContrato, searchStringDescripcionContrato, searchStringPuntoFacturacion, pageNumber, filas, sortOrder, compania);

           
            return View("Index", paginaContrato);
        }

        /// <summary>
        ///Filtra los clientes que contengan en su nombre la cadena de caracteres que se ha digitado
        /// </summary>
        /// <returns>json con la lista de clientes filtrado por su nombre</returns>
        [HttpGet]
        public JsonResult AutoCompleteCliente(string term)
        {
            var listaClientes = clienteService.ClienteSearch(term);
            var result = (from e in listaClientes select new { id = e.Id, value = e.Numero + " - " + e.Nombre, label = e.Numero + " - " + e.Nombre });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Este método lista y filtra los contratos que pertenecen al cliente indicado (idCliente)
        /// </summary>
        /// <returns>json con la lista de contoratos asociados al cliente</returns>
        [HttpPost]
        public JsonResult CargarContratos(long idCliente)
        {
            try
            {
                Cliente cliente = clienteService.Get(idCliente);
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Cliente", cliente);
                var numCompania = Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0;
                Compania compania = companiaService.Get(numCompania);
                criteria.Add("Compania", compania);

                List<ContratoModel> contratos = new List<ContratoModel>();
                var listaContratos = contratoMapper.GetListaClienteModel(contratoService.GetAll(criteria)).Where(s => s.Estado != "Inactivo").ToList();

                foreach (var item in listaContratos)
                {
                    //if (item.EstadoDescripcion == "Activo")
                    //{
                        contratos.Add(
                            new ContratoModel
                            {
                                Id = item.Id,
                                DescripcionContrato = item.DescripcionContrato
                            }
                         );
                    //}
                }
                return Json(contratos, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(null);
            }
        }

        /// <summary>
        /// Este método lista y filtra los contratos que pertenecen al cliente indicado (idCliente)
        /// </summary>
        /// <returns>json con la lista de contoratos asociados al cliente</returns>
        [HttpPost]
        public JsonResult CargarContratosProyecto(long idCliente)
        {
            try
            {
                Cliente cliente = clienteService.Get(idCliente);
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                //criteria.Add("Cliente", cliente);
                var numCompania = Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0;
                Compania compania = companiaService.Get(numCompania);
                criteria.Add("Proyecto", compania);
                criteria.Add("IsDeleted", false);

                List<ContratoModel> contratos = new List<ContratoModel>();
                IList<ProductoContrato> listaContratos = productoContratoService.GetAll(criteria);

                foreach (var item in listaContratos)
                {
                    var cantContrato = contratos.Where(s => s.Id == item.Contrato.Id).ToList().Count;
                    if (cantContrato == 0)
                    {
                        if (item.Contrato.Cliente.Id == idCliente && item.Contrato.Estado != "Inactivo")
                        {                           
                            contratos.Add(
                                new ContratoModel
                                {
                                    Id = item.Contrato.Id,
                                    DescripcionContrato = item.Contrato.DescripcionContrato
                                });                            
                        }
                    }
                }
                return Json(contratos, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(null);
            }
        }

        /// <summary>
        /// Este método carga el modelo del equipo que se desea obtener la información por medio de su placa
        /// </summary>
        /// <returns>Un json con el modelo de equipo que se desea consultar</returns>
        [HttpPost]
        public JsonResult FiltrarContratos(string busqueda, string tipo)
        {
            try
            {
                string compania = Session["Compania"] != null ? Session["Compania"].ToString() : "0";
                var Lista = contratoService.Filtrar(busqueda, compania, tipo);
                if (Lista == null)
                {
                    return Json(null);                         
                }

                List<Contrato> ListaContratos = new List<Contrato>();
                foreach (Contrato item in Lista)
                {
                    Contrato contrato = new Contrato();
                    contrato.Numero = item.Numero;
                    contrato.DescripcionContrato = item.DescripcionContrato;
                    contrato.Estado = item.Estado;
                    contrato.Cliente = new Cliente();
                    contrato.Cliente.Numero = item.Cliente.Numero;
                    contrato.Cliente.Nombre = item.Cliente.Nombre;
                    contrato.Productos = new List<ProductoContrato>();

                    foreach (ProductoContrato itemProducto in item.Productos)
                    {
                        if (itemProducto.Producto.Categoria.Tipo == "Recoleccion")
                        { 
                            ProductoContrato producto = new ProductoContrato();

                            producto.Id = itemProducto.Id;
                            producto.Ubicacion = new UbicacionCliente();
                            producto.Descripcion = itemProducto.Descripcion;
                            if (itemProducto.Ubicacion != null)
                            {
                                producto.Ubicacion.Id = itemProducto.Ubicacion.Id;
                                producto.Ubicacion.Descripcion = itemProducto.Ubicacion.Descripcion;
                            }
                            else
                            {
                                producto.Ubicacion.Id = 0;
                                producto.Ubicacion.Descripcion = String.Empty;
                            }

                            contrato.Productos.Add(producto);

                        }
                    }
                    ListaContratos.Add(contrato);                    
                }

                return Json(ListaContratos, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(null);
            }
        }


        /// <summary>
        /// Este método carga el modelo del equipo que se desea obtener la información por medio de su placa
        /// </summary>
        /// <returns>Un json con el modelo de equipo que se desea consultar</returns>
        [HttpPost]
        public JsonResult FiltrarContratosProyecto(string busqueda, string tipo)
        {
            try
            {
                string compania = Session["Compania"] != null ? Session["Compania"].ToString() : "0";
                var Lista = contratoService.Filtrar(busqueda, null, tipo);
                if (Lista == null)
                {
                    return Json(null);
                }

                List<Contrato> ListaContratos = new List<Contrato>();
                foreach (Contrato item in Lista)
                {
                    Contrato contrato = new Contrato();
                    contrato.Numero = item.Numero;
                    contrato.DescripcionContrato = item.DescripcionContrato;
                    //contrato.Estado = new Catalogo();
                    contrato.Estado = item.Estado;
                    contrato.Cliente = new Cliente();
                    contrato.Cliente.Numero = item.Cliente.Numero;
                    contrato.Cliente.Nombre = item.Cliente.Nombre;
                    contrato.Productos = new List<ProductoContrato>();

                    foreach (ProductoContrato itemProducto in item.Productos)
                    {
                        if(itemProducto.Proyecto.Id == Convert.ToInt64(compania))
                        {
                            if (itemProducto.Producto.Categoria.Tipo == "Recoleccion")
                            {
                                ProductoContrato producto = new ProductoContrato();

                                producto.Id = itemProducto.Id;
                                producto.Ubicacion = new UbicacionCliente();
                                producto.Descripcion = itemProducto.Descripcion;
                                if (itemProducto.Ubicacion != null)
                                {
                                    producto.Ubicacion.Id = itemProducto.Ubicacion.Id;
                                    producto.Ubicacion.Descripcion = itemProducto.Ubicacion.Descripcion;
                                }
                                else
                                {
                                    producto.Ubicacion.Id = 0;
                                    producto.Ubicacion.Descripcion = String.Empty;
                                }

                                contrato.Productos.Add(producto);
                            }
                        }
                    }
                    ListaContratos.Add(contrato);
                }

                return Json(ListaContratos, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(null);
            }
        }

        /// <summary>
        /// Este método carga los contratos 
        /// </summary>
        /// <returns>Un json con el modelo de equipo que se desea consultar</returns>
        [HttpPost]
        public JsonResult BusquedaContratos(string busqueda, string moneda)
        {
            try
            {
                string compania = Session["Compania"] != null ? Session["Compania"].ToString() : "0";
                var Lista = contratoService.Filtrar(busqueda, compania, String.Empty);
                if (Lista == null)
                {
                    return Json(null);
                }

                if(!String.IsNullOrEmpty(moneda))
                {
                    Lista = Lista.Where(s => s.Moneda == moneda).ToList();
                }

                IList<ContratoModel> ListaContratosModel = new List<ContratoModel>();

                foreach (var item in Lista)
                {
                    ContratoModel model = new ContratoModel();
                    model.Id = item.Id;
                    model.Estado = item.Estado;//.Id;
                    //model.EstadoDescripcion = item.Estado.Nombre;
                    model.FechaInicio = item.FechaInicio;
                    model.FacturarColones = item.FacturarColones;
                    model.DescripcionCliente = item.Cliente.Numero + " - " + item.Cliente.Nombre;
                    model.NumeroCliente = item.Cliente.Id;
                    model.DescripcionContrato = item.Numero + " - " + item.DescripcionContrato;
                    model.Numero = item.Numero;

                    ListaContratosModel.Add(model);                    
                }

                return Json(ListaContratosModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(null);
            }
        }

        #endregion

        #region PRODUCTO CONTRATO
        /// <summary>
        /// Este método se encarga de retornar la  vista que permite crear productos al contrato
        /// y carga los viewbags necesarios (Productos, Servicios, CategoriaProducto, Ubicacion, EsquemaRelevancia, Frecuencia, RutaRecoleecion, Contrato)
        /// </summary>
        /// <returns>La vista Crear</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CONTRATOS + "," + WPPConstants.ROLES_EDIT_CONTRATOS)]
        public ActionResult CrearProductoContrato(long IdContrato)
        {
            ProductoContratoModel model = new ProductoContratoModel();
            Contrato contrato = contratoService.Get(IdContrato);

            IDictionary<string, object> criteriaProducto = new Dictionary<string, object>();
            criteriaProducto.Add("Compania", contrato.Compania.Id);
             criteriaProducto.Add("IsDeleted", false);
            ViewBag.ListaProducto = productoMapper.GetListaProductoModel(productoService.GetAll(criteriaProducto));

            ViewBag.ListaCategoriaProducto = categoriaProductoMapper.GetListaCategoriaProductoModel(categoriaProductoService.ListAll());

            IDictionary<string, object> criteriaServicio = new Dictionary<string, object>();
            criteriaServicio.Add("Compania", companiaService.Get(contrato.Compania.Id));
            criteriaServicio.Add("IsDeleted", false);
            ViewBag.ListaServicio = servicioMapper.GetListaServicioModel(servicioService.GetAll(criteriaServicio));

            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("IsDeleted", false);
            criteria.Add("Cliente", contrato.Cliente);
            ViewBag.ListaUbicacion = ubicacionMapper.GetListaProductoModel(ubicacionService.GetAll(criteria)).OrderBy(s => s.Id);


            IDictionary<string, object> criteriaCompanias = new Dictionary<string, object>();
            criteriaCompanias.Add("IsDeleted", false);
            ViewBag.ListaProyectos = companiaMapper.GetListaCompaniaModel(companiaService.GetAll(criteriaCompanias));

            ViewBag.ListaEsquemaRelevancia = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("EsquemaRevaloracion")).OrderBy(s => s.Id);
            ViewBag.ListaFrecuencia = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("Frecuencia")).OrderBy(s => s.Id);
            ViewBag.ListaRutaRecoleccion = rutaMapper.GetListaRutaRecoleccionModel(rutaService.ListAll()).OrderBy(s => s.Id);
            ViewBag.ListaProductoRecoleccion = new List<ProductoContrato>();
            model.FechaFinal = DateTime.Now;
            model.FechaInicial = DateTime.Now;
            ViewBag.Contrato = contrato.DescripcionContrato;
            model.Contrato = IdContrato;
            model.Moneda = contrato.Moneda;
            return View(model);
        }

        /// <summary>
        /// Este método guarda la información del modelo ingresada en el formulario de Crear
        /// </summary>
        /// <returns>vista Detalles con el id del contrato</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CONTRATOS + "," + WPPConstants.ROLES_EDIT_CONTRATOS)]
        public ActionResult CrearProductoContrato(ProductoContratoModel productoModel)
        {
            Contrato contrato = contratoService.Get(productoModel.Contrato);

            if (ModelState.IsValid)
            {
                ProductoContrato producto = new ProductoContrato();
                producto = productoContratoMapper.GetProductoContrato(productoModel, producto);

                producto.Contrato = contrato;
                contrato.Productos.Add(producto);

                producto.DateLastModified = DateTime.Now;
                producto.CreatedBy = NombreUsuarioActual();
                producto.CreateDate = DateTime.Now;
                producto.FechaEstado = productoModel.Estado == "Activo" ? productoModel.FechaInicial : productoModel.FechaFinal;

                if (productoModel.EsquemaRelevancia.HasValue)
                    producto.EsquemaRelevancia = CatalogoService.Get(productoModel.EsquemaRelevancia.Value);

                if (productoModel.Frecuecia.HasValue)
                    producto.Frecuecia = CatalogoService.Get(productoModel.Frecuecia.Value);

                if (productoModel.RutaRecoleccion.HasValue)
                    producto.RutaRecoleccion = rutaService.Get(productoModel.RutaRecoleccion.Value);

                if (productoModel.Servicio > 0)
                    producto.Servicio = servicioService.Get(productoModel.Servicio);

                if (productoModel.Producto > 0)
                    producto.Producto = productoService.Get(productoModel.Producto);

                if (productoModel.Proyecto > 0)
                    producto.Proyecto = companiaService.Get(productoModel.Proyecto);

                if (productoModel.Ubicacion > 0)
                    producto.Ubicacion = ubicacionService.Get(productoModel.Ubicacion);

                if (productoModel.ProductoFosa.HasValue)
                    producto.ProductoFosa = productoService.Get(productoModel.ProductoFosa.Value);

                if (productoModel.Recoleccion.HasValue)
                    producto.Recoleccion = productoContratoService.Get(productoModel.Recoleccion.Value);

                productoContratoService.Create(producto);

                contratoService.Update(contrato);

                // return Detalles(contrato.Id);
                return RedirectToAction("Detalles", new { idContrato = contrato.Id });
            }
            else
            {
                ViewBag.ListaCategoriaProducto = categoriaProductoMapper.GetListaCategoriaProductoModel(categoriaProductoService.ListAll());

                IDictionary<string, object> criteriaProducto = new Dictionary<string, object>();
                criteriaProducto.Add("Compania", contrato.Compania.Id);
                criteriaProducto.Add("IsDeleted", false);
                ViewBag.ListaProducto = productoMapper.GetListaProductoModel(productoService.GetAll(criteriaProducto));

                IDictionary<string, object> criteriaServicio = new Dictionary<string, object>();
                criteriaServicio.Add("Compania", companiaService.Get(contrato.Compania.Id));
                criteriaServicio.Add("IsDeleted", false);
                ViewBag.ListaServicio = servicioMapper.GetListaServicioModel(servicioService.GetAll(criteriaServicio));

                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Cliente", contrato.Cliente);
                criteria.Add("IsDeleted", false);
                ViewBag.ListaUbicacion = ubicacionMapper.GetListaProductoModel(ubicacionService.GetAll(criteria)).OrderBy(s => s.Id);

                ViewBag.ListaEsquemaRelevancia = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("EsquemaRevaloracion")).OrderBy(s => s.Id);
                ViewBag.ListaFrecuencia = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("Frecuencia")).OrderBy(s => s.Id);
                ViewBag.ListaRutaRecoleccion = rutaMapper.GetListaRutaRecoleccionModel(rutaService.ListAll()).OrderBy(s => s.Id);
                ViewBag.ListaProductoRecoleccion = new List<ProductoContrato>();
                ViewBag.Contrato = contrato.DescripcionContrato;

                return View("CrearProductoContrato", productoModel);
            }
        }

        /// <summary>
        /// Este método carga la información del producto del contrato que se desea editar, por medio de su id
        /// y carga los viewbags necesarios (Productos, Servicios, CategoriaProducto, Ubicacion, EsquemaRelevancia, Frecuencia, RutaRecoleecion, Contrato)
        /// </summary>
        /// <returns>La vista Editar con el modelo del producto del contrato a editar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CONTRATOS + "," + WPPConstants.ROLES_EDIT_CONTRATOS)]
        public ActionResult EditarProductoContrato(long idProducto)
        {
            ProductoContrato producto = productoContratoService.Get(idProducto);
            ProductoContratoModel productoModel = productoContratoMapper.GetProductoContratoModel(producto);

            IDictionary<string, object> criteriaProducto = new Dictionary<string, object>();
            criteriaProducto.Add("Compania", producto.Contrato.Compania.Id);
            criteriaProducto.Add("IsDeleted", false);
            ViewBag.ListaProducto = productoMapper.GetListaProductoModel(productoService.GetAll(criteriaProducto));

            ViewBag.ListaCategoriaProducto = categoriaProductoMapper.GetListaCategoriaProductoModel(categoriaProductoService.ListAll());

            IDictionary<string, object> criteriaServicio = new Dictionary<string, object>();
            criteriaServicio.Add("Compania", companiaService.Get(producto.Contrato.Compania.Id));
            criteriaServicio.Add("IsDeleted", false);
            ViewBag.ListaServicio = servicioMapper.GetListaServicioModel(servicioService.GetAll(criteriaServicio));

            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Cliente", producto.Contrato.Cliente);
            criteria.Add("IsDeleted", false);
            ViewBag.ListaUbicacion = ubicacionMapper.GetListaProductoModel(ubicacionService.GetAll(criteria)).OrderBy(s => s.Id);

            IDictionary<string, object> criteriaCompanias = new Dictionary<string, object>();
            criteriaCompanias.Add("IsDeleted", false);
            ViewBag.ListaProyectos = companiaMapper.GetListaCompaniaModel(companiaService.GetAll(criteriaCompanias));

            ViewBag.ListaEsquemaRelevancia = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("EsquemaRevaloracion")).OrderBy(s => s.Id);
            ViewBag.ListaFrecuencia = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("Frecuencia")).OrderBy(s => s.Id);
            ViewBag.ListaRutaRecoleccion = rutaMapper.GetListaRutaRecoleccionModel(rutaService.ListAll()).OrderBy(s => s.Id);

            ViewBag.ListaProductoRecoleccion = new List<ProductoContrato>();
            ViewBag.Contrato = producto.Contrato.DescripcionContrato;
            productoModel.Moneda = producto.Contrato.Moneda;
            productoModel.RecoleccionDescripcion = productoModel.Recoleccion.ToString();


            return View("EditarProductoContrato", productoModel);
        }

        /// <summary>
        /// Este método actualiza la información del producto del contrato seleccionado, según lo datos del modelo
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CONTRATOS + "," + WPPConstants.ROLES_EDIT_CONTRATOS)]
        public ActionResult EditarProductoContrato(ProductoContratoModel productoModel)
        {
            ProductoContrato producto = productoContratoService.Get(productoModel.Id);

            if (ModelState.IsValid)
            {
                producto = productoContratoMapper.GetProductoContrato(productoModel, producto);
                producto.DateLastModified = DateTime.Now;
                producto.ModifiedBy = NombreUsuarioActual();
                producto.Version++;

                if (productoModel.EsquemaRelevancia.HasValue)
                    producto.EsquemaRelevancia = CatalogoService.Get(productoModel.EsquemaRelevancia.Value);

                if (productoModel.Frecuecia.HasValue)
                    producto.Frecuecia = CatalogoService.Get(productoModel.Frecuecia.Value);

                if (productoModel.RutaRecoleccion.HasValue)
                    producto.RutaRecoleccion = rutaService.Get(productoModel.RutaRecoleccion.Value);

                if (productoModel.Servicio > 0)
                    producto.Servicio = servicioService.Get(productoModel.Servicio);

                if (productoModel.Producto > 0)
                    producto.Producto = productoService.Get(productoModel.Producto);

                if (productoModel.Proyecto > 0)
                    producto.Proyecto = companiaService.Get(productoModel.Proyecto);

                if (productoModel.Ubicacion > 0)
                    producto.Ubicacion = ubicacionService.Get(productoModel.Ubicacion);

                if (productoModel.ProductoFosa.HasValue)
                    producto.ProductoFosa = productoService.Get(productoModel.ProductoFosa.Value);

                if (productoModel.Recoleccion.HasValue)
                    producto.Recoleccion = productoContratoService.Get(productoModel.Recoleccion.Value);

                productoContratoService.Update(producto);

                //return Detalles(producto.Contrato.Id);
                return RedirectToAction("Detalles", new { idContrato = producto.Contrato.Id });
            }

            return View("EditarProductoContrato", productoModel);
        }

        /// <summary>
        /// Este método carga la información del producto del contrato que se desea eliminar, esto según el id
        /// </summary>
        /// <returns>La vista Eliminar con el modelo del producto del conotrato a eliminar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CONTRATOS + "," + WPPConstants.ROLES_EDIT_CONTRATOS)]
        public ActionResult EliminarProductoContrato(long idProducto)
        {
            ProductoContrato producto = productoContratoService.Get(idProducto);
            
            ProductoContratoModel productoModel = productoContratoMapper.GetProductoContratoModel(producto);
            ViewBag.Contrato = producto.Contrato.DescripcionContrato;
            productoModel.Moneda = producto.Contrato.Moneda;
            ViewBag.Categoria = producto.Producto.Categoria.Nombre;
            return View("EliminarProductoContrato", productoModel);
        }

        /// <summary>
        /// Este método actualiza el estado IsDeleted a true, con el fin de indicar que este producto del conotrato fue eliminado
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CONTRATOS + "," + WPPConstants.ROLES_EDIT_CONTRATOS)]
        public ActionResult EliminarProductoContrato(ProductoContratoModel productoModel)
        {
            ProductoContrato producto = productoContratoService.Get(productoModel.Id);
            ViewBag.Contrato = producto.Contrato.DescripcionContrato;
            producto.IsDeleted = true;
            producto.DeleteDate = DateTime.Now;
            producto.DeletedBy = NombreUsuarioActual();
            productoContratoService.Update(producto);
            return RedirectToAction("Detalles", new { idContrato = producto.Contrato.Id });
            //return Detalles(producto.Contrato.Id);
        }

        /// <summary>
        /// Este método lista y filtra los productos del contrato asoaciados al contrato indicado (idContrato)
        /// </summary>
        /// <returns>json con la lista de contoratos asociados al contrato</returns>
        [HttpPost]
        public JsonResult CargarProductoContratos(long idContrato)
        {
            try
            {
                var numCompania = Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0;
                Compania compania = companiaService.Get(numCompania);

                Contrato contrato = contratoService.Get(idContrato);
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Contrato", contrato);
                criteria.Add("IsDeleted", false);
                criteria.Add("Proyecto", compania);

                List<ProductoContratoModel> productos = new List<ProductoContratoModel>();
                var listaProductosContratos = productoContratoService.GetAll(criteria);

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
                                Moneda = item.Contrato.Moneda,
                                TipoEquipo = item.Producto.Categoria.Tipo,
                                PrecioFosa = item.PrecioFosa,
                                
                            }
                         );
                    }
                }

                List<Object> resultado = new List<Object>();
                resultado.Add(productos);
                ContratoModel model = new ContratoModel()
                {
                    PagoContado = contrato.PagoContado,
                    DescripcionCliente = contrato.Cliente.Tipo.Nombre == "Comerciales" ? "Comercial" : "Municipal"
                };

                resultado.Add(model);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(null);
            }
        }

        /// <summary>
        /// Este método carga el modelo del equipo que se desea obtener la información por medio de su placa
        /// </summary>
        /// <returns>Un json con el modelo de equipo que se desea consultar</returns>
        [HttpPost]
        public JsonResult ObtenerProductosRecoleccion(long idContrato)
        {
            try
            {
                Contrato contrato = contratoService.Get(idContrato);

                IList<Contrato> ListaContratos = new List<Contrato>();
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Cliente", contrato.Cliente);
                criteria.Add("IsDeleted", false);

                ListaContratos = contratoService.GetAll(criteria);

                IList<ProductoContratoModel> ListaProductosRecoleccion = new List<ProductoContratoModel>();

                foreach (Contrato item in ListaContratos)
                {
                    if (item.Estado != "Inactivo")
                    {
                        List<ProductoContrato> Lista = item.Productos.Where(s => s.Producto.Categoria.Tipo == "Recoleccion").ToList();
                        foreach (ProductoContrato producto in Lista)
                        {
                            ProductoContratoModel model = new ProductoContratoModel();

                            model.Id = producto.Id;
                            model.Descripcion = producto.Descripcion + " (" + item.Numero + "-" + item.DescripcionContrato + ")";

                            ListaProductosRecoleccion.Add(model);
                        }
                    }
                }

                return Json(ListaProductosRecoleccion, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(null);
            }
        }
        #endregion
       


    }
}
