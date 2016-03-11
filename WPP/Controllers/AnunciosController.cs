using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WPP.Entities.Objects.Generales;
using WPP.Helpers;
using WPP.Mapper.General;
using WPP.Model.General;
using WPP.Security;
using WPP.Service.Generales;

namespace WPP.Controllers
{
    public class AnunciosController : BaseController
    {

        private IAnuncioService anuncioService;
        private AnuncioMapper anuncioMapper;

        public AnunciosController(IAnuncioService anuncio)
        {
            this.anuncioService = anuncio;
            anuncioMapper = new AnuncioMapper();
        }

        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADM_ANUNCIOS)]
        public ActionResult Index()
        {
            AnuncioModel model = anuncioMapper.GetAnuncioModel(anuncioService.ListAll().FirstOrDefault());
            return View(model);
        }

        [HttpPost]
        public JsonResult GuardarImagen(string id, string img1, string img2, string img3, string img4)
        {
            try
            {
                IList<String> lista = new List<String>();
                lista.Add(img1);
                lista.Add(img2);
                lista.Add(img3);
                lista.Add(img4);

                Anuncio anuncio = anuncioService.Get(Convert.ToInt64(id));

                int count = 1;
                foreach (var item in lista)
                {
                    if (item != String.Empty && item.Contains("http:") == false)
                    {
                        string base64 = item.Substring(item.IndexOf(',') + 1);
                        base64 = base64.Trim('\0');
                        byte[] data = Convert.FromBase64String(base64);

                        string path = Server.MapPath("~/Images/Anuncios/");

                        string nombre = "Img" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".png";
                        var file = Path.Combine(path, nombre);
                        System.IO.File.WriteAllBytes(file, data);

                        switch (count)
                        {
                            case 1:
                                anuncio.Imagen1 = nombre;
                                break;
                            case 2:
                                anuncio.Imagen2 = nombre;
                                break;
                            case 3:
                                anuncio.Imagen3 = nombre;
                                break;
                            case 4:
                                anuncio.Imagen4 = nombre;
                                break;
                        }
                    }
                    count++;

                }
                anuncio.Version++;
                anuncio.DateLastModified = DateTime.Now;
                anuncio.ModifiedBy = NombreUsuarioActual();
                anuncioService.Update(anuncio);
                return Json(true);
            }
            catch(Exception ex)
            {
                return Json(false);
            }
            
        }
    }
}
