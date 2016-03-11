using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WPP.Datos.OperacionRecoleccion;
using WPP.Entities.Objects.ModuloBascula;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;
using WPP.Helpers;
using WPP.Mapper.General;
using WPP.Mapper.ModuloBascula;
using WPP.Mapper.ModuloContratos;
using WPP.Mapper.ModuloOperacionRecoleccion;
using WPP.Model.ModuloBascula;
using WPP.Model.ModuloContratos;
using WPP.Model.ModuloOperacionRecoleccion;
using WPP.Security;
using WPP.Service.Generales;
using WPP.Service.ModuloBascula;
using WPP.Service.ModuloContratos;
using WPP.Service.ModuloOperacionRecoleccion;

namespace WPP.Controllers
{
    public class OTRController : BaseController
    {
        private IContratoService contratoService;       
        private ContratoMapper contratoMapper;
        private ClienteMapper clienteMapper;
        private EquipoMapper equipoMapper;
        private IClienteService clienteService;
        private ICompaniaService companiaService;
        private IConsecutivoOTRService consecutivoService;
        private IContenedorHistorialService contenedorHistorialService;
        private IEquipoService equipoService;
        private IOTRService otrService;
        private OTRMapper otrMapper;
        private IRellenoSanitarioService rellenoService;
        private RellenoSanitarioMapper rellenoMapper;
        private IProductoContratoService productoContratoService;
        private IRutaRecoleccionService rutaService;
        private ICuadrillaService cuadrillaService;
        private CuadrillaMapper cuadrillaMapper;
        private IBasculaService basculaService;
        private ICatalogoService catalogoSerrvice;
        private CatalogoMapper catalogoMapper;
        private ContenedorMapper contenedorMapper;
        private RutaRecoleccionMapper rutaMapper;
        private IContenedorService contenedorService;
        private IViajeOTRService viajeService;
        private ViajeOTRMapper viajeMapper;
        private ContenedorHistorialMapper contenedorHistorialMapper;

        public OTRController(IContratoService contrato, IClienteService cliente, ICompaniaService compania, IConsecutivoOTRService consecutivo, IEquipoService equipo, 
            IRellenoSanitarioService relleno, IOTRService otr, IProductoContratoService productoContrato, ICuadrillaService cuadrilla, IBasculaService bascula, 
            ICatalogoService catalogo, IRutaRecoleccionService ruta, IContenedorService contenedor, IViajeOTRService viaje, IContenedorHistorialService contenedorHistorial)
        {
            try
            {
                this.contratoService = contrato;
                this.clienteService = cliente;
                this.companiaService = compania;
                this.equipoService = equipo;
                this.consecutivoService = consecutivo;
                this.otrService = otr;
                this.rellenoService = relleno;
                this.productoContratoService = productoContrato;
                this.cuadrillaService = cuadrilla;
                this.basculaService = bascula;
                this.catalogoSerrvice = catalogo;
                this.rutaService = ruta;
                this.contenedorService = contenedor;
                this.viajeService = viaje;
                this.contenedorHistorialService = contenedorHistorial;
                catalogoMapper = new CatalogoMapper();
                contratoMapper = new ContratoMapper();
                clienteMapper = new ClienteMapper();
                equipoMapper = new EquipoMapper();
                otrMapper = new OTRMapper();
                rellenoMapper = new RellenoSanitarioMapper();
                cuadrillaMapper = new CuadrillaMapper();
                rutaMapper = new RutaRecoleccionMapper();
                contenedorMapper = new ContenedorMapper();
                viajeMapper = new ViajeOTRMapper();
                contenedorHistorialMapper = new ContenedorHistorialMapper();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }


        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_CONS_OTR + "," + WPPConstants.ROLES_ADMIN_OTR + "," + WPPConstants.ROLES_EDIT_OTR)]
        public ActionResult Index()
        {
            return Buscar(null, null, null, null, null, null, null, null, null, null, null,null,null);
        }

        #region OTR MUNICIPAL Y COMERCIAL

        /// <summary>
        /// Este método se encarga de retornar la  vista que permite crear otr y
        /// cargar los viewbags necesarios(Consecutivo, RellenosSanitarios) 
        /// </summary>
        /// <returns>La vista CrearOTR</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_OTR + "," + WPPConstants.ROLES_EDIT_OTR)]
        public ActionResult CrearOTR(string tipo)
        {
            Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Compania", compania);
            criteria.Add("IsDeleted", false);

            //ConsecutivoOTR consecutivo = consecutivoService.Get(criteria);

            ViewBag.ListaRellenos = rellenoMapper.GetListaModel(rellenoService.ListAll());

            criteria.Add("Estado", "Activo");
            ViewBag.ListaCuadrilla = cuadrillaMapper.GetListaModel(cuadrillaService.GetAll(criteria));
            if(tipo != "Servicios Internos")
            {
                criteria.Add("Tipo", tipo);
            }
            ViewBag.ListaRutaRecoleccion = rutaMapper.GetListaRutaRecoleccionModel(rutaService.GetAll(criteria));
            ViewBag.ListaContratos = new List<ContratoModel>();

            OTRModel model = new OTRModel();
         //   model.Consecutivo = consecutivo.Secuencia;
            model.Fecha = DateTime.Now;
            model.Tipo = tipo;

            IDictionary<string, object> criteriaCliente = new Dictionary<string, object>();
            criteriaCliente.Add("IsDeleted", false);
            List<Cliente> cliente = clienteService.GetAll(criteriaCliente).Where(s=> s.CompaniaId != 0).ToList<Cliente>();
            ViewBag.ListaClientes = clienteMapper.GetListaClienteModel(cliente);

            //consecutivo.Secuencia = consecutivo.Secuencia + 1;
            //consecutivoService.Update(consecutivo);                       
            return View(model);
        }


