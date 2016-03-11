using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Helpers;
using WPP.Mapper.General;
using WPP.Mapper.ModuloContratos;
using WPP.Model.ModuloContratos;
using WPP.Security;
using WPP.Service.ModuloContratos;

namespace WPP.Controllers
{
    public class ProductoController : BaseController
    {
        private IProductoService productoService;
        private ProductoMapper productoMapper;
        private CatalogoMapper catalogoMapper;
        private CategoriaProductoMapper categoriaMapper;
        private ICategoriaProductoService categoriaService;

        public ProductoController(IProductoService service, ICategoriaProductoService categoria)
        {
            try
            {
                this.productoService = service;
                this.categoriaService = categoria;
                categoriaMapper = new CategoriaProductoMapper();
                productoMapper = new ProductoMapper();
                catalogoMapper = new CatalogoMapper();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
                
        private ActionResult RedirectURL(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Producto");
            }
        }
        

        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROLES_ADMINISTRACION + "," + WPPConstants.ROLES_CONS_PRODUCTOS + "," + WPPConstants.ROLES_ADMIN_PRODUCTOS + "," + WPPConstants.ROLES_EDIT_PRODUCTOS)]
        public ActionResult Index()
        {

            return Buscar(null, null, null, null, null, null, null, null, null,null,null,null,null);
        }

