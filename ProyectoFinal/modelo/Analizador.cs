using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProyectoFinal.modelo
{
   public  class Analizador
    {

        public Dictionary<int, Item> items;
        public Dictionary<int, Transaccion> transacciones;
        public Dictionary<String,Cliente>clientes;
        public List<Transaccion> limitadas;

            public Analizador()
        {
            items = new Dictionary<int, Item>();
            transacciones = new Dictionary<int, Transaccion>();
            clientes = new Dictionary<string, Cliente>();
        }


        public void cargarItems(String ruta)
        {
            if (File.Exists(ruta))
            {
                String[] lineas = File.ReadAllLines(ruta);
                foreach (String linea in lineas)
                {
                    String[] spliteado = linea.Split('\t');
              
                    Item i = new Item(Int32.Parse(spliteado[0]), spliteado[1]);
                    items[i.codigo] = i;
                }
            }else
            {
                throw new Exception("La ruta especificada no existe");
            }
        }

        public void cargarClientes(String ruta)
        {
            if (File.Exists(ruta))
            {
                String[] lineas = File.ReadAllLines(ruta);
                foreach (String linea in lineas)
                {
                    String[] spliteado = linea.Split('\t');
                    Cliente c = new Cliente(spliteado[0], spliteado[1], spliteado[2], spliteado[3]);
                    clientes[c.codigo] = c;
                    
                }
            }
            else
            {
                throw new Exception("La ruta especificada no existe");
            }
        }


        public void cargarTransacciones(String ruta)
        {
            if (File.Exists(ruta))
            {
                String[] lineas = File.ReadAllLines(ruta);
                var t=lineas.Select(i =>
                {
                    String[] spliteado = i.Split('\t');
                    return new { codC = spliteado[0], codT = spliteado[1], fecha = spliteado[2], totalImpuesto = spliteado[3], item = spliteado[4], cant = spliteado[5],precio=spliteado[6]};
                }).ToList();

                for (int i = 1; i < t.Count; i++)
                {
                    if(t[i].codT==t[i-1].codT && t[i].item == t[i - 1].item)
                    {
                        t.Remove(t[i]);
                        i--;
                    }
                }

                t.GroupBy(i =>new { i.codT}).Select(g=>
                {
                    int codigo = Int32.Parse(g.First().codT);
                   
                    Cliente cliente = clientes[g.First().codC];
                    String[] fechaAux = g.First().fecha.Split('/');
                    DateTime fecha = new DateTime(Int32.Parse(fechaAux[2]), Int32.Parse(fechaAux[1]), Int32.Parse(fechaAux[0]));
                    double total = double.Parse(g.First().totalImpuesto);
                    Transaccion tran = new Transaccion(codigo, cliente, fecha, total);
                    var its = g.Select(h => items[Int32.Parse(h.item)]).ToList();
                    tran.items = its;
                
                    var cants = g.ToDictionary(d => items[Int32.Parse(d.item)] , d => Int32.Parse(d.cant));
                    tran.cantidadesCompradas = cants;
                    var precios = g.ToDictionary(d => items[Int32.Parse(d.item)], d => double.Parse(d.precio));
                    tran.precios = precios;
                    return tran;

                }).ToList().ForEach(f=> transacciones[f.codigo]=f);

            }
            else
            {
                throw new Exception("La ruta especificada no existe");
            }
        }


        public Dictionary<int,int> generarHistogramaArticulos()
        {
            int max = transacciones.Max(i => i.Value.items.Count);
            Dictionary<int, int> retorno = new Dictionary<int, int>();
            for (int i = 1; i <=max; i++)
            {
                retorno[i] = 0;
            }
            foreach (var trans in transacciones)
            {
                retorno[trans.Value.items.Count] += 1;

            }
            return retorno;
        }

        public Dictionary<int, int> generarHistogramaClientes()
        {
          


            List<int> compras = new List<int>();
            foreach (var c in clientes)
            {
                int cont=transacciones.Where(t => t.Value.cliente.codigo.Equals(c.Key)).Count();
                compras.Add(cont);
            }
            var com =compras.Where(i=>i!=0).GroupBy(i => i).ToDictionary(d => d.Key, d => d.Count());

            //com.OrderBy(i => i.Key).ToList().ForEach(i => Console.WriteLine(i.Key+"->"+i.Value));

            return com;
        }


        public List<Transaccion> aplicarFiltros(int minArticulos, int maxArticulos, int minClientes, int maxClientes)
        {
            var listaInicial = transacciones.Select(i => i.Value).Where(i => i.items.Count >= minArticulos && i.items.Count <= maxArticulos);
            var grupos = listaInicial.GroupBy(i => i.cliente.codigo).Where(i => i.Count() >= minClientes && i.Count() <= maxClientes);
            List<Transaccion> trans = new List<Transaccion>();
            foreach(var g in grupos)
            {
                foreach(var t in g)
                {
                    trans.Add(t);
                }
            }
            limitadas = trans;
            return trans;
        }

        
    }
}
