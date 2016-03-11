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
using WPP.Mapper.General;
using WPP.Mapper.ModuloContratos;
using WPP.Model.General;
using WPP.Model.ModuloContratos;
using WPP.Security;
using WPP.Service.Generales;
using WPP.Service.ModuloContratos;

namespace WPP.Controllers
{
    public class ClienteController : BaseController
    {
        public IClienteService ClienteService { get; set; }
        public IContactoClienteService ContactoService { get; set; }
        public ICompaniaService CompaniaService { get; set; }
        public IUbicacionClienteService UbicacionService { get; set; }
        public IContratoService ContratoService { get; set; }
        private IConsecutivoClienteService consecutivoService { set; get; }
        private IClienteBitacoraService bitacoraService { set; get; }
        private ClienteMapper clienteMapper;
        private RegionMapper regionMapper;
        private CatalogoMapper catalogoMapper;
        private ContactoClienteMapper contactoMapper;
        private UbicacionClienteMapper ubicacionMapper;



        public ClienteController(IClienteService clienteService, ICatalogoService catalogoService, IRegionService regionService, IContactoClienteService contactoService, 
            ICompaniaService companiaService, IUbicacionClienteService ubicacionService, IContratoService  contratoService, IConsecutivoClienteService consecutivo,
            IClienteBitacoraService clienteBitacora)
        {
            try
            {
                this.ClienteService = clienteService;
                this.CatalogoService = catalogoService;
                clienteMapper = new ClienteMapper();
                regionMapper = new RegionMapper();
                catalogoMapper = new CatalogoMapper();
                contactoMapper = new ContactoClienteMapper();
                ubicacionMapper = new UbicacionClienteMapper();
                this.RegionesService = regionService;
                this.ContactoService = contactoService;
                this.CompaniaService = companiaService;
                this.UbicacionService = ubicacionService;
                this.ContratoService = contratoService;
                this.consecutivoService = consecutivo;
                this.bitacoraService = clienteBitacora;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }


        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROLES_ADMINISTRACION + "," + WPPConstants.ROLES_ADMIN_CLIENTES + "," + WPPConstants.ROLES_EDIT_CLIENTES + "," + WPPConstants.ROLES_CONS_CLIENTES)]
        public ActionResult Index()
        {
            return Buscar(null, null, null, null, null, null, null, null, null, null, null, null,null);
        }

        #region CLIENTE
        /// <summary>
        /// Este método se encarga de retornar la  vista que permite crear clientes 
        /// y carga los viewbags necesarios (TipoCompania, Gruposm Provincias, Cantones y Distritos) 
        /// </summary>
        /// <returns>La vista Crear</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CLIENTES + "," + WPPConstants.ROLES_EDIT_CLIENTES)]
        public ActionResult Crear()
        {

            ClienteModel model = new ClienteModel();
            ViewBag.ListaTipoCompania = CatalogoService.GetByType("TipoCompania");
            ViewBag.ListaGrupos = CatalogoService.GetByType("Grupo");
            ViewBag.ListaProvincias = RegionesService.GetAll(0).OrderBy(x => x.Nombre).ToList();
            ViewBag.ListaCantones = new List<Region>();
            ViewBag.ListaDistritos = new List<Region>();

            return View(model);
        }

