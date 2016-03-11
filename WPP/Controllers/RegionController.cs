using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WPP.Mapper.General;
using WPP.Service.Generales;

namespace WPP.Controllers
{
    public class RegionController : BaseController
    {

        private RegionMapper regionMapper;
        public IRegionService RegionesService { get; set; }


        public RegionController(IRegionService regionService)
        {
            this.RegionesService = regionService;
            regionMapper = new RegionMapper();
        }

        /// <summary>
        /// Este método carga la lista de distritos según el código de cantón
        /// </summary>
        /// <returns>json con la lista de regiones (distritos) encontrados</returns>
        [HttpPost]
        public JsonResult CargarDistritos(long idCanton)
        {
            try
            {
                return Json(regionMapper.GetListaRegionModel(RegionesService.GetAll(idCanton)), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(null);
            }
        }

        /// <summary>
        /// Este método carga la lista de cantones según el código de provincia
        /// </summary>
        /// <returns>json con la lista de regiones (cantones) encontrados</returns>
        [HttpPost]
        public JsonResult CargarCantones(long idProvincia)
        {
            try
            {
                return Json(regionMapper.GetListaRegionModel(RegionesService.GetAll(idProvincia)), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(null);
            }
        }

    }
}
