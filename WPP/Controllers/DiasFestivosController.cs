using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WPP.Entities.Objects.ModuloNomina;
using WPP.Helpers;
using WPP.Mapper.ModuloNomina;
using WPP.Model.ModuloNomina;
using WPP.Security;
using WPP.Service.ModuloNomina;

namespace WPP.Controllers
{
    public class DiasFestivosController : BaseController
    {
        private IDiasFestivosService diasService;
        private DiasFestivosMapper diasMapper;

        public DiasFestivosController(IDiasFestivosService _diasService)
        {
            diasService = _diasService;
            diasMapper = new DiasFestivosMapper();
        }

        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROLES_ADMINISTRACION + "," + WPPConstants.ROLES_CONS_DIAS_FESTIVOS + "," + WPPConstants.ROLES_ADMIN_DIAS_FESTIVOS + "," + WPPConstants.ROLES_EDIT_DIAS_FESTIVOS)]
        public ActionResult Index()
        {
            return Buscar(null, null, null, null, null, null, null, null,null);
        }

        /// <summary>
        /// Este método se encarga de retornar la vista que permite crear dias festivos 
        /// </summary>
        /// <returns>La vista Crear</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROLES_ADMINISTRACION + "," +  WPPConstants.ROLES_ADMIN_DIAS_FESTIVOS + "," + WPPConstants.ROLES_EDIT_DIAS_FESTIVOS)]
        public ActionResult Crear()
        {
            DiasFestivosModel model = new DiasFestivosModel();       
            return View(model);
        }

        /// <summary>
        /// Este método guarda la información del modelo ingresada en el formulario de Crear
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROLES_ADMINISTRACION + "," + WPPConstants.ROLES_ADMIN_DIAS_FESTIVOS + "," + WPPConstants.ROLES_EDIT_DIAS_FESTIVOS)]
        public ActionResult Crear(DiasFestivosModel model)
        {
            if (ModelState.IsValid)
            {
                DiasFestivos nuevo = new DiasFestivos();
                nuevo = diasMapper.GetBoletaDiasFestivos(model, nuevo);
               
                nuevo.Version = 1;
                nuevo.CreateDate = DateTime.Now;
                nuevo.DateLastModified = DateTime.Now;
                nuevo.CreatedBy = NombreUsuarioActual();
              
                diasService.Create(nuevo);

                ViewBag.Mensaje = "Se ha creado un nuevo DiasFestivos.";
                
                return RedirectToAction("Index", "DiasFestivos"); 
                //return Index();
            }
            else
            {
                return View(model);
            }
        }


        /// <summary>
        /// Este método carga la información del DiasFestivos que se desea editar, por medio de su id
        /// </summary>
        /// <returns>La vista Editar con el modelo del DiasFestivos a editar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROLES_ADMINISTRACION + "," + WPPConstants.ROLES_ADMIN_DIAS_FESTIVOS + "," + WPPConstants.ROLES_EDIT_DIAS_FESTIVOS)]
        public ActionResult Editar(long id)
        {
            DiasFestivos entity = diasService.Get(id);
            DiasFestivosModel model = diasMapper.GetBoletaDiasFestivosModel(entity);

            return View("Editar", model);
        }

        /// <summary>
        /// Este método actualiza la información del DiasFestivos seleccionado, según lo datos del modelo
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROLES_ADMINISTRACION + "," + WPPConstants.ROLES_ADMIN_DIAS_FESTIVOS + "," + WPPConstants.ROLES_EDIT_DIAS_FESTIVOS)]
        
        public ActionResult Editar(DiasFestivosModel model)
        {

            DiasFestivos entity = diasService.Get(model.Id);
            entity = diasMapper.GetBoletaDiasFestivos(model, entity);

            entity.DateLastModified = DateTime.Now;
            entity.ModifiedBy = ObtenerUsuarioActual().Nombre;
            entity.Version++;

            diasService.Update(entity);

            //return Index();
            return RedirectToAction("Index", "DiasFestivos"); 
        }

        /// <summary>
        /// Este método carga la información del DiasFestivos que se desea eliminar, esto según el id
        /// </summary>
        /// <returns>La vista Eliminar con el modelo del a eliminar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROLES_ADMINISTRACION + "," + WPPConstants.ROLES_ADMIN_DIAS_FESTIVOS)]
        
        public ActionResult Eliminar(long id)
        {
            DiasFestivos entity = diasService.Get(id);
            DiasFestivosModel model = diasMapper.GetBoletaDiasFestivosModel(entity);

            return View(model);
        }

        /// <summary>
        /// Este método actualiza el estado IsDeleted a true, con el fin de indicar que este DiasFestivos fue eliminado
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROLES_ADMINISTRACION + "," + WPPConstants.ROLES_ADMIN_DIAS_FESTIVOS)]
        public ActionResult Eliminar(DiasFestivosModel model)
        {
            DiasFestivos DiasFestivos = diasService.Get(model.Id);
            DiasFestivos.IsDeleted = true;
            DiasFestivos.DateLastModified = DateTime.Now;
            DiasFestivos.Version++;
            DiasFestivos.DeletedBy = NombreUsuarioActual();
            DiasFestivos.DeleteDate = DateTime.Now;

            diasService.Update(DiasFestivos);
            // return Index();
            return RedirectToAction("Index", "DiasFestivos"); 
        }

        /// <summary>
        /// Este método carga el modelo del DiasFestivos que se desea obtener el detalle
        /// </summary>
        /// <returns>La vista de Detalles con el modelo de DiasFestivos que se desea consultar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROLES_ADMINISTRACION + "," + WPPConstants.ROLES_CONS_DIAS_FESTIVOS + "," + WPPConstants.ROLES_ADMIN_DIAS_FESTIVOS + "," + WPPConstants.ROLES_EDIT_DIAS_FESTIVOS)]
        public ActionResult Detalles(long id)
        {
            try
            {
                DiasFestivos entity = diasService.Get(id);
                DiasFestivosModel model = diasMapper.GetBoletaDiasFestivosModel(entity);

                return View("Detalles", model);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return View("Detalles");
            }
        }

           
        /// <summary>
        /// Este método lista y filtra el listado de registros de diass mostrados en el index
        /// </summary>
        /// <returns>Vista Index con el modelo IPageList(DiasFestivos)</returns>
        public ActionResult Buscar(string sortOrder, string currentFilterDescripcion, string currentFilterDia, string currentFilterMes,
                                   string searchStringDescripcion, string searchStringDia, string searchStringMes, string searchStringFilas, int? page)
        {
            ViewBag.CurrentSort = sortOrder;

            if (sortOrder == "DescripcionAsc" || sortOrder == null)
                ViewBag.DescripcionSortParam = "DescripcionDesc";
            else
                ViewBag.DescripcionSortParam = "DescripcionAsc";

            if (sortOrder == "DiaAsc" || sortOrder == null)
                ViewBag.DiaSortParam = "DiaDesc";
            else
                ViewBag.DiaSortParam = "DiaAsc";

            if (sortOrder == "MesAsc" || sortOrder == null)
                ViewBag.MesSortParam = "MesDesc";
            else
                ViewBag.MesSortParam = "MesAsc";



            if (searchStringDescripcion != null || searchStringDia != null || searchStringMes != null || searchStringFilas != null)
                page = 1;
            else
            {
                searchStringDescripcion = currentFilterDescripcion;
                searchStringDia = currentFilterDia;
                searchStringMes = currentFilterMes;
            }

            ViewBag.CurrentDescripcion = searchStringDescripcion;
            ViewBag.CurrentDia = searchStringDia;
            ViewBag.CurrentMes = searchStringMes;

            var filas = searchStringFilas == null ? 10 : Convert.ToInt32(searchStringFilas);
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var paginaCompania = diasService.PagingSearch(searchStringDia, searchStringMes,searchStringDescripcion,pageNumber, filas, sortOrder);

            return View("Index", paginaCompania);
        }


    }
}
