using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WPP.Entities.Objects.ModuloBascula;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Helpers;
using WPP.Model.ModuloBascula;
using WPP.Security;
using WPP.Service.ModuloBascula;
using WPP.Service.ModuloContratos;

namespace WPP.Controllers
{
    public class ConsecutivoBasculaController : BaseController
    {
       IConsecutivoBoletaService consecutivoService;
       ICompaniaService companiaService;
       public ConsecutivoBasculaController(IConsecutivoBoletaService service, ICompaniaService compania)
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


       [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CONSECUTIVO_BOLETAS)]
       public ActionResult Index()
        {
            Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Compania", compania);

            ConsecutivoBoleta consecutivo = consecutivoService.Get(criteria);
            ConsecutivoModel model = new ConsecutivoModel();
            model.Consecutivo = consecutivo.Secuencia;

            return View(model);
        }

        /// <summary>
        /// Este método actualiza el consecutivo de boleta con la información del modelo ConsecutivoModel ingresada 
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CONSECUTIVO_BOLETAS)]
        public ActionResult GuardarConsecutivo(ConsecutivoModel model)
        {
            if (ModelState.IsValid)
            {
                Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Compania", compania);

                ConsecutivoBoleta consecutivo = consecutivoService.Get(criteria);
                consecutivo.Secuencia = model.Consecutivo;

                consecutivoService.Update(consecutivo);                

            }
            return RedirectToAction("Index", "ConsecutivoBascula"); 
            //View("Index");
        }

    }
}