        /// <summary>
        /// Este método se encarga de retornar la  vista que permite crear productos 
        /// </summary>
        /// <returns>La vista Crear</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_PRODUCTOS + "," + WPPConstants.ROLES_EDIT_PRODUCTOS)]
        public ActionResult Crear()
        {
            ViewBag.ListaUnidadCobro = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("UnidadCobro"));
            ViewBag.ListaProcesoCobro = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("ProcesoCobro"));
            ViewBag.ListaTamano = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("Tamaño")).OrderBy(s=> s.Id);
            ViewBag.ListaTipoEquipo = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("TipoEquipo"));
            ViewBag.ListaEstado = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("EstadoProducto"));
            ViewBag.ListaCategoria = categoriaMapper.GetListaCategoriaProductoModel(categoriaService.ListAll()); 
            
            return View();
        }

        /// <summary>
        /// Este método guarda la información del modelo ingresada en el formulario de Crear
        /// </summary>
        /// <returns>La vista Index</returns>
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_PRODUCTOS + "," + WPPConstants.ROLES_EDIT_PRODUCTOS)]
        public ActionResult Crear(ProductoModel producto)
        {
            if (ModelState.IsValid)
            {
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Descripcion", producto.Descripcion);
                criteria.Add("IsDeleted", false);
               // criteria.Add("Categoria", Convert.ToInt64(producto.Categoria));
                criteria.Add("Compania", Convert.ToInt64(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0));
                
                if (productoService.Get(criteria) == null)
                {
                    Producto nuevaProducto = new Producto();
                    nuevaProducto = productoMapper.GetProducto(producto, nuevaProducto);
                    nuevaProducto.Version = 1;
                    nuevaProducto.CreateDate = DateTime.Now;
                    nuevaProducto.DateLastModified = DateTime.Now;
                    nuevaProducto.CreatedBy = NombreUsuarioActual();
                    nuevaProducto.Compania = Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0;

                    if (producto.Estado > 0)
                    {
                        nuevaProducto.Estado = CatalogoService.Get(producto.Estado);
                    }

                    if (producto.ProcesoCobro.HasValue)
                    {
                        nuevaProducto.ProcesoCobro = CatalogoService.Get(producto.ProcesoCobro.Value);
                    }

                    if (producto.TipoEquipo > 0)
                    {
                        nuevaProducto.TipoEquipo = CatalogoService.Get(producto.TipoEquipo);
                    }

                    if (producto.UnidadCobro.HasValue)
                    {
                        nuevaProducto.UnidadCobro = CatalogoService.Get(producto.UnidadCobro.Value);
                    }

                    if (producto.Categoria > 0)
                    {
                        nuevaProducto.Categoria = categoriaService.Get(producto.Categoria);
                    }

                    if (producto.Tamano.HasValue)
                    {
                        nuevaProducto.Tamano = CatalogoService.Get(producto.Tamano.Value);
                    }

                    // Se crea el producto
                    nuevaProducto = productoService.Create(nuevaProducto);

                    ViewBag.Mensaje = "Se ha creado el nuevo producto";

                    //return Index();

                    return RedirectToAction("Index", "Producto");
                }
                else
                {
                    ModelState.AddModelError("Descripcion", "Descripción no válida, ya existe un producto registrado con esta descripción");
                    ViewBag.ListaUnidadCobro = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("UnidadCobro"));
                    ViewBag.ListaProcesoCobro = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("ProcesoCobro"));
                    ViewBag.ListaTamano = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("Tamaño")).OrderBy(s => s.Id);
                    ViewBag.ListaTipoEquipo = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("TipoEquipo"));
                    ViewBag.ListaEstado = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("EstadoProducto"));
                    ViewBag.ListaCategoria = categoriaMapper.GetListaCategoriaProductoModel(categoriaService.ListAll());

                    return View(producto);
                }
            }
            else
            {
                ViewBag.ListaUnidadCobro = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("UnidadCobro"));
                ViewBag.ListaProcesoCobro = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("ProcesoCobro"));
                ViewBag.ListaTamano = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("Tamaño")).OrderBy(s => s.Id);
                ViewBag.ListaTipoEquipo = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("TipoEquipo"));
                ViewBag.ListaEstado = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("EstadoProducto"));
                ViewBag.ListaCategoria = categoriaMapper.GetListaCategoriaProductoModel(categoriaService.ListAll());

                return View();
            }
        }

        /// <summary>
        /// Este método carga la información del producto que se desea editar, por medio de su id
        /// </summary>
        /// <returns>La vista Editar con el modelo del producto a editar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_PRODUCTOS + "," + WPPConstants.ROLES_EDIT_PRODUCTOS)]
        public ActionResult Editar(long idProducto)
        {
            ProductoModel producto = productoMapper.GetProductoModel(productoService.Get(idProducto));
            ViewBag.ListaUnidadCobro = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("UnidadCobro"));
            ViewBag.ListaProcesoCobro = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("ProcesoCobro"));
            ViewBag.ListaTamano = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("Tamaño")).OrderBy(s => s.Id);
            ViewBag.ListaTipoEquipo = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("TipoEquipo"));
            ViewBag.ListaEstado = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("EstadoProducto"));
            ViewBag.ListaCategoria = categoriaMapper.GetListaCategoriaProductoModel(categoriaService.ListAll());

            return View(producto);
        }

        /// <summary>
        /// Este método actualiza la información del producto seleccionado, según lo datos del modelo
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_PRODUCTOS + "," + WPPConstants.ROLES_EDIT_PRODUCTOS)]
        public ActionResult Editar(ProductoModel productoModel)
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Descripcion", productoModel.Descripcion);
            // criteria.Add("Categoria", Convert.ToInt64(producto.Categoria));
            criteria.Add("IsDeleted", false);
            criteria.Add("Compania", Convert.ToInt64(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0));

            Producto productoValidacion = productoService.Get(criteria);
            if (productoValidacion != null)
            {
                if (productoValidacion.Id != productoModel.Id)
                {
                    ModelState.AddModelError("Descripción", "Descripción no válida, ya existe un producto registrado con est a descripción");

                    ViewBag.ListaUnidadCobro = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("UnidadCobro"));
                    ViewBag.ListaProcesoCobro = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("ProcesoCobro"));
                    ViewBag.ListaTamano = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("Tamaño")).OrderBy(s => s.Id);
                    ViewBag.ListaTipoEquipo = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("TipoEquipo"));
                    ViewBag.ListaEstado = catalogoMapper.GetListaCatalogoModel(CatalogoService.GetByType("EstadoProducto"));

                    ViewBag.ListaCategoria = categoriaMapper.GetListaCategoriaProductoModel(categoriaService.ListAll());

                    return View(productoModel);
                }
            }

            Producto producto = productoService.Get(productoModel.Id);
            producto = productoMapper.GetProducto(productoModel, producto);
            producto.DateLastModified = DateTime.Now;
            producto.ModifiedBy = NombreUsuarioActual();
            producto.Version++;

            if (productoModel.Estado > 0)
            {
                producto.Estado = CatalogoService.Get(productoModel.Estado);
            }

            if (productoModel.ProcesoCobro.HasValue)
            {
                producto.ProcesoCobro = CatalogoService.Get(productoModel.ProcesoCobro.Value);
            }

            if (productoModel.TipoEquipo > 0)
            {
                producto.TipoEquipo = CatalogoService.Get(productoModel.TipoEquipo);
            }

            if (productoModel.UnidadCobro.HasValue)
            {
                producto.UnidadCobro = CatalogoService.Get(productoModel.UnidadCobro.Value);
            }

            if (productoModel.Categoria > 0)
            {
                producto.Categoria = categoriaService.Get(productoModel.Categoria);
            }

            if (productoModel.Tamano.HasValue)
            {
                producto.Tamano = CatalogoService.Get(productoModel.Tamano.Value);
            }

            // Se actualiza la información de la compañía
            producto = productoService.Update(producto);

            //return Index(); 

            return RedirectToAction("Index", "Producto");
        }

        /// <summary>
        /// Este método carga la información del producto que se desea eliminar, esto según el id
        /// </summary>
        /// <returns>La vista Eliminar con el modelo del producto eliminar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_PRODUCTOS )]
        public ActionResult Eliminar(long idProducto)
        {
            Producto producto = productoService.Get(idProducto);
            ProductoModel productoModel = productoMapper.GetProductoModel(producto);
            ViewBag.Categoria = producto.Categoria;
            
            return View(productoModel);
        }

        /// <summary>
        /// Este método actualiza el estado IsDeleted a true, con el fin de indicar que este producto fue eliminado
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_PRODUCTOS)]
        public ActionResult Eliminar(ProductoModel productoModel)
        {
            Producto producto = productoService.Get(productoModel.Id);
            producto.IsDeleted = true;
            producto.DateLastModified = DateTime.Now;
            producto.Version++;
            producto.DeletedBy = NombreUsuarioActual();
            producto.DeleteDate = DateTime.Now;

            productoService.Update(producto);

            //return Index();
            return RedirectToAction("Index", "Producto");
        }

        /// <summary>
        /// Este método carga el modelo del producto que se desea obtener el detalle
        /// </summary>
        /// <returns>La vista de Detalles con el modelo de producto que se desea consultar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_CONS_PRODUCTOS + "," + WPPConstants.ROLES_ADMIN_PRODUCTOS + "," + WPPConstants.ROLES_EDIT_PRODUCTOS)]
        public ActionResult Detalles(long idProducto)
        {
            Producto producto = productoService.Get(idProducto);
            ProductoModel productoModel = productoMapper.GetProductoModel(producto);
            ViewBag.Categoria = producto.Categoria;
            return View(productoModel);
        }

        /// <summary>
        /// Este método lista y filtra el listado de registros de productos mostrados en el index
        /// </summary>
        /// <returns>Vista Index con el modelo IPageList(Producto)</returns>
        public ActionResult Buscar(string sortOrder, string currentFilterDescripcion, string currentFilterTipo, string currentFilterEstado,string currentFilterUnidad,string currentFilterEquipo,
                    string searchStringDescripcion, string searchStringTipo, string searchStringEstado, string searchStringUnidad, string searchStringEquipo, string searchStringFilas, int? page)
        {
            ViewBag.CurrentSort = sortOrder;

            if (sortOrder == "DescripcionAsc")
                ViewBag.DescripcionSortParam = "DescripcionDesc";
            else
                ViewBag.DescripcionSortParam = "DescripcionAsc";

            if (sortOrder == "EstadoAsc")
                ViewBag.EstadoSortParam = "EstadoDesc";
            else
                ViewBag.EstadoSortParam = "EstadoAsc";

            if (sortOrder == "UnidadAsc")
                ViewBag.UnidadSortParam = "UnidadDesc";
            else
                ViewBag.UnidadSortParam = "UnidadAsc";

            if (sortOrder == "TipoAsc")
                ViewBag.TipoSortParam = "TipoDesc";
            else
                ViewBag.TipoSortParam = "TipoAsc";

            if (sortOrder == "EquipoAsc")
                ViewBag.EquipoSortParam = "EquipoDesc";
            else
                ViewBag.EquipoSortParam = "EquipoAsc";

            if (searchStringDescripcion != null || searchStringTipo != null || searchStringEstado != null || searchStringUnidad != null || searchStringEquipo != null)
                page = 1;
            else
            {
                searchStringDescripcion = currentFilterDescripcion;
                searchStringEstado = currentFilterEstado;
                searchStringUnidad = currentFilterUnidad;
                searchStringTipo = currentFilterTipo;
                searchStringEquipo = currentFilterEquipo;
            }

            ViewBag.CurrentDescripcion = searchStringDescripcion;
            ViewBag.CurrentEstado = searchStringEstado;
            ViewBag.CurrentUnidad = searchStringUnidad;
            ViewBag.CurrentTipo = searchStringTipo;
            ViewBag.CurrentEquipo = searchStringEquipo;

            var filas = searchStringFilas == null ? 10 : Convert.ToInt32(searchStringFilas);
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var compania = Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0;
            var paginaProducto = productoService.PagingSearch(searchStringDescripcion, searchStringTipo, searchStringEstado, searchStringUnidad, searchStringEquipo, pageNumber, filas, sortOrder, compania);

            return View("Index", paginaProducto);
        }

        [HttpPost]
        public JsonResult CargarProducto(long idProducto)
        {
            try
            {
                Producto producto = productoService.Get(idProducto);
                ProductoModel producoModel = new ProductoModel
                {
                    Id = producto.Id,
                    ProcesoCobroNombre = producto.ProcesoCobro.Nombre,
                    UnidadCobroNombre = producto.UnidadCobro.Nombre,
                    TipoEquipoNombre = producto.TipoEquipo.Nombre,
                    CategoriaNombre = producto.Categoria.Tipo
                };
                return Json(producoModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(null);
            }
        }

    }
}
