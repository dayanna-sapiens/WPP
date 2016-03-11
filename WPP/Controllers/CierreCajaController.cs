using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WPP.Datos.CierreCaja;
using WPP.Entities.Objects.Generales;
using WPP.Entities.Objects.ModuloBascula;
using WPP.Entities.Objects.ModuloCierreCaja;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Helpers;
using WPP.Mapper;
using WPP.Mapper.ModuloBascula;
using WPP.Mapper.ModuloCierreCaja;
using WPP.Mapper.ModuloContratos;
using WPP.Model;
using WPP.Model.ModuloBascula;
using WPP.Model.ModuloCierreCaja;
using WPP.Model.ModuloContratos;
using WPP.Security;
using WPP.Service.Generales;
using WPP.Service.ModuloBascula;
using WPP.Service.ModuloCierreCaja;
using WPP.Service.ModuloContratos;

namespace WPP.Controllers
{
    public class CierreCajaController : BaseController
    {
        private IEfectivoCierreCajaService efectivoService;
        private ICatalogoService catalogoService;
        private ICierreCajaService cierreService;
        private ICompaniaService companiaService;
        private IPagoBasculaService pagoService;
        private IConsecutivoCierreCajaService consecutivoCierreCajaService;
        private CierreCajaMapper cierreMapper;
        private PagoBaculaMapper pagoMapper;
        private IBasculaService basculaService;
        private BasculaMapper basculaMapper;
        private ProductoContratoMapper productoMapper;
        private CompaniaMapper companiaMapper;
        
        public CierreCajaController(IEfectivoCierreCajaService efectivo, ICatalogoService catalogo, ICierreCajaService cierre, ICompaniaService compania, IPagoBasculaService pago, IBasculaService bascula, IConsecutivoCierreCajaService consecutivo)
        {
            try
            {
                this.efectivoService = efectivo;
                this.catalogoService = catalogo;
                this.cierreService = cierre;
                this.companiaService = compania;
                this.pagoService = pago;
                this.basculaService = bascula;
                this.consecutivoCierreCajaService = consecutivo;
                basculaMapper = new BasculaMapper();
                pagoMapper = new PagoBaculaMapper();
                cierreMapper = new CierreCajaMapper();
                productoMapper = new ProductoContratoMapper();
                companiaMapper = new CompaniaMapper();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        #region CIERRE CAJA

        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CIERRE_CAJA)]
        public ActionResult CierreCaja()
        {           
            return View();
        }

