using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WPP.Security;
using WPP.Service.BaseServiceClasses;
using WPP.Service.ModuloContratos;
using WPP.Mapper;
using WPP.Model;
using WPP.App_Start;
using WPP.Entities.Objects.Generales;
using WPP.Helpers;

namespace WPP.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/
        private IUsuarioService usuarioService;
        

        //public HomeController(ICompaniaService service)
        //{
        //    try
        //    {
        //        this.companiaService = service;
        //        this.logger.Debug("prueba");
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.Message);
        //    }
        //}

        public HomeController(IUsuarioService service)
        {
            try
            {
                this.usuarioService = service;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

       [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROLES_ADMINISTRACION)]
        public ActionResult Index()
        {
           return View("Index");
        }

    }
}
