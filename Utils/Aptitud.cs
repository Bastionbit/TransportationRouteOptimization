using Accord.Genetic;
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
    class Aptitud : IFitnessFunction
    {
        List<Tuple<double, double>> destinos { get; set; }
        public sedes sede { get; set; }
        public Aptitud(sedes sede, List<Tuple<double, double>> destinos) : base()
        {
            this.destinos = destinos;
            this.sede = sede;
        }

        public double Evaluate(IChromosome variacion)
        {
            //solución obtenida con esta variación presente:
            ushort[] solucionVariacion = ((PermutationChromosome)variacion).Value;
            //error a minimizar:
            double distancia = 0;
            //Por limitaciones de la api de Google no se hace así:
            //distancia de salida desde el origen hacia el primer destino:
            /*
            string peticion = string.Format("http://maps.googleapis.com/maps/api/distancematrix/xml?origins=" + sede.latitud+"," +sede.longitud+ "&destinations=" + destinos[solucionVariacion[0], 0] + "," + destinos[solucionVariacion[0], 1]+ "&key=GOOGLEAPIKEY");
            try
            {
                //Se lee la respuesta en formato xml:
                WebResponse respuesta = (WebRequest.Create(peticion)).GetResponse();
                XDocument respuestaXml = XDocument.Load(respuesta.GetResponseStream());
                //En caso de que contenga las coordenadas el campo status devuelve la cadena "OK":
                if (respuestaXml.Element("DistanceMatrixResponse").Element("status").Value == "OK")
                {
                    //Navegamos por los campos del xml para recuperar la distancia:
                    distancia= Double.Parse(respuestaXml.Element("DistanceMatrixResponse").Element("distance").Value, CultureInfo.InvariantCulture);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error de la llamada a la Google API: " + e);
                //se aproxima la distancia en línea recta:
                distancia = Math.Sqrt(Math.Pow((double)sede.latitud- destinos[solucionVariacion[0], 0], 2)+ Math.Pow((double)sede.longitud - destinos[solucionVariacion[0], 1], 2));
            }

            // resto de distancias de la ruta:
            
            for (int i = 1; i < solucionVariacion.Length-1; i++)
            {
                string peticionB = string.Format("http://maps.googleapis.com/maps/api/distancematrix/xml?origins=" + destinos[solucionVariacion[i], 0] + "," + destinos[solucionVariacion[i], 1] + "&destinations=" + destinos[solucionVariacion[i+1], 0] + "," + destinos[solucionVariacion[i+1], 1] + "&key=GOOGLEAPIKEY");
                try
                {
                    //Se lee la respuesta en formato xml:
                    WebResponse respuesta = (WebRequest.Create(peticion)).GetResponse();
                    XDocument respuestaXml = XDocument.Load(respuesta.GetResponseStream());
                    //En caso de que contenga las coordenadas el campo status devuelve la cadena "OK":
                    if (respuestaXml.Element("DistanceMatrixResponse").Element("status").Value == "OK")
                    {
                        //Navegamos por los campos del xml para recuperar la distancia:
                        distancia += Double.Parse(respuestaXml.Element("DistanceMatrixResponse").Element("distance").Value, CultureInfo.InvariantCulture);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error de la llamada a la Google API: " + e);
                    //se aproxima la distancia en línea recta:
                    distancia += Math.Sqrt(Math.Pow(destinos[solucionVariacion[i], 0] - destinos[solucionVariacion[i+1], 0], 2) + Math.Pow(destinos[solucionVariacion[i], 0] - destinos[solucionVariacion[i+1], 1], 2));
                }
            }*/


            //se aproxima a la distancia pitagórica en línea recta:
            if ((destinos.Count > 1) && (solucionVariacion.Length > 0))
            {
                //distancia entre el origen y el primer destino:
                distancia = Math.Sqrt(Math.Pow((double)sede.latitud - destinos[solucionVariacion[0]].Item1, 2) + Math.Pow((double)sede.longitud - destinos[solucionVariacion[0]].Item2, 2));
                //se le suma el resto de distancias de los puntos consecutivos:
                if (destinos.Count > 2)
                {
                    for (int i = 0; i < destinos.Count - 2; i++)
                    {
                        distancia += Math.Sqrt(Math.Pow(destinos[solucionVariacion[i]].Item1 - destinos[solucionVariacion[i + 1]].Item1, 2) + Math.Pow(destinos[solucionVariacion[i]].Item2 - destinos[solucionVariacion[i + 1]].Item2, 2));
                    }
                }
                //se usa el inverso de la longitud como aptitud:
                return (1 / distancia);
            }
            else
            {
                return 0;
            }
        }
    }
}