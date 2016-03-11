using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WPP.Entities.Objects.ModuloBascula;
using WPP.Helpers;
using WPP.Mapper.ModuloBascula;
using WPP.Mapper.ModuloContratos;
using WPP.Model.ModuloBascula;
using WPP.Security;
using WPP.Service.ModuloBascula;

namespace WPP.Controllers
{
    public class EquipoController : BaseController
    {
        private IEquipoService equipoService;
        private IConfiguracionPuertoService puertoService;
        private EquipoMapper equipoMapper;

        public EquipoController(IEquipoService _equipoService, IConfiguracionPuertoService puerto)
        {
            equipoService = _equipoService;
            puertoService = puerto;
            equipoMapper = new EquipoMapper();
        }

        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROLES_ADMINISTRACION + "," + WPPConstants.ROLES_CONS_EQUIPOS + "," + WPPConstants.ROLES_ADMIN_EQUIPOS + "," + WPPConstants.ROLES_EDIT_EQUIPOS)]
        public ActionResult Index()
        {
            return Buscar(null, null, null, null, null, null, null, null, null, null, null);
        }

        /// <summary>
        /// Este método se encarga de retornar la vista que permite crear equipos 
        /// </summary>
        /// <returns>La vista Crear</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_EQUIPOS + "," + WPPConstants.ROLES_EDIT_EQUIPOS)]
        public ActionResult Crear()
        {
            EquipoModel model = new EquipoModel();       
            return View(model);
        }

        /// <summary>
        /// Este método guarda la información del modelo ingresada en el formulario de Crear
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_EQUIPOS + "," + WPPConstants.ROLES_EDIT_EQUIPOS)]
        public ActionResult Crear(EquipoModel model)
        {
            if (ModelState.IsValid)
            {
                Equipo nuevo = new Equipo();
                nuevo = equipoMapper.GetEntity(model, nuevo);
               
                nuevo.Version = 1;
                nuevo.CreateDate = DateTime.Now;
                nuevo.DateLastModified = DateTime.Now;
                nuevo.CreatedBy = NombreUsuarioActual();
                nuevo.Compania = CompaniaActual();
              
                equipoService.Create(nuevo);

                ViewBag.Mensaje = "Se ha creado un nuevo equipo.";
                
                return RedirectToAction("Index", "Equipo"); 
                //return Index();
            }
            else
            {
                return View(model);
            }
        }


        /// <summary>
        /// Este método carga la información del equipo que se desea editar, por medio de su id
        /// </summary>
        /// <returns>La vista Editar con el modelo del equipo a editar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_EQUIPOS + "," + WPPConstants.ROLES_EDIT_EQUIPOS)]
        public ActionResult Editar(long id)
        {
            Equipo entity = equipoService.Get(id);
            EquipoModel model = equipoMapper.GetModel(entity);

            return View("Editar", model);
        }

        /// <summary>
        /// Este método actualiza la información del equipo seleccionado, según lo datos del modelo
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_EQUIPOS + "," + WPPConstants.ROLES_EDIT_EQUIPOS)]
        public ActionResult Editar(EquipoModel model)
        {

            Equipo entity = equipoService.Get(model.Id);
            entity = equipoMapper.GetEntity(model, entity);

            entity.DateLastModified = DateTime.Now;
            entity.ModifiedBy = ObtenerUsuarioActual().Nombre;
            entity.Version++;

            equipoService.Update(entity);

            //return Index();
            return RedirectToAction("Index", "Equipo"); 
        }

        /// <summary>
        /// Este método carga la información del equipo que se desea eliminar, esto según el id
        /// </summary>
        /// <returns>La vista Eliminar con el modelo del a eliminar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_EQUIPOS)]
        public ActionResult Eliminar(long id)
        {
            Equipo entity = equipoService.Get(id);
            EquipoModel model = equipoMapper.GetModel(entity);

            return View(model);
        }

        /// <summary>
        /// Este método actualiza el estado IsDeleted a true, con el fin de indicar que este equipo fue eliminado
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_EQUIPOS)]
        public ActionResult Eliminar(EquipoModel model)
        {
            Equipo equipo = equipoService.Get(model.Id);
            equipo.IsDeleted = true;
            equipo.DateLastModified = DateTime.Now;
            equipo.Version++;
            equipo.DeletedBy = NombreUsuarioActual();
            equipo.DeleteDate = DateTime.Now;

            equipoService.Update(equipo);
            // return Index();
            return RedirectToAction("Index", "Equipo"); 
        }

        /// <summary>
        /// Este método carga el modelo del equipo que se desea obtener el detalle
        /// </summary>
        /// <returns>La vista de Detalles con el modelo de equipo que se desea consultar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_CONS_EQUIPOS + "," + WPPConstants.ROLES_ADMIN_EQUIPOS + "," + WPPConstants.ROLES_EDIT_EQUIPOS)]
        public ActionResult Detalles(long id)
        {
            try
            {
                Equipo entity = equipoService.Get(id);
                EquipoModel model = equipoMapper.GetModel(entity);

                return View("Detalles", model);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return View("Detalles");
            }
        }

        /// <summary>
        /// Este método carga el modelo del equipo que se desea obtener la información por medio de su placa
        /// </summary>
        /// <returns>Un json con el modelo de equipo que se desea consultar</returns>
        [HttpPost]
        public JsonResult CargarEquipo(string idEquipo)
        {
            try
            {
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Nombre", idEquipo);
                criteria.Add("IsDeleted", false);

                Equipo equipo = equipoService.Get(criteria);
                EquipoModel model = equipoMapper.GetModel(equipo);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(null);
            }
        }

        /// <summary>
        ///Filtra los clientes que contengan en su nombre la cadena de caracteres que se ha digitado
        /// </summary>
        /// <returns>json con la lista de equipos filtrado por su nombre</returns>
        [HttpGet]
        public JsonResult AutoCompleteEquipo(string term)
        {
            try
            {
                var listaEquipos = equipoService.EquipoSearch(term);
                var result = (from e in listaEquipos select new { id = e.Id, value = e.Nombre, label = e.Nombre });
                return Json(result, JsonRequestBehavior.AllowGet);
             }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(null);
            }
        }


        #region CONEXION BASCULA

        SerialPort serialPort;
        bool _continue;

        [HttpPost]
        public JsonResult ObtenerPeso()
        {
            // Create a new SerialPort object with default settings.
            serialPort = new SerialPort();
            //Thread readThread = new Thread(Read);
            string peso = String.Empty;
            string nombre = Environment.MachineName;
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("NombrePC", nombre);
            ConfiguracionPuerto puerto = puertoService.Get(criteria);
            // Allow the user to set the appropriate properties.
            if (puerto != null)
            {
                serialPort.PortName = puerto.Puerto;
                serialPort.BaudRate = 9600; //9600 300 600  1200  2400  4800  110
                serialPort.Parity = System.IO.Ports.Parity.None;
                serialPort.DataBits = 7;
                serialPort.StopBits = System.IO.Ports.StopBits.One;

                // Set the read/write timeouts
                serialPort.ReadTimeout = 500;
                serialPort.WriteTimeout = 500;

                serialPort.Open();
                _continue = true;
                // readThread.Start();
                //Console.Write("Name: ");
                //name = Console.ReadLine();

                //Console.WriteLine("Type QUIT to exit");

                while (_continue)
                {
                    string mensaje = serialPort.ReadLine();
                    if (mensaje.Length >= 13)
                    {
                        _continue = false;
                        peso = mensaje.Substring(1, 9).Trim();
                    }
                    //if (stringComparer.Equals("quit", message))
                    //{
                    //    _continue = false;
                    //}
                    //else
                    //{
                    //    serialPort.WriteLine(
                    //        String.Format("<{0}>: {1}", name, message));
                    //}
                }

                // readThread.Join();
                serialPort.Close();
            }
            else
            {
                return Json(null);
            }

            return Json(peso);
        }

        public void Read()
        {
            while (_continue)
            {
                try
                {
                    string message = serialPort.ReadLine();
                    Console.WriteLine(message);
                }
                catch (TimeoutException) { }
            }
        }

        #endregion

        /// <summary>
        /// Este método lista y filtra el listado de registros de equipos mostrados en el index
        /// </summary>
        /// <returns>Vista Index con el modelo IPageList(equipo)</returns>
        public ActionResult Buscar(string sortOrder, string currentFilterNombre, string currentFilterPlaca, string currentFilterMarca,
                                    string currentFilterTipo,  string searchStringNombre, string searchStringPlaca, string searchStringMarca, 
                                string searchStringTipo, string searchStringFilas, int? page)
        {
            ViewBag.CurrentSort = sortOrder;

            if (sortOrder == "NombreAsc" || sortOrder == null)
                ViewBag.NombreSortParam = "NombreDesc";
            else
                ViewBag.NombreSortParam = "NombreAsc";

            if (sortOrder == "PlacaAsc" || sortOrder == null)
                ViewBag.PlacaSortParam = "PlacaDesc";
            else
                ViewBag.PlacaSortParam = "PlacaAsc";

            if (sortOrder == "MarcaAsc" || sortOrder == null)
                ViewBag.MarcaSortParam = "MarcaDesc";
            else
                ViewBag.MarcaSortParam = "MarcaAsc";

          

            if (searchStringNombre != null || searchStringPlaca != null || searchStringMarca != null || 
                        searchStringTipo != null || searchStringFilas != null)
                page = 1;
            else
            {
                searchStringNombre = currentFilterNombre;
                searchStringPlaca = currentFilterPlaca;
                searchStringMarca = currentFilterMarca;
                searchStringTipo = currentFilterTipo;
            }

            ViewBag.CurrentNombre = searchStringNombre;
            ViewBag.CurrentPlaca = searchStringPlaca;
            ViewBag.CurrentMarca = searchStringMarca;
            ViewBag.CurrentTipo = searchStringTipo;

            var filas = searchStringFilas == null ? 10 : Convert.ToInt32(searchStringFilas);
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var paginaCompania = equipoService.PagingSearch(searchStringNombre, searchStringPlaca, searchStringMarca, searchStringTipo, 
                                                         pageNumber, filas, sortOrder, CompaniaActual().Id);

            return View("Index", paginaCompania);
        }

    }
}
