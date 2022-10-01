using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.bruteForce
{
    public class TransaccionFormato
    {


        public int id;
       public String[] compras;


        public TransaccionFormato(int id, String[] compras)
        {
            this.id = id;
            this.compras = compras;
        }
    }
}
