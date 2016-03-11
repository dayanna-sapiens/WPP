using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;
using WPP.Helpers;
using WPP.Mapper.ModuloOperacionRecoleccion;
using WPP.Model.ModuloOperacionRecoleccion;
using WPP.Security;
using WPP.Service.ModuloContratos;
using WPP.Service.ModuloOperacionRecoleccion;

namespace WPP.Controllers
{
    public class CuadrillaController : BaseController
    {
        private ICuadrillaService cuadrillaService;
        private ICompaniaService companiaService;
        private CuadrillaMapper cuadrillaMapper;
        private IEmpleadoRecoleccionService empleadosService;

        public CuadrillaController(ICuadrillaService service, ICompaniaService compania, IEmpleadoRecoleccionService empleado)
        {
            try
            {
                this.cuadrillaService = service;
                this.companiaService = compania;
                this.empleadosService = empleado;
                cuadrillaMapper = new CuadrillaMapper();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
        
               

        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_CONS_CUADRILLA + "," + WPPConstants.ROLES_ADMIN_CUADRILLA + "," + WPPConstants.ROLES_EDIT_CUADRILLA)]
        public ActionResult Index()
        {
            return Buscar(null, null, null, null, null, null,null,null,null);
           
        }

        /// <summary>
        /// Este método se encarga de retornar la  vista que permite crear cuadrillas
        /// </summary>
        /// <returns>La vista Crear</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CUADRILLA + "," + WPPConstants.ROLES_EDIT_CUADRILLA)]
        public ActionResult Crear()
        {
            Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Compania", compania);
            criteria.Add("Puesto", "Chofer");
            criteria.Add("IsDeleted", false);
            ViewBag.ListaChoferes = empleadosService.GetAll(criteria);

            IDictionary<string, object> criteriaEmpleado = new Dictionary<string, object>();
            criteriaEmpleado.Add("Compania", compania);
            criteriaEmpleado.Add("IsDeleted", false);
            criteriaEmpleado.Add("Puesto", "Peon");

            ViewBag.ListaPeones = empleadosService.GetAll(criteriaEmpleado);

            return View();            
        }


