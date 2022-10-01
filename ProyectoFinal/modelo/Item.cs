using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.modelo
{
    public class Item
    {

        public int codigo;
        public String nombre;
   

        public Item(int codigo, string nombre)
        {
            this.codigo = codigo;
            this.nombre = nombre;
     
        }
    }
}
