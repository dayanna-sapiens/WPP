using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;
using WPP.Helpers;
using WPP.Mapper.ModuloOperacionRecoleccion;
using WPP.Model.ModuloOperacionRecoleccion;
using WPP.Security;
using WPP.Service.ModuloOperacionRecoleccion;

namespace WPP.Controllers
{
    public class RellenoSanitarioController : BaseController
    {
        private IRellenoSanitarioService rellenoSanitarioService;
        private RellenoSanitarioMapper rellenoSanitarioMapper;

        public RellenoSanitarioController(IRellenoSanitarioService _rellenoSanitarioService)
        {
            rellenoSanitarioService = _rellenoSanitarioService;
            rellenoSanitarioMapper = new RellenoSanitarioMapper();
        }

        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROLES_ADMINISTRACION + "," + WPPConstants.ROLES_CONS_RELLENO_SANITARIO + "," + WPPConstants.ROLES_ADMIN_RELLENO_SANITARIO + "," + WPPConstants.ROLES_EDIT_RELLENO_SANITARIO)]
        public ActionResult Index()
        {
            return Buscar(null, null, null, null, null, null, null);
        }

        /// <summary>
        /// Este método se encarga de retornar la vista que permite crear rellenoSanitarios 
        /// </summary>
        /// <returns>La vista Crear</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_RELLENO_SANITARIO + "," + WPPConstants.ROLES_EDIT_RELLENO_SANITARIO)]
        public ActionResult Crear()
        {
            RellenoSanitarioModel model = new RellenoSanitarioModel();       
            return View(model);
        }

        /// <summary>
        /// Este método guarda la información del modelo ingresada en el formulario de Crear
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_RELLENO_SANITARIO + "," + WPPConstants.ROLES_EDIT_RELLENO_SANITARIO)]
        public ActionResult Crear(RellenoSanitarioModel model)
        {
            if (ModelState.IsValid)
            {
                RellenoSanitario nuevo = new RellenoSanitario();
                nuevo = rellenoSanitarioMapper.GetEntity(model, nuevo);
               
                nuevo.Version = 1;
                nuevo.CreateDate = DateTime.Now;
                nuevo.DateLastModified = DateTime.Now;
                nuevo.CreatedBy = NombreUsuarioActual();
              
                rellenoSanitarioService.Create(nuevo);

                ViewBag.Mensaje = "Se ha creado un nuevo relleno Sanitario.";

                //return Index();
                return RedirectToAction("Index", "RellenoSanitario");
            }
            else
            {
                return View(model);
            }
        }


        /// <summary>
        /// Este método carga la información del rellenoSanitario que se desea editar, por medio de su id
        /// </summary>
        /// <returns>La vista Editar con el modelo del rellenoSanitario a editar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_RELLENO_SANITARIO + "," + WPPConstants.ROLES_EDIT_RELLENO_SANITARIO)]
        public ActionResult Editar(long id)
        {
            RellenoSanitario entity = rellenoSanitarioService.Get(id);
            RellenoSanitarioModel model = rellenoSanitarioMapper.GetModel(entity);

            return View("Editar", model);
        }

        /// <summary>
        /// Este método actualiza la información del rellenoSanitario seleccionado, según lo datos del modelo
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_RELLENO_SANITARIO + "," + WPPConstants.ROLES_EDIT_RELLENO_SANITARIO)]
        public ActionResult Editar(RellenoSanitarioModel model)
        {
            RellenoSanitario entity = rellenoSanitarioService.Get(model.Id);
            entity = rellenoSanitarioMapper.GetEntity(model, entity);

            entity.DateLastModified = DateTime.Now;
            entity.ModifiedBy = ObtenerUsuarioActual().Nombre;
            entity.Version++;

            rellenoSanitarioService.Update(entity);

            //return Index();
            return RedirectToAction("Index", "RellenoSanitario");
        }

        /// <summary>
        /// Este método carga la información del rellenoSanitario que se desea eliminar, esto según el id
        /// </summary>
        /// <returns>La vista Eliminar con el modelo del a eliminar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_RELLENO_SANITARIO)]
        public ActionResult Eliminar(long id)
        {
            RellenoSanitario entity = rellenoSanitarioService.Get(id);
            RellenoSanitarioModel model = rellenoSanitarioMapper.GetModel(entity);

            return View(model);
        }

        /// <summary>
        /// Este método actualiza el estado IsDeleted a true, con el fin de indicar que este rellenoSanitario fue eliminado
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_RELLENO_SANITARIO)]
        public ActionResult Eliminar(RellenoSanitarioModel model)
        {
            RellenoSanitario rellenoSanitario = rellenoSanitarioService.Get(model.Id);
            rellenoSanitario.IsDeleted = true;
            rellenoSanitario.DateLastModified = DateTime.Now;
            rellenoSanitario.Version++;
            rellenoSanitario.DeletedBy = NombreUsuarioActual();
            rellenoSanitario.DeleteDate = DateTime.Now;

            rellenoSanitarioService.Update(rellenoSanitario);
            //return Index();
            return RedirectToAction("Index", "RellenoSanitario");
        }

        /// <summary>
        /// Este método carga el modelo del rellenoSanitario que se desea obtener el detalle
        /// </summary>
        /// <returns>La vista de Detalles con el modelo de rellenoSanitario que se desea consultar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_CONS_RELLENO_SANITARIO + "," + WPPConstants.ROLES_ADMIN_RELLENO_SANITARIO + "," + WPPConstants.ROLES_EDIT_RELLENO_SANITARIO)]
        public ActionResult Detalles(long id)
        {
            RellenoSanitario entity = rellenoSanitarioService.Get(id);
            RellenoSanitarioModel model = rellenoSanitarioMapper.GetModel(entity);

            return View("Detalles", model);
        }
        
        
        /// <summary>
        /// Este método lista y filtra el listado de registros de rellenoSanitarios mostrados en el index
        /// </summary>
        /// <returns>Vista Index con el modelo IPageList(rellenoSanitario)</returns>
        public ActionResult Buscar(string sortOrder, string currentFilterNombre, string currentFilterEstado, 
                            string searchStringNombre, string searchStringEstado, string searchStringFilas, int? page)
        {
            ViewBag.CurrentSort = sortOrder;

            if (sortOrder == "NombreAsc" || sortOrder == null)
                ViewBag.NombreSortParam = "NombreDesc";
            else
                ViewBag.NombreSortParam = "NombreAsc";

            if (sortOrder == "EstadoAsc" || sortOrder == null)
                ViewBag.EstadoSortParam = "EstadoDesc";
            else
                ViewBag.EstadoSortParam = "EstadoAsc";

          
            if (searchStringNombre != null || searchStringEstado != null || searchStringFilas != null)
                page = 1;
            else
            {
                searchStringNombre = currentFilterNombre;
                searchStringEstado = currentFilterEstado;
            }

            ViewBag.CurrentNombre = searchStringNombre;
            ViewBag.CurrentEstado = searchStringEstado;

            var filas = searchStringFilas == null ? 10 : Convert.ToInt32(searchStringFilas);
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var paginaCompania = rellenoSanitarioService.PagingSearch(searchStringNombre, searchStringEstado, pageNumber, filas, sortOrder);

            return View("Index", paginaCompania);
        }

    }
}
