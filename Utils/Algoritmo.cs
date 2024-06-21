using Accord.Genetic;
using ElGatoMensajero.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
/*
 * Pasos del algoritmo:
Se crea una población con parámetros aleatorios (en este caso: número de destinos).
Se varían aleatoriamente modificando un número de parámetros (en este caso el número de destinos*100).
Se buscan a los que mejor resultado han dado utilizando la función de aptitud.
De los mejores seleccionados se realizan variaciones para generar la siguiente generación de redes neuronales junto a los seleccionados anteriormente.
Se repite el proceso con la nueva población las veces que haga falta aplicándose la mejora continua.
  */
namespace ElGatoMensajero.Utilidades
{
    public class Algoritmo
    {
        private static elGatoMensajeroEntities db = new elGatoMensajeroEntities();
        //Se guardan las coordenadas de los destinos de las posibles rutas en una lista de coordenadas:
        private static List<Tuple<double, double>> destinos;

        //cálculo de la ruta óptima entre un centro logístico y los destinos de los paquetes que contiene:
        public static ushort[] TransporteSedesPersonas(sedes sede)
        {
            destinos = new List<Tuple<double, double>>();
            for (int i = 0; i < db.paquetes.Count<paquetes>(); i++)
            {
                //la sede solo se tendrá en cuenta si posee almacenadas coordenadas:
                if (db.paquetes.OrderBy(o => o.id).Skip(i).First().latitud.ToString() != "")
                {
                    //cada sede reparte solo los paquetes que posee:
                    if (db.paquetes.OrderBy(o => o.id).Skip(i).First().sedes.FirstOrDefault<sedes>().id == sede.id)
                    {
                        //se almacenan las coordenadas de destino de cada paquete de la sede:
                        Tuple<double, double> coords = new Tuple<double, double>((double)db.paquetes.OrderBy(o => o.id).Skip(i).First().latitud, (double)db.paquetes.OrderBy(o => o.id).Skip(i).First().longitud);
                        destinos.Add(coords);
                    }
                }
            }
            if (destinos.Count > 0)
            {
                //función que medirá la aptitud de cada solución propuesta:
                Aptitud aptitud = new Aptitud(sede, destinos);
                //Conjunto de soluciones propuestas:
                //parámetros: número de soluciones iniciales aleatorias, número de parámetros a modificar en cada propuesta de solución, función de error, algoritmo de selección de las mejores soluciones:
                Population poblacion = new Population(100*destinos.Count, new PermutationChromosome(destinos.Count), aptitud, new EliteSelection());
                //se ejecuta el proceso de aprendizaje automático competitivo:
                for (int i = 0; i < 1000 * destinos.Count; i++)
                {
                    poblacion.RunEpoch();
                }
                //solución obtenida por la mejor de las variaciones en todas las generaciones de la población:
                return (((PermutationChromosome)poblacion.BestChromosome).Value);
            }
            else
            {
                return null;
            }
        }

        public static string calcularRuta(sedes sede)
        {
            //se obtiene una ruta optimizada para la sede y se imprime:
            ushort[] solucionArray = TransporteSedesPersonas(sede);
            string solucion = "";
            if ((solucionArray != null) && (solucionArray.Length > 0))
            {
                if (destinos.Count > 1)
                {
                    for (int i = 0; i < destinos.Count; i++)
                    {
                        solucion += "El punto de destino número: " + (i + 1) + " es el de coordenadas: x=" + destinos[solucionArray[i]].Item1 + " , y= " + destinos[solucionArray[i]].Item2 + ". \n";
                    }
                }
                else
                {
                    solucion += "El punto de destino es el de coordenadas: x=" + destinos[0].Item1 + " , y= " + destinos[0].Item2 + ". \n";
                }
            }
            destinos.Clear();
            destinos.Capacity = 0;
            solucionArray = null;
            return solucion;
        }
    }
}