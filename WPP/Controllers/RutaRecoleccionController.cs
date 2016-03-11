using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;
using WPP.Helpers;
using WPP.Mapper.ModuloContratos;
using WPP.Mapper.ModuloOperacionRecoleccion;
using WPP.Model.ModuloContratos;
using WPP.Model.ModuloOperacionRecoleccion;
using WPP.Security;
using WPP.Service.ModuloContratos;
using WPP.Service.ModuloOperacionRecoleccion;

namespace WPP.Controllers
{
    public class RutaRecoleccionController : BaseController
    {
        private IRutaRecoleccionService rutaService;
        private RutaRecoleccionMapper rutaMapper;
        private ICompaniaService companiaService;
        private IProductoContratoService productoService;

        public RutaRecoleccionController(IRutaRecoleccionService ruta, ICompaniaService compania, IProductoContratoService producto )
        {
            try
            {
                this.rutaService = ruta;
                this.companiaService = compania;
                this.productoService = producto;
                rutaMapper = new RutaRecoleccionMapper();
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
                return RedirectToAction("Index", "RutaRecoleccion");
            }
        }
        

        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROLES_ADMINISTRACION + "," + WPPConstants.ROLES_CONS_RUTA_RECOLECCION + "," + WPPConstants.ROLES_ADMIN_RUTA_RECOLECCION + "," + WPPConstants.ROLES_EDIT_RUTA_RECOLECCION)]
        public ActionResult Index()
        {
            return Buscar(null, null, null, null, null, null, null, null,null);
        }


        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_RUTA_RECOLECCION + "," + WPPConstants.ROLES_EDIT_RUTA_RECOLECCION)]
        public ActionResult Crear()
        {
           // RutaRecoleccionModel model = new RutaRecoleccionModel();
            return View();
        }


        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_RUTA_RECOLECCION + "," + WPPConstants.ROLES_EDIT_RUTA_RECOLECCION)]
        public ActionResult Crear(RutaRecoleccionModel ruta)
        {
            if (ModelState.IsValid)
            {
                RutaRecoleccion nuevaRuta = new RutaRecoleccion();
                nuevaRuta = rutaMapper.GetRutaRecoleccion(ruta, nuevaRuta);
                nuevaRuta.Version = 1;
                nuevaRuta.CreateDate = DateTime.Now;
                nuevaRuta.CreatedBy = NombreUsuarioActual();
                nuevaRuta.Compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                nuevaRuta.Rutas = new List<ProductoContrato>();

                var Rutas = ruta.ListaRutas.Split(',');
                foreach (var item in Rutas)
                {
                    ProductoContrato producto = new ProductoContrato();
                    producto = productoService.Get(Convert.ToInt64(item));
                    nuevaRuta.Rutas.Add(producto);
                }
  
                // Se crea la ruta
                nuevaRuta = rutaService.Create(nuevaRuta);

                ViewBag.Mensaje = "Se ha creado la nueva ruta de recolección";

                //return Index();     
                return RedirectToAction("Index", "RutaRecoleccion");           
            }
            else
            {
                //return View();
                return RedirectToAction("Index", "RutaRecoleccion");
            }
        }

         
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_RUTA_RECOLECCION + "," + WPPConstants.ROLES_EDIT_RUTA_RECOLECCION)]
        public ActionResult Editar(long idRuta)
        {
            RutaRecoleccion ruta = rutaService.Get(idRuta);
            RutaRecoleccionModel model = rutaMapper.GetRutaRecoleccionModel(ruta);
            List<ProductoContratoModel> list = new List<ProductoContratoModel>();

            foreach (var item in ruta.Rutas)
            {
                ProductoContratoModel productoModel = new ProductoContratoModel();
                productoModel.Id = item.Id;
                productoModel.ContratoDescripcion = item.Contrato.Id + " - " + item.Contrato.DescripcionContrato;
                productoModel.UbicacionDescripcion = item.Ubicacion == null ? "" : item.Ubicacion.Descripcion;
                productoModel.Descripcion = item.Contrato.Cliente.Id + " - " + item.Contrato.Cliente.Nombre;
                productoModel.ProductoDescripcion = item.Descripcion;

                list.Add(productoModel);
            }

            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            ViewBag.Viajes = oSerializer.Serialize(list);

            return View(model);
        }


        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_RUTA_RECOLECCION + "," + WPPConstants.ROLES_EDIT_RUTA_RECOLECCION)]
        public ActionResult Editar(RutaRecoleccionModel rutaModel)
        {
            RutaRecoleccion ruta = rutaService.Get(rutaModel.Id);
            var compania = ruta.Compania;
            ruta = rutaMapper.GetRutaRecoleccion(rutaModel, ruta);
            ruta.DateLastModified = DateTime.Now;
            ruta.ModifiedBy = NombreUsuarioActual();
            ruta.Version++;
            ruta.Compania = compania;
            ruta.Rutas = new List<ProductoContrato>();

            var Rutas = rutaModel.ListaRutas.Split(',');
            foreach (var item in Rutas)
            {
                ProductoContrato producto = new ProductoContrato();
                producto = productoService.Get(Convert.ToInt64(item));
                ruta.Rutas.Add(producto);
            }
  
            // Se actualiza la información de la compañía
            ruta = rutaService.Update(ruta);

            //return Index(); 
            return RedirectToAction("Index", "RutaRecoleccion");
        }

        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_RUTA_RECOLECCION )]
        public ActionResult Eliminar(long idRuta)
        {
            RutaRecoleccion ruta = rutaService.Get(idRuta);
            RutaRecoleccionModel rutaModel = rutaMapper.GetRutaRecoleccionModel(ruta);
            ViewBag.Viajes = ruta.Rutas;
            return View(rutaModel);
        }


        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_RUTA_RECOLECCION)]
        public ActionResult Eliminar(RutaRecoleccionModel rutaModel)
        {
            RutaRecoleccion ruta = rutaService.Get(rutaModel.Id);
            ruta.IsDeleted = true;
            ruta.DateLastModified = DateTime.Now;
            ruta.Version++;
            ruta.DeletedBy = NombreUsuarioActual();
            ruta.DeleteDate = DateTime.Now;

            rutaService.Update(ruta);

            //return Index();
            return RedirectToAction("Index", "RutaRecoleccion");
        }


        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_CONS_RUTA_RECOLECCION + "," + WPPConstants.ROLES_ADMIN_RUTA_RECOLECCION + "," + WPPConstants.ROLES_EDIT_RUTA_RECOLECCION)]
        public ActionResult Detalles(long idRuta)
        {
            RutaRecoleccion ruta = rutaService.Get(idRuta);
            RutaRecoleccionModel rutaModel = rutaMapper.GetRutaRecoleccionModel(ruta);
            ViewBag.Viajes = ruta.Rutas;
            return View(rutaModel);
        }

        /// <summary>
        /// Este método lista y filtra los contratos que pertenecen al cliente indicado (idCliente)
        /// </summary>
        /// <returns>json con la lista de contoratos asociados al cliente</returns>
        [HttpPost]
        public JsonResult CargarRutasRecoleccion(string ruta)
        {
            try
            {
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Id", Convert.ToInt64(ruta));
                criteria.Add("IsDeleted", false);
                var numCompania = Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0;
                Compania compania = companiaService.Get(numCompania);
                criteria.Add("Compania", compania);

                List<ProductoContratoModel> Rutas = new List<ProductoContratoModel>();
                RutaRecoleccion rutaRecoleccion = rutaService.Get(criteria);

                foreach (var item in rutaRecoleccion.Rutas)
                {
                    Rutas.Add(
                        new ProductoContratoModel
                        {
                            Id = item.Id,
                            ContratoDescripcion = item.Contrato.Numero + " - " + item.Contrato.DescripcionContrato,
                            UbicacionDescripcion = item.Ubicacion != null ? item.Ubicacion.Descripcion : "",
                            Ubicacion = item.Ubicacion != null ? item.Ubicacion.Id : 0,
                            Descripcion = item.Contrato.Cliente.Numero + " - " + item.Contrato.Cliente.Nombre
                        }
                    );                   
                }
                return Json(Rutas, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(null);
            }
        }


        public ActionResult Buscar(string sortOrder, string currentFilterDescripcion, string currentFilterEstado, string currentFilterTipo,
                    string searchStringDescripcion, string searchStringEstado, string searchStringTipo, string searchStringFilas, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            String searchStringEstadoBool = String.Empty;

            if (sortOrder == "DescripcionAsc")
                ViewBag.DescripcionSortParam = "DescripcionDesc";
            else
                ViewBag.DescripcionSortParam = "DescripcionAsc";

            if (sortOrder == "EstadoAsc")
                ViewBag.EstadoSortParam = "EstadoDesc";
            else
                ViewBag.EstadoSortParam = "EstadoAsc";

            if (sortOrder == "TipoAsc")
                ViewBag.TipoSortParam = "TipoDesc";
            else
                ViewBag.TipoSortParam = "TipoAsc";

            if (searchStringDescripcion != null || searchStringEstado != null || searchStringTipo != null)
            {
                page = 1;
                searchStringEstadoBool = searchStringEstado == "Activo" ? "true" : searchStringEstado == "Inactivo" ? "false" : null;
            }
            else
            {
                searchStringDescripcion = currentFilterDescripcion;
                searchStringEstadoBool = currentFilterEstado == "Activo" ? "true" : currentFilterEstado == "Inactivo" ? "false" : null;
                searchStringEstado = currentFilterEstado;
                searchStringTipo = currentFilterTipo;
            }

            ViewBag.CurrentDescripcion = searchStringDescripcion;
            ViewBag.CurrentEstado = searchStringEstado;
            ViewBag.CurrentTipo = searchStringTipo;

            var filas = searchStringFilas == null ? 10 : Convert.ToInt32(searchStringFilas);
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var compania = Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0;
            var paginaRuta = rutaService.PagingSearch(searchStringDescripcion, searchStringEstadoBool, searchStringTipo, compania, pageNumber, filas, sortOrder);

            return View("Index", paginaRuta);
        }

       

    }
}
