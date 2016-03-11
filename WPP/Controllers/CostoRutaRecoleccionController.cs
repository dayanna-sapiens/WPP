using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WPP.Entities.Objects.ModuloNomina;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;
using WPP.Helpers;
using WPP.Mapper.ModuloNomina;
using WPP.Mapper.ModuloOperacionRecoleccion;
using WPP.Model.ModuloContratos;
using WPP.Model.ModuloNomina;
using WPP.Model.ModuloOperacionRecoleccion;
using WPP.Security;
using WPP.Service.ModuloNomina;
using WPP.Service.ModuloOperacionRecoleccion;

namespace WPP.Controllers
{
    public class CostoRutaRecoleccionController : BaseController
    {
        public IRutaRecoleccionService rutaService;
        public ICostoRutaRecoleccionService costoRutaService;
        private RutaRecoleccionMapper rutaMapper;
        private CostoRutaRecoleccionMapper costoRutaMapper;

        public CostoRutaRecoleccionController(IRutaRecoleccionService ruta, ICostoRutaRecoleccionService costo)
        {
            rutaService = ruta;
            costoRutaService = costo;
            rutaMapper = new RutaRecoleccionMapper();
            costoRutaMapper = new CostoRutaRecoleccionMapper();
        }

        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROLES_ADMINISTRACION + "," + WPPConstants.ROLES_ADM_COSTO_HORA)]
        
        public ActionResult Index()
        {
            return Buscar(null,null,null,null,null,null,null,null,null);
        }


        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_CONS_RUTA_RECOLECCION + "," + WPPConstants.ROLES_ADMIN_RUTA_RECOLECCION + "," + WPPConstants.ROLES_EDIT_RUTA_RECOLECCION)]
        public ActionResult Detalles(long Id)
        {
            RutaRecoleccion ruta = rutaService.Get(Id);
            RutaRecoleccionModel rutaModel = rutaMapper.GetRutaRecoleccionModel(ruta);
            ViewBag.Viajes = ruta.Rutas;
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("RutaRecoleccion", ruta);
            criteria.Add("IsDeleted", false);
            ViewBag.Costos = costoRutaMapper.GetListaModel(costoRutaService.GetAll(criteria));
            return View(rutaModel);
        }

        /// <summary>
        /// Este método se encarga de retornar la lista vista que permite crear costos 
        /// </summary>
        /// <returns>La vista Crear</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROLES_ADMINISTRACION + "," + WPPConstants.ROLES_ADM_COSTO_HORA)]
        public ActionResult RegistrarCosto(long Id)
        {
            RutaRecoleccion ruta = rutaService.Get(Id);
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

            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("RutaRecoleccion", ruta);
            criteria.Add("IsDeleted", false);
            ViewBag.Costos = oSerializer.Serialize(costoRutaMapper.GetListaModel(costoRutaService.GetAll(criteria)));

            return View(model);
        }

        /// <summary>
        /// Este método guarda la información del modelo ingresada en el formulario de Crear
        /// </summary>
        /// <returns>el id del nuevo costo registrado</returns>
        [HttpPost]
        public JsonResult Crear(long idRuta, double costo, string desde, string hasta)
        {
            try
            {
                CostoRutaRecoleccion costoRuta = new CostoRutaRecoleccion();
                costoRuta.Costo = costo;
                costoRuta.RutaRecoleccion = rutaService.Get(idRuta);
                costoRuta.FechaDesde = Convert.ToDateTime(desde);
                costoRuta.FechaHasta = Convert.ToDateTime(hasta);
                costoRuta.IsDeleted = false;
                costoRuta.ModifiedBy = NombreUsuarioActual();
                costoRuta.Version = 1;
                costoRuta.CreateDate = DateTime.Now;
                costoRuta.CreatedBy = NombreUsuarioActual();

                costoRuta = costoRutaService.Create(costoRuta);

                return Json(costoRuta.Id, JsonRequestBehavior.AllowGet); 
            }
            catch (Exception ex)  
            {
                return null;
            }
        }


    
        /// <summary>
        /// Este método actualiza la información del costo seleccionado, según lo datos del modelo
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        public JsonResult Eliminar(long id)
        {
            CostoRutaRecoleccion costo = costoRutaService.Get(id);
            if (ModelState.IsValid)
            {
                costo.IsDeleted = true;
                costo.DeletedBy = NombreUsuarioActual();
                costo.Version++;
                costo.DeleteDate = DateTime.Now;
                costoRutaService.Update(costo);

                return Json(true, JsonRequestBehavior.AllowGet); 
            }
            else
            {
                return null;
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
