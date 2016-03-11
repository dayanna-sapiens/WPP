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
    public class UsuarioController : BaseController
    {

        private IWPPMembershipProvider wppMemberShipProvider;
        private UsuarioMapper usuarioMapper;
        private CompaniaMapper companiaMapper;
        private AnuncioMapper anuncioMapper;
        private ICompaniaService companiaService;
        private IAnuncioService anuncioService;

        private IRecoveryPasswordService recoveryPasswordService;

        public UsuarioController(IWPPMembershipProvider WPPMemberProvider, ICompaniaService companiaService, IRecoveryPasswordService _recoveryPasswordService, IAnuncioService anuncio)
        {
            try
            {
                wppMemberShipProvider = WPPMemberProvider;
                usuarioMapper = new UsuarioMapper();
                companiaMapper = new CompaniaMapper();
                anuncioMapper = new AnuncioMapper();
                this.anuncioService = anuncio;
                this.companiaService = companiaService;
                this.recoveryPasswordService = _recoveryPasswordService;

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// Este método redirecciona a la vista que se ha indicado, 
        /// en caso contrario se envia a seleccionar la companoia con la que se desea trabajar
        /// </summary>
        /// <returns>Vista deseada</returns>
        private ActionResult RedirectURL(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Compania", "Usuario");
            }
        }


        #region LOGIN

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Este método valida si el usuario y contraseña ingresados pertenecen a un usuario del sistema, 
        /// en caso de ser permitido da el acceso de lo contrario solicita nuevamente los credenciales
        /// </summary>
        /// <returns>Pagina donde tenga permiso el usuario o al Login</returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel login, string returnUrl)
        {
            if (ModelState.IsValid && wppMemberShipProvider.ValidateUser(login.Email, login.Password))
            {
                Session["Usuario"] = login.Email;
                FormsAuthentication.SetAuthCookie(login.Email, true);
                IDictionary<string, object> criteriaUser = new Dictionary<string, object>();
                criteriaUser.Add("Email", login.Email);
                criteriaUser.Add("IsDeleted", false);
                Usuario usuario = UsuarioService.Get(criteriaUser);
                Session["Roles"] = usuario.Roles;

                return RedirectURL(returnUrl);
            }
            else
            {
                ModelState.AddModelError("", "El correo o contraseña es inválido.");
                return View(login);
            }
        }


        /// <summary>
        /// Este método redirecciona al Login y da por finalizada la sesion del usuario
        /// </summary>
        /// <returns>Vista deseada</returns>
        [Authorize]
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Login");
        }

        #endregion

        #region COMPANIA 
        /// <summary>
        /// Este método asigna la compañía seleccionada a la sesion actual, esto para identificar con cual compañía se va a trabajar
        /// </summary>
        /// <returns>Index</returns>
        [HttpPost]
        public ActionResult AsignarCompania(string button)
        {
            Session["Compania"] = button;
            return RedirectToAction("Home", "Usuario");
               //if (Session["Roles"].ToString().Contains("Super Usuario"))
               //{
               //    //return Index();
               //    return RedirectToAction("Home", "Usuario");
               //}
               //else
               //{
               //    return RedirectToAction("Home", "Usuario");
               //}
        }

        /// <summary>
        /// Este método lista todas las compañías a las cuales el usuario activo tiene acceso
        /// </summary>
        /// <returns>vista Compania</returns>
        [HttpGet]
        public ActionResult Compania()
        {
            //IDictionary<string, object> criteriaUser = new Dictionary<string, object>();
            //criteriaUser.Add("Email",  Session["Usuario"].ToString());
            Usuario usuario = ObtenerUsuarioActual();// UsuarioService.Get(criteriaUser);

            ViewBag.ListaCompanias = usuario.Companias;
            Session["Roles"] = usuario.Roles;
            return View();
        }

        #endregion


        [HttpGet]
        [AllowAnonymous]
        public ActionResult Home()
        {
            AnuncioModel model = anuncioMapper.GetAnuncioModel(anuncioService.ListAll().FirstOrDefault());
            return View(model);
        }

        #region RECUPERAR CONTRASENA
        /// <summary>
        /// Este método muestra la vista para solicitar el cambio de contraseña
        /// </summary>
        /// <returns>vista SolicitarContrasena</returns>
        [HttpGet]
        public ActionResult SolicitarContrasena()
        {
            return View();

        }

        /// <summary>
        /// Este método valida que el usuario ingresado que desea realizar el cambio de contraseña sea valido,
        /// en caso que este lo sea, inactiva al usuario hasta que realice el cambio de contraseña, 
        /// crea un token y envia un conrreo con el link donde puede cambiar la contraseña
        /// </summary>
        /// <returns>vista SolicitarContrasena</returns>
        public ActionResult SolicitarContrasena(SolicitarContrasenaModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Usuario nuevoUsuario = new Usuario();
                    IDictionary<string, object> criteriaUser = new Dictionary<string, object>();
                    criteriaUser.Add("Email", model.Email);
                    nuevoUsuario = UsuarioService.Get(criteriaUser);

                    if ( nuevoUsuario != null)
                    {
                        if (nuevoUsuario.Cedula == model.Cedula)
                        {
                            // Desactivar contraseña del usuario
                            nuevoUsuario.DateLastModified = DateTime.Now;
                            nuevoUsuario.ModifiedBy = nuevoUsuario.Email;
                            nuevoUsuario.PasswordActivo = false;
                            UsuarioService.Create(nuevoUsuario);

                            // crear Token del usuario
                            RecoveryPassword password = new RecoveryPassword();
                            password.Usuario = nuevoUsuario;
                            Guid token = Guid.NewGuid();
                            password.Token = token.ToString();
                            password.CreateDate = DateTime.Now;
                            password.CreatedBy = model.Email;
                            password.Version = 1;

                            recoveryPasswordService.Create(password);

                            // Enviar Email
                            String rootPath = Request.Url.AbsoluteUri.Substring(0, (Request.Url.AbsoluteUri.Length) - 19) + "RecuperarContrasenia?token="+password.Token;
                            
                            string mensaje = String.Format("<h1>Solicitud de cambio de contraseña</h1><p>Hola {0} " +
                                    "hemos recibido una solicitud para cambiar su contraseña del sistema WPP, " +
                                    "para realizar el cambio de contraseña por favor diríjase al siguiente <a href='" + rootPath + "'>link</a>. </br>" +
                                    "Cualquier consulta por favor comuníquese con el administrador del sistema. Muchas Gracias.</p>",
                                    model.Email);

                            //MailHelper.sendMail("dayanna.barboza@sapiensdev.com", mensaje, "Cambio de Contraseña WPP");
                            MailHelper.sendMail(model.Email, mensaje, "Cambio de Contraseña WPP");
                            
                            return View("Login");
                        }
                        else
                        {
                            ModelState.AddModelError("Cedula", "Cédula no válida");
                            return View();
                        }                       
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "Email no válido");
                        return View();
                    }
                }
                else
                {
                    ViewBag.Roles = WPPConstants.ListaRoles;
                    ViewBag.ListaCompanias = companiaMapper.GetListaCompaniaModel(companiaService.ListAll());
                    return View();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return View();
            }

        }


        [HttpGet]
        public ActionResult RecuperarContrasenia(String token)
        {
            RecoveryPassword recovery = recoveryPasswordService.Get(token);

            UsuarioModel model = usuarioMapper.GetUsuarioModel(recovery.Usuario);

            return View(model);
        }


        [HttpPost]
        public ActionResult RecuperarContrasenia(UsuarioModel model)
        {

            Usuario usuario = UsuarioService.Get(model.Id);

            usuario.Password = WPPHelper.ComputeHash(model.Password, null);
            usuario.PasswordActivo = true;

            UsuarioService.Update(usuario);

            return RedirectToAction("Login", "Usuario");
                        //return Index();
        }

        #endregion

        #region MANTENIMIENTO DE USUARIOS


        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_USUARIOS + "," + WPPConstants.ROLES_EDIT_USUARIOS + "," + WPPConstants.ROLES_CONS_USUARIOS)]
        public ActionResult Index()
        {
            return Buscar(null, null, null, null, null, null, null, null, null, null, null, null, null);
        }

        /// <summary>
        /// Este método se encarga de retornar la  vista que permite crear usuarios 
        /// </summary>
        /// <returns>La vista Crear</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_USUARIOS + "," + WPPConstants.ROLES_EDIT_USUARIOS )]
        public ActionResult Crear()
        {
            ViewBag.Roles = WPPConstants.ListaRoles;
            ViewBag.ListaCompanias = companiaMapper.GetListaCompaniaModel(companiaService.ListAll());
            return View();            
        }


        /// <summary>
        /// Este método guarda la información del modelo ingresada en el formulario de Crear
        /// </summary>
        /// <returns>La vista Index</returns>
  
       [HttpPost, ValidateInput(false)]
       [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_USUARIOS + "," + WPPConstants.ROLES_EDIT_USUARIOS)]
        public ActionResult Crear(UsuarioModel usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IDictionary<string, object> criteriaUser = new Dictionary<string, object>();
                    criteriaUser.Add("Email", usuario.Email);

                    if (UsuarioService.Get(criteriaUser) == null)
                    {
                        Usuario nuevoUsuario = new Usuario();
                        nuevoUsuario = usuarioMapper.GetUsuario(usuario, nuevoUsuario);
                        nuevoUsuario.Version = 1;
                        nuevoUsuario.CreateDate = DateTime.Now;
                        nuevoUsuario.DateLastModified = DateTime.Now;
                        nuevoUsuario.Password = WPPHelper.ComputeHash(nuevoUsuario.Password, null);
                        nuevoUsuario.CreatedBy = NombreUsuarioActual();
                        nuevoUsuario.ModifiedBy = NombreUsuarioActual();
                        nuevoUsuario.PasswordActivo = true;

                        var listaCompanias = usuario.Companias.Split(',');
                        nuevoUsuario.Companias = new List<Compania>();
                        foreach (var item in listaCompanias)
                        {
                            Compania compania = companiaService.Get(Convert.ToInt64(item));
                            nuevoUsuario.Companias.Add(compania);
                        }

                        UsuarioService.Create(nuevoUsuario);

                        ViewBag.Mensaje = "Se ha creado el usuario";

                        return RedirectToAction("Index", "Usuario");
                        //return Index();
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "Email no válido, ya existe un usuario registrado con este email");
                        ViewBag.ListaCompanias = companiaMapper.GetListaCompaniaModel(companiaService.ListAll());
                        ViewBag.Roles = WPPConstants.ListaRoles;
                        return View(usuario);
                    }
                }
                else
                {
                    ViewBag.Roles = WPPConstants.ListaRoles;
                    ViewBag.ListaCompanias = companiaMapper.GetListaCompaniaModel(companiaService.ListAll());
                    return View();
                }
           }
           catch(Exception ex)
           {
               logger.Error(ex.Message);
               ViewBag.Roles = WPPConstants.ListaRoles;
               ViewBag.ListaCompanias = companiaMapper.GetListaCompaniaModel(companiaService.ListAll());
               return View();
           }
        }

       /// <summary>
       /// Este método carga la información del usuario que se desea editar, por medio de su id
       /// </summary>
       /// <returns>La vista Editar con el modelo del usuario a editar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_USUARIOS + "," + WPPConstants.ROLES_EDIT_USUARIOS)]
        public ActionResult Editar(long idUsuario)
        {            
            UsuarioMapper mapper = new UsuarioMapper();
            Usuario usuario = UsuarioService.Get(idUsuario);
            UsuarioModel usuarioModel = mapper.GetUsuarioModel(usuario);
            String testing = DateTime.Now.ToShortDateString();
            testing += "";
            ViewBag.Roles = WPPConstants.ListaRoles;
            ViewBag.ListaCompanias = companiaMapper.GetListaCompaniaModel(companiaService.ListAll());

            usuarioModel.Companias = String.Empty;
            foreach (var item in usuario.Companias)
            {
                usuarioModel.Companias += (usuarioModel.Companias == String.Empty ? item.Id.ToString() : ("," + item.Id));
            }

            return View(usuarioModel);
        }

        /// <summary>
        /// Este método actualiza la información del usuario seleccionado, según lo datos del modelo
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost, ValidateInput(false)]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_USUARIOS + "," + WPPConstants.ROLES_EDIT_USUARIOS)]
        public ActionResult Editar(UsuarioModel usuarioModel)
        {
            if (ModelState.IsValid)
            {
                IDictionary<string, object> criteriaUser = new Dictionary<string, object>();
                criteriaUser.Add("Email", usuarioModel.Email);
                Usuario usuarioValidacion = UsuarioService.Get(criteriaUser);
                if (usuarioValidacion != null)
                {
                    if (usuarioValidacion.Id == usuarioModel.Id)
                    {
                        Usuario usuario = UsuarioService.Get(usuarioModel.Id);
                        usuario = usuarioMapper.GetUsuario(usuarioModel, usuario);
                        usuario.DateLastModified = DateTime.Now;
                        usuario.Password = WPPHelper.ComputeHash(usuarioModel.Password, null);
                        usuario.ModifiedBy = NombreUsuarioActual();
                        usuario.Version++;
                        usuario.PasswordActivo = true;

                        var listaCompanias = usuarioModel.Companias.Split(',');
                        usuario.Companias = new List<Compania>();
                        foreach (var item in listaCompanias)
                        {
                            Compania compania = companiaService.Get(Convert.ToInt64(item));
                            usuario.Companias.Add(compania);
                        }

                        UsuarioService.Update(usuario);

                        ViewBag.Roles = WPPConstants.ListaRoles;
                        ViewBag.ListaCompanias = companiaMapper.GetListaCompaniaModel(companiaService.ListAll());
                        return RedirectToAction("Index", "Usuario");
                        //return Index();
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "Email no válido, ya existe un usuario registrado con este email");
                        ViewBag.Roles = WPPConstants.ListaRoles;
                        ViewBag.ListaCompanias = companiaMapper.GetListaCompaniaModel(companiaService.ListAll());
                        return View(usuarioModel);
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "Email no válido, ya existe un usuario registrado con este email");
                    ViewBag.Roles = WPPConstants.ListaRoles;
                    ViewBag.ListaCompanias = companiaMapper.GetListaCompaniaModel(companiaService.ListAll());
                    return View(usuarioModel);
                }
            }
            else
            {
                ViewBag.Roles = WPPConstants.ListaRoles;
                ViewBag.ListaCompanias = companiaMapper.GetListaCompaniaModel(companiaService.ListAll());
                return View(usuarioModel);
            }           
        }

        /// <summary>
        /// Este método carga la información del usuario que se desea eliminar, esto según el id
        /// </summary>
        /// <returns>La vista Eliminar con el modelo del a eliminar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_USUARIOS )]
        public ActionResult Eliminar(long idUsuario)
        {
            Usuario usuario = UsuarioService.Get(idUsuario);
            UsuarioModel usuarioModel = usuarioMapper.GetUsuarioModel(usuario);
            ViewBag.ListaCompanias = usuario.Companias;
            return View(usuarioModel);
        }

        /// <summary>
        /// Este método carga el modelo del usuario que se desea obtener el detalle
        /// </summary>
        /// <returns>La vista de Visualizar con el modelo de usuario que se desea consultar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_USUARIOS + "," + WPPConstants.ROLES_EDIT_USUARIOS + "," + WPPConstants.ROLES_CONS_USUARIOS)]
        public ActionResult Visualizar(long idUsuario)
        {
            Usuario usuario = UsuarioService.Get(idUsuario);
            UsuarioModel usuarioModel = usuarioMapper.GetUsuarioModel(usuario);
            ViewBag.ListaCompanias = usuario.Companias;
            return View(usuarioModel);
        }

        /// <summary>
        /// Este método actualiza el estado IsDeleted a true, con el fin de indicar que este usuario fue eliminado
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_USUARIOS)]
        public ActionResult Eliminar(UsuarioModel usuarioModel)
        {
            Usuario usuario = UsuarioService.Get(usuarioModel.Id);
            usuario.IsDeleted = true;
            usuario.DateLastModified = DateTime.Now;
            usuario.Version++;
            usuario.DeletedBy = NombreUsuarioActual();
            usuario.DeleteDate = DateTime.Now;

            UsuarioService.Update(usuario);

            ViewBag.Roles = WPPConstants.ListaRoles;
            ViewBag.ListaCompanias = usuario.Companias;
            return RedirectToAction("Index", "Usuario");
            //return Index();
        }

        /// <summary>
        /// Este método lista y filtra el listado de registros de usuarios mostrados en el index
        /// </summary>
        /// <returns>Vista Index con el modelo IPageList(Usuario)</returns>
        public ActionResult Buscar(string sortOrder, string currentFilterNombre, string currentFilterEmail, string currentFilterApellido1, string currentFilterApellido2, string currentFilterCedula,
                    string searchStringNombre, string searchStringEmail, string searchStringApellido1, string searchStringApellido2, string searchStringCedula, string searchStringFilas, int? page)
        {
            ViewBag.CurrentSort = sortOrder;

            if(sortOrder == "NombreAsc")
                ViewBag.NameSortParam = "NombreDesc";
            else
                ViewBag.NameSortParam = "NombreAsc";

            if (sortOrder == "EmailAsc")
                ViewBag.EmailSortParam = "EmailDesc";
            else
                ViewBag.EmailSortParam = "EmailAsc";

            if (sortOrder == "Apellido1Asc")
                ViewBag.Apellido1SortParam = "Apellido1Desc";
            else
                ViewBag.Apellido1SortParam = "Apellido1Asc";

            if (sortOrder == "Apellido2Asc")
                ViewBag.Apellido2SortParam = "Apellido2Desc";
            else
                ViewBag.Apellido2SortParam = "Apellido2Asc";

            if (sortOrder == "CedulaAsc")
                ViewBag.CedulaSortParam = "CedulaDesc";
            else
                ViewBag.CedulaSortParam = "CedulaAsc";


            if (searchStringNombre != null || searchStringEmail != null || searchStringApellido1 != null || searchStringApellido2 != null || searchStringCedula != null)
                page = 1;
            else
            {
                searchStringNombre = currentFilterNombre;
                searchStringEmail = currentFilterEmail;
                searchStringApellido1 = currentFilterApellido1;
                searchStringApellido2 = currentFilterApellido2;
                searchStringCedula = currentFilterCedula;
            }

            ViewBag.CurrentNombre = searchStringNombre;
            ViewBag.CurrentEmail = searchStringEmail;
            ViewBag.CurrentApellido1 = searchStringApellido1;
            ViewBag.CurrentApellido2 = searchStringApellido2;
            ViewBag.CurrentCedula = searchStringCedula;

            var filas = searchStringFilas == null ? 10 : Convert.ToInt32(searchStringFilas);
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var paginaUsuario = UsuarioService.PagingSearch(searchStringEmail, searchStringNombre, searchStringApellido1, searchStringApellido2, searchStringCedula, pageNumber, filas, sortOrder);

            return View("Index", paginaUsuario);
        }

        #endregion
    }
}
