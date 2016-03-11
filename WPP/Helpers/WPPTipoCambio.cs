using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace WPP.Helpers
{
    public static class WPPTipoCambio
    {
        public static double TipoCambioWebService(string fechaInicio, string fechaFin, string tipo)
        {
            int tipoCambio = tipo == "Compra" ? 317 : 318;
            HttpWebRequest request = 
                (HttpWebRequest)WebRequest.Create("http://indicadoreseconomicos.bccr.fi.cr/indicadoreseconomicos/WebServices/wsIndicadoresEconomicos.asmx/ObtenerIndicadoresEconomicosXML?tcIndicador="
                +tipoCambio+"&tcFechaInicio="+fechaInicio+"&tcFechaFinal="+fechaFin+"&tcNombre=tq&tnSubNiveles=N");
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);

                    string ObjTipoCambio = reader.ReadToEnd();
                    var atributos = ObjTipoCambio.Split(' ');
                    string valor = (atributos[17]).Substring(17, 8);
                    double valorTipoCambio = Convert.ToDouble(valor);
                    return valorTipoCambio;
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
       
                }
                throw;
            }
        }
        
    }
}