        /// <summary>
        /// Este método guarda la información del modelo CuadrillaModel ingresada en el formulario de Crear
        /// </summary>
        /// <returns>La vista Index</returns>
       [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CUADRILLA + "," + WPPConstants.ROLES_EDIT_CUADRILLA)]
        public ActionResult Crear(CuadrillaModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Cuadrilla cuadrilla = new Cuadrilla();
                    cuadrilla = cuadrillaMapper.GetEntity(model, cuadrilla);
                    cuadrilla.CreateDate = DateTime.Now;
                    cuadrilla.CreatedBy = NombreUsuarioActual();
                    cuadrilla.DateLastModified = DateTime.Now;
                    cuadrilla.IsDeleted = false;
                    cuadrilla.ModifiedBy = NombreUsuarioActual();
                    cuadrilla.Version++;
                    Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                    cuadrilla.Compania = compania;
                    cuadrilla.ListaEmpleados = new List<EmpleadoRecoleccion>();

                    foreach (var id in model.EmpleadosId.Split(','))
                    {
                        EmpleadoRecoleccion empleado = empleadosService.Get(long.Parse(id));
                        cuadrilla.ListaEmpleados.Add(empleado);
                    }

                    if (model.Chofer > 0)
                    {
                        EmpleadoRecoleccion empleado = empleadosService.Get(model.Chofer);
                        cuadrilla.ListaEmpleados.Add(empleado);
                    }

                    cuadrillaService.Create(cuadrilla);

                    //return Index();
                    return RedirectToAction("Index", "Cuadrilla"); 
                }
                else
                {
                    Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                    IDictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("Compania", compania);
                    criteria.Add("Puesto", "Chofer");
                    criteria.Add("IsDeleted", false);
                    ViewBag.ListaChoferes = empleadosService.GetAll(criteria);

                    IDictionary<string, object> criteriaEmpleado = new Dictionary<string, object>();
                    criteriaEmpleado.Add("Compania", compania);
                    criteriaEmpleado.Add("Puesto", "Peon");
                    criteriaEmpleado.Add("IsDeleted", false);
                    ViewBag.ListaPeones = empleadosService.GetAll(criteriaEmpleado);
                    return View("Crear", model);
                }
           }
           catch(Exception ex)
           {
               logger.Error(ex.Message);
               Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
               IDictionary<string, object> criteria = new Dictionary<string, object>();
               criteria.Add("Compania", compania);
               criteria.Add("Puesto", "Chofer");
               criteria.Add("IsDeleted", false);

               ViewBag.ListaChoferes = empleadosService.GetAll(criteria);

               IDictionary<string, object> criteriaEmpleado = new Dictionary<string, object>();
               criteriaEmpleado.Add("Compania", compania);
               criteriaEmpleado.Add("Puesto", "Peon");
               criteriaEmpleado.Add("IsDeleted", false);

               ViewBag.ListaPeones = empleadosService.GetAll(criteriaEmpleado);
               return View("Crear", model);
           }
        }

       /// <summary>
       /// Este método carga la información de la cuadrilla que se desea editar, por medio de su id
       /// </summary>
       /// <returns>La vista Editar con el modelo de la cuadrilla a editar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CUADRILLA + "," + WPPConstants.ROLES_EDIT_CUADRILLA)]
        public ActionResult Editar(long id)
        {            
            Cuadrilla cuadrilla = cuadrillaService.Get(id);
            CuadrillaModel model = cuadrillaMapper.GetModel(cuadrilla);
            Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Compania", compania);
            criteria.Add("Puesto", "Chofer");
            criteria.Add("IsDeleted", false);
            ViewBag.ListaChoferes = empleadosService.GetAll(criteria);

            IDictionary<string, object> criteriaEmpleado = new Dictionary<string, object>();
            criteriaEmpleado.Add("Compania", compania);
            criteriaEmpleado.Add("Puesto", "Peon");
            criteriaEmpleado.Add("IsDeleted", false);
            ViewBag.ListaPeones = empleadosService.GetAll(criteriaEmpleado);

            model.EmpleadosId = String.Empty;
            foreach (var item in cuadrilla.ListaEmpleados)
            {
                if (item.Puesto == "Chofer")
                {
                    model.Chofer = item.Id;
                }
                else
                {
                    model.EmpleadosId += (model.EmpleadosId == String.Empty ? item.Id.ToString() : ("," + item.Id));                
                }
            }

            return View(model);
        }

        /// <summary>
        /// Este método actualiza la información de la cuadrilla seleccionado, según lo datos del modelo
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CUADRILLA + "," + WPPConstants.ROLES_EDIT_CUADRILLA)]
        public ActionResult Editar(CuadrillaModel model)
        {
            if (ModelState.IsValid)
            {
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Descripcion", model.Descripcion);
                criteria.Add("IsDeleted", false);
                Cuadrilla cuadrillaValidacion = cuadrillaService.Get(criteria);
                if (cuadrillaValidacion != null)
                {
                    if (cuadrillaValidacion.Id != model.Id)
                    {
                        ModelState.AddModelError("Descripcion", "Descripción no válida, ya existe una cuadrilla registrada con esta descripción");
                        return View(model);
                    }
                }

               Cuadrilla cuadrilla = cuadrillaService.Get(model.Id);
                cuadrilla = cuadrillaMapper.GetEntity(model, cuadrilla);
                cuadrilla.DateLastModified = DateTime.Now;
                cuadrilla.ModifiedBy = NombreUsuarioActual();
                cuadrilla.Version++;

                cuadrilla.ListaEmpleados = new List<EmpleadoRecoleccion>();

                foreach (var id in model.EmpleadosId.Split(','))
                {
                    EmpleadoRecoleccion empleado = empleadosService.Get(long.Parse(id));
                    cuadrilla.ListaEmpleados.Add(empleado);
                }

                if (model.Chofer > 0)
                {
                    EmpleadoRecoleccion empleado = empleadosService.Get(model.Chofer);
                    cuadrilla.ListaEmpleados.Add(empleado);
                }

                cuadrillaService.Update(cuadrilla);

                //return Index();
                return RedirectToAction("Index", "Cuadrilla"); 
            }
            else
            {
                Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Compania", compania);
                criteria.Add("Puesto", "Chofer");

                criteria.Add("IsDeleted", false);
                ViewBag.ListaChoferes = empleadosService.GetAll(criteria);

                IDictionary<string, object> criteriaEmpleado = new Dictionary<string, object>();
                criteriaEmpleado.Add("Compania", compania);
                criteriaEmpleado.Add("Puesto", "Peon");

                criteriaEmpleado.Add("IsDeleted", false);
                ViewBag.ListaPeones = empleadosService.GetAll(criteriaEmpleado);
                ModelState.AddModelError("Descripcion", "Descripción no válida, ya existe una cuadrilla registrada con esta descripción");
                return View(model);
            }           
        }

        /// <summary>
        /// Este método carga la información de la cuadrilla que se desea eliminar, esto según el id
        /// </summary>
        /// <returns>La vista Eliminar con el modelo del a eliminar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CUADRILLA)]
        public ActionResult Eliminar(long id)
        {
           Cuadrilla cuadrilla = cuadrillaService.Get(id);
           CuadrillaModel model = cuadrillaMapper.GetModel(cuadrilla);
            return View(model);
        }

        /// <summary>
        /// Este método actualiza el estado IsDeleted a true, con el fin de indicar que esta cuadrilla fue eliminado
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CUADRILLA)]
        public ActionResult Eliminar(CuadrillaModel model)
        {
            Cuadrilla cuadrilla = cuadrillaService.Get(model.Id);
            cuadrilla.IsDeleted = true;
            cuadrilla.DateLastModified = DateTime.Now;
            cuadrilla.Version++;
            cuadrilla.DeletedBy = NombreUsuarioActual();
            cuadrilla.DeleteDate = DateTime.Now;

            cuadrillaService.Update(cuadrilla);

            //return Index();
            return RedirectToAction("Index", "Cuadrilla"); 
        }

        /// <summary>
        /// Este método carga el modelo de la cuadrilla que se desea obtener el detalle
        /// </summary>
        /// <returns>La vista de Visualizar con el modelo de cuadrilla que se desea consultar</returns>
       [HttpGet]
       [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_CONS_CUADRILLA + "," + WPPConstants.ROLES_ADMIN_CUADRILLA + "," + WPPConstants.ROLES_EDIT_CUADRILLA)]
       public ActionResult Detalles(long id)
       {
           Cuadrilla cuadrilla = cuadrillaService.Get(id);
           CuadrillaModel model = cuadrillaMapper.GetModel(cuadrilla);

           return View(model);
       }

       /// <summary>
       /// Este método lista y filtra el listado de registros de cuadrillas mostrados en el index
       /// </summary>
       /// <returns>Vista Index con el modelo IPageList(Cuadrilla)</returns>
        public ActionResult Buscar(string sortOrder, string currentDescripcion, string currentEstado, string currentChofer, 
            string searchStringDescripcion, string searchStringEstado, string searchStringChofer, int? searchStringFilas, int? page)
        {
            ViewBag.CurrentSort = sortOrder;

            if(sortOrder == "DescripcionAsc")
                ViewBag.DescripcionSort = "DescripcionDesc";
            else
                ViewBag.DescripcionSort = "DescripcionAsc";

            if (sortOrder == "EstadoAsc")
                ViewBag.EstadoSort = "EstadoDesc";
            else
                ViewBag.EstadoSort = "EstadoAsc";

            if (sortOrder == "ChoferAsc")
                ViewBag.ChoferSort = "ChoferDesc";
            else
                ViewBag.ChoferSort = "ChoferAsc";


            if (searchStringDescripcion != null || searchStringEstado != null || searchStringChofer != null)
                page = 1;
            else
            {
                searchStringDescripcion = currentDescripcion;
                searchStringEstado = currentEstado;
                searchStringChofer = currentChofer;
            }

            ViewBag.CurrentDescripcion = searchStringDescripcion;
            ViewBag.CurrentEstado = searchStringEstado;
            ViewBag.CurrentChofer = searchStringChofer;

            var compania = Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0;
            var filas = searchStringFilas == null ? 10 : Convert.ToInt32(searchStringFilas);
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var pagina = cuadrillaService.PagingSearch(searchStringDescripcion,searchStringChofer,searchStringEstado,  pageNumber, filas, sortOrder, compania);

            return View("Index", pagina);
        }

     

    }
}
