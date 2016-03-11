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

        public NominaController(IPlanillaService nomina, IItemNominaService item, ICompaniaService compania, IEmpleadoRecoleccionService empleado, ICostoRutaRecoleccionService costo,
            IOTRService otr, ICostoHoraService costoHora, IBasculaService bascula, IBoletaManualService boleta)
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

        [HttpPost]
        public ActionResult Importar(HttpPostedFileBase excelfile)
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
                                item.Fecha = Convert.ToDateTime(table.Rows[row][2].ToString());
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

                    if(DetallesNomina.Count > 0)
                    {
                        Compania compania = companiaService.Get(Session["Compania"] != null ? Convert.ToInt64(Session["Compania"].ToString()) : 0);
                        Planilla nomina = new Planilla();
                        nomina.Version = 1;
                        nomina.CreateDate = DateTime.Now;
                        nomina.IsDeleted = false;
                        nomina.CreatedBy = ObtenerUsuarioActual().Nombre;
                        nomina.DetallesNomina = new List<ItemNomina>();
                        nomina.Compania = compania;
                        nomina.Estado = "Borrador";
                        nomina.Descripcion = Request.Form["txtDescripcion"] != null ? Request.Form["txtDescripcion"] : "Planilla";
                        nomina = planillaService.Create(nomina);

                        foreach (var item in DetallesNomina)
                        {
                            item.Nomina = nomina;
                            item.IsDeleted = false;
                            item.CreateDate = DateTime.Now;
                            item.CreatedBy = ObtenerUsuarioActual().Nombre;
                            item.Version = 1;

                            //Obtener OTR
                             IDictionary<string, object> criteriaDetails = new Dictionary<string, object>();
                            criteriaDetails.Add("Fecha", item.Fecha);
                            criteriaDetails.Add("IsDeleted", false);
                            var OTRs = otrService.GetAll(criteriaDetails);
                            double toneladas = 0;
                            foreach (var otr in OTRs)
                            {
                                var Cuadrilla = otr.Cuadrilla.ListaEmpleados.Where(s=>s.Id == item.Empleado.Id && s.Puesto != "Chofer").ToList();
                                if(Cuadrilla.Count > 0)
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
                                        if(boletaManual != null)
                                        {
                                            peso = boletaManual.PesoNeto < 1000 ? 1 : boletaManual.PesoNeto / 1000;
                                        }
                                    }
                                    
                                    // La cantidad de toneladas obtenidas se multiplican por el costo de la ruta
                                    // y se divide entre la cantidad de recolectores de la cuadrilla
                                    toneladas += (costo * peso) / Cuadrilla.Count;
                                }
                                
                            }

                           // IDictionary<string, object> criteriaDetails = new Dictionary<string, object>();
                            //criteriaDetails.Add("Codigo", item  
                            criteriaDetails.Add("IsDeleted", false);
                            var costoHora = costoHoraService.GetByDate(item.Fecha, "Recolector");

                            if(item.Salida != null)
                            {
                                var cantHoras = Convert.ToDateTime(item.Salida) - Convert.ToDateTime(item.Entrada);
                                item.TotalHoras = Convert.ToDouble(cantHoras.TotalHours + "." + cantHoras.TotalMinutes);
                                if (Convert.ToDouble(Convert.ToDateTime(item.Entrada)) > 18 ) // Nocturno
                                {
                                    item.HorasOrdinarias = item.TotalHoras > 6 ? 6 : item.TotalHoras;
                                    item.HorasExtra = item.TotalHoras > 6 ? item.TotalHoras - 6 : 0;
                                    item.MontoOrdinario = item.HorasOrdinarias * costoHora.Monto;
                                }
                                else  
                                {
                                    if (Convert.ToDouble(Convert.ToDateTime(item.Entrada)) > 12) // Nocturno con algunas horas mixtas
                                    {
                                        item.HorasOrdinarias = item.TotalHoras > 6 ? 6 : item.TotalHoras;
                                        item.HorasExtra = item.TotalHoras > 6 ? item.TotalHoras - 6 : 0;
                                        var horasMixtas = (19 - Convert.ToDouble(Convert.ToDateTime(item.Entrada))) * 0.14 * costoHora.Monto;
                                        item.MontoOrdinario = horasMixtas + (item.HorasOrdinarias * costoHora.Monto);
                                      
                                    }
                                    else // Diurno
                                    {
                                        item.HorasOrdinarias = item.TotalHoras > 8 ? 8 : item.TotalHoras;
                                        item.HorasExtra = item.TotalHoras > 8 ? item.TotalHoras - 8 : 0;
                                        if (Convert.ToDouble(Convert.ToDateTime(item.Entrada)) < 5) // en caso de que existan horas mixtas
                                        {
                                            var horasMixtas = (5 - Convert.ToDouble(Convert.ToDateTime(item.Entrada))) * 0.14 * costoHora.Monto;
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
                            itemService.Create(item);
                        }
                        nomina.DetallesNomina = DetallesNomina;
                        planillaService.Update(nomina);
                    }
                }
                else
                {
                    ViewBag.Error = "El archivo a importar no es permitido";
                }
            }

            return View("Cargar");
        }
    }
}
