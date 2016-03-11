using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WPP.Entities.Objects.Generales;
using WPP.Helpers;
using WPP.Security;
using WPP.Service.ModuloContratos;
using WPP.Service.Generales;
using WPP.Mapper;
using WPP.Model;
using WPP.Model.General;
using PagedList;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Mapper.General;

namespace WPP.Controllers
{
    public class PuntoFacturacionController : BaseController
    {

        private PuntoFacturacionMapper puntoFacturacionMapper;
        private IPuntoFacturacionService puntoFacturacionService;
        public ICompaniaService companiaService { get; set; }

        public PuntoFacturacionController(IPuntoFacturacionService puntoFacturacionService, ICompaniaService companiaService)
        {
            try
            {
                this.puntoFacturacionService = puntoFacturacionService;
                this.companiaService = companiaService;
                puntoFacturacionMapper = new PuntoFacturacionMapper();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
               

        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_PUNTOS_FACTURACION + "," + WPPConstants.ROLES_CONS_PUNTOS_FACTURACION + "," + WPPConstants.ROLES_EDIT_PUNTOS_FACTURACION)]
        public ActionResult Index()
        {
            return Buscar(null, null, null, null, null);           
        }

        /// <summary>
        /// Este método se encarga de retornar la  vista que permite crear puntos de facturación 
        /// </summary>
        /// <returns>La vista Crear</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_PUNTOS_FACTURACION + "," + WPPConstants.ROLES_EDIT_PUNTOS_FACTURACION)]
        public ActionResult Crear()
        {
            ViewBag.ListaCompania = companiaService.ListAll();
            //ViewBag.ListaCompanias = companiaMapper.GetListaCompaniaModel(companiaService.ListAll());
            return View();            
        }


