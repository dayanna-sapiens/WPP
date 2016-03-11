using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WPP.Entities.Objects;
using WPP.Entities.Objects.Generales;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Helpers;
using WPP.Mapper;
using WPP.Mapper.General;
using WPP.Model;
using WPP.Security;
using WPP.Service.Generales;
using WPP.Service.ModuloContratos;

namespace WPP.Controllers
{
    public class CompaniaController : BaseController
    {
        private ICompaniaService companiaService;
        private CompaniaMapper companiaMapper;
        private CatalogoMapper catalogoMapper;
        private IClienteService clienteService;
        private IConsecutivoClienteService consecutivoService;

        public CompaniaController(ICompaniaService service,  IClienteService cliente, IConsecutivoClienteService consecutivo)
        {
            try
            {
                this.companiaService = service;
                this.clienteService = cliente;
                this.consecutivoService = consecutivo;
                companiaMapper = new CompaniaMapper();
                catalogoMapper = new CatalogoMapper();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        
        private ActionResult RedirectURL(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Compania");
            }
        }
        

        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROLES_ADMINISTRACION + "," + WPPConstants.ROLES_ADMIN_COMPANIA + "," + WPPConstants.ROLES_CONS_COMPANIA + "," + WPPConstants.ROLES_EDIT_COMPANIA)]
        public ActionResult Index()
        {
            return Buscar(null, null, null, null, null, null, null, null, null,null,null);
        }

        /// <summary>
        /// Este método se encarga de retornar la  vista que permite crear Companias y cargar los ViewBags necesarios (TipoCompania, Grupo) 
        /// </summary>
        /// <returns>La vista Crear</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_COMPANIA + "," + WPPConstants.ROLES_EDIT_COMPANIA)]
        public ActionResult Crear()
        {
            ViewBag.ListaTipoCompania = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("TipoCompania"));

