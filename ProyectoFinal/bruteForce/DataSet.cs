using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ProyectoFinal;
using ProyectoFinal.modelo;

namespace ProyectoFinal.bruteForce
{
    public class DataSet
    {


        public List<TransaccionFormato> transacciones;
        public List<String> items;
        public List<Asociacion> asociaciones;
        public double umbralConfianza;
        public double umbralSoporte;
        public ProyectoFinal.modelo.Analizador analizador;

        public DataSet(Analizador a,double uC, double uS)
        {
            analizador = a;
            umbralConfianza = uC;
            umbralSoporte = uS;
            cargarDatos();
            getItems();
            generarAsociaciones();
            refinarAsociaciones();
        }

        private void getItems()
        {
            items = transacciones.SelectMany(t => t.compras).Distinct().ToList();
        }



        private void cargarDatos()
        {
            transacciones = analizador.limitadas.Select(i => new TransaccionFormato(i.codigo, i.items.Select(k => k.codigo + "").ToArray())).ToList();
        }


        private void generarAsociaciones()
        {
            asociaciones = new List<Asociacion>();
            var itemsArray = items.ToArray();
            var powerSet = PowerSet.FastPowerSet(itemsArray);
            //foreach (String[] arr in powerSet)
            //{
            //    Console.WriteLine(String.Join(" ", arr) + "\r\n");
            //}
            foreach (String[] arr in powerSet)
            {
                String[] faltantes = itemsArray.Except(arr).ToArray();
                var nPowerSet = PowerSet.FastPowerSet(faltantes);
                foreach( String[] arr2 in nPowerSet)
                {
                    asociaciones.Add(new Asociacion(arr, arr2));
                }
            }


            //foreach (Asociacion a in asociaciones)
            //{
            //    Console.WriteLine("{" + String.Join(",", a.de) + "} ----> {" + String.Join(",", a.a) + "}");
            //}
            Console.WriteLine("Asociaciones totales:" + asociaciones.Count);
        }


        protected double calcularSoporte(Asociacion a)
        {
            String[] union = a.de.Union(a.a).ToArray();
            int cont = 0;
            foreach(TransaccionFormato t in transacciones)
            {
                bool esta = !union.Except(t.compras).Any();
                if (esta)
                {
                    cont++;
                }
            }
            return (double)cont / transacciones.Count;
        }


        protected double calcularConfianza(Asociacion a)
        {
            String[] union = a.de.Union(a.a).ToArray();
            int cont1 = 0;
            int cont2 = 0;
            foreach (TransaccionFormato t in transacciones)
            {
                bool estaUnion = !union.Except(t.compras).Any();
                if (estaUnion)
                {
                    cont1++;
                }
                bool estaX = !a.de.Except(t.compras).Any();
                if (estaX)
                {
                    cont2++;
                }
            }
            return (double)cont1 / cont2;
        }


        private void refinarAsociaciones()
        {
            int cont = 1;
            asociaciones = asociaciones.Where(i => {
                if (cont % 100000 == 0)
                {
                    Console.WriteLine(cont);
                }
                cont++;
                return calcularConfianza(i) >= umbralConfianza && calcularSoporte(i) >= umbralSoporte;
                
                
                
                
                }).ToList();
        }
    }
}
