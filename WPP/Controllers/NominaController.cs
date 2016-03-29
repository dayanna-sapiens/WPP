using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using WPP.Service.ModuloNomina;
using WPP.Service.ModuloContratos;
using WPP.Entities.Objects.ModuloNomina;
using WPP.Service.ModuloOperacionRecoleccion;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;
using Excel;
using System.Data;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Service.Generales;
using WPP.Service.ModuloBascula;
using WPP.Service.ModuloBoletaManual;
using WPP.Mapper.ModuloNomina;
using WPP.Model.ModuloNomina;
using Newtonsoft.Json;
using WPP.Model;

namespace WPP.Controllers
{
    public class NominaController : BaseController
    {
        IPlanillaService planillaService;
        IItemNominaService itemService;
        ICompaniaService companiaService;
        IEmpleadoRecoleccionService empleadoService;
        ICostoRutaRecoleccionService costoRutaService;
        IOTRService otrService;
        ICostoHoraService costoHoraService;
        IBasculaService basculaService;
        IBoletaManualService boletaManualService;
        IDiasFestivosService feriadosService;
        PlanillaMapper nominaMapper;
        IConsecutivoNominaService consecutivoService;

        public NominaController(IPlanillaService nomina, IItemNominaService item, ICompaniaService compania, IEmpleadoRecoleccionService empleado, ICostoRutaRecoleccionService costo,
            IOTRService otr, ICostoHoraService costoHora, IBasculaService bascula, IBoletaManualService boleta, IDiasFestivosService feriados, IConsecutivoNominaService consecutivo)
        {
            planillaService = nomina;
            itemService = item;
            companiaService = compania;
            empleadoService = empleado;
            costoRutaService = costo;
            otrService = otr;
            costoHoraService = costoHora;
            basculaService = bascula;
            boletaManualService = boleta;
            feriadosService = feriados;
            consecutivoService = consecutivo;
            nominaMapper = new PlanillaMapper();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Cargar()
        {
            ViewBag.Error = String.Empty;
            return View();
        }

   
        public ActionResult NominaRecolector(PlanillaModel model)
        {
            // Se divide los items obtenidos por dias y se retorna el modelo
            var DetallesNomina = (List<ItemNomina>)Session["Detalles"];
            ViewBag.ItemsLunes = DetallesNomina.Where(s => s.Fecha.DayOfWeek == DayOfWeek.Monday).ToList();
            ViewBag.ItemsMartes = DetallesNomina.Where(s => s.Fecha.DayOfWeek == DayOfWeek.Tuesday).ToList();
            ViewBag.ItemsMiercoles = DetallesNomina.Where(s => s.Fecha.DayOfWeek == DayOfWeek.Wednesday).ToList();
            ViewBag.ItemsJueves = DetallesNomina.Where(s => s.Fecha.DayOfWeek == DayOfWeek.Thursday).ToList();
            ViewBag.ItemsViernes = DetallesNomina.Where(s => s.Fecha.DayOfWeek == DayOfWeek.Friday).ToList();
            ViewBag.ItemsSabado = DetallesNomina.Where(s => s.Fecha.DayOfWeek == DayOfWeek.Saturday).ToList();
            return View(model);
        }
        public ActionResult NominaChofer(PlanillaModel model)
        {
            // Se divide los items obtenidos por dias y se retorna el modelo
            var DetallesNomina = (List<ItemNomina>)Session["Detalles"];
            ViewBag.ItemsLunes = DetallesNomina.Where(s => s.Fecha.DayOfWeek == DayOfWeek.Monday).ToList();
            ViewBag.ItemsMartes = DetallesNomina.Where(s => s.Fecha.DayOfWeek == DayOfWeek.Tuesday).ToList();
            ViewBag.ItemsMiercoles = DetallesNomina.Where(s => s.Fecha.DayOfWeek == DayOfWeek.Wednesday).ToList();
            ViewBag.ItemsJueves = DetallesNomina.Where(s => s.Fecha.DayOfWeek == DayOfWeek.Thursday).ToList();
            ViewBag.ItemsViernes = DetallesNomina.Where(s => s.Fecha.DayOfWeek == DayOfWeek.Friday).ToList();
            ViewBag.ItemsSabado = DetallesNomina.Where(s => s.Fecha.DayOfWeek == DayOfWeek.Saturday).ToList();
            return View(model);
        }
        public ActionResult NominaTaller(PlanillaModel model)
        {
            // Se divide los items obtenidos por dias y se retorna el modelo
            var DetallesNomina = (List<ItemNomina>)Session["Detalles"];
            ViewBag.ItemsLunes = DetallesNomina.Where(s => s.Fecha.DayOfWeek == DayOfWeek.Monday).ToList();
            ViewBag.ItemsMartes = DetallesNomina.Where(s => s.Fecha.DayOfWeek == DayOfWeek.Tuesday).ToList();
            ViewBag.ItemsMiercoles = DetallesNomina.Where(s => s.Fecha.DayOfWeek == DayOfWeek.Wednesday).ToList();
            ViewBag.ItemsJueves = DetallesNomina.Where(s => s.Fecha.DayOfWeek == DayOfWeek.Thursday).ToList();
            ViewBag.ItemsViernes = DetallesNomina.Where(s => s.Fecha.DayOfWeek == DayOfWeek.Friday).ToList();
            ViewBag.ItemsSabado = DetallesNomina.Where(s => s.Fecha.DayOfWeek == DayOfWeek.Saturday).ToList();
            return View(model);
        }

        public JsonResult GuardarNominaRecolector(PlanillaModel model)
        {
            try
            {
                List<ItemNominaModel> detalles = JsonConvert.DeserializeObject<List<ItemNominaModel>>(Request.Form["ListaDetallesModel"]);
                

                // Se crea la nomina 
                Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                Planilla nomina = new Planilla();
                nomina.Descripcion = model.Descripcion;
                nomina.Version = 1;
                nomina.CreateDate = DateTime.Now;
                nomina.IsDeleted = false;
                nomina.CreatedBy = ObtenerUsuarioActual().Nombre;
                nomina.Compania = compania;
                nomina.Estado = "Generada";
                nomina.Tipo = "Recolector";
                nomina.DetallesNomina = new List<ItemNomina>();
                nomina = planillaService.Create(nomina);

                foreach (var item in detalles)
                {
                    ItemNomina detalle = new ItemNomina();
                    detalle.Nomina = nomina;
                    detalle.IsDeleted = false;
                    detalle.CreateDate = DateTime.Now;
                    detalle.CreatedBy = ObtenerUsuarioActual().Nombre;
                    detalle.Version = 1;
                    detalle.Compensacion = item.Compensacion;
                    detalle.Entrada = item.Entrada;
                    detalle.Salida = item.Salida;
                    detalle.Toneladas = item.Toneladas;
                    detalle.Total = item.Total;
                    detalle.TotalHoras = item.TotalHoras;
                    detalle.HorasExtra = item.HorasExtra;
                    detalle.HorasOrdinarias = item.HorasOrdinarias;
                    detalle.MontoExtra = item.MontoExtra;
                    detalle.MontoOrdinario = item.MontoOrdinario;
                    detalle.Fecha = Convert.ToDateTime(item.Fecha);
                    detalle.Empleado = empleadoService.Get(item.Empleado);

                    itemService.Create(detalle);

                    nomina.DetallesNomina.Add(detalle);
                }

                planillaService.Update(nomina);
                return Json(nomina.Id);
            }
            catch(Exception ex)
            {
                return Json(null);
            }
        }

        public JsonResult GuardarNominaChofer(PlanillaModel model)
        {
            try
            {
                List<ItemNominaModel> detalles = JsonConvert.DeserializeObject<List<ItemNominaModel>>(Request.Form["ListaDetallesModel"]);


                // Se crea la nomina 
                Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                Planilla nomina = new Planilla();
                nomina.Descripcion = model.Descripcion;
                nomina.Version = 1;
                nomina.CreateDate = DateTime.Now;
                nomina.IsDeleted = false;
                nomina.CreatedBy = ObtenerUsuarioActual().Nombre;
                nomina.Compania = compania;
                nomina.Estado = "Generada";
                nomina.Tipo = "Chofer";
                nomina.DetallesNomina = new List<ItemNomina>();
                nomina = planillaService.Create(nomina);

                foreach (var item in detalles)
                {
                    ItemNomina detalle = new ItemNomina();
                    detalle.Nomina = nomina;
                    detalle.IsDeleted = false;
                    detalle.CreateDate = DateTime.Now;
                    detalle.CreatedBy = ObtenerUsuarioActual().Nombre;
                    detalle.Version = 1;
                    detalle.Compensacion = 0;
                    detalle.Entrada = item.Entrada;
                    detalle.Salida = item.Salida;
                    detalle.Toneladas = 0;
                    detalle.Total = item.Total;
                    detalle.TotalHoras = item.TotalHoras;
                    detalle.HorasExtra = item.HorasExtra;
                    detalle.HorasOrdinarias = item.HorasOrdinarias;
                    detalle.MontoExtra = item.MontoExtra;
                    detalle.MontoOrdinario = item.MontoOrdinario;
                    detalle.Fecha = Convert.ToDateTime(item.Fecha);
                    detalle.Empleado = empleadoService.Get(item.Empleado);

                    itemService.Create(detalle);

                    nomina.DetallesNomina.Add(detalle);
                }

                planillaService.Update(nomina);
                return Json(nomina.Id);
            }
            catch (Exception ex)
            {
                return Json(null);
            }
        }


        public JsonResult GuardarNominaTaller(PlanillaModel model)
        {
            try
            {
                List<ItemNominaModel> detalles = JsonConvert.DeserializeObject<List<ItemNominaModel>>(Request.Form["ListaDetallesModel"]);


                // Se crea la nomina 
                Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                Planilla nomina = new Planilla();
                nomina.Descripcion = model.Descripcion;
                nomina.Version = 1;
                nomina.CreateDate = DateTime.Now;
                nomina.IsDeleted = false;
                nomina.CreatedBy = ObtenerUsuarioActual().Nombre;
                nomina.Compania = compania;
                nomina.Estado = "Generada";
                nomina.Tipo = "Taller";
                nomina.DetallesNomina = new List<ItemNomina>();
                nomina = planillaService.Create(nomina);

                foreach (var item in detalles)
                {
                    ItemNomina detalle = new ItemNomina();
                    detalle.Nomina = nomina;
                    detalle.IsDeleted = false;
                    detalle.CreateDate = DateTime.Now;
                    detalle.CreatedBy = ObtenerUsuarioActual().Nombre;
                    detalle.Version = 1;
                    detalle.Compensacion = 0;
                    detalle.Entrada = item.Entrada;
                    detalle.Salida = item.Salida;
                    detalle.Toneladas = 0;
                    detalle.Total = item.Total;
                    detalle.TotalHoras = item.TotalHoras;
                    detalle.HorasExtra = item.HorasExtra;
                    detalle.HorasOrdinarias = item.HorasOrdinarias;
                    detalle.MontoExtra = item.MontoExtra;
                    detalle.MontoOrdinario = item.MontoOrdinario;
                    detalle.Fecha = Convert.ToDateTime(item.Fecha);
                    detalle.Empleado = empleadoService.Get(item.Empleado);

                    itemService.Create(detalle);

                    nomina.DetallesNomina.Add(detalle);
                }

                planillaService.Update(nomina);
                return Json(nomina.Id);
            }
            catch (Exception ex)
            {
                return Json(null);
            }
        }

        [HttpPost]
        public ActionResult Cargar(HttpPostedFileBase excelfile)
        {
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "Por favor seleccione el archivo excel que desea cargar";
            }
            else
            {
                if(excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    string path = Server.MapPath("~/NominaDocs/" + excelfile.FileName);
                    if(System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    excelfile.SaveAs(path);
                    
                   //Se lee el archivo
                    FileStream stream = new FileStream(path, FileMode.Open);
                    IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                    IList<ItemNomina> DetallesNomina = new List<ItemNomina>();

                    DataSet result = excelReader.AsDataSet();

                    //Se recorre las pestañas del excel (Cada uno de los dias)
                    foreach (DataTable table in result.Tables)
                    {
                        for (int row = 1; row < table.Rows.Count; row++)
                        {

                            ItemNomina item = new ItemNomina();
                            string codigoEmpleado = table.Rows[row][0].ToString();
                            var fecha = table.Rows[row][2].ToString();
                            if(fecha != String.Empty)
                            {
                                item.Fecha = Convert.ToDateTime(fecha);
                                item.Entrada = table.Rows[row][3].ToString();
                                item.Salida = table.Rows[row][4].ToString();                            
                            }

                            IDictionary<string, object> criteria = new Dictionary<string, object>();
                            criteria.Add("Codigo", codigoEmpleado);
                            criteria.Add("IsDeleted", false);
                            EmpleadoRecoleccion empleado = empleadoService.Get(criteria);
                            if(empleado != null)
                            {
                                item.Empleado = empleado;
                            }
                            DetallesNomina.Add(item);
                        }
                    }

                    excelReader.Close();

                    String tipo = Request.Form["ddlTipo"] != null ? Request.Form["ddlTipo"] : "Recolector";
                    PlanillaModel model = new PlanillaModel();
                    String Action = String.Empty;
                    switch (tipo)
                    {
                        case "Recolector":
                            model = CargarNominaRecolector(DetallesNomina);
                            Session["Detalles"] = DetallesNomina;
                            Action = "NominaRecolector";
                            break;
                        case "Chofer":
                             model = CargarNominaRecolector(DetallesNomina);
                            Session["Detalles"] = DetallesNomina;
                            Action = "NominaChofer";
                          //  return RedirectToAction("NominaRecolector", model);
                            break;
                        case "Taller":
                             model = CargarNominaRecolector(DetallesNomina);
                            Session["Detalles"] = DetallesNomina;
                            Action = "NominaTaller";
                          //  return RedirectToAction("NominaRecolector", model);
                            break;
                    }
                    return RedirectToAction(Action, model);
                }
                else
                {
                    ViewBag.Error = "El archivo a importar no es permitido";
                    RedirectToAction("Cargar");
                }
            }

            return  View();
        }


        private PlanillaModel CargarNominaRecolector(IList<ItemNomina> DetallesNomina)
        {
            PlanillaModel model = new PlanillaModel();
            if (DetallesNomina.Count > 0)
            {
                Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                Planilla nomina = new Planilla();
                nomina.DetallesNomina = new List<ItemNomina>();
                nomina.Descripcion = Request.Form["txtDescripcion"] != null ? Request.Form["txtDescripcion"] : "Planilla";

                foreach (var item in DetallesNomina)
                {
                    //Obtener OTR
                    IDictionary<string, object> criteriaDetails = new Dictionary<string, object>();
                    criteriaDetails.Add("Fecha", item.Fecha);
                    criteriaDetails.Add("IsDeleted", false);
                    var OTRs = otrService.GetAll(criteriaDetails);
                    double toneladas = 0;
                    foreach (var otr in OTRs)
                    {
                        var Cuadrilla = otr.Cuadrilla.ListaEmpleados.Where(s => s.Id == item.Empleado.Id && s.Puesto != "Chofer").ToList();
                        if (Cuadrilla.Count > 0)
                        {
                            // se obtiene el costo de la ruta de recoleccion de la OTR
                            IDictionary<string, object> criteriaRuta = new Dictionary<string, object>();
                            criteriaRuta.Add("RutaRecoleccion", otr.RutaRecoleccion);
                            criteriaRuta.Add("IsDeleted", false);
                            var costoRuta = costoRutaService.Get(criteriaRuta);
                            double costo = costoRuta == null ? 0 : costoRuta.Costo;

                            // Se obtiene las toneladas 
                            criteriaRuta = new Dictionary<string, object>();
                            criteriaRuta.Add("OTR", otr);
                            criteriaRuta.Add("IsDeleted", false);
                            var boleta = basculaService.Get(criteriaRuta);
                            double peso = 0;
                            if (boleta != null)
                            {
                                peso = boleta.PesoNeto < 1000 ? 1 : boleta.PesoNeto / 1000;
                            }
                            else
                            {
                                var boletaManual = boletaManualService.Get(criteriaRuta);
                                if (boletaManual != null)
                                {
                                    peso = boletaManual.PesoNeto < 1000 ? 1 : boletaManual.PesoNeto / 1000;
                                }
                            }

                            // La cantidad de toneladas obtenidas se multiplican por el costo de la ruta
                            // y se divide entre la cantidad de recolectores de la cuadrilla
                            toneladas += (costo * peso) / Cuadrilla.Count;
                        }
                    }

                    criteriaDetails = new Dictionary<string, object>();
                    criteriaDetails.Add("IsDeleted", false);
                    var listCosto = costoHoraService.GetByDate("Recolector");
                    listCosto = listCosto.Where(s => s.FechaInicio.AddDays(-1) <= item.Fecha && s.FechaFin.AddDays(1) >= item.Fecha).ToList();
                    var costoHora = listCosto.FirstOrDefault();

                    if (item.Salida != null && item.Salida != String.Empty)
                    {
                        var cantHoras = Convert.ToDateTime(item.Salida) - Convert.ToDateTime(item.Entrada);
                        item.TotalHoras = Convert.ToDouble(cantHoras.Hours + "." + cantHoras.Minutes);
                        if (Convert.ToDateTime(item.Entrada).Hour > 18) // Nocturno
                        {
                            item.HorasOrdinarias = item.TotalHoras > 6 ? 6 : item.TotalHoras;
                            item.HorasExtra = item.TotalHoras > 6 ? item.TotalHoras - 6 : 0;
                            item.MontoOrdinario = item.HorasOrdinarias * costoHora.Monto;
                        }
                        else
                        {
                            if (Convert.ToDateTime(item.Entrada).Hour > 12) // Nocturno con algunas horas mixtas
                            {
                                item.HorasOrdinarias = item.TotalHoras > 6 ? 6 : item.TotalHoras;
                                item.HorasExtra = item.TotalHoras > 6 ? item.TotalHoras - 6 : 0;
                                var horasMixtas = (19 - Convert.ToDateTime(item.Entrada).TimeOfDay.TotalHours) * 0.14 * costoHora.Monto;
                                item.MontoOrdinario = horasMixtas + (item.HorasOrdinarias * costoHora.Monto);

                            }
                            else // Diurno
                            {
                                item.HorasOrdinarias = item.TotalHoras > 8 ? 8 : item.TotalHoras;
                                item.HorasExtra = item.TotalHoras > 8 ? item.TotalHoras - 8 : 0;
                                if (Convert.ToDateTime(item.Entrada).Hour < 5) // en caso de que existan horas mixtas
                                {
                                    var horasMixtas = (5 - Convert.ToDateTime(item.Entrada).TimeOfDay.TotalHours) * 0.14 * costoHora.Monto;
                                    item.MontoOrdinario = horasMixtas + (item.HorasOrdinarias * costoHora.Monto);
                                }
                                else
                                {
                                    item.MontoOrdinario = item.HorasOrdinarias * costoHora.Monto;
                                }
                            }
                        }
                        item.MontoExtra = item.HorasExtra * (costoHora.Monto * 1.5);
                        item.Total = item.MontoOrdinario + item.MontoExtra;
                        item.Toneladas = toneladas;
                        item.Compensacion = toneladas > item.Total ? 0 : item.Total - toneladas;

                    }
                    else
                    {
                        // en caso que el dia laborado sea feriado por ley 
                        IDictionary<string, object> criteriaFeriado = new Dictionary<string, object>();
                        criteriaFeriado.Add("Dia", item.Fecha.Day);
                        criteriaFeriado.Add("Mes", item.Fecha.Month);
                        var feriado = feriadosService.Get(criteriaFeriado);
                        if (feriado != null)
                        {
                            // Se busca la nomina que se genero hace una semana
                            criteriaFeriado = new Dictionary<string, object>();
                            criteriaFeriado.Add("IsDeletd", false);
                            criteriaFeriado.Add("Compania", compania);
                            criteriaFeriado.Add("Tipo", "Recolector");
                            criteriaFeriado.Add("Estado", "Generada");
                            var ListNomina = planillaService.GetAll(criteriaFeriado);
                            ListNomina = ListNomina.Where(s => s.CreateDate >= DateTime.Now.AddDays(-9) && s.CreateDate <= DateTime.Now.AddDays(-5)).ToList();
                            var nominaAnterior = ListNomina.FirstOrDefault();

                            //Se selcciona los detalles de la nomina que corresponden al empleado actual
                            var itemsAnterior = nominaAnterior.DetallesNomina.Where(s => s.Empleado.Id == item.Empleado.Id).ToList();

                            //Si hay un dia feriado y se saca el promedio de la cantidad de toneladas que se hicieron durante esa semana
                            //en caso que en esa semana tambien se haya dado un dia feriado, se saca el promedio entre los dias laborados
                            if (itemsAnterior.Count > 0)
                            {
                                var totalToneladas = itemsAnterior.Sum(s => s.Toneladas);
                                listCosto = costoHoraService.GetByDate("Recolector");
                                listCosto = listCosto.Where(s => s.FechaInicio.AddDays(-1) <= item.Fecha && s.FechaFin.AddDays(1) >= item.Fecha).ToList();
                                costoHora = listCosto.FirstOrDefault();

                                totalToneladas = totalToneladas / 6;
                                item.Salida = String.Empty;
                                item.Entrada = String.Empty;
                                item.TotalHoras = 8;
                                item.HorasExtra = 0;
                                item.HorasOrdinarias = 8;
                                item.MontoExtra = 0;
                                item.MontoOrdinario = 8 * costoHora.Monto;
                                item.Toneladas = totalToneladas;
                                item.Total = item.MontoOrdinario;
                                item.Compensacion = totalToneladas > item.Total ? 0 : item.Total - totalToneladas;
                            }
                        }
                        else
                        {
                            //En caso de que no se presentó 
                            item.Salida = String.Empty;
                            item.Entrada = String.Empty;
                            item.TotalHoras = 0;
                            item.Compensacion = 0;
                            item.HorasExtra = 0;
                            item.HorasOrdinarias = 0;
                            item.MontoExtra = 0;
                            item.MontoOrdinario = 0;
                            item.Toneladas = 0;
                            item.Total = 0;
                        }
                    }
                }

                model = nominaMapper.GetBoletaNominaModel(nomina);
                model.ListaDetalles = DetallesNomina;
                return model;
            }
            else
            {
                return model;
            }
        }

        private PlanillaModel CargarNominaChofer(IList<ItemNomina> DetallesNomina)
        {
            PlanillaModel model = new PlanillaModel();
            if (DetallesNomina.Count > 0)
            {
                Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                Planilla nomina = new Planilla();
                nomina.DetallesNomina = new List<ItemNomina>();
                nomina.Descripcion = Request.Form["txtDescripcion"] != null ? Request.Form["txtDescripcion"] : "Planilla";

                foreach (var item in DetallesNomina)
                {
                    //Obtener OTR
                    IDictionary<string, object> criteriaDetails = new Dictionary<string, object>();                    
                    criteriaDetails = new Dictionary<string, object>();
                    criteriaDetails.Add("IsDeleted", false);
                    var listCosto = costoHoraService.GetByDate("Chofer");
                    listCosto = listCosto.Where(s => s.FechaInicio.AddDays(-1) <= item.Fecha && s.FechaFin.AddDays(1) >= item.Fecha).ToList();
                    var costoHora = listCosto.FirstOrDefault();

                    if (item.Salida != null && item.Salida != String.Empty)
                    {
                        var cantHoras = Convert.ToDateTime(item.Salida) - Convert.ToDateTime(item.Entrada);
                        item.TotalHoras = Convert.ToDouble(cantHoras.Hours + "." + cantHoras.Minutes);
                        if (Convert.ToDateTime(item.Entrada).Hour > 18) // Nocturno
                        {
                            item.HorasOrdinarias = item.TotalHoras > 6 ? 6 : item.TotalHoras;
                            item.HorasExtra = item.TotalHoras > 6 ? item.TotalHoras - 6 : 0;
                            item.MontoOrdinario = item.HorasOrdinarias * costoHora.Monto;
                        }
                        else
                        {
                            if (Convert.ToDateTime(item.Entrada).Hour > 12) // Nocturno con algunas horas mixtas
                            {
                                item.HorasOrdinarias = item.TotalHoras > 6 ? 6 : item.TotalHoras;
                                item.HorasExtra = item.TotalHoras > 6 ? item.TotalHoras - 6 : 0;
                                var horasMixtas = (19 - Convert.ToDateTime(item.Entrada).TimeOfDay.TotalHours) * 0.14 * costoHora.Monto;
                                item.MontoOrdinario = horasMixtas + (item.HorasOrdinarias * costoHora.Monto);

                            }
                            else // Diurno
                            {
                                item.HorasOrdinarias = item.TotalHoras > 8 ? 8 : item.TotalHoras;
                                item.HorasExtra = item.TotalHoras > 8 ? item.TotalHoras - 8 : 0;
                                if (Convert.ToDateTime(item.Entrada).Hour < 5) // en caso de que existan horas mixtas
                                {
                                    var horasMixtas = (5 - Convert.ToDateTime(item.Entrada).TimeOfDay.TotalHours) * 0.14 * costoHora.Monto;
                                    item.MontoOrdinario = horasMixtas + (item.HorasOrdinarias * costoHora.Monto);
                                }
                                else
                                {
                                    item.MontoOrdinario = item.HorasOrdinarias * costoHora.Monto;
                                }
                            }
                        }
                        item.MontoExtra = item.HorasExtra * (costoHora.Monto * 1.5);
                        item.Total = item.MontoOrdinario + item.MontoExtra;
                        item.Toneladas = 0;
                        item.Compensacion = 0;

                    }
                    else
                    {
                        // en caso que el dia laborado sea feriado por ley 
                        IDictionary<string, object> criteriaFeriado = new Dictionary<string, object>();
                        criteriaFeriado.Add("Dia", item.Fecha.Day);
                        criteriaFeriado.Add("Mes", item.Fecha.Month);
                        var feriado = feriadosService.Get(criteriaFeriado);
                        if (feriado != null)
                        {
                            // Se busca la nomina que se genero hace una semana                           
                            listCosto = costoHoraService.GetByDate("Chofer");
                            listCosto = listCosto.Where(s => s.FechaInicio.AddDays(-1) <= item.Fecha && s.FechaFin.AddDays(1) >= item.Fecha).ToList();
                            costoHora = listCosto.FirstOrDefault();

                            item.Salida = String.Empty;
                            item.Entrada = String.Empty;
                            item.HorasOrdinarias = item.TotalHoras > 8 ? 8 : item.TotalHoras;
                            item.HorasExtra = item.TotalHoras > 8 ? item.TotalHoras - 8 : 0;
                            item.MontoExtra = item.HorasExtra * (costoHora.Monto * 1.5);
                            item.MontoOrdinario = item.TotalHoras * costoHora.Monto;
                            item.TotalHoras = item.HorasOrdinarias + item.HorasExtra;
                            item.Toneladas = 0;
                            item.Total = item.MontoOrdinario;
                            item.Compensacion = 0;
                            
                        }
                        else
                        {
                            //En caso de que no se presentó 
                            item.Salida = String.Empty;
                            item.Entrada = String.Empty;
                            item.TotalHoras = 0;
                            item.Compensacion = 0;
                            item.HorasExtra = 0;
                            item.HorasOrdinarias = 0;
                            item.MontoExtra = 0;
                            item.MontoOrdinario = 0;
                            item.Toneladas = 0;
                            item.Total = 0;
                        }
                    }
                }

                model = nominaMapper.GetBoletaNominaModel(nomina);
                model.ListaDetalles = DetallesNomina;
                return model;
            }
            else
            {
                return model;
            }
        }

        private PlanillaModel CargarNominaTaller(IList<ItemNomina> DetallesNomina)
        {

            PlanillaModel model = new PlanillaModel();
            if (DetallesNomina.Count > 0)
            {
                Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                Planilla nomina = new Planilla();
                nomina.DetallesNomina = new List<ItemNomina>();
                nomina.Descripcion = Request.Form["txtDescripcion"] != null ? Request.Form["txtDescripcion"] : "Planilla";

                foreach (var item in DetallesNomina)
                {
                    //Obtener OTR
                    IDictionary<string, object> criteriaDetails = new Dictionary<string, object>();
                    criteriaDetails = new Dictionary<string, object>();
                    criteriaDetails.Add("IsDeleted", false);
                    var listCosto = costoHoraService.GetByDate("Taller");
                    listCosto = listCosto.Where(s => s.FechaInicio.AddDays(-1) <= item.Fecha && s.FechaFin.AddDays(1) >= item.Fecha).ToList();
                    var costoHora = listCosto.FirstOrDefault();

                    if (item.Salida != null && item.Salida != String.Empty)
                    {
                        var cantHoras = Convert.ToDateTime(item.Salida) - Convert.ToDateTime(item.Entrada);
                        item.TotalHoras = Convert.ToDouble(cantHoras.Hours + "." + cantHoras.Minutes);
                        if (Convert.ToDateTime(item.Entrada).Hour > 18) // Nocturno
                        {
                            item.HorasOrdinarias = item.TotalHoras > 6 ? 6 : item.TotalHoras;
                            item.HorasExtra = item.TotalHoras > 6 ? item.TotalHoras - 6 : 0;
                            item.MontoOrdinario = item.HorasOrdinarias * costoHora.Monto;
                        }
                        else
                        {
                            if (Convert.ToDateTime(item.Entrada).Hour > 12) // Nocturno con algunas horas mixtas
                            {
                                item.HorasOrdinarias = item.TotalHoras > 6 ? 6 : item.TotalHoras;
                                item.HorasExtra = item.TotalHoras > 6 ? item.TotalHoras - 6 : 0;
                                var horasMixtas = (19 - Convert.ToDateTime(item.Entrada).TimeOfDay.TotalHours) * 0.14 * costoHora.Monto;
                                item.MontoOrdinario = horasMixtas + (item.HorasOrdinarias * costoHora.Monto);

                            }
                            else // Diurno
                            {
                                item.HorasOrdinarias = item.TotalHoras > 8 ? 8 : item.TotalHoras;
                                item.HorasExtra = item.TotalHoras > 8 ? item.TotalHoras - 8 : 0;
                                if (Convert.ToDateTime(item.Entrada).Hour < 5) // en caso de que existan horas mixtas
                                {
                                    var horasMixtas = (5 - Convert.ToDateTime(item.Entrada).TimeOfDay.TotalHours) * 0.14 * costoHora.Monto;
                                    item.MontoOrdinario = horasMixtas + (item.HorasOrdinarias * costoHora.Monto);
                                }
                                else
                                {
                                    item.MontoOrdinario = item.HorasOrdinarias * costoHora.Monto;
                                }
                            }
                        }
                        item.MontoExtra = item.HorasExtra * (costoHora.Monto * 1.5);
                        item.Total = item.MontoOrdinario + item.MontoExtra;
                        item.Toneladas = 0;
                        item.Compensacion = 0;

                    }
                    else
                    {
                        // en caso que el dia laborado sea feriado por ley 
                        IDictionary<string, object> criteriaFeriado = new Dictionary<string, object>();
                        criteriaFeriado.Add("Dia", item.Fecha.Day);
                        criteriaFeriado.Add("Mes", item.Fecha.Month);
                        var feriado = feriadosService.Get(criteriaFeriado);
                        if (feriado != null)
                        {
                            // Se busca la nomina que se genero hace una semana                           
                            listCosto = costoHoraService.GetByDate("Taller");
                            listCosto = listCosto.Where(s => s.FechaInicio.AddDays(-1) <= item.Fecha && s.FechaFin.AddDays(1) >= item.Fecha).ToList();
                            costoHora = listCosto.FirstOrDefault();

                            item.Salida = String.Empty;
                            item.Entrada = String.Empty;
                            item.HorasOrdinarias =  item.TotalHoras > 8 ? 8: item.TotalHoras;
                            item.HorasExtra = item.TotalHoras > 8 ? item.TotalHoras - 8 : 0;
                            item.MontoExtra = item.HorasExtra * (costoHora.Monto * 1.5);
                            item.MontoOrdinario = item.HorasOrdinarias * costoHora.Monto;
                            item.TotalHoras = item.HorasOrdinarias + item.HorasExtra;
                            item.Toneladas = 0;
                            item.Total = item.MontoOrdinario;
                            item.Compensacion = 0;

                        }
                        else
                        {
                            //En caso de que no se presentó 
                            item.Salida = String.Empty;
                            item.Entrada = String.Empty;
                            item.TotalHoras = 0;
                            item.Compensacion = 0;
                            item.HorasExtra = 0;
                            item.HorasOrdinarias = 0;
                            item.MontoExtra = 0;
                            item.MontoOrdinario = 0;
                            item.Toneladas = 0;
                            item.Total = 0;
                        }
                    }
                }

                model = nominaMapper.GetBoletaNominaModel(nomina);
                model.ListaDetalles = DetallesNomina;
                return model;
            }
            else
            {
                return model;
            }
        }
    }
}
