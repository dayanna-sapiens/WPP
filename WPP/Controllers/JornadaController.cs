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
    public class JornadaController : BaseController
    {

        IJornadaService jornadaService;
        JornadaMapper jornadaMapper;

        public JornadaController(IJornadaService jornada)
        {
            jornadaService = jornada;
            jornadaMapper = new JornadaMapper();
        }


        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROLES_ADMINISTRACION + "," + WPPConstants.ROLES_ADM_JORNADA + "," + WPPConstants.ROLES_CONS_JORNADA)]
        public ActionResult Index()
        {
            return Buscar(null, null, null, null, null, null, null);
           
        }
        /// <summary>
        /// Este método carga la información del jornada que se desea editar, por medio de su id
        /// </summary>
        /// <returns>La vista Editar con el modelo del jornada a editar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADM_JORNADA )]
        public ActionResult Editar(long id)
        {
            Jornada entity = jornadaService.Get(id);
            JornadaModel model = jornadaMapper.GetJornadaModel(entity);

            return View("Editar", model);
        }

        /// <summary>
        /// Este método actualiza la información del jornada seleccionado, según lo datos del modelo
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROLES_ADMINISTRACION + "," + WPPConstants.ROLES_ADM_JORNADA )]
        public ActionResult Editar(JornadaModel model)
        {

            Jornada entity = jornadaService.Get(model.Id);
            entity = jornadaMapper.GetJornada(model, entity);

            entity.DateLastModified = DateTime.Now;
            entity.ModifiedBy = ObtenerUsuarioActual().Nombre;
            entity.Version++;

            jornadaService.Update(entity);

            //return Index();
            return RedirectToAction("Index", "Jornada");
        }


        /// <summary>
        /// Este método carga el modelo de la jornada que se desea obtener el detalle
        /// </summary>
        /// <returns>La vista de Detalles con el modelo de la jornada que se desea consultar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROLES_ADMINISTRACION + "," + WPPConstants.ROLES_ADM_JORNADA + "," + WPPConstants.ROLES_CONS_JORNADA)]
        public ActionResult Detalles(long id)
        {
            try
            {
                Jornada entity = jornadaService.Get(id);
                JornadaModel model = jornadaMapper.GetJornadaModel(entity);

                return View("Detalles", model);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return View("Detalles");
            }
        }


        /// <summary>
        /// Este método lista y filtra el listado de registros de jornada mostrados en el index
        /// </summary>
        /// <returns>Vista Index con el modelo IPageList(jornada)</returns>
        public ActionResult Buscar(string sortOrder, string currentFilterDescripcion, string currentFilterTipo, string searchStringDescripcion, 
              string searchStringTipo, string searchStringFilas, int? page)
        {
            ViewBag.CurrentSort = sortOrder;

            if (sortOrder == "DescripcionAsc" || sortOrder == null)
                ViewBag.DescripcionSortParam = "DescripcionDesc";
            else
                ViewBag.DescripcionSortParam = "DescripcionAsc";

            if (sortOrder == "TipoAsc" || sortOrder == null)
                ViewBag.TipoSortParam = "TipoDesc";
            else
                ViewBag.TipoSortParam = "TipoAsc";

            if (searchStringDescripcion != null || searchStringTipo != null || searchStringFilas != null)
                page = 1;
            else
            {
                searchStringDescripcion = currentFilterDescripcion;
                searchStringTipo = currentFilterTipo;
            }

            ViewBag.CurrentDescripcion = searchStringDescripcion;
            ViewBag.CurrentTipo = searchStringTipo;

            var filas = searchStringFilas == null ? 10 : Convert.ToInt32(searchStringFilas);
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var paginaCompania = jornadaService.PagingSearch(searchStringDescripcion, searchStringTipo, pageNumber, filas, sortOrder);

            return View("Index", paginaCompania);
        }

    }
}