            ViewBag.ListaGrupo = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("Grupo")); 
            
            return View();
        }

        /// <summary>
        /// Este método guarda la información del modelo de la compañía ingresada en el formulario de Crear
        /// en caso de que se haya seleccionado la opción de crear un cliente apartir de esta compañía 
        /// se debe crear un cliente con la información de esta compañía
        /// </summary>
        /// <returns>La vista Index</returns>
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_COMPANIA + "," + WPPConstants.ROLES_EDIT_COMPANIA)]
        public ActionResult Crear(CompaniaModel compania)
        {
            if (ModelState.IsValid)
            {
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Nombre", compania.Nombre);
                criteria.Add("IsDeleted", false);

                if (companiaService.Get(criteria) == null)
                {
                    Compania nuevaCompania = new Compania();
                    nuevaCompania = companiaMapper.GetCompania(compania, nuevaCompania);
                    nuevaCompania.Version = 1;
                    nuevaCompania.CreateDate = DateTime.Now;
                    nuevaCompania.DateLastModified = DateTime.Now;
                    nuevaCompania.CreatedBy = NombreUsuarioActual();
                    nuevaCompania.ClienteId = 0;
                    if ( compania.Grupo > 0)
                    {
                        nuevaCompania.Grupo = CatalogoService.Get(compania.Grupo);
                    }

                    if (compania.Tipo > 0)
                    {
                        nuevaCompania.Tipo = CatalogoService.Get(compania.Tipo);
                    }
                    
                    // Se crea la compañía
                    nuevaCompania = companiaService.Create(nuevaCompania);

                    // En caso que se desee incluir como cliente, se crea el cliente referenciado a la compañía
                    if (compania.ClienteId == 1)
                    {
                        Cliente cliente = new Cliente();
                        cliente.Cedula = nuevaCompania.Cedula;
                        cliente.Email = nuevaCompania.Email;
                        cliente.Grupo = nuevaCompania.Grupo;
                        cliente.Tipo = nuevaCompania.Tipo;
                        cliente.Nombre = nuevaCompania.Nombre;
                        cliente.NombreCorto = nuevaCompania.NombreCorto;
                        cliente.RepresentanteLegal = nuevaCompania.RepresentanteLegal;
                        cliente.Telefono1 = nuevaCompania.Telefono;
                        cliente.CompaniaId = nuevaCompania.Id;
                        cliente.Version = 1;
                        cliente.CreateDate = DateTime.Now;
                        cliente.CreatedBy = NombreUsuarioActual();

                        ConsecutivoCliente consecutivo = consecutivoService.GetAll().FirstOrDefault();
                        cliente.Numero = consecutivo.Secuencia;

                        //Se actualiza el consecutivo del cliente
                        consecutivo.Secuencia++;
                        consecutivoService.Update(consecutivo);

                        cliente = clienteService.Create(cliente);

                        //Se actualiza la compañía indicando a cual cliente esta ligado
                        nuevaCompania.ClienteId = cliente.Id;
                        companiaService.Update(nuevaCompania);
                    }

                    ViewBag.Mensaje = "Se ha creado la compañía";

                    //return Index();
                    return RedirectToAction("Index", "Compania");
                }
                else
                {
                    ModelState.AddModelError("Nombre", "Nombre no válido, ya existe una compañía registrada con este nombre");
                    ViewBag.ListaTipoCompania = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("TipoCompania"));
                    ViewBag.ListaGrupo = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("Grupo")); 

                    return View(compania);
                }
            }
            else
            {
                ViewBag.ListaTipoCompania = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("TipoCompania"));
                ViewBag.ListaGrupo = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("Grupo")); 

                return View();
            }
        }

        /// <summary>
        /// Este método carga la información de la compañía que se desea editar, por medio de su id
        /// y se cargan los viewBags necesarios (TipoComapania, Grupo)     
        /// </summary>
        /// <returns>La vista Editar con el modelo de la compañía a editar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_COMPANIA + "," + WPPConstants.ROLES_EDIT_COMPANIA)]
        public ActionResult Editar(long idCompania)
        {
            CompaniaModel compania = companiaMapper.GetCompaniaModel(companiaService.Get(idCompania));
            String testing  = DateTime.Now.ToShortDateString();
            testing+="";
            ViewBag.ListaTipoCompania = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("TipoCompania"));
            ViewBag.ListaGrupo = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("Grupo")); 
            
            return View(compania);
        }

        /// <summary>
        /// Este método actualiza la información de la compañía seleccionado, según lo datos del modelo
        /// en caso de que esta compañía a su vez sea cliente se actualiza la información del cliente también
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_COMPANIA + "," + WPPConstants.ROLES_EDIT_COMPANIA)]
        public ActionResult Editar(CompaniaModel companiaModel)
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Nombre", companiaModel.Nombre);
            criteria.Add("IsDeleted", false);
            Compania companiaValidacion = companiaService.Get(criteria);
            if (companiaValidacion != null)
            {
                if (companiaValidacion.Id != companiaModel.Id)
                {
                    ModelState.AddModelError("Nombre", "Nombre no válido, ya existe una compañía registrada con este nombre");

                    ViewBag.ListaTipoCompania = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("TipoCompania"));
                    ViewBag.ListaGrupo = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("Grupo")); 
                                            
                    return View(companiaModel);
                }
            }

            Compania compania = companiaService.Get(companiaModel.Id);
            long clienteID = compania.ClienteId;
            compania = companiaMapper.GetCompania(companiaModel, compania);
            compania.ClienteId = clienteID;
            compania.DateLastModified = DateTime.Now;
            compania.ModifiedBy = NombreUsuarioActual();
            compania.Version++;

            if (companiaModel.Grupo > 0)
            {
                compania.Grupo = CatalogoService.Get(companiaModel.Grupo);
            }

            if (companiaModel.Tipo > 0)
            {
                compania.Tipo = CatalogoService.Get(companiaModel.Tipo);
            }

            // Se actualiza la información de la compañía
            compania = companiaService.Update(compania);

            // En caso que desee crear un cliente apartir de esta compañía y esto no lo haya hecho antes, se crea el cliente
            if(clienteID == 0 && companiaModel.ClienteId != 0)
            {
                Cliente cliente = new Cliente();
                cliente.Cedula = compania.Cedula;
                cliente.Email = compania.Email;
                cliente.Grupo = compania.Grupo;
                cliente.Tipo = compania.Tipo;
                cliente.Nombre = compania.Nombre;
                cliente.NombreCorto = compania.NombreCorto;
                cliente.RepresentanteLegal = compania.RepresentanteLegal;
                cliente.Telefono1 = compania.Telefono;
                cliente.CompaniaId = compania.Id;
                cliente.Version = 1;
                cliente.CreateDate = DateTime.Now;
                cliente.CreatedBy = NombreUsuarioActual();

                ConsecutivoCliente consecutivo = consecutivoService.GetAll().FirstOrDefault();
                cliente.Numero = consecutivo.Secuencia;

                //Se actualiza el consecutivo del cliente
                consecutivo.Secuencia++;
                consecutivoService.Update(consecutivo);

                cliente = clienteService.Create(cliente);

                //Se actualiza la compañía indicando a cual cliente esta ligado
                compania.ClienteId = cliente.Id;
                companiaService.Update(compania);
            }

            if (clienteID != 0)
            {
                Cliente cliente = clienteService.Get(clienteID);
                cliente.Cedula = compania.Cedula;
                cliente.Email = compania.Email;
                cliente.Grupo = compania.Grupo;
                cliente.Tipo = compania.Tipo;
                cliente.Nombre = compania.Nombre;
                cliente.NombreCorto = compania.NombreCorto;
                cliente.RepresentanteLegal = compania.RepresentanteLegal;
                cliente.Telefono1 = compania.Telefono;
                cliente.CompaniaId = compania.Id;
                cliente.Version++;
                cliente.DateLastModified = DateTime.Now;
                cliente.ModifiedBy = NombreUsuarioActual();

                cliente = clienteService.Update(cliente);                
            }

            //return Index(); 
            return RedirectToAction("Index", "Compania");
        }

        /// <summary>
        /// Este método carga la información de la compañía que se desea eliminar, esto según el id
        /// </summary>
        /// <returns>La vista Eliminar con el modelo del a eliminar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_COMPANIA)]
        public ActionResult Eliminar(long idCompania)
        {
            Compania compania = companiaService.Get(idCompania);
            CompaniaModel companiaModel = companiaMapper.GetCompaniaModel(compania);            
            companiaModel.GrupoNombre = compania.Grupo.Nombre;
            companiaModel.TipoNombre = compania.Tipo.Nombre;

            return View(companiaModel);
        }

        /// <summary>
        /// Este método actualiza el estado IsDeleted a true, con el fin de indicar que esta compañía fue eliminada
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_COMPANIA)]
        public ActionResult Eliminar(CompaniaModel companiaModel)
        {
            Compania compania = companiaService.Get(companiaModel.Id);
            compania.IsDeleted = true;
            compania.DateLastModified = DateTime.Now;
            compania.Version++;
            compania.DeletedBy = NombreUsuarioActual();
            compania.DeleteDate = DateTime.Now;

            companiaService.Update(compania);

            //return Index();
            return RedirectToAction("Index", "Compania");
        }

        /// <summary>
        /// Este método carga el modelo de la compañía que se desea obtener el detalle
        /// </summary>
        /// <returns>La vista de Detalles con el modelo de la compañía que se desea consultar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_COMPANIA + "," + WPPConstants.ROLES_CONS_COMPANIA + "," + WPPConstants.ROLES_EDIT_COMPANIA)]
        public ActionResult Visualizar(long idCompania)
        {
            Compania compania = companiaService.Get(idCompania);
            CompaniaModel companiaModel = companiaMapper.GetCompaniaModel(compania);
            companiaModel.GrupoNombre = compania.Grupo.Nombre;
            companiaModel.TipoNombre = compania.Tipo.Nombre;

            return View(companiaModel);
        }

        /// <summary>
        /// Este método lista y filtra el listado de registros de compañías mostrados en el index
        /// </summary>
        /// <returns>Vista Index con el modelo IPageList(Compania)</returns>
        public ActionResult Buscar(string sortOrder, string currentFilterNombre, string currentFilterNombreCorto, string currentFilterGrupo,string currentFilterTipo,
                    string searchStringNombre, string searchStringNombreCorto, string searchStringGrupo, string searchStringTipo, string searchStringFilas, int? page)
        {
            ViewBag.CurrentSort = sortOrder;

            if(sortOrder == "NombreAsc")
                ViewBag.NameSortParam = "NombreDesc";
            else
                ViewBag.NameSortParam = "NombreAsc";

            if (sortOrder == "NombreCortoAsc")
                ViewBag.NombreCortoSortParam = "NombreCortoDesc";
            else
                ViewBag.NombreCortoSortParam = "NombreCortoAsc";

            if (sortOrder == "GrupoAsc")
                ViewBag.GrupoSortParam = "GrupoDesc";
            else
                ViewBag.GrupoSortParam = "GrupoAsc";

            if (sortOrder == "TipoAsc")
                ViewBag.TipoSortParam = "TipoDesc";
            else
                ViewBag.TipoSortParam = "TipoAsc";

            if (searchStringNombre != null || searchStringNombreCorto != null || searchStringGrupo != null || searchStringTipo != null)
                page = 1;
            else
            {
                searchStringNombre = currentFilterNombre;
                searchStringNombreCorto = currentFilterNombreCorto;
                searchStringGrupo = currentFilterGrupo;
                searchStringTipo= currentFilterTipo;
            }

            ViewBag.CurrentNombre = searchStringNombre;
            ViewBag.CurrentNombreCorto = searchStringNombreCorto;
            ViewBag.CurrentGrupo = searchStringGrupo;
            ViewBag.CurrentTipo = searchStringTipo;

            var filas = searchStringFilas == null ? 10 : Convert.ToInt32(searchStringFilas);
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var paginaCompania = companiaService.PagingSearch(searchStringNombre, searchStringNombreCorto, searchStringGrupo,searchStringTipo, pageNumber, filas, sortOrder);

            return View("Index", paginaCompania);
        }


    }
}
