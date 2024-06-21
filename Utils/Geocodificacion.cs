using ElGatoMensajero.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Linq;

namespace ElGatoMensajero.Utilidades
{
    public class Geocodificacion
    {
        //Método que devuelve la latitud y la longitud de una dirección dada:
        public static string obtenerCoordenadas(string direccion)
        {
            //Se llama a la API de Google:
            string peticion = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?address=" + direccion + "&key=GOOGLEAPIKEY", Uri.EscapeDataString(direccion));
            try
            {
                //Se lee la respuesta en formato xml:
                WebResponse respuesta = (WebRequest.Create(peticion)).GetResponse();
                XDocument respuestaXml = XDocument.Load(respuesta.GetResponseStream());
                //En caso de que contenga las coordenadas el campo status devuelve la cadena "OK":
                if (respuestaXml.Element("GeocodeResponse").Element("status").Value == "OK")
                {
                    //Navegamos por los campos del xml para recuperar las coordenadas:
                    string latitud = respuestaXml.Element("GeocodeResponse").Element("result").Element("geometry").Element("location").Element("lat").Value;
                    string longitud = respuestaXml.Element("GeocodeResponse").Element("result").Element("geometry").Element("location").Element("lng").Value;
                    return (latitud + ";" + longitud);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error de la llamada a la Google API: " + e);
            }
            return "";
        }

        //Método que guarda las coordenadas de la sede:
        public static void guardarCoordenadasSedes(sedes sedes)
        {
            string[] coordenadas = (Utilidades.Geocodificacion.obtenerCoordenadas(sedes.num + " " + sedes.calle + ", " + sedes.localidad + ", " + sedes.provincia + " " + sedes.cp)).Split(new char[] { ';' }, 2);
            if (coordenadas[0] != "")
            {
                sedes.latitud = Double.Parse(coordenadas[0], CultureInfo.InvariantCulture);
                sedes.longitud = Double.Parse(coordenadas[1], CultureInfo.InvariantCulture);
            }
        }
        //Método que guarda las coordenadas del paquete:
        public static void guardarCoordenadasPaquetes(paquetes paquetes)
        {
            string[] coordenadas = (Utilidades.Geocodificacion.obtenerCoordenadas(paquetes.direccionOrigen + " " + paquetes.codPostalOrigen)).Split(new char[] { ';' }, 2);
            if (coordenadas[0] != "")
            {
                paquetes.latitud = Double.Parse(coordenadas[0], CultureInfo.InvariantCulture);
                paquetes.longitud = Double.Parse(coordenadas[1], CultureInfo.InvariantCulture);
            }
        }
    }
}
