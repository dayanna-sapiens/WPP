
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WPP.Entities.Objects.Generales;
using WPP.Helpers;
using WPP.Mapper.General;
using WPP.Model.General;
using WPP.Security;
using WPP.Service.Generales;

namespace WPP.Controllers
{
    public class CostoHoraController : BaseController
    {
        public ICostoHoraService costoService;
        private CostoHoraMapper costoMapper;

        public CostoHoraController(ICostoHoraService _costo)
        {
            costoService = _costo;
            costoMapper = new CostoHoraMapper();
        }

        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROLES_ADMINISTRACION + "," + WPPConstants.ROLES_ADMIN_CARGAR_NOMINA)]
        
        public ActionResult Index()
        {
            return Buscar(null,null,null,null,null,null,null);
        }

        /// <summary>
        /// Este método se encarga de retornar la lista vista que permite crear costos 
        /// </summary>
        /// <returns>La vista Crear</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROLES_ADMINISTRACION + "," + WPPConstants.ROLES_ADMIN_CARGAR_NOMINA)]
        public ActionResult Crear()
        {
            return View();
        }

        /// <summary>
        /// Este método guarda la información del modelo ingresada en el formulario de Crear
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROLES_ADMINISTRACION + "," + WPPConstants.ROLES_ADMIN_CARGAR_NOMINA)]
        public ActionResult Crear(CostoHoraModel model)
        {
            if (ModelState.IsValid)
            {
                CostoHora costo = new CostoHora();
                costo = costoMapper.GetCostoHora(model, costo);
                costo.IsDeleted = false;
                costo.ModifiedBy = NombreUsuarioActual();
                costo.Version = 1;
                costo.CreateDate = DateTime.Now;
                costo.CreatedBy = NombreUsuarioActual();

                costoService.Create(costo);
                
                return RedirectToAction("Index", "CostoHora"); 
            }
            else
            {
                return View(model);
            }
        }


        /// <summary>
        /// Este método carga la información del costo que se desea editar, por medio de su id
        /// </summary>
        /// <returns>La vista Editar con el modelo del costo a editar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROLES_ADMINISTRACION + "," + WPPConstants.ROLES_ADMIN_CARGAR_NOMINA)]
        public ActionResult Editar(long id)
        {
            CostoHoraModel costo = costoMapper.GetCostoHoraModel(costoService.Get(id));

            return View(costo);
        }

        /// <summary>
        /// Este método actualiza la información del costo seleccionado, según lo datos del modelo
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROLES_ADMINISTRACION + "," + WPPConstants.ROLES_ADMIN_CARGAR_NOMINA)]
        public ActionResult Editar(CostoHoraModel model)
        {

            CostoHora costo = costoService.Get(model.Id);
            if (ModelState.IsValid)
            {
                costo.Monto = model.Monto;
                costo.Tipo = model.Tipo;
                costoService.Update(costo);

                return RedirectToAction("Index", "CostoHora"); 
            }
            else
            {
                return View("Editar", model);
            }
        }




        /// <summary>
        /// Este método lista y filtra el listado de registros de costos mostrados en el index
        /// </summary>
        /// <returns>Vista Index con el modelo IPageList(costo)</returns>
        public ActionResult Buscar(string sortOrder, string currentFechaInicial, string currentFechaFinal, string searchStringFechaInicial, string searchStringFechaFinal, 
                                    string searchStringFilas, int? page)
        {
            ViewBag.CurrentSort = sortOrder;

            if (sortOrder == "FechaFinalAsc")
                ViewBag.FechaFinalSortParam = "FechaFinalAsc";
            else
                ViewBag.FechaFinalSortParam = "FechaFinalDesc";

            if (sortOrder == "FechaInicialAsc")
                ViewBag.FechaInicialSortParam = "FechaInicialAsc";
            else
                ViewBag.FechaInicialSortParam = "FechaInicialDesc";


            if (searchStringFechaInicial!= null )
                page = 1;
            else
            {
                searchStringFechaInicial = currentFechaInicial;
                searchStringFechaFinal = currentFechaFinal;
            }

            ViewBag.CurrentFechaInicial = searchStringFechaInicial;
            ViewBag.CurrentFechaFinal = searchStringFechaFinal;

            ViewBag.currentFilterFilas = searchStringFilas;

            var filas = searchStringFilas == null ? 10 : Convert.ToInt32(searchStringFilas);
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var paginaProducto = costoService.PagingSearch(searchStringFechaInicial, searchStringFechaFinal, pageNumber, filas, sortOrder);

            return View("Index", paginaProducto);
        }

    }
}
