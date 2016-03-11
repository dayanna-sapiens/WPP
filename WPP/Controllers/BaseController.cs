using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WPP.Entities.Objects.Generales;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Helpers;
using WPP.Service.BaseServiceClasses;
using WPP.Service.Generales;
using WPP.Service.ModuloContratos;

namespace WPP.Controllers
{
    public class BaseController : Controller
    {
        [Inject]
        public IUnitOfWork UnitOfWork { get; set; }
        public readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [Inject]
        public IUsuarioService UsuarioService { get; set; }
        [Inject]
        public ICatalogoService CatalogoService { get; set; }
        [Inject]
        public IRegionService RegionesService { get; set; }
         [Inject]
        public ICompaniaService CompaniaService { get; set; }
         [Inject]
         public ITipoCambioService TipoCambioService { get; set; }
         public string Mensaje = "";
       
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.IsChildAction)
                UnitOfWork.BeginTransaction();
        }


        public ActionResult AccessDenied()
        {
            return View("AccessDenied");
        }


        public Usuario ObtenerUsuarioActual()
        {
            string nombreUsuario = User.Identity.Name;
            IDictionary<string, object> crit = new Dictionary<string, object>();
            crit.Add("Email", nombreUsuario);
            Usuario usuario = UsuarioService.Get(crit);
            return usuario;

        }

        public String NombreUsuarioActual()
        {
            return User.Identity.Name;
        }

        public Compania CompaniaActual()
        {
            IDictionary<String, object> crit = new Dictionary<String, object>();

            if (Session["Compania"] != null)
            {
                long idCompania = long.Parse(Session["Compania"].ToString());
                return CompaniaService.Get(idCompania);
            }
            else
            {
                Compania compania = ObtenerUsuarioActual().Companias.FirstOrDefault();
                Session["Compania"] = compania.Id;
                return compania;
            }
        }

        public bool ActualizarTipoCambio()
        {
            try
            {
                string fecha = (DateTime.Now).ToString("dd/MM/yyyy");
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Fecha", fecha);
                criteria.Add("Tipo", "Compra");
                TipoCambioService = DependencyResolver.Current.GetService<ITipoCambioService>();
                TipoCambio tipoCambio = TipoCambioService.Get(criteria);

                if (tipoCambio == null)
                {
                    double compra = WPPTipoCambio.TipoCambioWebService(fecha, fecha, "Compra");
                    double venta = WPPTipoCambio.TipoCambioWebService(fecha, fecha, "Venta");

                    // Se registra el tipo de cambio (Compra) del día de hoy
                    TipoCambio tipoCambioCompra = new TipoCambio();
                    tipoCambioCompra.Fecha = fecha;
                    tipoCambioCompra.Valor = compra;
                    tipoCambioCompra.Tipo = "Compra";
                    TipoCambioService.Create(tipoCambioCompra);

                    // Se registra el tipo de cambio (Venta) del día de hoy
                    TipoCambio tipoCambioVenta = new TipoCambio();
                    tipoCambioVenta.Fecha = fecha;
                    tipoCambioVenta.Valor = venta;
                    tipoCambioVenta.Tipo = "Venta";
                    TipoCambioService.Create(tipoCambioVenta);
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return false;
            }
        }

        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            try
            {
                if (!filterContext.IsChildAction)
                    UnitOfWork.Commit();
            }
            catch (Exception)
            {
                
                throw;
            }
          
        }



    }
}
