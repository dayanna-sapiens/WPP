using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Helpers;
using WPP.Mapper.ModuloContratos;
using WPP.Model.ModuloContratos;
using WPP.Security;
using WPP.Service.ModuloContratos;

namespace WPP.Controllers
{
    public class ServicioController : BaseController
    {
        private IServicioService servicioService;
        private ServicioMapper servicioMapper;

        public ServicioController(IServicioService _servicioService)
        {
            servicioService = _servicioService;
            servicioMapper = new ServicioMapper();
        }

        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROLES_ADMINISTRACION + "," + WPPConstants.ROLES_CONS_SERVICIOS + "," + WPPConstants.ROLES_ADMIN_SERVICIOS + "," + WPPConstants.ROLES_EDIT_SERVICIOS)]
        
        public ActionResult Index()
        {
            return Buscar(null,null,null,null,null,null,null);
        }

        /// <summary>
        /// Este método se encarga de retornar la lista vista que permite crear servicios 
        /// </summary>
        /// <returns>La vista Crear</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROLES_ADMINISTRACION + "," + WPPConstants.ROLES_ADMIN_SERVICIOS + "," + WPPConstants.ROLES_EDIT_SERVICIOS)]
        public ActionResult Crear()
        {
            return View();
        }

        /// <summary>
        /// Este método guarda la información del modelo ingresada en el formulario de Crear
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROLES_ADMINISTRACION + "," + WPPConstants.ROLES_ADMIN_SERVICIOS + "," + WPPConstants.ROLES_EDIT_SERVICIOS)]
        public ActionResult Crear(ServicioModel model)
        {
            if (ModelState.IsValid)
            {

                Servicio servicio = new Servicio();
                servicio = servicioMapper.GetServicio(model, servicio);
                servicio.IsDeleted = false;
                servicio.ModifiedBy = NombreUsuarioActual();
                servicio.Version = 1;
                servicio.CreateDate = DateTime.Now;
                servicio.CreatedBy = NombreUsuarioActual();
                servicio.Activo = true;
                servicio.DateLastModified = DateTime.Now;
                servicio.Compania = CompaniaActual();

                servicioService.Create(servicio);

                return RedirectToAction("Index", "Servicio"); 
            }
            else
            {
                return View(model);
            }
        }


        /// <summary>
        /// Este método carga la información del servicio que se desea editar, por medio de su id
        /// </summary>
        /// <returns>La vista Editar con el modelo del servicio a editar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_SERVICIOS + "," + WPPConstants.ROLES_EDIT_SERVICIOS)]
        public ActionResult Editar(long id)
        {
            ServicioModel servicio = servicioMapper.GetServicioModel(servicioService.Get(id));           
            return View(servicio);
        }

        /// <summary>
        /// Este método actualiza la información del servicio seleccionado, según lo datos del modelo
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_SERVICIOS + "," + WPPConstants.ROLES_EDIT_SERVICIOS)]
        public ActionResult Editar(ServicioModel model)
        {

            Servicio servicio = servicioService.Get(model.Id);
            if (ModelState.IsValid)
            {
                servicio.Nombre = model.Nombre;
                servicio.Activo = model.Activo;

                servicioService.Update(servicio);

                return RedirectToAction("Index", "Servicio"); 
            }
            else
            {
                return View("Editar", model);
            }
        }

        /// <summary>
        /// Este método carga la información del servicio que se desea eliminar, esto según el id
        /// </summary>
        /// <returns>La vista Eliminar con el modelo del a eliminar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_SERVICIOS)]
        public ActionResult Eliminar(long id)
        {
            Servicio servicio = servicioService.Get(id);
            ServicioModel model = servicioMapper.GetServicioModel(servicio);

            return View("Eliminar", model);
        }

        /// <summary>
        /// Este método actualiza el estado IsDeleted a true, con el fin de indicar que este servicio fue eliminado
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_SERVICIOS)]
        public ActionResult Eliminar(ServicioModel model)
        {
            Servicio servicio = servicioService.Get(model.Id);

            servicio.IsDeleted = true;
            servicio.DeleteDate = DateTime.Now;
            servicio.DeletedBy = NombreUsuarioActual();
            servicioService.Update(servicio);
            
            return RedirectToAction("Index", "Servicio"); 
        }

        /// <summary>
        /// Este método carga el modelo del servicio que se desea obtener el detalle
        /// </summary>
        /// <returns>La vista de Detalles con el modelo de servicio que se desea consultar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_CONS_SERVICIOS + "," + WPPConstants.ROLES_ADMIN_SERVICIOS + "," + WPPConstants.ROLES_EDIT_SERVICIOS)]
        public ActionResult Detalles(long id)
        {
            Servicio servicio = servicioService.Get(id);
            ServicioModel model = servicioMapper.GetServicioModel(servicio);

            return View("Detalles", model);
        }

        /// <summary>
        /// Este método lista y filtra el listado de registros de servicios mostrados en el index
        /// </summary>
        /// <returns>Vista Index con el modelo IPageList(servicio)</returns>
        public ActionResult Buscar(string sortOrder, string currentNombre, string currentActivo,  string searchStringNombre, string searchActivo, 
                                    string searchStringFilas, int? page)
        {
            ViewBag.CurrentSort = sortOrder;

            if (sortOrder == "NombreAsc")
                ViewBag.NombreSortParam = "NombreDesc";
            else
                ViewBag.NombreSortParam = "NombreAsc";
            

            if (searchStringNombre != null )
                page = 1;
            else
            {
                searchStringNombre = currentNombre;
                searchActivo = currentActivo;
            }

            ViewBag.CurrentNombre = searchStringNombre;
            ViewBag.CurrentActivo = searchActivo;

            ViewBag.currentFilterFilas = searchStringFilas;

            var filas = searchStringFilas == null ? 10 : Convert.ToInt32(searchStringFilas);
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var paginaProducto = servicioService.PagingSearch(searchStringNombre, CompaniaActual().Id, searchActivo, pageNumber, filas, sortOrder);

            return View("Index", paginaProducto);
        }


    }
}