        /// <summary>
        /// Este método lista lista las denominaciones segun el tipo de moneda
        /// </summary>
        /// <returns>json con la lista de denominaciones </returns>
        [HttpPost]
        public JsonResult CargarDenominacionesMoneda(String Moneda)
        {
            try
            {
                String denominacion = Moneda == "Colones" ? "DenominacionColon" : "DenominacionDolar";
                IList<Catalogo> catalogo = catalogoService.GetByType(denominacion);

                return Json(catalogo, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(null);
            }
        }


        /// <summary>
        /// Este método la lista de pagos en transferencias, tarjetas y la sumatoria de lo pagos que aun no han sido tomados en cuenta para el cierre de caja
        /// </summary>
        /// <returns>json con la lista de denominaciones </returns>
        [HttpPost]
        public JsonResult CargarValoresTotales(String Moneda)
        {
            try
            {
                Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);

                // Se obtienen los pagos que aun no han sido cerrados
                IList<PagoBascula> Pagos = new List<PagoBascula>();
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("CierreCaja", false);
                criteria.Add("Moneda", Moneda);
                Pagos = pagoService.GetAll(criteria);

                // Se filtran para que los pagos no pertenezcan a boletas borradas o anuladas y se separan segun la forma de pago 
                Pagos = Pagos.Where(s => s.Boleta.Estado != "Pendiente" && s.Boleta.IsDeleted == false && s.Boleta.Compania == compania).ToList();
                IList<PagoBascula> PagosTarjeta = Pagos.Where(s => s.FormaPago == "Tarjeta").ToList();
                IList<PagoBascula> PagosTransferencia = Pagos.Where(s => s.FormaPago == "Transferencia").ToList();
                IList<PagoBascula> PagosEfectivo = Pagos.Where(s => s.FormaPago == "Efectivo").ToList();
                
                // Se saca la sumatoria segun la forma de pago
                CierreCajaModel cierre = new CierreCajaModel();
                var efectivo = PagosEfectivo.Sum(s => s.Monto);
                cierre.Tarjetas = PagosTarjeta.Sum(s => s.Monto);
                cierre.Transferencia = PagosTransferencia.Sum(s => s.Monto);
                cierre.BalanceApertura = PagosTarjeta.Sum(s => s.Monto) + PagosTransferencia.Sum(s => s.Monto) + PagosEfectivo.Sum(s => s.Monto);
                cierre.BalanceCierre = cierre.BalanceApertura;

                // Lista de Pagos con Tarjetas
                IList<PagoBasculaModel> ListaPagosTarjeta = new List<PagoBasculaModel>();
                ListaPagosTarjeta = pagoMapper.GetListaPagoBasculaModel(PagosTarjeta);

                // Lista de Pagos con Transferencia
                IList<PagoBasculaModel> ListaPagosTransferencia = new List<PagoBasculaModel>();
                ListaPagosTransferencia = pagoMapper.GetListaPagoBasculaModel(PagosTransferencia);

                // Lista de todos los pagos efectuados
                IList<PagoBasculaModel> ListaPagos = new List<PagoBasculaModel>();
                ListaPagos = pagoMapper.GetListaPagoBasculaModel(Pagos);

               // Lista de boletas de credito
                IList<BasculaModel> ListaCreditos = new List<BasculaModel>();
          
                criteria = new Dictionary<string, object>();
                criteria.Add("IsDeleted", false);
                criteria.Add("Compania", compania);
                criteria.Add("CierreCredito", false);

                var Creditos = basculaService.GetAll(criteria);
                Creditos = Creditos.Where(s => s.Estado != "Pendiente" && s.NumeroRecibo == null && s.Fecha <= DateTime.Now.AddDays(1)).ToList();
                ListaCreditos = basculaMapper.GetListaBoletaModel(Creditos);
                
                /////////////////////////////////////////////

                // Se forma la lista que se desea retornar a la vista con la informacion necesaria
                List<Object> Listas = new List<Object>();
                Listas.Add(cierre);
                Listas.Add(ListaPagosTarjeta);
                Listas.Add(ListaPagosTransferencia);
                Listas.Add(ListaPagos);
                Listas.Add(ListaCreditos);

                return Json(Listas, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(null);
            }
        }

        /// <summary>
        /// Este método guarda la información del cierre de caja y 
        /// actualiza los pagos de bascula indicando que ya ese pago fue tomado en cuenta para el cierre de caja
        /// </summary>
        /// <returns>La vista CierreCaja</returns>
        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CIERRE_CAJA)]
        public ActionResult CierreCaja1(CierreCajaModel model)
        {
            if (ModelState.IsValid)
            {
                // Se crea el cierre de caja
                CierreCaja cierre = new CierreCaja();
                cierre = cierreMapper.GetEntity(model, cierre);
                cierre.Version = 1;
                cierre.CreateDate = DateTime.Now;
                cierre.CreatedBy = NombreUsuarioActual();
                cierre.Compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                cierre.Diferencia = model.AjusteCaja.HasValue ? model.AjusteCaja.Value : 0;
                 
                cierreService.Create(cierre);

                cierre.ListaPagoEfectivo = new List<EfectivoCierreCaja>();
                List<EfectivoCierreCajaModel> ListaEfectivo = JsonConvert.DeserializeObject<List<EfectivoCierreCajaModel>>(model.PagoEfectivo);

                // Se guarda la lista de pagos en efectivo rgistrados en el cierre de caja
                foreach (var item in ListaEfectivo)
                {
                    EfectivoCierreCaja efectivo = new EfectivoCierreCaja();
                    efectivo.CierreCaja = cierre;
                    efectivo.Version = 1;
                    efectivo.CreateDate = DateTime.Now;
                    efectivo.CreatedBy = NombreUsuarioActual();                                        
                    efectivo.Denominacion = catalogoService.Get(item.Denominacion);
                    efectivo = efectivoService.Create(efectivo);
                    cierre.ListaPagoEfectivo.Add(efectivo);
                }


                // Se actualizan los pagos de bascula indicando que ya los pagos fueron cerrados
                List<PagoBasculaModel> PagosBascula = JsonConvert.DeserializeObject<List<PagoBasculaModel>>(model.PagosBascula);
                cierre.Pagos = new List<PagoBascula>();

                foreach (var item in PagosBascula)
                {
                    PagoBascula pago = new PagoBascula();
                    pago = pagoService.Get(item.Id);
                    pago.Version ++;
                    pago.DateLastModified = DateTime.Now;
                    pago.ModifiedBy = NombreUsuarioActual();
                    pago.CierreCaja = true;
                    pago.Cierre = new CierreCaja();
                    pago.Cierre = cierre;
                    
                    pagoService.Update(pago);
                    cierre.Pagos.Add(pago);
                }

                cierreService.Update(cierre);

                return View("CierreCaja", cierre);
            }
            else
            {
                return View("CierreCaja");
            }
        }

        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CIERRE_CAJA)]
        public JsonResult CierreCaja(CierreCajaModel model)
        {
            if (ModelState.IsValid)
            {
                // Se crea el cierre de caja
                CierreCaja cierre = new CierreCaja();
                cierre = cierreMapper.GetEntity(model, cierre);
                cierre.Version = 1;
                cierre.CreateDate = DateTime.Now;
                cierre.CreatedBy = NombreUsuarioActual();
                cierre.Compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                cierre.Diferencia = model.AjusteCaja.HasValue ? model.AjusteCaja.Value : 0;

                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Compania", cierre.Compania);

                ConsecutivoCierreCaja consecutivo = consecutivoCierreCajaService.Get(criteria);
                cierre.Consecutivo = consecutivo.Secuencia;

                cierreService.Create(cierre);

                cierre.ListaPagoEfectivo = new List<EfectivoCierreCaja>();
                List<EfectivoCierreCajaModel> ListaEfectivo = JsonConvert.DeserializeObject<List<EfectivoCierreCajaModel>>(model.PagoEfectivo);

                // Se guarda la lista de pagos en efectivo rgistrados en el cierre de caja
                foreach (var item in ListaEfectivo)
                {
                    EfectivoCierreCaja efectivo = new EfectivoCierreCaja();
                    efectivo.CierreCaja = cierre;
                    efectivo.Version = 1;
                    efectivo.CreateDate = DateTime.Now;
                    efectivo.CreatedBy = NombreUsuarioActual();

                    efectivo.Denominacion = catalogoService.Get(item.Denominacion);

                    efectivo = efectivoService.Create(efectivo);

                    cierre.ListaPagoEfectivo.Add(efectivo);
                }
                
                // Se actualizan los pagos de bascula indicando que ya los pagos fueron cerrados
                List<PagoBasculaModel> PagosBascula = JsonConvert.DeserializeObject<List<PagoBasculaModel>>(model.PagosBascula);
                cierre.Pagos = new List<PagoBascula>();

                foreach (var item in PagosBascula)
                {
                    PagoBascula pago = new PagoBascula();
                    pago = pagoService.Get(item.Id);
                    pago.Version++;
                    pago.DateLastModified = DateTime.Now;
                    pago.ModifiedBy = NombreUsuarioActual();
                    pago.CierreCaja = true;
                    pago.Cierre = new CierreCaja();
                    pago.Cierre = cierre;

                    pagoService.Update(pago);
                    cierre.Pagos.Add(pago);                   
                }

                List<BasculaModel> ListaCreditos = JsonConvert.DeserializeObject<List<BasculaModel>>(model.ListaCreditos);
                cierre.Creditos = new List<Bascula>();

                foreach (var item in ListaCreditos)
                {
                    Bascula bascula = new Bascula();
                    bascula = basculaService.Get(item.Id);
                    bascula.Version++;
                    bascula.DateLastModified = DateTime.Now;
                    bascula.ModifiedBy = NombreUsuarioActual();
                    bascula.CierreCredito = true;
                    bascula.CierreCaja = new CierreCaja();
                    bascula.CierreCaja = cierre;

                    basculaService.Update(bascula);
                    
                    cierre.Creditos.Add(bascula);
                }

                cierreService.Update(cierre);

                consecutivo.Secuencia++;
                consecutivoCierreCajaService.Update(consecutivo);

                return Json(cierre.Id);
            }
            else
            {
                return Json(null);
            }
        }

        public void ReporteCierreCaja(long id)
        {
            CierreCaja cierre = new CierreCaja();
            cierre = cierreService.Get(id);

            //ReportDocument reporte = new ReportDocument();
            WPP.Datos.CierreCaja.ds_CierreCaja dsCierre = new WPP.Datos.CierreCaja.ds_CierreCaja();
            ds_PagosTarjeta dsPagos = new ds_PagosTarjeta();

            var dtReporte = dsCierre.Tables["Reporte"];
            var dtDatos = dsCierre.Tables["Datos"];

            var dtTarjetas = dsPagos.Tables["Datos"];

            // Rows DataTable Reporte
            DataRow row = dtReporte.NewRow();
            row["Compania"] = cierre.Compania.Nombre;
            row["FechaActual"] = DateTime.Now;
            row["Moneda"] = cierre.Moneda;
            row["Cajero"] = cierre.CreatedBy;
            row["Total"] = cierre.ConteoTotal;
            row["MontoEfectivo"] = cierre.Efectivo;
            row["MontoTarjeta"] = cierre.Tarjetas;
            row["MontoTransferencia"] = cierre.Transferencia;
            row["Sitio"] = cierre.Compania.Nombre;
            row["Consecutivo"] = cierre.Consecutivo;
            dtReporte.Rows.Add(row);

            foreach (var item in cierre.Pagos)
            {
                DataRow dato = dtDatos.NewRow();
                dato["Boleta"] = item.Boleta.Boleta;
                dato["Recibo"] = item.Boleta.NumeroRecibo;
                dato["Fecha"] = item.CreateDate;
                dato["Nombre"] = item.Boleta.NombreCliente;
                dato["Cargo"] = item.Monto;
                dato["Abono"] = item.Monto;
                dato["Estado"] = item.Boleta.Estado;
                dtDatos.Rows.Add(dato);

                if(item.FormaPago == "Tarjeta")
                {
                    DataRow datosTarjeta = dtTarjetas.NewRow();
                    datosTarjeta["Cliente"] = item.Boleta.NombreCliente;
                    datosTarjeta["Boleta"] = item.Boleta.Boleta;
                    datosTarjeta["Fecha"] = item.CreateDate;
                    datosTarjeta["Monto"] = item.Monto;
                    dtTarjetas.Rows.Add(datosTarjeta);
                }
            }

            ReportDocument rd = new ReportDocument();
            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reportes//CierreCaja//rpt_CierreCaja.rpt";
            rd.Load(strRptPath);

            rd.SetDataSource(dsCierre);
            rd.Subreports[0].SetDataSource(dsPagos);
            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "CierreCaja");
        }


        #endregion

        #region REIMPRESION DE RECIBOS

        public ActionResult Reimpresion()
        {
            return View();
        }


        [HttpPost]
        [AccessDeniedAuthorizeAttribute(Roles = WPPConstants.ROL_SUPER_USUARIO + "," + WPPConstants.ROLES_ADMIN_CIERRE_CAJA_REIMPRESION)]
        public JsonResult CargarInformacionRecibo(string recibo)
        {
            try
            {
                Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                IDictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("NumeroRecibo", recibo);
                criteria.Add("Compania", compania);

                Bascula bascula = new Bascula();
                bascula = basculaService.Get(criteria);
                BasculaModel model = basculaMapper.GetBoletaBasculaModel(bascula);
                model.Moneda = bascula.Contrato.Moneda;

                ProductoContratoModel productoModel = productoMapper.GetProductoContratoModel(bascula.Producto);
                String Categoria = bascula.Producto.Producto.Categoria.Tipo;
                CompaniaModel companiaModel = companiaMapper.GetCompaniaModel(compania);

                Double SumatoriaPagos = 0;
                foreach (var item in bascula.ListaPagos)
                {
                    if (item.Moneda != model.Moneda)
                    {
                        if (item.Moneda == "Colones")
                        {
                            SumatoriaPagos += (item.Monto / item.TipoCambio);
                        }
                        else
                        {
                            SumatoriaPagos += (item.Monto * item.TipoCambio);
                        }
                    }
                    else
                    {
                        SumatoriaPagos += item.Monto;
                    }
                }

                List<object> resultados = new List<object>();
                resultados.Add(model);
                resultados.Add(productoModel);
                resultados.Add(companiaModel);
                resultados.Add(SumatoriaPagos);
                resultados.Add(Categoria);

                return Json(resultados, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(null);
            }
        }

        #endregion
    }

}
