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
    public class CombustibleController : BaseController
    {     
        public ICombustibleService combustibleService;
        private CombustibleMapper combustibleMapper; 

        public CombustibleController(ICombustibleService _combustible)
        {
            combustibleService = _combustible;
            combustibleMapper = new CombustibleMapper();
        }

        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROLES_ADMINISTRACION + "," + WPPConstants.ROLES_ADM_COMBUSTIBLE)]
        
        public ActionResult Index()
        {
            return Buscar(null,null,null,null,null,null,null);
        }

        /// <summary>
        /// Este método se encarga de retornar la lista vista que permite crear combustibles 
        /// </summary>
        /// <returns>La vista Crear</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROLES_ADMINISTRACION + "," + WPPConstants.ROLES_ADM_COMBUSTIBLE)]
        public ActionResult Crear()
        {
            return View();
        }

        /// <summary>
        /// Este método guarda la información del modelo ingresada en el formulario de Crear
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROLES_ADMINISTRACION + "," + WPPConstants.ROLES_ADM_COMBUSTIBLE)]
        public ActionResult Crear(CombustibleModel model)
        {
            if (ModelState.IsValid)
            {

                Combustible coombustible = new Combustible();
                coombustible = combustibleMapper.GetCombustible(model, coombustible);
                coombustible.IsDeleted = false;
                coombustible.ModifiedBy = NombreUsuarioActual();
                coombustible.Version = 1;
                coombustible.CreateDate = DateTime.Now;
                coombustible.CreatedBy = NombreUsuarioActual();

                combustibleService.Create(coombustible);
                
                return RedirectToAction("Index", "Combustible"); 
            }
            else
            {
                return View(model);
            }

        }


        /// <summary>
        /// Este método carga la información del combustible que se desea editar, por medio de su id
        /// </summary>
        /// <returns>La vista Editar con el modelo del combustible a editar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROLES_ADMINISTRACION + "," + WPPConstants.ROLES_ADM_COMBUSTIBLE)]
        public ActionResult Editar(long id)
        {
            CombustibleModel combustible = combustibleMapper.GetCombustibleModel(combustibleService.Get(id));
           
            return View(combustible);
        }

        /// <summary>
        /// Este método actualiza la información del combustible seleccionado, según lo datos del modelo
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROLES_ADMINISTRACION + "," + WPPConstants.ROLES_ADM_COMBUSTIBLE)]
        public ActionResult Editar(CombustibleModel model)
        {

            Combustible combustible = combustibleService.Get(model.Id);
            if (ModelState.IsValid)
            {
                combustible.Diesel = model.Diesel;
                combustible.Gasolina = model.Gasolina;

                combustibleService.Update(combustible);

                return RedirectToAction("Index", "Combustible"); 
            }
            else
            {
                return View("Editar", model);
            }
        }




        /// <summary>
        /// Este método lista y filtra el listado de registros de combustibles mostrados en el index
        /// </summary>
        /// <returns>Vista Index con el modelo IPageList(combustible)</returns>
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
            var paginaProducto = combustibleService.PagingSearch(searchStringFechaInicial, searchStringFechaFinal, pageNumber, filas, sortOrder);

            return View("Index", paginaProducto);
        }

    }
}
