using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WPP.Entities.Objects.ModuloBascula;
using WPP.Helpers;
using WPP.Mapper.ModuloBascula;
using WPP.Mapper.ModuloOperacionRecoleccion;
using WPP.Model.ModuloBascula;
using WPP.Model.ModuloOperacionRecoleccion;
using WPP.Security;
using WPP.Service.ModuloBascula;
using WPP.Service.ModuloOperacionRecoleccion;

namespace WPP.Controllers
{
    public class ContenedorController : BaseController
    {
        private IContenedorService contenedorService;
        private IContenedorHistorialService historialService;
        private ContenedorMapper contenedorMapper;
        private IViajeOTRService viajeOTRService;
        private ContenedorHistorialMapper historialMapper;
        private IConfiguracionPuertoService puertoService;

        public ContenedorController(IContenedorService _contenedorService, IViajeOTRService viajeOtr, IContenedorHistorialService historial, IConfiguracionPuertoService puerto)
        {
            contenedorService = _contenedorService;
            viajeOTRService = viajeOtr;
            historialService = historial;
            puertoService = puerto;
            contenedorMapper = new ContenedorMapper();
            historialMapper = new ContenedorHistorialMapper();
        }


        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROLES_ADMINISTRACION + "," + WPPConstants.ROLES_CONS_CONTENEDORES + "," + WPPConstants.ROLES_ADMIN_CONTENEDORES + "," + WPPConstants.ROLES_EDIT_CONTENEDORES)]
        public ActionResult Index()
        {
            return Buscar(null, null, null, null, null, null, null,null,null);
        }

        /// <summary>
        /// Este método se encarga de retornar la  vista que permite crear Contenedores 
        /// </summary>
        /// <returns>La vista Crear</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CONTENEDORES + "," + WPPConstants.ROLES_EDIT_CONTENEDORES)]
        public ActionResult Crear()
        {
            ContenedorModel model = new ContenedorModel();
       
            return View(model);
        }

        /// <summary>
        /// Este método guarda la información del modelo ingresada en el formulario de Crear
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CONTENEDORES + "," + WPPConstants.ROLES_EDIT_CONTENEDORES)]
        public ActionResult Crear(ContenedorModel model)
        {
            if (ModelState.IsValid)
            {
                Contenedor nuevo = new Contenedor();
                nuevo = contenedorMapper.GetEntity(model, nuevo);
               
                nuevo.Version = 1;
                nuevo.CreateDate = DateTime.Now;
                nuevo.DateLastModified = DateTime.Now;
                nuevo.CreatedBy = NombreUsuarioActual();
              
                contenedorService.Create(nuevo);

                ViewBag.Mensaje = "Se ha creado un nuevo contenedor.";

                return RedirectToAction("Index", "Contenedor"); 
                //return Index();
            }
            else
            {
                return View(model);
            }
        }

        /// <summary>
        /// Este método carga la información del contenedor que se desea editar, por medio de su id
        /// </summary>
        /// <returns>La vista Editar con el modelo del contenedor a editar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CONTENEDORES + "," + WPPConstants.ROLES_EDIT_CONTENEDORES)]
        public ActionResult Editar(long id)
        {
            Contenedor entity = contenedorService.Get(id);
            ContenedorModel model = contenedorMapper.GetModel(entity);

            return View("Editar", model);
        }

        /// <summary>
        /// Este método actualiza la información del contenedor seleccionado, según lo datos del modelo
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CONTENEDORES + "," + WPPConstants.ROLES_EDIT_CONTENEDORES)]
        public ActionResult Editar(ContenedorModel model)
        {

            Contenedor entity = contenedorService.Get(model.Id);
            entity = contenedorMapper.GetEntity(model, entity);

            entity.DateLastModified = DateTime.Now;
            entity.ModifiedBy = ObtenerUsuarioActual().Nombre;
            entity.Version++;

            contenedorService.Update(entity);

            // return Index();
            return RedirectToAction("Index", "Contenedor"); 
        }

        /// <summary>
        /// Este método carga la información del contenedor que se desea eliminar, esto según el id
        /// </summary>
        /// <returns>La vista Eliminar con el modelo del contenedor a eliminar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CONTENEDORES)]
        public ActionResult Eliminar(long id)
        {
            Contenedor entity = contenedorService.Get(id);
            ContenedorModel model = contenedorMapper.GetModel(entity);
            
            return View(model);
        }

        /// <summary>
        /// Este método actualiza el estado IsDeleted a true, con el fin de indicar que este contenedor fue eliminado
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CONTENEDORES)]
        public ActionResult Eliminar(ContenedorModel model)
        {
            Contenedor contenedor = contenedorService.Get(model.Id);
            contenedor.IsDeleted = true;
            contenedor.DateLastModified = DateTime.Now;
            contenedor.Version++;
            contenedor.DeletedBy = NombreUsuarioActual();
            contenedor.DeleteDate = DateTime.Now;

            contenedorService.Update(contenedor);
            //return Index();
            return RedirectToAction("Index", "Contenedor"); 
        }

        /// <summary>
        /// Este método carga la lista de ubicaciones que ha tenido este contenedor 
        /// </summary>
        /// <returns>La vista de HistorialUbicaciones con el modelo de contenedor que se desea consultar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_CONS_CONTENEDORES + "," + WPPConstants.ROLES_ADMIN_CONTENEDORES + "," + WPPConstants.ROLES_EDIT_CONTENEDORES)]
        public ActionResult HistorialUbicacion(long id)
        {
            Contenedor entity = contenedorService.Get(id);
            ContenedorModel model = contenedorMapper.GetModel(entity);

            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Contenedor", entity);
            criteria.Add("IsDeleted", false);
            var Lista = historialService.GetAll(criteria);
            List<OTRModel> listaModel = new List<OTRModel>();
            int cont = 0, index = 0;
            while (index < Lista.Count && cont <= 10)
            {
                var item = Lista[index];
                 if (item.OTR.Estado == "Procesada" || item.OTR.Estado == "Cerrada")
                 {
                     OTRModel ubicacion = new OTRModel()
                    {
                        DescripcionCliente = item.Cliente,
                        Fecha = item.OTR.Fecha,
                        Consecutivo = item.OTR.Consecutivo,
                        RutaRecoleccionDescripcion = item.Ubicacion != null ? item.Ubicacion : String.Empty
                    };

                    listaModel.Add(ubicacion);
                    cont++;
                 }
                 index++;
            }
           

            ViewBag.ListaUbicaciones = listaModel;

            return View("HistorialUbicacion", model);
        }

        /// <summary>
        /// Este método carga el modelo del contenedor que se desea obtener el detalle
        /// </summary>
        /// <returns>La vista de Detalles con el modelo de contenedor que se desea consultar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_CONS_CONTENEDORES + "," + WPPConstants.ROLES_ADMIN_CONTENEDORES + "," + WPPConstants.ROLES_EDIT_CONTENEDORES)]
        public ActionResult Detalles(long id)
        {
            Contenedor entity = contenedorService.Get(id);
            ContenedorModel model = contenedorMapper.GetModel(entity);

            return View("Detalles", model);
        }

        /// <summary>
        /// Este método carga el modelo del contenedor que se desea obtener la información
        /// </summary>
        /// <returns>Un json con el modelo de contenedor que se desea consultar</returns>
        [HttpPost]
        public JsonResult CargarContenedor(string idContenedor)
        {
            try
            {
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Codigo", idContenedor);

                criteria.Add("IsDeleted", false);
                Contenedor contenedor = contenedorService.Get(criteria);
                ContenedorModel model = contenedorMapper.GetModel(contenedor);
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
        public JsonResult AutoCompleteContenedor(string term)
        {
            var listaContenedores = contenedorService.ContenedorSearch(term);
            var result = (from e in listaContenedores select new { id = e.Id, value = e.Codigo, label = e.Codigo });
            return Json(result, JsonRequestBehavior.AllowGet);
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
        /// Este método lista y filtra el listado de registros de contenedores mostrados en el index
        /// </summary>
        /// <returns>Vista Index con el modelo IPageList(Contenedor)</returns>
        public ActionResult Buscar(string sortOrder, string currentFilterDescripcion, string currentFilterCodigo, string currentFilterEstado,
                                string searchStringDescripcion, string searchStringCodigo, string searchStringEstado, string searchStringFilas, int? page)
        {
            ViewBag.CurrentSort = sortOrder;

            if (sortOrder == "DescripcionAsc" || sortOrder == null)
                ViewBag.DescripcionSortParam = "DescripcionDesc";
            else
                ViewBag.DescripcionSortParam = "DescripcionAsc";

            if (sortOrder == "CodigoAsc" || sortOrder == null)
                ViewBag.CodigoSortParam = "CodigoDesc";
            else
                ViewBag.CodigoSortParam = "CodigoAsc";
            
            if (sortOrder == "EstadoAsc" || sortOrder == null)
                ViewBag.EstadoSortParam = "EstadoDesc";
            else
                ViewBag.EstadoSortParam = "EstadoAsc";

            if (searchStringDescripcion != null || searchStringCodigo != null || searchStringEstado != null || searchStringFilas != null)
                page = 1;
            else
            {
                searchStringDescripcion= currentFilterDescripcion;
                searchStringCodigo = currentFilterCodigo;
                searchStringEstado = currentFilterEstado;
            }

            ViewBag.CurrentDescripcion = searchStringDescripcion;
            ViewBag.CurrentCodigo = searchStringCodigo;
            ViewBag.CurrentEstado = searchStringEstado;

            var filas = searchStringFilas == null ? 10 : Convert.ToInt32(searchStringFilas);
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var paginaCompania = contenedorService.PagingSearch(searchStringDescripcion, searchStringCodigo, searchStringEstado, pageNumber, filas, sortOrder, CompaniaActual().Id);

            return View("Index", paginaCompania);
        }
    }
}
