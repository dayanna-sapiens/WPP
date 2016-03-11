using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WPP.Entities.Objects.ModuloBascula;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;
using WPP.Helpers;
using WPP.Mapper.ModuloBascula;
using WPP.Mapper.ModuloOperacionRecoleccion;
using WPP.Model.ModuloBascula;
using WPP.Security;
using WPP.Service.ModuloBascula;
using WPP.Service.ModuloBoletaManual;
using WPP.Service.ModuloContratos;
using WPP.Service.ModuloOperacionRecoleccion;

namespace WPP.Controllers
{
    public class BoletaManualController : BaseController
    {
        private IBoletaManualService boletaService;
        private IRellenoSanitarioService rellenoService;
        private IOTRService otrService;
        private IEquipoService equipoService;
        private ICompaniaService companiaService;
        private BoletaManualMapper boletaMapper;
        private RellenoSanitarioMapper rellenoMapper;
        private OTRMapper otrMapper;
        private EquipoMapper equipoMapper;

        public BoletaManualController(IBoletaManualService boleta, IRellenoSanitarioService relleno, IOTRService otr, IEquipoService equipo, ICompaniaService compania)
        {
            boletaService = boleta;
            rellenoService = relleno;
            otrService = otr;
            equipoService = equipo;
            companiaService = compania;
            boletaMapper = new BoletaManualMapper();
            rellenoMapper = new RellenoSanitarioMapper();
            otrMapper = new OTRMapper();
            equipoMapper = new EquipoMapper();
        }

        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROLES_ADMINISTRACION + "," + WPPConstants.ROLES_CONS_BOLETAS_MANUALES + "," + WPPConstants.ROLES_ADMIN_BOLETAS_MANUALES + "," + WPPConstants.ROLES_EDIT_BOLETAS_MANUALES)]
        public ActionResult Index()
        {
            return Buscar(null, null, null, null, null, null, null, null, null, null, null);
        }


        /// <summary>
        /// Este método se encarga de retornar la vista que permite crear boletas manuales 
        /// </summary>
        /// <returns>La vista Crear</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_BOLETAS_MANUALES + "," + WPPConstants.ROLES_EDIT_BOLETAS_MANUALES)]
        public ActionResult Crear()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("IsDeleted", false);

            ViewBag.ListaRellenos = rellenoMapper.GetListaModel(rellenoService.GetAll(criteria));
            return View();
        }

        /// <summary>
        /// Este método guarda la información del modelo ingresada en el formulario de Crear
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_BOLETAS_MANUALES + "," + WPPConstants.ROLES_EDIT_BOLETAS_MANUALES)]
        public ActionResult Crear(BoletaManualModel model)
        {
            if (ModelState.IsValid)
            {
                BoletaManual nuevo = new BoletaManual();
                nuevo = boletaMapper.GetEntity(model, nuevo);

                nuevo.Estado = "Activo";
                nuevo.Version = 1;
                nuevo.CreateDate = DateTime.Now;
                nuevo.DateLastModified = DateTime.Now;
                nuevo.CreatedBy = NombreUsuarioActual();
                nuevo.Compania = CompaniaActual();

                if (model.OTR != 0)
                {
                    nuevo.OTR = otrService.Get(model.OTR);
                }

                if (model.Sitio != 0)
                {
                    nuevo.Sitio = rellenoService.Get(model.Sitio);
                }

                boletaService.Create(nuevo);

                // Se actualiza el estado de la OTR indicandi que ya fue procesada (ya paso por báscula)
                nuevo.OTR.Estado = "Procesada";

                //if (nuevo.OTR.OTRMadre)
                //{
                //    IList<OTR> lista = nuevo.OTR.ListaOTRHijas;

                //    foreach (var item in lista)
                //    {
                //        item.Estado = "Procesada";
                //        otrService.Update(item);
                //    }
                //}
                otrService.Update(nuevo.OTR);

               // return Index();
                return RedirectToAction("Index", "BoletaManual");
            }
            else
            {
                return View(model);
            }
        }


        /// <summary>
        /// Este método carga la información de la boleta manual que se desea editar, por medio de su id
        /// </summary>
        /// <returns>La vista Editar con el modelo de la boleta a editar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_BOLETAS_MANUALES + "," + WPPConstants.ROLES_EDIT_BOLETAS_MANUALES)]
        public ActionResult Editar(long id)
        {
            BoletaManual entity = boletaService.Get(id);
            BoletaManualModel model = boletaMapper.GetModel(entity);
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("IsDeleted", false);

            ViewBag.ListaRellenos = rellenoMapper.GetListaModel(rellenoService.GetAll(criteria));

            return View("Editar", model);
        }

        /// <summary>
        /// Este método actualiza la información de la boleta manual seleccionada, según lo datos del modelo
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_BOLETAS_MANUALES + "," + WPPConstants.ROLES_EDIT_BOLETAS_MANUALES)]
        public ActionResult Editar(BoletaManualModel model)
        {
            BoletaManual entity = boletaService.Get(model.Id);
            entity = boletaMapper.GetEntity(model, entity);
            entity.DateLastModified = DateTime.Now;
            entity.ModifiedBy = ObtenerUsuarioActual().Nombre;
            entity.Version++;

            if (model.OTR != 0)
            {
                entity.OTR = otrService.Get(model.OTR);
            }

            if (model.Sitio != 0)
            {
                entity.Sitio = rellenoService.Get(model.Sitio);
            }
            
            boletaService.Update(entity);

            //return Index();
            return RedirectToAction("Index", "BoletaManual");
        }

        /// <summary>
        /// Este método carga la información de la boleta manual que se desea eliminar, esto según el id
        /// </summary>
        /// <returns>La vista Eliminar con el modelo del a eliminar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO+ "," + WPPConstants.ROLES_ADMIN_BOLETAS_MANUALES )]
        public ActionResult Eliminar(long id)
        {
            BoletaManual entity = boletaService.Get(id);
            BoletaManualModel model = boletaMapper.GetModel(entity);
            ViewBag.Cliente = entity.OTR.Cliente.Numero + " - " + entity.OTR.Cliente.Nombre;
            ViewBag.Equipo = entity.OTR.Equipo.Placa;

            return View(model);
        }

        /// <summary>
        /// Este método actualiza el estado IsDeleted a true, con el fin de indicar que esta boleta fue eliminada
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_BOLETAS_MANUALES)]
        public ActionResult Eliminar(BoletaManualModel model)
        {
            BoletaManual boleta = boletaService.Get(model.Id);
            boleta.IsDeleted = true;
            boleta.DateLastModified = DateTime.Now;
            boleta.Version++;
            boleta.DeletedBy = NombreUsuarioActual();
            boleta.DeleteDate = DateTime.Now;

            boletaService.Update(boleta);
            //return Index();
            return RedirectToAction("Index", "BoletaManual");
        }

        /// <summary>
        /// Este método carga el modelo de la boleta manual que se desea obtener el detalle
        /// </summary>
        /// <returns>La vista de Detalles con el modelo de la boleta que se desea consultar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_CONS_BOLETAS_MANUALES + "," + WPPConstants.ROLES_ADMIN_BOLETAS_MANUALES + "," + WPPConstants.ROLES_EDIT_BOLETAS_MANUALES)]
        public ActionResult Detalles(long id)
        {
            try
            {
                BoletaManual entity = boletaService.Get(id);
                BoletaManualModel model = boletaMapper.GetModel(entity);
                ViewBag.Cliente = entity.OTR.Cliente.Numero + " - " + entity.OTR.Cliente.Nombre;
                ViewBag.Equipo = entity.OTR.Equipo.Placa;
                return View("Detalles", model);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return View("Detalles");
            }
        }

        /// <summary>
        /// Este método lista y filtra el listado de registros de equipos mostrados en el index
        /// </summary>
        /// <returns>Vista Index con el modelo IPageList(boleta)</returns>
        public ActionResult Buscar(string sortOrder, string currentFilterBoleta, string currentFilterOTR, string currentFilterEquipo,
                                    string currentFilterEstado, string searchStringBoleta, string searchStringOTR, string searchStringEquipo,
                                string searchStringEstado, string searchStringFilas, int? page)
        {
            ViewBag.CurrentSort = sortOrder;

            if (sortOrder == "BoletaAsc" || sortOrder == null)
                ViewBag.BoletaSortParam = "BoletaDesc";
            else
                ViewBag.BoletaSortParam = "BoletaAsc";

            if (sortOrder == "OTRAsc" || sortOrder == null)
                ViewBag.OTRSortParam = "OTRDesc";
            else
                ViewBag.OTRSortParam = "OTRAsc";

            if (sortOrder == "EquipoAsc" || sortOrder == null)
                ViewBag.EquipoSortParam = "EquipoDesc";
            else
                ViewBag.EquipoSortParam = "EquipoAsc";

            if (sortOrder == "EstdoAsc" || sortOrder == null)
                ViewBag.EstadoSortParam = "EstadoDesc";
            else
                ViewBag.EstadoSortParam = "EstadoAsc";

            if (searchStringBoleta != null || searchStringOTR != null || searchStringEquipo != null ||
                        searchStringEstado != null || searchStringFilas != null)
                page = 1;
            else
            {
                searchStringBoleta = currentFilterBoleta;
                searchStringOTR = currentFilterOTR;
                searchStringEquipo = currentFilterEquipo;
                searchStringEstado = currentFilterEstado;
            }

            ViewBag.CurrentBoleta = searchStringBoleta;
            ViewBag.CurrentOTR = searchStringOTR;
            ViewBag.CurrentEquipo = searchStringEquipo;
            ViewBag.CurrentEstado = searchStringEstado;

            var filas = searchStringFilas == null ? 10 : Convert.ToInt32(searchStringFilas);
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var paginaCompania = boletaService.PagingSearch(searchStringBoleta, searchStringOTR, searchStringEquipo, searchStringEstado,
                                                          CompaniaActual().Id, pageNumber, filas, sortOrder);

            return View("Index", paginaCompania);
        }

        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_BOLETAS_MANUALES)]
        public JsonResult AnularBoletaManual(long id)
        {
            try
            {
                BoletaManual boleta = boletaService.Get(id);
                boleta.Version++;
                boleta.Estado = "Anulada";
                boleta.DateLastModified = DateTime.Now;
                boleta.ModifiedBy = ObtenerUsuarioActual().Nombre;

                boletaService.Update(boleta);

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(null);
            }
        }

        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_BOLETAS_MANUALES )]
        public JsonResult ReimprimirBoletaManual(long id)
        {
            try
            {
                BoletaManual boleta = boletaService.Get(id);
                boleta.Version++;
                boleta.Estado = "Anulada";
                boleta.DateLastModified = DateTime.Now;
                boleta.ModifiedBy = ObtenerUsuarioActual().Nombre;

                boletaService.Update(boleta);

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(null);
            }
        }
    }
}
