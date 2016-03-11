using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WPP.Entities.Objects.Generales;
using WPP.Helpers;
using WPP.Mapper.ModuloContratos;
using WPP.Model.ModuloContratos;
using WPP.Security;
using WPP.Service.ModuloContratos;

namespace WPP.Controllers
{
    public class CategoriaProductoController : BaseController
    {

        private CategoriaProductoMapper categoriaMapper;
        private ICategoriaProductoService categoriaService;

        public CategoriaProductoController(ICategoriaProductoService categoria)
        {
            try
            {
                this.categoriaService = categoria;
                categoriaMapper = new CategoriaProductoMapper();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_CONS_CATEGORIA_PRODUCTOS + "," + WPPConstants.ROLES_ADMIN_CATEGORIA_PRODUCTOS + "," + WPPConstants.ROLES_EDIT_CATEGORIA_PRODUCTOS )]
        public ActionResult Index()
        {
            return Buscar(null, null, null, null, null);

        }

        /// <summary>
        /// Este método se encarga de retornar la lista vista que permite crear Categorías de productos 
        /// </summary>
        /// <returns>La vista Crear</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CATEGORIA_PRODUCTOS + "," + WPPConstants.ROLES_EDIT_CATEGORIA_PRODUCTOS)]
        public ActionResult Crear()
        {
            return View();
        }

        /// <summary>
        /// Este método guarda la información del modelo ingresada en el formulario de Crear
        /// </summary>
        /// <returns>La vista Index</returns>
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CATEGORIA_PRODUCTOS + "," + WPPConstants.ROLES_EDIT_CATEGORIA_PRODUCTOS)]
        public ActionResult Crear(CategoriaProductoModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IDictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("Nombre", model.Nombre);
                    criteria.Add("IsDeleted", false);

                    if (categoriaService.Get(criteria) == null)
                    {
                        CategoriaProducto nuevaCategoria = new CategoriaProducto();
                        nuevaCategoria = categoriaMapper.GetCategoriaProducto(model, nuevaCategoria);
                        nuevaCategoria.Version = 1;
                        nuevaCategoria.CreateDate = DateTime.Now;
                        nuevaCategoria.DateLastModified = DateTime.Now;
                        nuevaCategoria.CreatedBy = NombreUsuarioActual();
                                                
                        // Se crea la categoría
                        nuevaCategoria = categoriaService.Create(nuevaCategoria);

                        ViewBag.Mensaje = "Se ha creado la categoría";

                        //return Index();
                        return RedirectToAction("Index", "CategoriaProducto");
                    }
                    else
                    {
                        ModelState.AddModelError("Nombre", "Nombre no válido, ya existe una compañía registrada con este nombre");
                        return View(model);
                    }
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return View("Crear", model);
            }
        }

        /// <summary>
        /// Este método carga la información de la categoría que se desea editar, por medio de su id
        /// </summary>
        /// <returns>La vista Editar con el modelo de la categoría del producto a editar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CATEGORIA_PRODUCTOS + "," + WPPConstants.ROLES_EDIT_CATEGORIA_PRODUCTOS)]
        public ActionResult Editar(long id)
        {
            CategoriaProductoModel categoria = categoriaMapper.GetCategoriaProductoModel(categoriaService.Get(id));
            return View(categoria);
        }

        /// <summary>
        /// Este método actualiza la información de la categoría del producto seleccionado, según lo datos del modelo
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CATEGORIA_PRODUCTOS + "," + WPPConstants.ROLES_EDIT_CATEGORIA_PRODUCTOS)]
        public ActionResult Editar(CategoriaProductoModel categoriaModel)
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Nombre", categoriaModel.Nombre);
            criteria.Add("IsDeleted", false);

            CategoriaProducto categoriaValidacion = categoriaService.Get(criteria);
            if (categoriaValidacion != null)
            {
                if (categoriaValidacion.Id != categoriaModel.Id)
                {
                    ModelState.AddModelError("Nombre", "Nombre no válido, ya existe una categoría de producto registrado con este nombre");
                    return View(categoriaModel);
                }
            }

            CategoriaProducto categoria = categoriaService.Get(categoriaModel.Id);
            categoria = categoriaMapper.GetCategoriaProducto(categoriaModel, categoria);
            categoria.DateLastModified = DateTime.Now;
            categoria.ModifiedBy = NombreUsuarioActual();
            categoria.Version++;
            
            // Se actualiza la información de la compañía
            categoria = categoriaService.Update(categoria);

            //return Index();
            return RedirectToAction("Index", "CategoriaProducto");
        }

        /// <summary>
        /// Este método carga la información de la categoría que se desea eliminar, esto según el id
        /// </summary>
        /// <returns>La vista Eliminar con el modelo del a eliminar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CATEGORIA_PRODUCTOS)]
        public ActionResult Eliminar(long id)
        {
            CategoriaProducto categoria = categoriaService.Get(id);
            CategoriaProductoModel model = categoriaMapper.GetCategoriaProductoModel(categoria);
            ViewBag.TipoDescripcion = model.Tipo == "DisposicionFinal" ? "Disposición Final / Destrucción" : model.Tipo;
            return View(model);
        }

        /// <summary>
        /// Este método actualiza el estado IsDeleted a true, con el fin de indicar que esta categoría fue eliminada
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CATEGORIA_PRODUCTOS )]
        public ActionResult Eliminar(CategoriaProductoModel model)
        {
            CategoriaProducto categoria = categoriaService.Get(model.Id);
            categoria.IsDeleted = true;
            categoria.DateLastModified = DateTime.Now;
            categoria.Version++;
            categoria.DeletedBy = NombreUsuarioActual();
            categoria.DeleteDate = DateTime.Now;

            categoriaService.Update(categoria);

            //return Index();
            return RedirectToAction("Index", "CategoriaProducto");
        }

        /// <summary>
        /// Este método carga el modelo de la categoría del producto que se desea obtener el detalle
        /// </summary>
        /// <returns>La vista de Detalles con el modelo de categoría que se desea consultar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_CONS_CATEGORIA_PRODUCTOS + "," + WPPConstants.ROLES_ADMIN_CATEGORIA_PRODUCTOS + "," + WPPConstants.ROLES_EDIT_CATEGORIA_PRODUCTOS)]
        public ActionResult Detalles(long id)
        {
            CategoriaProducto categoria = categoriaService.Get(id);
            CategoriaProductoModel model = categoriaMapper.GetCategoriaProductoModel(categoria);
            ViewBag.TipoDescripcion = model.Tipo == "DisposicionFinal" ? "Disposición Final / Destrucción" : model.Tipo;
            return View(model);
        }

        /// <summary>
        /// Este método lista y filtra el listado de registros de categorías del producto mostrados en el index
        /// </summary>
        /// <returns>Vista Index con el modelo IPageList(CategoriaProducto)</returns>
        public ActionResult Buscar(string sortOrder, string currentNombre,
                    string searchStringNombre, int? searchStringFilas, int? page)
        {
            ViewBag.CurrentSort = sortOrder;

            if (sortOrder == "NombreAsc")
                ViewBag.NombreSort = "NombreDesc";
            else
                ViewBag.NombreSort = "NombreAsc";


            if (searchStringNombre != null)
                page = 1;
            else
            {
                searchStringNombre = currentNombre;
                searchStringNombre = currentNombre;
            }

            ViewBag.CurrentNombre = searchStringNombre;

            var filas = searchStringFilas == null ? 10 : Convert.ToInt32(searchStringFilas);
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var pagina = categoriaService.PagingSearch(searchStringNombre, pageNumber, filas, sortOrder);

            return View("Index", pagina);
        }
    }
}
