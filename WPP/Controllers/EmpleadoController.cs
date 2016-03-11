using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;
using WPP.Helpers;
using WPP.Mapper.ModuloNomina;
using WPP.Mapper.ModuloOperacionRecoleccion;
using WPP.Model.ModuloOperacionRecoleccion;
using WPP.Security;
using WPP.Service.ModuloNomina;
using WPP.Service.ModuloOperacionRecoleccion;

namespace WPP.Controllers
{
    public class EmpleadoController : BaseController
    {
        private IEmpleadoRecoleccionService empleadoService;
        private EmpleadoRecoleccionMapper empleadoMapper;
        private IJornadaService jornadaService;
        private JornadaMapper jornadaMapper;

        public EmpleadoController(IEmpleadoRecoleccionService _empleadoService, IJornadaService _jornadaService)
        {
            empleadoService = _empleadoService;
            jornadaService = _jornadaService;
            empleadoMapper = new EmpleadoRecoleccionMapper();
            jornadaMapper = new JornadaMapper();
        }

        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROLES_ADMINISTRACION + "," + WPPConstants.ROLES_ADM_EMPLEADO + "," + WPPConstants.ROLES_CONS_EMPLEADO)]
        
        public ActionResult Index()
        {
            return Buscar(null,null,null,null,null,null,null,null,null,null,null);
        }


        /// <summary>
        /// Este método carga la información del empleado que se desea editar, por medio de su id
        /// </summary>
        /// <returns>La vista Editar con el modelo del empleado a editar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADM_EMPLEADO )]
        public ActionResult Editar(long id)
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("IsDeleted", false);

            EmpleadoRecoleccionModel servicio = empleadoMapper.GetModel(empleadoService.Get(id));
            ViewBag.ListaJornada = jornadaMapper.GetListaJornadaModel(jornadaService.GetAll(criteria));
            return View(servicio);
        }

        /// <summary>
        /// Este método actualiza la información del servicio seleccionado, según lo datos del modelo
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADM_EMPLEADO)]
        public ActionResult Editar(EmpleadoRecoleccionModel model)
        {

            EmpleadoRecoleccion empleado = empleadoService.Get(model.Id);
            if (ModelState.IsValid)
            {

                empleado.Jornada = jornadaService.Get(model.Jornada);

                empleadoService.Update(empleado);

                return RedirectToAction("Index", "Empleado"); 
            }
            else
            {
                return View("Editar", model);
            }
        }

        /// <summary>
        /// Este método carga la información del empleado que se desea eliminar, esto según el id
        /// </summary>
        /// <returns>La vista Eliminar con el modelo del a eliminar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADM_EMPLEADO)]
        public ActionResult Eliminar(long id)
        {
            EmpleadoRecoleccion empleado = empleadoService.Get(id);
            EmpleadoRecoleccionModel model = empleadoMapper.GetModel(empleado);

            return View("Eliminar", model);
        }

        /// <summary>
        /// Este método actualiza el estado IsDeleted a true, con el fin de indicar que este empleado fue eliminado
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADM_EMPLEADO)]
        public ActionResult Eliminar(EmpleadoRecoleccionModel model)
        {
            EmpleadoRecoleccion empleado = empleadoService.Get(model.Id);

            empleado.IsDeleted = true;
            empleado.DeleteDate = DateTime.Now;
            empleado.DeletedBy = NombreUsuarioActual();
            empleadoService.Update(empleado);
            
            return RedirectToAction("Index", "Empleado"); 
        }

        /// <summary>
        /// Este método carga el modelo del servicio que se desea obtener el detalle
        /// </summary>
        /// <returns>La vista de Detalles con el modelo de servicio que se desea consultar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADM_EMPLEADO + "," + WPPConstants.ROLES_CONS_EMPLEADO )]
        public ActionResult Detalles(long id)
        {
            EmpleadoRecoleccion empleado = empleadoService.Get(id);
            EmpleadoRecoleccionModel model = empleadoMapper.GetModel(empleado);

            return View("Detalles", model);
        }

        /// <summary>
        /// Este método lista y filtra el listado de registros de servicios mostrados en el index
        /// </summary>
        /// <returns>Vista Index con el modelo IPageList(servicio)</returns>
        public ActionResult Buscar(string sortOrder, string currentNombre, string currentCedula, string currentCodigo, string currentPuesto,
                string searchStringNombre, string searchStringCedula, string searchStringCodigo, string searchStringPuesto, string searchStringFilas, int? page)
        {
            ViewBag.CurrentSort = sortOrder;

            if (sortOrder == "NombreAsc")
                ViewBag.NombreSortParam = "NombreDesc";
            else
                ViewBag.NombreSortParam = "NombreAsc";

            if (sortOrder == "CedulaAsc")
                ViewBag.CedulaSortParam = "CedulaDesc";
            else
                ViewBag.CedulaSortParam = "CedulaAsc";

            if (sortOrder == "CodigoAsc")
                ViewBag.CodigoSortParam = "CodigoDesc";
            else
                ViewBag.CodigoSortParam = "CodigoAsc";

            if (sortOrder == "PuestoAsc")
                ViewBag.PuestoSortParam = "PuestoDesc";
            else
                ViewBag.PueestoSortParam = "PuestoAsc";

            if (searchStringNombre != null || searchStringCedula != null || searchStringCodigo != null || searchStringPuesto != null)
                page = 1;
            else
            {
                searchStringNombre = currentNombre;
                searchStringCedula = currentCedula;
                searchStringCodigo = currentCodigo;
                searchStringPuesto = currentPuesto;
            }

            ViewBag.CurrentNombre = searchStringNombre;
            ViewBag.CurrentCedula = searchStringCedula;
            ViewBag.CurrentCodigo = searchStringCodigo;
            ViewBag.CurrentPuesto = searchStringPuesto;

            ViewBag.currentFilterFilas = searchStringFilas;

            var filas = searchStringFilas == null ? 10 : Convert.ToInt32(searchStringFilas);
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var paginaProducto = empleadoService.PagingSearch(searchStringNombre, searchStringCedula, searchStringCodigo, searchStringPuesto, pageNumber, filas, sortOrder);

            return View("Index", paginaProducto);
        }


    }
}
