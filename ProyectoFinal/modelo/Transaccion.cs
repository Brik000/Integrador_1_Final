using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.modelo
{
    public class Transaccion
    {

        public int codigo;
        public Cliente cliente;
        public DateTime fecha;
        public double total;

        public List<Item>items;
        public Dictionary<Item, int> cantidadesCompradas;
        public Dictionary<Item, double> precios;


        public Transaccion(int codigo, Cliente cliente, DateTime fecha, double total)
        {
            this.codigo = codigo;
            this.cliente = cliente;
            this.fecha = fecha;
            this.total = total;
            items = new List<Item>();
            cantidadesCompradas = new Dictionary<Item, int>();
            precios=new Dictionary<Item,double> ();

        }
    }
}