        /// <summary>
        /// Este método guarda la información del modelo PuntoFacturacionModel ingresada en el formulario de Crear
        /// </summary>
        /// <returns>La vista Index</returns>
       [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_PUNTOS_FACTURACION + "," + WPPConstants.ROLES_EDIT_PUNTOS_FACTURACION)]
        public ActionResult Crear(PuntoFacturacionModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    PuntoFacturacion punto = new PuntoFacturacion();
                    punto = puntoFacturacionMapper.GetPuntoFacturacion(model, punto);
                    punto.CreateDate = DateTime.Now;
                    punto.CreatedBy = NombreUsuarioActual();
                    punto.DateLastModified = DateTime.Now;
                    punto.IsDeleted = false;
                    punto.ModifiedBy = NombreUsuarioActual();
                    punto.Version++;
                    punto.Companias = new List<Compania>();
                    Compania compania = null;

                    foreach (var id in model.IdCompanias.Split(','))
                    {
                        compania = companiaService.Get(long.Parse(id));
                        punto.Companias.Add(compania);
                    }

                    puntoFacturacionService.Create(punto);
                    
                    return RedirectToAction("Index", "PuntoFacturacion");
                    //return Index();
                }
                else
                {
                    ViewBag.ListaCompania = companiaService.ListAll();
                    return View("Crear", model);
                }
           }
           catch(Exception ex)
           {
               logger.Error(ex.Message);
               ViewBag.ListaCompania = companiaService.ListAll();
               return View("Crear", model);
           }
        }

       /// <summary>
       /// Este método carga la información del punto de facturación que se desea editar, por medio de su id
       /// </summary>
       /// <returns>La vista Editar con el modelo del punto de facturación a editar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_PUNTOS_FACTURACION + "," + WPPConstants.ROLES_EDIT_PUNTOS_FACTURACION)]
        public ActionResult Editar(long id)
        {            
            PuntoFacturacion puntoFacturacion = puntoFacturacionService.Get(id);
            PuntoFacturacionModel puntoModel = puntoFacturacionMapper.GetPuntoFacturacionModel(puntoFacturacion);
            ViewBag.ListaCompania = companiaService.ListAll();

            puntoModel.IdCompanias = String.Empty;
            foreach (var item in puntoFacturacion.Companias)
            {
                puntoModel.IdCompanias += (puntoModel.IdCompanias == String.Empty ? item.Id.ToString() : ("," + item.Id));
            }

            return View(puntoModel);
        }

        /// <summary>
        /// Este método actualiza la información del punto de facturación seleccionado, según lo datos del modelo
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_PUNTOS_FACTURACION + "," + WPPConstants.ROLES_EDIT_PUNTOS_FACTURACION)]
        public ActionResult Editar(PuntoFacturacionModel puntoModel)
        {
            if (ModelState.IsValid)
            {
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Nombre", puntoModel.Nombre);
                criteria.Add("IsDeleted", false);
                PuntoFacturacion puntoValidacion = puntoFacturacionService.Get(criteria);
                if (puntoValidacion != null)
                {
                    if (puntoValidacion.Id != puntoModel.Id)
                    {
                        ModelState.AddModelError("Nombre", "Nombre no válido, ya existe un punto de facturación registrado con este nombre");
                        ViewBag.ListaCompania = companiaService.ListAll();
                        return View(puntoModel);
                    }
                }

                PuntoFacturacion punto = puntoFacturacionService.Get(puntoModel.Id);
                punto = puntoFacturacionMapper.GetPuntoFacturacion(puntoModel, punto);
                punto.DateLastModified = DateTime.Now;
                punto.ModifiedBy = NombreUsuarioActual();
                punto.Version++;

                var listaCompanias = puntoModel.IdCompanias.Split(',');
                punto.Companias = new List<Compania>();
                foreach (var item in listaCompanias)
                {
                    Compania compania = companiaService.Get(Convert.ToInt64(item));
                    punto.Companias.Add(compania);
                }

                puntoFacturacionService.Update(punto);

                ViewBag.ListaCompania = companiaService.ListAll();
                return RedirectToAction("Index", "PuntoFacturacion");
                //return Index();
            }
            else
            {
                ModelState.AddModelError("Nombre", "Nombre no válido, ya existe un punto de facturación registrado con este nombre");
                ViewBag.ListaCompania = companiaService.ListAll();
                return View(puntoModel);
            }           
        }

        /// <summary>
        /// Este método carga la información del punto de facturación que se desea eliminar, esto según el id
        /// </summary>
        /// <returns>La vista Eliminar con el modelo del a eliminar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_PUNTOS_FACTURACION)]
        public ActionResult Eliminar(long id)
        {
            PuntoFacturacion punto = puntoFacturacionService.Get(id);
            PuntoFacturacionModel model = puntoFacturacionMapper.GetPuntoFacturacionModel(punto);
            model.Companias = punto.Companias;
            return View(model);
        }

        /// <summary>
        /// Este método actualiza el estado IsDeleted a true, con el fin de indicar que este punto de facturación fue eliminado
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_PUNTOS_FACTURACION)]
        public ActionResult Eliminar(PuntoFacturacionModel model)
        {
            PuntoFacturacion punto = puntoFacturacionService.Get(model.Id);
            punto.IsDeleted = true;
            punto.DateLastModified = DateTime.Now;
            punto.Version++;
            punto.DeletedBy = NombreUsuarioActual();
            punto.DeleteDate = DateTime.Now;

            puntoFacturacionService.Update(punto);
            
            return RedirectToAction("Index", "PuntoFacturacion"); 
            //return Index();
        }

        /// <summary>
        /// Este método carga el modelo del punto de facturación que se desea obtener el detalle
        /// </summary>
        /// <returns>La vista de Visualizar con el modelo de puntos de facturación que se desea consultar</returns>
       [HttpGet]
       [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_PUNTOS_FACTURACION + "," + WPPConstants.ROLES_CONS_PUNTOS_FACTURACION + "," + WPPConstants.ROLES_EDIT_PUNTOS_FACTURACION)]
       public ActionResult Visualizar(long id)
       {
           PuntoFacturacion punto = puntoFacturacionService.Get(id);
           PuntoFacturacionModel model = puntoFacturacionMapper.GetPuntoFacturacionModel(punto);
           model.Companias = punto.Companias;

           return View(model);
       }

       /// <summary>
       /// Este método lista y filtra el listado de registros de puntos de facturación mostrados en el index
       /// </summary>
       /// <returns>Vista Index con el modelo IPageList(PuntoFacturacion)</returns>
        public ActionResult Buscar(string sortOrder, string currentNombre, 
                    string searchStringNombre, int? searchStringFilas, int? page)
        {
            ViewBag.CurrentSort = sortOrder;

            if(sortOrder == "NombreAsc")
                ViewBag.NombreSort = "NombreDesc";
            else
                ViewBag.NombreSort = "NombreAsc";

            if (searchStringNombre != null )
                page = 1;
            else
            {
                searchStringNombre = currentNombre;
                searchStringNombre = currentNombre;
            }

            ViewBag.CurrentNombre = searchStringNombre;

            var filas = searchStringFilas == null ? 10 : Convert.ToInt32(searchStringFilas);
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var pagina = puntoFacturacionService.PagingSearch(searchStringNombre,  pageNumber, filas, sortOrder);

            return View("Index", pagina);
        }

    }
}
