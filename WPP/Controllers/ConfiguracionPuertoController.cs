using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WPP.Entities.Objects.ModuloBascula;
using WPP.Helpers;
using WPP.Mapper.ModuloBascula;
using WPP.Model.General;
using WPP.Model.ModuloBascula;
using WPP.Security;
using WPP.Service.ModuloBascula;

namespace WPP.Controllers
{
    public class ConfiguracionPuertoController : BaseController
    {
        private IConfiguracionPuertoService configuracionService;
        private ConfiguracionPuertoMapper configuracionMapper;

        public ConfiguracionPuertoController(IConfiguracionPuertoService configuracion)
        {
            configuracionService = configuracion;
            configuracionMapper = new ConfiguracionPuertoMapper();

        }
        public ActionResult Configuracion()
        {
            string nombre = Environment.MachineName;
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("NombrePC", nombre);
            ConfiguracionPuertoModel model = configuracionMapper.GetModel(configuracionService.Get(criteria));
            if(model == null)
            {
                model = new ConfiguracionPuertoModel();
                model.NombrePC = nombre;
            }

            List<CatalogoModel> Lista = new List<CatalogoModel>();
            foreach (var item in System.IO.Ports.SerialPort.GetPortNames())
	        {
                CatalogoModel puerto = new CatalogoModel();
                puerto.Nombre = item;
                Lista.Add(puerto);
	        }

            ViewBag.Puertos = Lista;
            return View(model);
        }

        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_BOLETAS + "," + WPPConstants.ROLES_EDIT_BOLETAS)]
        public ActionResult GuardarConfiguracion(ConfiguracionPuertoModel model)
        {
            ConfiguracionPuerto puerto = new ConfiguracionPuerto();

            if (model.Id == 0)
            {
                puerto = configuracionMapper.GetEntity(model, puerto);
                puerto.CreateDate = DateTime.Now;
                puerto.CreatedBy = NombreUsuarioActual();
                puerto.Version = 1;
                puerto.IsDeleted = false;
                configuracionService.Create(puerto);
            }
            else
            {
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("NombrePC", model.NombrePC);
                puerto = configuracionService.Get(criteria);
                puerto.Puerto = model.Puerto;
                puerto.ModifiedBy = NombreUsuarioActual();
                puerto.DateLastModified = DateTime.Now;
                puerto.Version++;
                configuracionService.Update(puerto);
            }

            return RedirectToAction("Configuracion", "ConfiguracionPuerto");
        }

    }
}