        /// <summary>
        /// Este método guarda la información del modelo cliente ingresada en el formulario de Crear
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CLIENTES + "," + WPPConstants.ROLES_EDIT_CLIENTES)]
        public ActionResult Crear(ClienteModel cliente)
        {
            if (ModelState.IsValid)
            {
                Cliente nuevo = new Cliente();
                nuevo = clienteMapper.GetCliente(cliente, nuevo);
                nuevo.Tipo = CatalogoService.Get(cliente.Tipo);
                nuevo.Grupo = CatalogoService.Get(cliente.Grupo);
                nuevo.Numero = cliente.Numero.HasValue ? cliente.Numero.Value : 0;
                nuevo.Version = 1;
                nuevo.CreateDate = DateTime.Now;
                nuevo.DateLastModified = DateTime.Now;
                nuevo.CreatedBy = NombreUsuarioActual();

                if (cliente.Provincia.HasValue && cliente.Provincia.Value > 0)
                    nuevo.Provincia = RegionesService.Get(cliente.Provincia.Value);

                if (cliente.Canton.HasValue && cliente.Canton.Value > 0)
                    nuevo.Canton = RegionesService.Get(cliente.Canton.Value);

                if (cliente.Distrito.HasValue && cliente.Distrito.Value > 0)
                    nuevo.Distrito = RegionesService.Get(cliente.Distrito.Value);

                //nuevo.Contactos = JsonConvert.DeserializeObject<List<ContactosCompania>>(cliente.CadenaContactos);

                ConsecutivoCliente consecutivo = consecutivoService.GetAll().FirstOrDefault();
                nuevo.Numero = consecutivo.Secuencia;

                //Se actualiza el consecutivo del cliente
                consecutivo.Secuencia++;
                consecutivoService.Update(consecutivo);

                ClienteService.Create(nuevo);

                ViewBag.Mensaje = "Se ha creado un nuevo cliente.";

                return RedirectToAction("Detalles", new { idCliente = nuevo.Id });//Detalles(nuevo.Id);
            }
            else
            {
                ViewBag.ListaTipoCompania = CatalogoService.GetByType("TipoCompania");
                ViewBag.ListaGrupos = CatalogoService.GetByType("Grupo");
                ViewBag.ListaProvincias = RegionesService.GetAll(0).OrderBy(x => x.Nombre).ToList();

                ViewBag.ListaCantones = new List<Region>();
                ViewBag.ListaDistritos = new List<Region>();

                return View();
            }
        }

        /// <summary>
        /// Este método carga la información del cliente que se desea editar, por medio de su id
        /// y carga los viewbags necesarios (TipoCompania, Gruposm Provincias, Cantones y Distritos) 
        /// </summary>
        /// <returns>La vista Editar con el modelo del contenedor a editar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CLIENTES + "," + WPPConstants.ROLES_EDIT_CLIENTES)]
        public ActionResult Editar(long idCliente)
        {
            Cliente entity = ClienteService.Get(idCliente);
            ClienteModel cliente = clienteMapper.GetClienteModel(entity);

            cliente.FechaDesactivacion = entity.FechaDesactivacion;

            ViewBag.ListaTipoCompania = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("TipoCompania"));
            ViewBag.ListaGrupos = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("Grupo"));
            ViewBag.ListaProvincias = regionMapper.GetListaRegionModel(RegionesService.GetAll(0).OrderBy(x => x.Nombre).ToList());

            cliente.Contactos = entity.Contactos;

            if (entity.Provincia != null)
            {
                cliente.Provincia = entity.Provincia.Id;
                ViewBag.ListaCantones = regionMapper.GetListaRegionModel(RegionesService.GetAll(cliente.Provincia.Value).OrderBy(x => x.Nombre).ToList());
            }

            if (entity.Canton != null)
            {
                cliente.Canton = entity.Canton.Id;
                ViewBag.ListaDistritos = regionMapper.GetListaRegionModel(RegionesService.GetAll(cliente.Canton.Value).OrderBy(x => x.Nombre).ToList());
            }

            if (entity.Distrito != null)
            {
                cliente.Distrito = entity.Distrito.Id;
            }

            return View("Editar", cliente);
        }

        /// <summary>
        /// Este método actualiza la información del cliente seleccionado, según lo datos del modelo
        /// en caso de que este cliente este asociado a una compañía tambien actualliza la información de la compañía
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CLIENTES + "," + WPPConstants.ROLES_EDIT_CLIENTES)]
        public ActionResult Editar(ClienteModel clienteModel)
        {
            Cliente cliente = ClienteService.Get(clienteModel.Id);
            GuardarBitacora(cliente, clienteModel); // Se guarda los cambios realizados en la bitacora
            cliente = clienteMapper.GetCliente(clienteModel, cliente);
            cliente.Grupo = CatalogoService.Get(clienteModel.Grupo);
            cliente.Tipo = CatalogoService.Get(clienteModel.Tipo);
            cliente.Numero = clienteModel.Numero.HasValue ? clienteModel.Numero.Value : 0;
            cliente.DateLastModified = DateTime.Now;
            cliente.ModifiedBy = ObtenerUsuarioActual().Nombre;
            cliente.Version++;

            if (clienteModel.Provincia.HasValue)
                cliente.Provincia = RegionesService.Get(clienteModel.Provincia.Value);

            if (clienteModel.Canton.HasValue)
                cliente.Canton = RegionesService.Get(clienteModel.Canton.Value);

            if (clienteModel.Distrito.HasValue)
                cliente.Distrito = RegionesService.Get(clienteModel.Distrito.Value);
            
            cliente = ClienteService.Update(cliente);

            if (cliente.CompaniaId != 0)
            {
                Compania compania = CompaniaService.Get(cliente.CompaniaId);
                compania.Cedula = cliente.Cedula;
                compania.Email = cliente.Email;
                compania.Grupo = cliente.Grupo;
                compania.Nombre = cliente.Nombre;
                compania.NombreCorto = cliente.NombreCorto;
                compania.RepresentanteLegal = cliente.RepresentanteLegal;
                compania.Telefono = cliente.Telefono1;
                compania.Tipo = cliente.Tipo;
                compania.Version++;
                compania.ModifiedBy = NombreUsuarioActual();
                compania.DateLastModified = DateTime.Now;

                CompaniaService.Update(compania);
            }

            //return Index();
            return RedirectToAction("Index", "Cliente");
        }

        public void GuardarBitacora(Cliente cliente, ClienteModel model)
        {
            ClienteBitacora bitacora = new ClienteBitacora();
            string usuario =  ObtenerUsuarioActual().Nombre;

            if(cliente.Numero != model.Numero)
            {
                bitacora.CreateDate = DateTime.Now;
                bitacora.CreatedBy = usuario;
                bitacora.Campo = "Número Empleado";
                bitacora.valorAnterior = cliente.Numero != null ? cliente.Numero.ToString() : String.Empty;
                bitacora.valorNuevo = model.Numero != null ? model.Numero.ToString() : String.Empty;
                bitacora.Cliente = cliente;
                bitacoraService.Create(bitacora);
            }

            if (cliente.Nombre != model.Nombre)
            {
                bitacora = new ClienteBitacora();
                bitacora.CreateDate = DateTime.Now;
                bitacora.CreatedBy = usuario;
                bitacora.Campo = "Nombre";
                bitacora.valorAnterior = cliente.Nombre;
                bitacora.valorNuevo = model.Nombre;
                bitacora.Cliente = cliente;
                bitacoraService.Create(bitacora);
            }

            if (cliente.Cedula != model.Cedula)
            {
                bitacora = new ClienteBitacora();
                bitacora.CreateDate = DateTime.Now;
                bitacora.CreatedBy = usuario;
                bitacora.Campo = "Cedula";
                bitacora.valorAnterior = cliente.Cedula;
                bitacora.valorNuevo = model.Cedula;
                bitacora.Cliente = cliente;
                bitacoraService.Create(bitacora);
            }

            if (cliente.Cedula != model.Cedula)
            {
                bitacora = new ClienteBitacora();
                bitacora.CreateDate = DateTime.Now;
                bitacora.CreatedBy = usuario;
                bitacora.Campo = "Número Empleado";
                bitacora.valorAnterior = cliente.Numero.ToString();
                bitacora.valorNuevo = model.Numero.ToString();
                bitacora.Cliente = cliente;
                bitacoraService.Create(bitacora);
            }

            if (cliente.NombreComercial != model.NombreComercial)
            {
                bitacora = new ClienteBitacora();
                bitacora.CreateDate = DateTime.Now;
                bitacora.CreatedBy = usuario;
                bitacora.Campo = "Nombre Comercial";
                bitacora.valorAnterior = cliente.NombreComercial;
                bitacora.valorNuevo = model.NombreComercial;
                bitacora.Cliente = cliente;
                bitacoraService.Create(bitacora);
            }

            if (cliente.NombreCorto != model.NombreCorto)
            {
                bitacora = new ClienteBitacora();
                bitacora.CreateDate = DateTime.Now;
                bitacora.CreatedBy = usuario;
                bitacora.Campo = "Nombre Corto";
                bitacora.valorAnterior = cliente.NombreCorto;
                bitacora.valorNuevo = model.NombreCorto;
                bitacora.Cliente = cliente;
                bitacoraService.Create(bitacora);
            }

            if (cliente.Email != model.Email)
            {
                bitacora = new ClienteBitacora();
                bitacora.CreateDate = DateTime.Now;
                bitacora.CreatedBy = usuario;
                bitacora.Campo = "Email";
                bitacora.valorAnterior = cliente.Email;
                bitacora.valorNuevo = model.Email;
                bitacora.Cliente = cliente;
                bitacoraService.Create(bitacora);
            }

            if (cliente.Telefono1 != model.Telefono1)
            {
                bitacora = new ClienteBitacora();
                bitacora.CreateDate = DateTime.Now;
                bitacora.CreatedBy = usuario;
                bitacora.Campo = "Teléfono 1";
                bitacora.valorAnterior = cliente.Telefono1;
                bitacora.valorNuevo = model.Telefono1;
                bitacora.Cliente = cliente;
                bitacoraService.Create(bitacora);
            }

            if (cliente.Telefono2 != model.Telefono2)
            {
                bitacora = new ClienteBitacora();
                bitacora.CreateDate = DateTime.Now;
                bitacora.CreatedBy = usuario;
                bitacora.Campo = "Teléfono 2";
                bitacora.valorAnterior = cliente.Telefono2;
                bitacora.valorNuevo = model.Telefono2;
                bitacora.Cliente = cliente;
                bitacoraService.Create(bitacora);
            }

            if (cliente.Fax != model.Fax)
            {
                bitacora = new ClienteBitacora();
                bitacora.CreateDate = DateTime.Now;
                bitacora.CreatedBy = usuario;
                bitacora.Campo = "Fax";
                bitacora.valorAnterior = cliente.Fax;
                bitacora.valorNuevo = model.Fax;
                bitacora.Cliente = cliente;
                bitacoraService.Create(bitacora);
            }

            if (cliente.RepresentanteLegal != model.RepresentanteLegal)
            {
                bitacora = new ClienteBitacora();
                bitacora.CreateDate = DateTime.Now;
                bitacora.CreatedBy = usuario;
                bitacora.Campo = "Representante Legal";
                bitacora.valorAnterior = cliente.RepresentanteLegal;
                bitacora.valorNuevo = model.RepresentanteLegal;
                bitacora.Cliente = cliente;
                bitacoraService.Create(bitacora);
            }
            if (cliente.Direccion != model.Direccion)
            {
                bitacora = new ClienteBitacora();
                bitacora.CreateDate = DateTime.Now;
                bitacora.CreatedBy = usuario;
                bitacora.Campo = "Dirección";
                bitacora.valorAnterior = cliente.Direccion;
                bitacora.valorNuevo = model.Direccion;
                bitacora.Cliente = cliente;
                bitacoraService.Create(bitacora);
            }
            if (cliente.Numero != model.Numero)
            {
                bitacora = new ClienteBitacora();
                bitacora.CreateDate = DateTime.Now;
                bitacora.CreatedBy = usuario;
                bitacora.Campo = "Número Empleado";
                bitacora.valorAnterior = cliente.Numero.ToString();
                bitacora.valorNuevo = model.Numero.ToString();
                bitacora.Cliente = cliente;
                bitacoraService.Create(bitacora);
            }
            if (cliente.Grupo.Id != model.Grupo)
            {
                bitacora = new ClienteBitacora();
                bitacora.CreateDate = DateTime.Now;
                bitacora.CreatedBy = usuario;
                bitacora.Campo = "Grupo";
                Catalogo grupoActual = CatalogoService.Get(model.Grupo);
                bitacora.valorAnterior = cliente.Grupo.Nombre;
                bitacora.valorNuevo = grupoActual.Nombre;
                bitacora.Cliente = cliente;
                bitacoraService.Create(bitacora);
            }
            if (cliente.Tipo.Id != model.Tipo)
            {
                bitacora = new ClienteBitacora();
                bitacora.CreateDate = DateTime.Now;
                bitacora.CreatedBy = usuario;
                bitacora.Campo = "Tipo";
                Catalogo tipoActual = CatalogoService.Get(model.Tipo);
                bitacora.valorAnterior = cliente.Tipo.Nombre;
                bitacora.valorNuevo = tipoActual.Nombre;
                bitacora.Cliente = cliente;
                bitacoraService.Create(bitacora);
            }
            if (cliente.FechaDesactivacion != model.FechaDesactivacion)
            {
                bitacora = new ClienteBitacora();
                bitacora.CreateDate = DateTime.Now;
                bitacora.CreatedBy = usuario;
                bitacora.Campo = "Fecha Desactivación";
                bitacora.valorAnterior = cliente.FechaDesactivacion != null ? cliente.FechaDesactivacion.ToString() : String.Empty;
                bitacora.valorNuevo = model.FechaDesactivacion != null ? model.FechaDesactivacion.ToString() : String.Empty;
                bitacora.Cliente = cliente;
                bitacoraService.Create(bitacora);
            }
            if (cliente.Provincia != null)
            {
                if (cliente.Provincia.Id != model.Provincia)
                {
                    bitacora = new ClienteBitacora();
                    bitacora.CreateDate = DateTime.Now;
                    bitacora.CreatedBy = usuario;
                    bitacora.Campo = "Provincia";
                    Region provinciaActual = RegionesService.Get(model.Provincia.Value);
                    bitacora.valorAnterior = cliente.Provincia != null ? cliente.Provincia.Nombre.ToString() : String.Empty;
                    bitacora.valorNuevo = provinciaActual != null ? provinciaActual.ToString() : String.Empty;
                    bitacora.Cliente = cliente;
                    bitacoraService.Create(bitacora);
                }
            }
            if (cliente.Canton != null)
            {
                if (cliente.Canton.Id != model.Canton)
                {
                    bitacora = new ClienteBitacora();
                    bitacora.CreateDate = DateTime.Now;
                    bitacora.CreatedBy = usuario;
                    bitacora.Campo = "Canton";
                    Region cantonActual = RegionesService.Get(model.Canton.Value);
                    bitacora.valorAnterior = cliente.Canton != null ? cliente.Canton.Nombre.ToString() : String.Empty;
                    bitacora.valorNuevo = cantonActual != null ? cantonActual.ToString() : String.Empty;
                    bitacora.Cliente = cliente;
                    bitacoraService.Create(bitacora);
                }
            }

            if (cliente.Distrito != null)
            {
                if (cliente.Distrito.Id != model.Distrito)
                {
                    bitacora = new ClienteBitacora();
                    bitacora.CreateDate = DateTime.Now;
                    bitacora.CreatedBy = usuario;
                    bitacora.Campo = "Distrito ";
                    Region distritoActual = RegionesService.Get(model.Distrito.Value);
                    bitacora.valorAnterior = cliente.Distrito != null ? cliente.Distrito.Nombre.ToString() : String.Empty;
                    bitacora.valorNuevo = distritoActual != null ? distritoActual.ToString() : String.Empty;
                    bitacora.Cliente = cliente;
                    bitacoraService.Create(bitacora);
                }
            }
        }

        /// <summary>
        /// Este método carga la información del cliente que se desea eliminar, esto según el id
        /// </summary>
        /// <returns>La vista Eliminar con el modelo del cliente a eliminar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CLIENTES)]
        public ActionResult Eliminar(long idCliente)
        {
            Cliente entity = ClienteService.Get(idCliente);
            ClienteModel cliente = clienteMapper.GetClienteModel(entity);

            cliente.GrupoNombre = entity.Grupo.Nombre;

            cliente.TipoNombre = entity.Tipo.Nombre;

            if (entity.Provincia != null)
                cliente.ProvinciaNombre = entity.Provincia.Nombre;

            if (entity.Canton != null)
                cliente.CantonNombre = entity.Canton.Nombre;

            if (entity.Distrito != null)
                cliente.DistritoNombre = entity.Distrito.Nombre;
            
            return View(cliente);
        }

        /// <summary>
        /// Este método actualiza el estado IsDeleted a true, con el fin de indicar que este cliente fue eliminado
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CLIENTES)]
        public ActionResult Eliminar(ClienteModel clienteModel)
        {
            Cliente cliente = ClienteService.Get(clienteModel.Id);
            cliente.IsDeleted = true;
            cliente.DateLastModified = DateTime.Now;
            cliente.Version++;
            cliente.DeletedBy = NombreUsuarioActual();
            cliente.DeleteDate = DateTime.Now;

            ClienteService.Update(cliente);
            //return Index();
            return RedirectToAction("Index", "Cliente");
        }

        /// <summary>
        /// Este método carga el modelo del cliente que se desea obtener el detalle, sus contactos y sus ubicaciones
        /// </summary>
        /// <returns>La vista de Detalles con el modelo de contenedor que se desea consultar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CLIENTES + "," + WPPConstants.ROLES_EDIT_CLIENTES + "," + WPPConstants.ROLES_CONS_CLIENTES)]
        public ActionResult Detalles(long idCliente)
        {
            Cliente entity = ClienteService.Get(idCliente);
            ClienteModel cliente = clienteMapper.GetClienteModel(entity);

            cliente.GrupoNombre = entity.Grupo.Nombre;

            cliente.TipoNombre = entity.Tipo.Nombre;

            if (entity.Provincia != null)
                cliente.ProvinciaNombre = entity.Provincia.Nombre;

            if (entity.Canton != null)
                cliente.CantonNombre = entity.Canton.Nombre;

            if (entity.Distrito != null)
                cliente.DistritoNombre = entity.Distrito.Nombre;

            cliente.Contactos = entity.Contactos;

           // ViewBag.ListaContratos = ContratoService.GetAll(criteria);//ListAll().Where(s => s.Cliente.Id == idCliente);

            return View("Detalles", cliente);
        }

        /// <summary>
        /// Este método lista y filtra el listado de registros de clientes mostrados en el index
        /// </summary>
        /// <returns>Vista Index con el modelo IPageList(Cliente)</returns>
        public ActionResult Buscar(string sortOrder, string currentFilterNombre, string currentFilterNombreCorto, string currentFilterGrupo, string currentFilterTipo, string currentFilterNumero,
                    string searchStringNombre, string searchStringNombreCorto, string searchStringGrupo, string searchStringTipo, string searchStringNumero, string searchStringFilas, int? page)
        {
            ViewBag.CurrentSort = sortOrder;

            if (sortOrder == "NombreAsc" || sortOrder == null)
                ViewBag.NombreSortParam = "NombreDesc";
            else
                ViewBag.NombreSortParam = "NombreAsc";

            if (sortOrder == "NombreCortoAsc" || sortOrder == null)
                ViewBag.NombreCortoSortParam = "NombreCortoDesc";
            else
                ViewBag.NombreCortoSortParam = "NombreCortoAsc";

            if (sortOrder == "GrupoAsc" || sortOrder == null)
                ViewBag.GrupoSortParam = "GrupoDesc";
            else
                ViewBag.GrupoSortParam = "GrupoAsc";

            if (sortOrder == "TipoAsc" || sortOrder == null)
                ViewBag.TipoSortParam = "TipoDesc";
            else
                ViewBag.TipoSortParam = "TipoAsc";

            if (sortOrder == "NumeroAsc" || sortOrder == null)
                ViewBag.NumeroSortParam = "NumeroDesc";
            else
                ViewBag.NumeroSortParam = "NumeroAsc";
            
            if (searchStringNombre != null || searchStringNombreCorto != null || searchStringGrupo != null || searchStringTipo != null || searchStringNumero != null)
                page = 1;
            else
            {
                searchStringNombre = currentFilterNombre;
                searchStringNombreCorto = currentFilterNombreCorto;
                searchStringGrupo = currentFilterGrupo;
                searchStringTipo = currentFilterTipo;
                searchStringNumero = currentFilterNumero;
            }

            ViewBag.CurrentNombre = searchStringNombre;
            ViewBag.CurrentNombreCorto = searchStringNombreCorto;
            ViewBag.CurrentGrupo = searchStringGrupo;
            ViewBag.CurrentTipo = searchStringTipo;
            ViewBag.CurrentNumero = searchStringNumero;

            var filas = searchStringFilas == null ? 10 : Convert.ToInt32(searchStringFilas);
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var paginaCompania = ClienteService.PagingSearch(searchStringNombre, searchStringNombreCorto, searchStringGrupo, searchStringTipo, searchStringNumero, pageNumber, filas, sortOrder);

            return View("Index", paginaCompania);
        }

        #endregion

        #region CONTACTO CLIENTE

        /// <summary>
        /// Este método se encarga de retornar la  vista que permite crear contectos del cliente 
        /// </summary>
        /// <returns>La vista CrearContacto</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CLIENTES + "," + WPPConstants.ROLES_EDIT_CLIENTES)]
        public ActionResult CrearContacto(long IdCliente)
        {
            Cliente cliente = ClienteService.Get(IdCliente);
            ContactoModel model = new ContactoModel();
            model.ParentId = cliente.Id;
            ViewBag.NombreCliente = cliente.Nombre;

            return View("CrearContacto", model);            
        }

        /// <summary>
        /// Este método guarda la información del modelo ingresada en el formulario de Crear
        /// </summary>
        /// <returns>La vista Detalles con el id del cliente al que pertenece el contacto</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO)]
        public ActionResult CrearContacto(ContactoModel contactoModel)
        {
            Cliente cliente = ClienteService.Get(contactoModel.ParentId);

            if (ModelState.IsValid)
            {                
                ContactoCliente contacto = new ContactoCliente();
                contactoMapper.GetContactoCliente(contactoModel, contacto);

                contacto.Cliente = cliente;
                cliente.Contactos.Add(contacto);
                contacto.DateLastModified = DateTime.Now;
                contacto.CreatedBy = NombreUsuarioActual();
                contacto.CreateDate = DateTime.Now;

                ContactoService.Create(contacto);

                return RedirectToAction("Detalles", new { idCliente = cliente.Id });
                //return Detalles(cliente.Id);
            }
            else
            {
                return View("CrearContacto", contactoModel);
            }
        }

        /// <summary>
        /// Este método carga la información del contacto que se desea editar, por medio de su id
        /// </summary>
        /// <returns>La vista Editar con el modelo del contacto a editar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CLIENTES + "," + WPPConstants.ROLES_EDIT_CLIENTES )]
        public ActionResult EditarContacto(long idContacto)
        {
            //ContactoModel contactoModel = new ContactoModel();
            ContactoCliente contacto = ContactoService.Get(idContacto);

            ContactoModel contactoModel = contactoMapper.GetContratoClienteModel(contacto);
            ViewBag.Cliente = contacto.Nombre;
            ViewBag.NombreCliente = contacto.Cliente.Nombre;

            return View("EditarContacto", contactoModel);               
        }

        /// <summary>
        /// Este método actualiza la información del contacto seleccionado, según lo datos del modelo
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO)]
        public ActionResult EditarContacto(ContactoModel contactoModel)
        {

            ContactoCliente contacto = ContactoService.Get(contactoModel.Id);

            if (ModelState.IsValid)
            {
                contactoMapper.GetContactoCliente(contactoModel, contacto);
                contacto.DateLastModified = DateTime.Now;
                contacto.ModifiedBy = NombreUsuarioActual();

                ContactoService.Update(contacto);

                return RedirectToAction("Detalles", new { idCliente = contacto.Cliente.Id });
               // return Detalles(contacto.Cliente.Id);
            }           

            return View("EditarContacto", contactoModel);
        }

        /// <summary>
        /// Este método carga la información del contacto que se desea eliminar, esto según el id
        /// </summary>
        /// <returns>La vista Eliminar con el modelo del contacto a eliminar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CLIENTES )]
        public ActionResult EliminarContacto(long idContacto)
        {
            //ContactoModel contactoModel = new ContactoModel();
            ContactoCliente contacto = ContactoService.Get(idContacto);

            ContactoModel contactoModel = contactoMapper.GetContratoClienteModel(contacto);
            ViewBag.Cliente = contacto.Nombre;
            ViewBag.NombreCliente = contacto.Cliente.Nombre;

            return View("EliminarContacto", contactoModel);
        }

        /// <summary>
        /// Este método actualiza el estado IsDeleted a true, con el fin de indicar que este contacto fue eliminado
        /// </summary>
        /// <returns>La vista Detalles con el id del cliente al que pertenece este contacto</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CLIENTES )]
        public ActionResult EliminarContacto(ContactoModel contactoModel)
        {
            ContactoCliente contacto = ContactoService.Get(contactoModel.Id);
            ViewBag.NombreCliente = contacto.Cliente.Nombre;
            contacto.IsDeleted = true;
            contacto.DeleteDate = DateTime.Now;
            contacto.DeletedBy = NombreUsuarioActual();
            ContactoService.Update(contacto);

            return RedirectToAction("Detalles", new { idCliente = contacto.Cliente.Id });
        }

        #endregion

        #region UBICACION CLIENTE

        /// <summary>
        /// Este método se encarga de retornar la  vista que permite crear una ubicación para este cliente 
        /// </summary>
        /// <returns>La vista CrearUbicacion</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CLIENTES + "," + WPPConstants.ROLES_EDIT_CLIENTES)]
        public ActionResult CrearUbicacion(long IdCliente)
        {
            Cliente cliente = ClienteService.Get(IdCliente);
            UbicacionClienteModel model = new UbicacionClienteModel();
            ViewBag.ListaEstado = CatalogoService.GetByType("EstadoUbicacion");
            model.Cliente = cliente.Id;
            ViewBag.NombreCliente = cliente.Nombre;
            return View("CrearUbicacion", model);
        }

        /// <summary>
        /// Este método guarda la información del modelo ingresada en el formulario de CrearUbicacion
        /// </summary>
        /// <returns>La vista Detalles con el id del cliente al que pertenece esta ubicación</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CLIENTES + "," + WPPConstants.ROLES_EDIT_CLIENTES)]
        public ActionResult CrearUbicacion(UbicacionClienteModel ubicacionModel)
        {
            Cliente cliente = ClienteService.Get(ubicacionModel.Cliente);

            if (ModelState.IsValid)
            {
                UbicacionCliente ubicacion = new UbicacionCliente();
                ubicacion = ubicacionMapper.GetUbicacionCliente(ubicacionModel, ubicacion);

                ubicacion.Cliente = cliente;
                cliente.Ubicaciones.Add(ubicacion);

                ubicacion.DateLastModified = DateTime.Now;
                ubicacion.CreatedBy = NombreUsuarioActual();
                ubicacion.CreateDate = DateTime.Now;

                if (ubicacionModel.Estado > 0)
                    ubicacion.Estado = CatalogoService.Get(ubicacionModel.Estado);

                if (ubicacionModel.Cliente > 0)
                    ubicacion.Cliente = ClienteService.Get(ubicacionModel.Cliente);

                UbicacionService.Create(ubicacion);

                return RedirectToAction("Detalles", new { idCliente = ubicacion.Cliente.Id });
                //return Detalles(cliente.Id);
            }
            else
            {
                return View("CrearUbicacion", ubicacionModel);
            }
        }

        /// <summary>
        /// Este método carga la información del contacto que se desea editar, por medio de su id
        /// </summary>
        /// <returns>La vista EditarUbicacion con el modelo de la ubicación a editar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CLIENTES + "," + WPPConstants.ROLES_EDIT_CLIENTES)]
        public ActionResult EditarUbicacion(long idUbicacion)
        {
            UbicacionCliente ubicacion = UbicacionService.Get(idUbicacion);

            UbicacionClienteModel ubicacionModel = ubicacionMapper.GetContratoClienteModel(ubicacion);
            ViewBag.ListaEstado = CatalogoService.GetByType("EstadoUbicacion");
            ViewBag.NombreCliente = ubicacion.Cliente.Nombre;

            return View("EditarUbicacion", ubicacionModel);
        }
        
        /// <summary>
        /// Este método actualiza la información de la ubicación seleccionada, según lo datos del modelo
        /// </summary>
        /// <returns>La vista Detalles con el id del cliente de esta ubicación</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CLIENTES + "," + WPPConstants.ROLES_EDIT_CLIENTES)]
        public ActionResult EditarUbicacion(UbicacionClienteModel ubicacionModel)
        {
            UbicacionCliente ubicacion = UbicacionService.Get(ubicacionModel.Id);

            if (ModelState.IsValid)
            {
                ubicacionMapper.GetUbicacionCliente(ubicacionModel, ubicacion);

                ubicacion.DateLastModified = DateTime.Now;
                ubicacion.ModifiedBy = NombreUsuarioActual();
                ubicacion.Version++;

                if (ubicacionModel.Estado > 0)
                    ubicacion.Estado = CatalogoService.Get(ubicacionModel.Estado);

                if (ubicacionModel.Cliente > 0)
                    ubicacion.Cliente = ClienteService.Get(ubicacionModel.Cliente);

                UbicacionService.Update(ubicacion);

                //return Detalles(ubicacion.Cliente.Id);
                return RedirectToAction("Detalles", new { idCliente = ubicacion.Cliente.Id });
            }

            return View("EditarUbicacion", ubicacionModel);
        }

        /// <summary>
        /// Este método carga la información de la ubicación que se desea eliminar, esto según el id
        /// </summary>
        /// <returns>La vista EliminarUbicacion con el modelo de la ubicación a eliminar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CLIENTES)]
        public ActionResult EliminarUbicacion(long idUbicacion)
        {
            UbicacionCliente ubicacion = UbicacionService.Get(idUbicacion);

            UbicacionClienteModel ubicacionModel = ubicacionMapper.GetContratoClienteModel(ubicacion);
            ViewBag.NombreCliente = ubicacion.Cliente.Nombre;
            return View("EliminarUbicacion", ubicacionModel);
        }

        /// <summary>
        /// Este método actualiza el estado IsDeleted a true, con el fin de indicar que este contenedor fue eliminado
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CLIENTES)]
        public ActionResult EliminarUbicacion(UbicacionClienteModel ubicacionModel)
        {
            UbicacionCliente ubicacion = UbicacionService.Get(ubicacionModel.Id);

            ubicacion.IsDeleted = true;
            ubicacion.DeleteDate = DateTime.Now;
            ubicacion.DeletedBy = NombreUsuarioActual();
            UbicacionService.Update(ubicacion);

            //return Detalles(ubicacion.Cliente.Id);
            return RedirectToAction("Detalles", new { idCliente = ubicacion.Cliente.Id });
        }

        #endregion
    }
}