        /// <summary>
        /// Este método guarda la información del modelo de la OTR ingresada en el formulario de Crear
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_OTR + "," + WPPConstants.ROLES_EDIT_OTR)]
        public JsonResult CrearOTR(OTRModel model)
        {
            if (ModelState.IsValid)
            {
                OTR nuevaOTR = new OTR();
                nuevaOTR = otrMapper.GetBoletaOTR(model, nuevaOTR);
                nuevaOTR.Version = 1;
                nuevaOTR.CreateDate = DateTime.Now;
                nuevaOTR.CreatedBy = NombreUsuarioActual();
                nuevaOTR.Compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                nuevaOTR.Facturada = false;

                if (model.Origen > 0)
                {
                    nuevaOTR.Origen = rellenoService.Get(model.Origen);
                }

                if (model.Destino > 0)
                {
                    nuevaOTR.Destino = rellenoService.Get(model.Destino);
                }

                if (model.Relleno > 0)
                {
                    nuevaOTR.Relleno = rellenoService.Get(model.Relleno);
                }  

                if (model.RutaRecoleccion > 0)
                {
                    nuevaOTR.RutaRecoleccion = rutaService.Get(model.RutaRecoleccion.Value);
                }

                 if (model.Cuadrilla > 0)
                {
                    nuevaOTR.Cuadrilla = cuadrillaService.Get(model.Cuadrilla);
                }

                 if (model.Cliente > 0)
                 {
                     nuevaOTR.Cliente = clienteService.Get(model.Cliente);
                 }

                 if (model.Contrato > 0)
                 {
                     nuevaOTR.Contrato = contratoService.Get(model.Contrato);
                 }

                if (model.Equipo > 0)
                {
                    Equipo equipo = equipoService.Get(model.Equipo);
                    if (equipo == null)
                    {
                        ModelState.AddModelError("NombreEquipo", "Equipo no válido");
                        return Json(null);//return View(model);
                    }
                    nuevaOTR.Equipo = equipo;
                }
                else {
                    ModelState.AddModelError("NombreEquipo", "Equipo no válido");
                    return Json(null);// View(model);
                }

               // nuevaOTR.ListaOTRHijas = new List<OTR>();

                //if (model.Tipo != "Municipal" && model.OTRMadre == true)
                //{
                //    var arrayHijas = model.OTRHijas.Split(',');
                //    foreach (var item in arrayHijas)
                //    {
                //        if (item != String.Empty)
                //        {
                //            var OTR = otrService.Get(Convert.ToInt64(item));
                //            nuevaOTR.ListaOTRHijas.Add(OTR);
                //        }
                //    }                
                //}

                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Compania", nuevaOTR.Compania);
                ConsecutivoOTR consecutivo = consecutivoService.Get(criteria);
                nuevaOTR.Consecutivo = consecutivo.Secuencia;
                consecutivo.Secuencia = consecutivo.Secuencia + 1;
                consecutivoService.Update(consecutivo);

                // Se crea el producto
                nuevaOTR = otrService.Create(nuevaOTR);
                
               // return Index();
                return Json(nuevaOTR.Id, JsonRequestBehavior.AllowGet);//return RedirectToAction("Index", "OTR");
            }
            else
            {
                Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Compania", compania);
                ConsecutivoOTR consecutivo = consecutivoService.Get(criteria);
                ViewBag.Consecutivo = consecutivo.Secuencia;
                consecutivo.Secuencia = consecutivo.Secuencia + 1;
                consecutivoService.Update(consecutivo);
                ViewBag.ListaRellenos = rellenoMapper.GetListaModel(rellenoService.ListAll());
                criteria.Add("Estado", "Activo");
                criteria.Add("IsDeleted", false);
                ViewBag.ListaCuadrilla = cuadrillaMapper.GetListaModel(cuadrillaService.GetAll(criteria));
                ViewBag.ListaRutaRecoleccion = rutaMapper.GetListaRutaRecoleccionModel(rutaService.GetAll(criteria));
                ViewBag.ListaContratos = new List<ContratoModel>();

                IDictionary<string, object> criteriaCliente = new Dictionary<string, object>();
                criteriaCliente.Add("IsDeleted", false);
                List<Cliente> cliente = clienteService.GetAll(criteriaCliente).Where(s => s.CompaniaId != 0).ToList<Cliente>();
                ViewBag.ListaClientes = clienteMapper.GetListaClienteModel(cliente);

                return Json(null);//return View();
            }
        }

        public void ReporteOTR(long id)
        {
            OTR otr = otrService.Get(id);

            switch (otr.Tipo)
            {
                case "Municipal":
                    GenerarReporteOTRGeneral(otr);
                    break;
                case "Comercial":
                    GenerarReporteOTRComercial(otr);
                    break;
                case "Roll-Off":
                    GenerarReporteOTRRollOff(otr);
                    break;
                default: 
                    GenerarReporteOTRGeneral(otr);
                    break;              
            }                
        }

        public void GenerarReporteOTRComercial(OTR otr)
        {
            ds_OTRComercial dsOTR = new ds_OTRComercial();
            var dtReporte = dsOTR.Tables["Reporte"];
            var dtDatos = dsOTR.Tables["Datos"];

            // Rows DataTable Reporte
            DataRow row = dtReporte.NewRow();
            row["Compania"] = otr.Compania.Nombre;
            row["Fecha"] = otr.Fecha;
            row["Placa"] = otr.Equipo.Placa;
            var chofer = otr.Cuadrilla.ListaEmpleados.Where(s => s.Puesto == "Chofer").FirstOrDefault();
            row["Chofer"] = chofer != null ? chofer.Nombre : String.Empty;
            row["OTR"] = otr.Consecutivo;
            dtReporte.Rows.Add(row);

            // Rows DataTable Datos
            String origen = otr.Origen.Nombre;
            String destino = otr.Destino.Nombre;
            DataRow datos = dtDatos.NewRow();

            // De Cliente 1 a Cliente n
            for (int i = 0; i < otr.RutaRecoleccion.Rutas.Count; i++)
            {
                var item = otr.RutaRecoleccion.Rutas[i];               
                datos = dtDatos.NewRow();

                if(i == 0)
                {
                    datos["Origen"] = origen;
                }
                else
                {
                    var itemAnterior = otr.RutaRecoleccion.Rutas[i - 1];
                    datos["Origen"] = itemAnterior.Ubicacion.Descripcion;
                }

                datos["Destino"] = item.Ubicacion.Descripcion;    
                datos["Cliente"] = item.Contrato.Cliente.Nombre;
                dtDatos.Rows.Add(datos);
            }

            // De Cliente n a Relleno
            var anterior = otr.RutaRecoleccion.Rutas[otr.RutaRecoleccion.Rutas.Count - 1];
            datos = dtDatos.NewRow();
            datos["Origen"] = anterior.Ubicacion.Descripcion;
            datos["Destino"] = otr.Relleno.Nombre;
            datos["Cliente"] = "WPP - Disposición";
            dtDatos.Rows.Add(datos);

            // De Relleno a Destino
            datos = dtDatos.NewRow();
            datos["Origen"] = otr.Relleno.Nombre;
            datos["Destino"] = destino;
            datos["Cliente"] = "WPP - Guardar Equipo";
            dtDatos.Rows.Add(datos);

            // Se genera el reporte deseado
            ReportDocument rd = new ReportDocument();
            string strRptPath = strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//Recoleccion//rpt_OTRComercial.rpt";

            rd.Load(strRptPath);

            rd.SetDataSource(dsOTR);// Se asigna el dataset al datasorce del reporte
            var tipoFormato = ExportFormatType.PortableDocFormat;
            rd.ExportToHttpResponse(tipoFormato, System.Web.HttpContext.Current.Response, false, "OTR_" + otr.Tipo);
        }

        public void GenerarReporteOTRRollOff(OTR otr)
        {
            ds_OTRRollOff dsOTR = new ds_OTRRollOff();
            var dtReporte = dsOTR.Tables["Reporte"];
            var dtDatos = dsOTR.Tables["Datos"];

            // Rows DataTable Reporte
            DataRow row = dtReporte.NewRow();
            row["Compania"] = otr.Compania.Nombre;
            row["Fecha"] = otr.Fecha;
            row["Placa"] = otr.Equipo.Placa;
            var chofer = otr.Cuadrilla.ListaEmpleados.Where(s => s.Puesto == "Chofer").FirstOrDefault();
            row["Chofer"] = chofer != null ? chofer.Nombre : String.Empty;
            row["OTR"] = otr.Consecutivo;
            dtReporte.Rows.Add(row);

            // Rows DataTable Datos           
            var item = otr.ListaViajesOTR[0];
            var viaje = otr.RutaRecoleccion.Rutas.Where(s=>s.Id == item.Viaje.Id).FirstOrDefault();

            // Del Origen al Cliente
            DataRow datos = dtDatos.NewRow();
            datos["Origen"] = otr.Origen.Nombre;
            datos["Destino"] = viaje.Ubicacion.Descripcion;
            datos["Acción"] = item.Accion.Nombre;
            datos["Tipo"] = item.TipoEquipo.Nombre;
            datos["Contenedor"] = item.Contenedor.Codigo;
            dtDatos.Rows.Add(datos);

            // Del Cliente al Relleno
            datos = dtDatos.NewRow();
            datos["Origen"] = viaje.Ubicacion.Descripcion;
            datos["Destino"] = otr.Relleno != null ? otr.Relleno.Nombre : "";
            datos["Acción"] = "Disposición";
            datos["Tipo"] = item.TipoEquipo.Nombre;
            datos["Contenedor"] = "";
            dtDatos.Rows.Add(datos);
            
            // Del Relleno al Destino
            datos = dtDatos.NewRow();
            datos["Origen"] = otr.Relleno != null ? otr.Relleno.Nombre : "";
            datos["Destino"] = otr.Destino.Nombre;
            datos["Acción"] = "Regresar Equipo";
            datos["Tipo"] = item.TipoEquipo.Nombre;
            datos["Contenedor"] = "";
            dtDatos.Rows.Add(datos);
            
            // Se genera el reporte deseado
            ReportDocument rd = new ReportDocument();
            string strRptPath = strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//Recoleccion//rpt_OTRRollOff.rpt";

            rd.Load(strRptPath);

            rd.SetDataSource(dsOTR);// Se asigna el dataset al datasorce del reporte
            var tipoFormato = ExportFormatType.PortableDocFormat;
            rd.ExportToHttpResponse(tipoFormato, System.Web.HttpContext.Current.Response, false, "OTR_" + otr.Tipo);
        }

        public void GenerarReporteOTRGeneral(OTR otr)
        {
            ds_OTRMunicipal dsOTR = new ds_OTRMunicipal();
            var dtDatos = dsOTR.Tables["Datos"];

            // Rows DataTable Datos
            DataRow row = dtDatos.NewRow();
            row["Compania"] = otr.Compania.Nombre;
            row["Fecha"] = otr.Fecha;
            row["Placa"] = otr.Equipo.Placa;
            var chofer = otr.Cuadrilla.ListaEmpleados.Where(s=> s.Puesto == "Chofer").FirstOrDefault();
            row["Chofer"] = chofer != null ? chofer.Nombre : String.Empty;
            row["Origen"] = otr.Origen.Nombre;
            row["Destino"] = otr.Destino.Nombre;
            row["Relleno"] = otr.Relleno != null ? otr.Relleno.Nombre : String.Empty;
            row["OTR"] = otr.Consecutivo;
            dtDatos.Rows.Add(row);

            // Se genera el reporte deseado
            ReportDocument rd = new ReportDocument();
            string strRptPath = strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//Recoleccion//rpt_OTRMunicipal.rpt";
            
            rd.Load(strRptPath);

            rd.SetDataSource(dsOTR);// Se asigna el dataset al datasorce del reporte
            var tipoFormato = ExportFormatType.PortableDocFormat;
            rd.ExportToHttpResponse(tipoFormato, System.Web.HttpContext.Current.Response, false, "OTR_" + otr.Tipo);
        }


        /// <summary>
        /// Este método carga la información de la OTR que se desea editar, por medio de su id y
        /// cargar los viewbags necesarios(RellenosSanitarios) 
        /// </summary>
        /// <returns>La vista EditarOTR con el modelo de la OTR a editar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_OTR + "," + WPPConstants.ROLES_EDIT_OTR)]
        public ActionResult EditarOTR(long id)
        {
            OTR actual = otrService.Get(id);
            OTRModel otr = otrMapper.GetBoletaOTRModel(actual);
            ViewBag.ListaRellenos = rellenoMapper.GetListaModel(rellenoService.ListAll());
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
            criteria.Add("Compania", compania);
            criteria.Add("Estado", "Activo");
            criteria.Add("IsDeleted", false);
            ViewBag.ListaCuadrilla = cuadrillaMapper.GetListaModel(cuadrillaService.GetAll(criteria));
            if(otr.Tipo != "Servicios Internos")
            {
                criteria.Add("Tipo", otr.Tipo);
            }
            ViewBag.ListaRutaRecoleccion = rutaMapper.GetListaRutaRecoleccionModel(rutaService.GetAll(criteria));

            IDictionary<string, object> criteriaCliente = new Dictionary<string, object>();
            criteriaCliente.Add("IsDeleted", false);
            List<Cliente> cliente = clienteService.GetAll(criteriaCliente).Where(s => s.CompaniaId != 0).ToList<Cliente>();
            ViewBag.ListaClientes = clienteMapper.GetListaClienteModel(cliente);

            IDictionary<string, object> criteriaContrato = new Dictionary<string, object>();
            criteriaContrato.Add("Cliente", actual.Cliente);

            List<ContratoModel> contratos = new List<ContratoModel>();
            var listaContratos = contratoMapper.GetListaClienteModel(contratoService.GetAll(criteriaContrato));

            foreach (var item in listaContratos)
            {
                contratos.Add(
                    new ContratoModel
                    {
                        Id = item.Id,
                        DescripcionContrato = item.DescripcionContrato
                    }
                 );
            }

            ViewBag.ListaContratos = contratos;

            //if (otr.OTRMadre)
            //{
            //    String hijas = String.Empty;
            //    foreach (var item in actual.ListaOTRHijas)
            //    {
            //        if (hijas == String.Empty)
            //        {
            //            hijas += item.Id;
            //        }
            //        else {
            //            hijas +=  "," + item.Id;
            //        }
            //    }
            //    otr.OTRHijas = hijas;
            //}
            return View(otr);
        }

        /// <summary>
        /// Este método actualiza la información de la OTR seleccionado, según lo datos del modelo
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_OTR + "," + WPPConstants.ROLES_EDIT_OTR)]
        public JsonResult EditarOTR(OTRModel model)
        {
            OTR otr = otrService.Get(model.Id);
            Compania compania = otr.Compania;
            otr = otrMapper.GetBoletaOTR(model, otr);
            otr.DateLastModified = DateTime.Now;
            otr.ModifiedBy = NombreUsuarioActual();
            otr.Version++;
            otr.Compania = compania;

            if (model.Origen > 0)
            {
                otr.Origen = rellenoService.Get(model.Origen);
            }

            if (model.Destino > 0)
            {
                otr.Destino = rellenoService.Get(model.Destino);
            }

            if (model.Relleno > 0)
            {
                otr.Relleno = rellenoService.Get(model.Relleno);
            }

            if (model.RutaRecoleccion > 0)
            {
                otr.RutaRecoleccion = rutaService.Get(model.RutaRecoleccion.Value);
            }

            if (model.Cuadrilla > 0)
            {
                otr.Cuadrilla = cuadrillaService.Get(model.Cuadrilla);
            }
            
            if (model.Cliente > 0)
            {
                otr.Cliente = clienteService.Get(model.Cliente);
            }

            if (model.Contrato > 0)
            {
                otr.Contrato = contratoService.Get(model.Contrato);
            }

            if (model.Equipo > 0)
            {
                Equipo equipo = equipoService.Get(model.Equipo);
                if (equipo == null)
                {
                    ModelState.AddModelError("NombreEquipo", "Equipo no válido");
                    return Json(null);
                    //return View(model);
                }
                otr.Equipo = equipo;
            }
            else
            {
                ModelState.AddModelError("NombreEquipo", "Equipo no válido");
                return Json(null);
                //return View(model);
            }

            //otr.ListaOTRHijas = new List<OTR>();

            //if (model.Tipo != "Municipal" && model.OTRMadre == true)
            //{
            //    var arrayHijas = model.OTRHijas.Split(',');
            //    foreach (var item in arrayHijas)
            //    {
            //        var OTR = otrService.Get(Convert.ToInt64(item));
            //        otr.ListaOTRHijas.Add(OTR);
            //    }
            //}

            // Se actualiza la información de la OTR
            otr = otrService.Update(otr);


            return Json(otr.Id, JsonRequestBehavior.AllowGet);
            //return Index();
           // return RedirectToAction("Index", "OTR");
        }

        /// <summary>
        /// Este método carga la información de la OTR que se desea eliminar, esto según el id
        /// </summary>
        /// <returns>La vista EliminarOTR con el modelo del contrato a eliminar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_OTR)]
        public ActionResult EliminarOTR(long id)
        {
            OTR otr = otrService.Get(id);
            OTRModel model = otrMapper.GetBoletaOTRModel(otr);
          //  ViewBag.ListaHijas = otr.ListaOTRHijas;
            return View(model);
        }

        /// <summary>
        /// Este método actualiza el estado IsDeleted a true, con el fin de indicar que esta OTR fue eliminado
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_OTR)]
        public ActionResult EliminarOTR(OTRModel model)
        {
            OTR otr = otrService.Get(model.Id);
            otr.IsDeleted = true;
            otr.DateLastModified = DateTime.Now;
            otr.Version++;
            otr.DeletedBy = NombreUsuarioActual();
            otr.DeleteDate = DateTime.Now;

            otrService.Update(otr);

            //return Index();
            return RedirectToAction("Index", "OTR");
        }


        /// <summary>
        /// Este método carga la información de la OTR que se desea cerrar, esto según el id
        /// </summary>
        /// <returns>La vista CerrarOTR con el modelo de la otr a cerrar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_OTR )]
        public ActionResult CerrarOTR(long id)
        {
            OTR otr = otrService.Get(id);
            OTRModel model = otrMapper.GetBoletaOTRModel(otr);
         //   ViewBag.ListaHijas = otr.ListaOTRHijas;
            return View(model);
        }

        /// <summary>
        /// Este método actualiza el estado a procesado, con el fin de indicar que esta OTR fue procesada 
        /// y que por lo tanto ya no se pueden realizar cambios en ella
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_OTR)]
        public ActionResult CerrarOTR(OTRModel model)
        {
            OTR otr = otrService.Get(model.Id);
            otr.Estado = "Procesada";
            otr.DateLastModified = DateTime.Now;
            otr.Version++;
            otr.DeletedBy = NombreUsuarioActual();
            otr.DeleteDate = DateTime.Now;

            //if (otr.OTRMadre)
            //{
            //    IList<OTR> lista = otr.ListaOTRHijas;
            //    foreach (var item in lista)
            //    {
            //        item.Estado = "Procesada";
            //        otrService.Update(item);
            //    }
            //}

            otrService.Update(otr);

            //return Index();
            return RedirectToAction("Index", "OTR");
        }

        /// <summary>
        /// Este método carga el modelo de la OTR que se desea obtener el detalle
        /// </summary>
        /// <returns>La vista de Detalles con el modelo de contrato que se desea consultar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_CONS_OTR + "," + WPPConstants.ROLES_ADMIN_OTR + "," + WPPConstants.ROLES_EDIT_OTR)]
        public ActionResult DetallesOTR(long id)
        {
            OTR otr = otrService.Get(id);
            OTRModel model = otrMapper.GetBoletaOTRModel(otr);
           // ViewBag.ListaHijas = otr.ListaOTRHijas;
            return View("DetallesOTR", model);
        }

        /// <summary>
        /// Este método efectua la anulación de una OTR, lo cual consiste en: cambiar a estado Anulado la OTR
        /// </summary>
        /// <returns>un Json con el id de la OTR</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_OTR)]
        public JsonResult AnularOTR(long id)
        {
            try
            {
                OTR otr = otrService.Get(id);
                otr.DateLastModified = DateTime.Now;
                otr.ModifiedBy = NombreUsuarioActual();
                otr.Version++;
                otr.Estado = "Anulada";
                otrService.Update(otr);

                return Json(otr.Id, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(null);
            }
        }

        /// <summary>
        /// Este método carga la información de la OTR que se desea cerrar, esto según el id
        /// </summary>
        /// <returns>La vista CerrarOTR con el modelo del contrato a cerrar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_OTR + "," + WPPConstants.ROLES_EDIT_OTR)]
        public ActionResult EstadisticasOTR(long id)
        {
            OTR otr = otrService.Get(id);
            OTRModel model = new OTRModel();
            //if (model.Estado == "Procesada")
            //{

            model = otrMapper.GetBoletaOTRModel(otr);
       //     ViewBag.RutaProducto = otr.Ruta;

            IDictionary<string, object> criteria = new Dictionary<string, object>();
            Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
            criteria.Add("Compania", compania);
            criteria.Add("Estado", "Activo");
           // criteria.Add("IsDeleted", false);
            criteria.Add("OTR", otr);

            ViewBag.Bascula = basculaService.Get(criteria);

            //}
            //else
            //{
            //    ModelState.AddModelError("", "Esta OTR aún no ha sido procesada en Báscula.");
            //}
            return View(model);
        }

        /// <summary>
        /// Este método actualiza el estado IsDeleted a true, con el fin de indicar que esta OTR fue eliminado
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_OTR + "," + WPPConstants.ROLES_EDIT_OTR)]
        public ActionResult EstadisticasOTR(OTRModel model)
        {
            OTR otr = otrService.Get(model.Id);
            otr = otrMapper.GetBoletaOTR(model, otr);
            otr.DateLastModified = DateTime.Now;
            otr.Version++;
            otr.ModifiedBy = NombreUsuarioActual();
            otr.Estado = "Cerrado";

            otrService.Update(otr);

            //return Index();
            return RedirectToAction("Index", "OTR");
        }

        #endregion

        #region OTR ROLL OFF

        /// <summary>
        /// Este método se encarga de retornar la  vista que permite crear otr y
        /// cargar los viewbags necesarios(Consecutivo, RellenosSanitarios) 
        /// </summary>
        /// <returns>La vista CrearOTRRollOff</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_OTR + "," + WPPConstants.ROLES_EDIT_OTR)]
        public ActionResult CrearOTRRollOff(string tipo)
        {
            try
            {
                Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Compania", compania);
                
                ViewBag.ListaRellenos = rellenoMapper.GetListaModel(rellenoService.ListAll());

                criteria.Add("Estado", "Activo");
                criteria.Add("IsDeleted", false);
                ViewBag.ListaCuadrilla = cuadrillaMapper.GetListaModel(cuadrillaService.GetAll(criteria));
                criteria.Add("Tipo", tipo);
                ViewBag.ListaRutaRecoleccion = rutaMapper.GetListaRutaRecoleccionModel(rutaService.GetAll(criteria));               
                ViewBag.ListaContratos = new List<ContratoModel>();

                IDictionary<string, object> criteriaCliente = new Dictionary<string, object>();
                criteriaCliente.Add("IsDeleted", false);
                List<Cliente> cliente = clienteService.GetAll(criteriaCliente).Where(s => s.CompaniaId != 0).ToList<Cliente>();
                ViewBag.ListaClientes = clienteMapper.GetListaClienteModel(cliente);
                
                var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                ViewBag.ListaTamanos = oSerializer.Serialize(catalogoMapper.GetListaCatalogoModel(catalogoSerrvice.GetByType("Tamaño")));
                ViewBag.ListaTipoEquipo = oSerializer.Serialize(catalogoMapper.GetListaCatalogoModel(catalogoSerrvice.GetByType("TipoEquipoRollOff")));
                ViewBag.ListaAcciones = oSerializer.Serialize(catalogoMapper.GetListaCatalogoModel(catalogoSerrvice.GetByType("AccionRollOff")));
                ViewBag.ListaContenedores = oSerializer.Serialize(contenedorMapper.GetListaModel(contenedorService.ListAll()));
                
                OTRModel model = new OTRModel();
                model.Fecha = DateTime.Now;
                model.Tipo = tipo;

                return View(model);
            }
            catch
            {
                ViewBag.ListaRellenos = new List<RellenoSanitario>();
                ViewBag.ListaCuadrilla = new List<Cuadrilla>();
                ViewBag.ListaUbicaciones = new List<UbicacionCliente>();
                return View();
            }
        }
        
        /// <summary>
        /// Este método guarda la información del modelo de la OTR ingresada en el formulario de Crear
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_OTR + "," + WPPConstants.ROLES_EDIT_OTR)]
        public JsonResult CrearOTRRollOff(OTRModel model)
        {
            if (ModelState.IsValid)
            {
                OTR nuevaOTR = new OTR();
                nuevaOTR = otrMapper.GetBoletaOTR(model, nuevaOTR);
                nuevaOTR.Version = 1;
                nuevaOTR.CreateDate = DateTime.Now;
                nuevaOTR.CreatedBy = NombreUsuarioActual();
                nuevaOTR.Compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                nuevaOTR.Facturada = false;
                
                if (model.Origen > 0)
                {
                    nuevaOTR.Origen = rellenoService.Get(model.Origen);
                }

                if (model.Destino > 0)
                {
                    nuevaOTR.Destino = rellenoService.Get(model.Destino);
                }

                if (model.Relleno > 0)
                {
                    nuevaOTR.Relleno = rellenoService.Get(model.Relleno);
                }

                if (model.RutaRecoleccion > 0)
                {
                    nuevaOTR.RutaRecoleccion = rutaService.Get(model.RutaRecoleccion.Value);
                }

                if (model.Cuadrilla > 0)
                {
                    nuevaOTR.Cuadrilla = cuadrillaService.Get(model.Cuadrilla);
                }
                
                if (model.Cliente > 0)
                {
                    nuevaOTR.Cliente = clienteService.Get(model.Cliente);
                }

                if (model.Contrato > 0)
                {
                    nuevaOTR.Contrato = contratoService.Get(model.Contrato);
                }

                if (model.Equipo > 0)
                {
                    Equipo equipo = equipoService.Get(model.Equipo);
                    if (equipo == null)
                    {
                        ModelState.AddModelError("NombreEquipo", "Equipo no válido");
                        return Json(null); //return View(model);
                    }
                    nuevaOTR.Equipo = equipo;
                }
                else
                {
                    ModelState.AddModelError("NombreEquipo", "Equipo no válido");
                    return Json(null); //return View(model);
                }

              //  nuevaOTR.ListaOTRHijas = new List<OTR>();
                nuevaOTR.ListaViajesOTR = new List<ViajeOTR>();

                //if (model.OTRMadre)
                //{
                //    var arrayHijas = model.OTRHijas.Split(',');
                //    foreach (var item in arrayHijas)
                //    {
                //        if (item != String.Empty)
                //        {
                //            var OTR = otrService.Get(Convert.ToInt64(item));
                //            nuevaOTR.ListaOTRHijas.Add(OTR);
                //        }
                //    }
                //}

                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Compania", nuevaOTR.Compania);
                ConsecutivoOTR consecutivo = consecutivoService.Get(criteria);
                nuevaOTR.Consecutivo = consecutivo.Secuencia;
                consecutivo.Secuencia = consecutivo.Secuencia + 1;
                consecutivoService.Update(consecutivo);

                // Se crea el producto
                nuevaOTR = otrService.Create(nuevaOTR);

                //if (!model.OTRMadre)
                //{
                List<ViajeOTRModel> ListaRutas = JsonConvert.DeserializeObject<List<ViajeOTRModel>>(model.ListaRutas);

                foreach (var item in ListaRutas)
                {
                    ViajeOTR viaje = new ViajeOTR();
                    viaje.Compania = nuevaOTR.Compania;
                    viaje.OTR = nuevaOTR;
                    viaje.Version = 1;
                    viaje.CreateDate = DateTime.Now;
                    viaje.CreatedBy = NombreUsuarioActual();
                    viaje.Observaciones = item.Observaciones;

                    viaje.Accion = catalogoSerrvice.Get(item.Accion);
                    viaje.Contenedor = contenedorService.Get(item.Contenedor);
                    viaje.Tamano = catalogoSerrvice.Get(item.Tamano);
                    viaje.TipoEquipo = catalogoSerrvice.Get(item.TipoEquipo);
                    viaje.Viaje = productoContratoService.Get(item.Viaje);

                    viaje = viajeService.Create(viaje);

                    nuevaOTR.ListaViajesOTR.Add(viaje);

                    if (viaje.TipoEquipo.Nombre.Contains("Contenedor"))
                    {
                        ContenedorHistorial historial = new ContenedorHistorial();
                        historial.Contenedor = viaje.Contenedor;
                        historial.OTR = viaje.OTR;
                        historial.CreateDate = DateTime.Now;
                        historial.Version = 1;
                        historial.CreatedBy = NombreUsuarioActual();
                        historial.Fecha = viaje.OTR.Fecha;
                        historial.Cliente =  viaje.Accion.Nombre.Contains("retirar") ? "WPP" : viaje.OTR.Cliente.Nombre;
                        historial.Ubicacion = viaje.Accion.Nombre.Contains("retirar") ? viaje.OTR.Destino.Nombre : viaje.Viaje.Ubicacion.Descripcion;
                        contenedorHistorialService.Create(historial);
                    }
                }
                //}
                nuevaOTR = otrService.Update(nuevaOTR);

                return Json(nuevaOTR.Id, JsonRequestBehavior.AllowGet);
                //return RedirectToAction("Index", "OTR");
                //return Index();
            }
            else
            {
                Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Compania", compania);
                ConsecutivoOTR consecutivo = consecutivoService.Get(criteria);
                ViewBag.Consecutivo = consecutivo.Secuencia;
                ViewBag.ListaRellenos = rellenoMapper.GetListaModel(rellenoService.ListAll());

                return Json(null);//return View();
            }
        }

        /// <summary>
        /// Este método carga la información de la OTR que se desea editar, por medio de su id y
        /// cargar los viewbags necesarios(RellenosSanitarios) 
        /// </summary>
        /// <returns>La vista EditarOTRRollOff con el modelo de la OTR a editar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_OTR + "," + WPPConstants.ROLES_EDIT_OTR)]
        public ActionResult EditarOTRRollOff(long id)
        {
            OTR actual = otrService.Get(id);
            OTRModel otr = otrMapper.GetBoletaOTRModel(actual);
            ViewBag.ListaRellenos = rellenoMapper.GetListaModel(rellenoService.ListAll());
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
            criteria.Add("Compania", compania);
            criteria.Add("Estado", "Activo");
            criteria.Add("IsDeleted", false);
            ViewBag.ListaCuadrilla = cuadrillaMapper.GetListaModel(cuadrillaService.GetAll(criteria));
            criteria.Add("Tipo", otr.Tipo);
            ViewBag.ListaRutaRecoleccion = rutaMapper.GetListaRutaRecoleccionModel(rutaService.GetAll(criteria));

            IDictionary<string, object> criteriaCliente = new Dictionary<string, object>();
            criteriaCliente.Add("IsDeleted", false);
            List<Cliente> cliente = clienteService.GetAll(criteriaCliente).Where(s => s.CompaniaId != 0).ToList<Cliente>();
            ViewBag.ListaClientes = clienteMapper.GetListaClienteModel(cliente);

            IDictionary<string, object> criteriaContrato = new Dictionary<string, object>();
            criteriaContrato.Add("Cliente", actual.Cliente);
            criteriaContrato.Add("Compania", compania);
            criteriaContrato.Add("IsDeleted", false);

            List<ContratoModel> contratos = new List<ContratoModel>();          
            var listaContratos = contratoMapper.GetListaClienteModel(contratoService.GetAll(criteriaContrato));

            foreach (var item in listaContratos)
            {
                contratos.Add(
                    new ContratoModel
                    {
                        Id = item.Id,
                        DescripcionContrato = item.DescripcionContrato
                    }
                 );
            }

            ViewBag.ListaContratos = contratos;

            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            ViewBag.ListaTamanos = oSerializer.Serialize(catalogoMapper.GetListaCatalogoModel(catalogoSerrvice.GetByType("Tamaño")));
            ViewBag.ListaTipoEquipo = oSerializer.Serialize(catalogoMapper.GetListaCatalogoModel(catalogoSerrvice.GetByType("TipoEquipoRollOff")));
            ViewBag.ListaAcciones = oSerializer.Serialize(catalogoMapper.GetListaCatalogoModel(catalogoSerrvice.GetByType("AccionRollOff")));
            ViewBag.ListaContenedores = oSerializer.Serialize(contenedorMapper.GetListaModel(contenedorService.ListAll()));
                
            //if (otr.OTRMadre)
            //{
            //    String hijas = String.Empty;
            //    foreach (var item in actual.ListaOTRHijas)
            //    {
            //        if (hijas == String.Empty)
            //        {
            //            hijas += item.Id;
            //        }
            //        else
            //        {
            //            hijas += "," + item.Id;
            //        }
            //    }
            //    otr.OTRHijas = hijas;
            //}
            //else
            //{
                IList<ViajeOTRModel> lista = new List<ViajeOTRModel>();
                lista = viajeMapper.GetListaViajeOTRModel(actual.ListaViajesOTR);
                otr.ListaRutas = oSerializer.Serialize(lista);
            //}

            return View(otr);
        }

        /// <summary>
        /// Este método actualiza la información de la OTR seleccionado, según lo datos del modelo
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_OTR + "," + WPPConstants.ROLES_EDIT_OTR)]
        public JsonResult EditarOTRRollOff(OTRModel model)
        {
            OTR otr = otrService.Get(model.Id);
            Compania compania = otr.Compania;
            otr = otrMapper.GetBoletaOTR(model, otr);
            otr.DateLastModified = DateTime.Now;
            otr.ModifiedBy = NombreUsuarioActual();
            otr.Version++;
            otr.Compania = compania;
            var MismaRuta = true;
            
            if (model.Origen > 0)
            {
                otr.Origen = rellenoService.Get(model.Origen);
            }

            if (model.Destino > 0)
            {
                otr.Destino = rellenoService.Get(model.Destino);
            }

            if (model.Relleno > 0)
            {
                otr.Relleno = rellenoService.Get(model.Relleno);
            }

            if (model.RutaRecoleccion > 0)
            {
                if (otr.RutaRecoleccion.Id != model.RutaRecoleccion)
                {
                    MismaRuta = false;
                }
                otr.RutaRecoleccion = rutaService.Get(model.RutaRecoleccion.Value);
            }

            if (model.Cuadrilla > 0)
            {
                otr.Cuadrilla = cuadrillaService.Get(model.Cuadrilla);
            }

            if (model.Cliente > 0)
            {
                otr.Cliente = clienteService.Get(model.Cliente);
            }

            if (model.Contrato > 0)
            {
                otr.Contrato = contratoService.Get(model.Contrato);
            }

            if (model.Equipo > 0)
            {
                Equipo equipo = equipoService.Get(model.Equipo);
                if (equipo == null)
                {
                    ModelState.AddModelError("NombreEquipo", "Equipo no válido");
                    return Json(null);
                    //return View(model);
                }
                otr.Equipo = equipo;
            }
            else
            {
                ModelState.AddModelError("NombreEquipo", "Equipo no válido");
                return Json(null);
               // return View(model);
            }

           // otr.ListaOTRHijas = new List<OTR>();
           // otr.ListaViajesOTR = new List<ViajeOTR>();
            //if (model.OTRMadre == true)
            //{
            //    var arrayHijas = model.OTRHijas.Split(',');
            //    foreach (var item in arrayHijas)
            //    {
            //        if (item != String.Empty)
            //        {
            //            var OTR = otrService.Get(Convert.ToInt64(item));
            //            otr.ListaOTRHijas.Add(OTR);
            //        }
            //    }
            //}

            otr = otrService.Update(otr);

            if (MismaRuta)
            { 
                //if (!model.OTRMadre)
                //{
                    List<ViajeOTRModel> ListaRutas = JsonConvert.DeserializeObject<List<ViajeOTRModel>>(model.ListaRutas);

                    foreach (var item in otr.ListaViajesOTR)
                    {
                        var viaje= ListaRutas.First(s => s.Id == item.Id);
                        item.OTR = otr;
                        item.Version++;
                        item.Observaciones = viaje.Observaciones;

                        item.Accion = catalogoSerrvice.Get(viaje.Accion);
                        Contenedor contenedorAux = item.Contenedor;
                        item.Contenedor = contenedorService.Get(viaje.Contenedor);
                        item.Tamano = catalogoSerrvice.Get(viaje.Tamano);
                        item.TipoEquipo = catalogoSerrvice.Get(viaje.TipoEquipo);
                        item.Viaje = productoContratoService.Get(viaje.Viaje);

                        viajeService.Update(item);

                        if (item.TipoEquipo.Nombre == "Contenedor")
                        {
                            IDictionary<string, object> criteria = new Dictionary<string, object>();
                            criteria.Add("Contenedor", contenedorAux);
                            criteria.Add("OTR", item.OTR);
                            ContenedorHistorial historial = contenedorHistorialService.Get(criteria);
                            historial.DateLastModified = DateTime.Now;
                            historial.Version++;
                            historial.ModifiedBy = NombreUsuarioActual();
                            historial.Fecha = item.OTR.Fecha;
                            historial.Cliente = item.Accion.Nombre.Contains("retirar") ? "WPP" : item.OTR.Cliente.Nombre;
                            historial.Ubicacion = item.Accion.Nombre.Contains("retirar") ? item.OTR.Destino.Nombre : item.Viaje.Ubicacion.Descripcion;
                            contenedorHistorialService.Update(historial);
                        }
                        //otr.ListaViajesOTR.Add(viaje);
                    }
                //}         
            }
            else
            {
                foreach (var item in otr.ListaViajesOTR)
                {
                    viajeService.Delete(item);
                }

                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("OTR", otr);
                criteria.Add("IsDeleted", false);
                var contHistorial = contenedorHistorialService.GetAll(criteria);
                foreach (var itemHistorial in contHistorial)
                {
                    itemHistorial.IsDeleted = true;
                    itemHistorial.DeleteDate = DateTime.Now;
                    contenedorHistorialService.Delete(itemHistorial);
                }

                otr.ListaViajesOTR.Clear();

                 List<ViajeOTRModel> ListaRutas = JsonConvert.DeserializeObject<List<ViajeOTRModel>>(model.ListaRutas);

                 foreach (var item in ListaRutas)
                 {
                     ViajeOTR viaje = new ViajeOTR();
                     viaje.Compania = otr.Compania;
                     viaje.OTR = otr;
                     viaje.Version = 1;
                     viaje.CreateDate = DateTime.Now;
                     viaje.CreatedBy = NombreUsuarioActual();
                     viaje.Observaciones = item.Observaciones;

                     viaje.Accion = catalogoSerrvice.Get(item.Accion);
                     viaje.Contenedor = contenedorService.Get(item.Contenedor);
                     viaje.Tamano = catalogoSerrvice.Get(item.Tamano);
                     viaje.TipoEquipo = catalogoSerrvice.Get(item.TipoEquipo);
                     viaje.Viaje = productoContratoService.Get(item.Viaje);

                     viaje = viajeService.Create(viaje);

                     otr.ListaViajesOTR.Add(viaje);

                     if (viaje.TipoEquipo.Nombre == "Contenedor")
                     {

                         ContenedorHistorial historial = new ContenedorHistorial();
                         historial.Contenedor = viaje.Contenedor;
                         historial.OTR = viaje.OTR;
                         historial.Version++;
                         historial.CreatedBy = NombreUsuarioActual();
                         historial.Fecha = viaje.OTR.Fecha;
                         historial.Cliente = viaje.Accion.Nombre.Contains("retirar") ? "WPP" : viaje.OTR.Cliente.Nombre;
                         historial.Ubicacion = viaje.Accion.Nombre.Contains("retirar") ? viaje.OTR.Destino.Nombre : viaje.Viaje.Ubicacion.Descripcion;
                         contenedorHistorialService.Create(historial);
                     }
                 }
            }
            
            // Se actualiza la información de la OTR
            otr = otrService.Update(otr);

            return Json(otr.Id, JsonRequestBehavior.AllowGet);
            //return Index();
           // return RedirectToAction("Index", "OTR");
        }

        /// <summary>
        /// Este método carga la información de la OTR que se desea eliminar, esto según el id
        /// </summary>
        /// <returns>La vista EliminarOTRRollOff con el modelo del contrato a eliminar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_OTR )]
        public ActionResult EliminarOTRRollOff(long id)
        {
            OTR otr = otrService.Get(id);
            OTRModel model = otrMapper.GetBoletaOTRModel(otr);
           // ViewBag.ListaHijas = otr.ListaOTRHijas;
            return View(model);
        }

        /// <summary>
        /// Este método actualiza el estado IsDeleted a true, con el fin de indicar que esta OTR fue eliminado
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_OTR)]
        public ActionResult EliminarOTRRollOff(OTRModel model)
        {
            OTR otr = otrService.Get(model.Id);
            otr.IsDeleted = true;
            otr.DateLastModified = DateTime.Now;
            otr.Version++;
            otr.DeletedBy = NombreUsuarioActual();
            otr.DeleteDate = DateTime.Now;

            otrService.Update(otr);

            return Index();
        }

        /// <summary>
        /// Este método carga el modelo de la OTR que se desea obtener el detalle
        /// </summary>
        /// <returns>La vista de Detalles con el modelo de contrato que se desea consultar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_CONS_OTR + "," + WPPConstants.ROLES_ADMIN_OTR + "," + WPPConstants.ROLES_EDIT_OTR)]
        public ActionResult DetallesOTRRollOff(long id)
        {
            OTR otr = otrService.Get(id);
            OTRModel model = otrMapper.GetBoletaOTRModel(otr);
           // ViewBag.ListaHijas = otr.ListaOTRHijas;
            return View("DetallesOTRRollOff", model);
        }

        /// <summary>
        /// Este método carga la información de la OTR que se desea cerrar, esto según el id
        /// </summary>
        /// <returns>La vista CerrarOTRRollOff con el modelo de la otr a cerrar</returns>
        [HttpGet]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_CONS_OTR + "," + WPPConstants.ROLES_ADMIN_OTR + "," + WPPConstants.ROLES_EDIT_OTR)]
        public ActionResult CerrarOTRRollOff(long id)
        {
            OTR otr = otrService.Get(id);
            OTRModel model = otrMapper.GetBoletaOTRModel(otr);
           // ViewBag.ListaHijas = otr.ListaOTRHijas;
            return View(model);
        }

        /// <summary>
        /// Este método actualiza el estado a procesado, con el fin de indicar que esta OTR fue procesada 
        /// y que por lo tanto ya no se pueden realizar cambios en ella
        /// </summary>
        /// <returns>La vista Index</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_OTR + "," + WPPConstants.ROLES_EDIT_OTR)]
        public ActionResult CerrarOTRRollOff(OTRModel model)
        {
            OTR otr = otrService.Get(model.Id);
            otr.Estado = "Procesada";
            otr.DateLastModified = DateTime.Now;
            otr.Version++;
            otr.DeletedBy = NombreUsuarioActual();
            otr.DeleteDate = DateTime.Now;

            //if (otr.OTRMadre)
            //{
            //    IList<OTR> lista = otr.ListaOTRHijas;

            //    foreach (var item in lista)
            //    {
            //        item.Estado = "Procesada";
            //        otrService.Update(item);
            //    }
            //}

            otrService.Update(otr);

            //return Index();
            return RedirectToAction("Index", "OTR");
        }

        #endregion


        /// <summary>
        /// Este método carga una OTR específica segun su consecutivo de OTR
        /// </summary>
        /// <returns>Un json con el modelo de la otr que se desea consultar</returns>
        [HttpPost]
        public JsonResult CargarOTR(string id)
        {
            try
            {
                Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Compania", compania); 
                criteria.Add("Consecutivo", Convert.ToInt64(id));

                OTR otr = otrService.Get(criteria);
                OTRModel model = new OTRModel();
                model.Id = otr.Id;
                model.Consecutivo = otr.Consecutivo;
                model.Equipo = otr.Equipo.Id;
                model.NombreEquipo = otr.Equipo.Placa;
                model.DescripcionCliente = otr.Cliente.Numero + " - " + otr.Cliente.Nombre;
                model.Cliente = otr.Cliente.Id;
                model.DescripcionContrato = otr.Contrato.Numero + " - " + otr.Contrato.DescripcionContrato;
                model.Contrato = otr.Contrato.Id;
                model.Tipo = otr.Tipo;
                if (otr.RutaRecoleccion != null)
                { 
                    model.RutaRecoleccion = otr.RutaRecoleccion.Id;
                }

                EquipoModel equipo = new EquipoModel();
                equipo = equipoMapper.GetModel(otr.Equipo);

                String moneda = String.Empty, tipoCliente = String.Empty, nombreCliente = String.Empty;
                if (model.Tipo != "Roll-Off")
                { 
                    moneda = otr.Contrato.Moneda;
                    tipoCliente = otr.Contrato.Cliente.Tipo.Nombre == "Comerciales" ? "Comercial" : "Municipal";
                    nombreCliente = otr.Contrato.Cliente.Nombre;
                }
                                 
                List<Object> resultado = new List<object>();
                resultado.Add(model);
                resultado.Add(equipo);
                resultado.Add(moneda);
                resultado.Add(tipoCliente);
                resultado.Add(nombreCliente);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(null);
            }
        }

        /// <summary>
        /// Este método lista y filtra las OTRs que aún se encuentran activas que pertenecen al tipo indicado (Tipo) y a la compañía actual
        /// </summary>
        /// <returns>json con la lista de OTR que cumplen con las condiciones especificadas</returns>
        [HttpPost]
        public JsonResult CargarListaOTR(string tipo)
        {
            try
            {
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                var numCompania = Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0;
                Compania compania = companiaService.Get(numCompania);
                criteria.Add("Compania", compania);
                criteria.Add("Estado", "Activo");
                criteria.Add("IsDeleted", false);
                if (tipo != "Servicios Internos")
                { 
                    criteria.Add("Tipo", tipo);                
                }

                IList<OTRModel> ListaOTR = new List<OTRModel>();
                ListaOTR = otrMapper.GetListaBoletaModel(otrService.GetAll(criteria));
                
                return Json(ListaOTR, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(null);
            }
        }

        /// <summary>
        /// Este método lista y filtra el listado de registros de boletas de báscula mostrados en el index
        /// </summary>
        /// <returns>Vista Index con el modelo IPageList(Bascula)</returns>
        public ActionResult Buscar(string sortOrder, string currentOTR, string currentEquipo, string currentEstado, string currentRutaRecoleccion, string currentTipo,
                 string searchStringOTR, string searchStringEquipo, string searchStringEstado, string searchStringRutaRecoleccion, string searchStringTipo, int? searchStringFilas, int? page)
        {
            ViewBag.CurrentSort = sortOrder;

            if (sortOrder == "OTRAsc")
                ViewBag.OTRSort = "OTRDesc";
            else
                ViewBag.OTRSort = "OTRAsc";
            
            if (sortOrder == "EquipoAsc")
                ViewBag.EquipoSort = "EquipoDesc";
            else
                ViewBag.EquipoSort = "EquipoAsc";

            if (sortOrder == "EstadoAsc")
                ViewBag.EstadoSort = "EstadoDesc";
            else
                ViewBag.EstadoSort = "EstadoAsc";

            if (sortOrder == "RutaRecoleccionAsc")
                ViewBag.RutaRecoleccionSort = "RutaRecoleccionDesc";
            else
                ViewBag.RutaRecoleccionSort = "RutaRecoleccionAsc";

            if (sortOrder == "TipoAsc")
                ViewBag.TipoSort = "TipoDesc";
            else
                ViewBag.TipoSort = "TipoAsc";


            if (searchStringOTR != null || searchStringRutaRecoleccion != null || searchStringEquipo != null || searchStringEstado != null || searchStringTipo != null)
                page = 1;
            else
            {
                searchStringOTR = currentOTR;
                searchStringRutaRecoleccion = currentRutaRecoleccion;
                searchStringEquipo = currentEquipo;
                searchStringEstado = currentEstado;
                searchStringTipo = currentTipo;
            }

            ViewBag.CurrentOTR = searchStringOTR;
            ViewBag.CurrentRutaRecoleccion = searchStringRutaRecoleccion;
            ViewBag.CurrentEquipo = searchStringEquipo;
            ViewBag.CurrentEstado = searchStringEstado;
            ViewBag.CurrentTipo = searchStringTipo;

            var compania = Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0;
            var filas = searchStringFilas == null ? 10 : Convert.ToInt32(searchStringFilas);
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var pagina = otrService.PagingSearch(searchStringOTR, searchStringEquipo, searchStringEstado, searchStringRutaRecoleccion, searchStringTipo, pageNumber, filas, sortOrder, compania);

            return View("Index", pagina);
        }

    }
}
