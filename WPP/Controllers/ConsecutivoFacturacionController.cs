using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Entities.Objects.ModuloFacturacion;
using WPP.Helpers;
using WPP.Model.ModuloFacturacion;
using WPP.Security;
using WPP.Service.ModuloContratos;
using WPP.Service.ModuloFacturacion;

namespace WPP.Controllers
{
    public class ConsecutivoFacturacionController : BaseController
    {
       IConsecutivoFacturacionService consecutivoService;
       ICompaniaService companiaService;
       public ConsecutivoFacturacionController(IConsecutivoFacturacionService service, ICompaniaService compania)
        {
            try
            {
                this.consecutivoService = service;
                this.companiaService = compania;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }


       [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CONSECUTIVO_FACTURACION)]
       public ActionResult Index()
        {
            Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Compania", compania);
            ConsecutivoFacturacion consecutivo = consecutivoService.Get(criteria);
            ConsecutivoFacturacionModel model = new ConsecutivoFacturacionModel();
            model.Secuencia = consecutivo.Secuencia;

            return View(model);
        }

        /// <summary>
        /// Este método actualiza el consecutivo de boleta con la información del modelo ConsecutivoModel ingresada 
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CONSECUTIVO_FACTURACION)]
        public ActionResult GuardarConsecutivo(ConsecutivoFacturacionModel model)
        {
            if (ModelState.IsValid)
            {
                Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Compania", compania);

                ConsecutivoFacturacion consecutivo = consecutivoService.Get(criteria);
                consecutivo.Secuencia = model.Secuencia;
                consecutivoService.Update(consecutivo);                

            }
            return RedirectToAction("Index", "ConsecutivoFacturacion"); 
            //return View("Index");
        }


    }
}
