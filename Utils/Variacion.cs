using Accord.Genetic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElGatoMensajero.Utilidades
{
    //Cada variación será un valor aleatorio entre 0 y el nº de destinos-1 (destinos.Count-1) que indicará la posición en el List.
    //Se utilizan los métodos de cruce y variación por defecto heredando de PermutationChromosome.
    //En el PermutationChromosome no se puede repetir ninguna variación, por lo que se garantiza que solo se visitará cada destino una vez.
    class Variacion : PermutationChromosome
    {
        //Los parámetros que serán variados en cada solución propuesta son los destinos, 
        //puesto que la sede ya está determinada al ser la del paquete. 
        public List<Tuple<double, double>> destinos { get; set; }
        public Variacion(List<Tuple<double, double>> destinos) : base(destinos.Count)
        {
            this.destinos = destinos;
        }
    }
}