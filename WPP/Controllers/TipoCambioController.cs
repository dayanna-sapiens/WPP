using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WPP.Entities.Objects.Generales;
using WPP.Helpers;
using WPP.Model.General;
using WPP.Security;
using WPP.Service.Generales;

namespace WPP.Controllers
{
    public class TipoCambioController : BaseController
    {
        ITipoCambioService tipoService;
        public TipoCambioController(ITipoCambioService service)
        {
            try
            {
                this.tipoService = service;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }


        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_TIPO_CAMBIO)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Este método guarda la información del modelo tipo de cambio que fue ingresada en el formulario
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_TIPO_CAMBIO)]
        public ActionResult GuardarTipoCambio(TipoCambioModel model)
        {
            if (ModelState.IsValid)
            {
                 string fecha = (DateTime.Now).ToString("dd/MM/yyyy");
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Fecha", fecha);
                criteria.Add("Tipo", "Compra");
                TipoCambio tipoCambio = tipoService.Get(criteria);

                if (tipoCambio == null)
                {
                    TipoCambio tipoVenta = new TipoCambio();
                    tipoVenta.Fecha = DateTime.Now.ToString("dd/MM/yyyy");
                    tipoVenta.Valor = model.Venta;
                    tipoVenta.Tipo = "Venta";
                    tipoVenta.CreateDate = DateTime.Now;
                    tipoVenta.CreatedBy = NombreUsuarioActual();

                    tipoService.Create(tipoVenta);

                    TipoCambio tipoCompra = new TipoCambio();
                    tipoCompra.Fecha = DateTime.Now.ToString("dd/MM/yyyy");
                    tipoCompra.Valor = model.Venta;
                    tipoCompra.CreateDate = DateTime.Now;
                    tipoCompra.Tipo = "Compra";
                    tipoCompra.CreatedBy = NombreUsuarioActual();

                    tipoService.Create(tipoCompra);
                }
                else
                {
                    ModelState.AddModelError("", "Ya fue registrado el tipo de cambio de hoy");                
                }

                //Se envia al cobro
                return RedirectToAction("Index", "TipoCambio"); 
            }
            else
            {
                ModelState.AddModelError("","Ya fue registrado el tipo de cambio de hoy");
                //return View("Index");
                return RedirectToAction("Index", "TipoCambio"); 
            }
        }
    }
}
