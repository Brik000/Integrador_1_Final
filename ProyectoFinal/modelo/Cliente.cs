using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.modelo
{
    public class Cliente
    {

        public String codigo;
        public String grupo;
        public String ciudad;
        public String departamento;

        public Cliente(string codigo, string grupo, string ciudad, string departamento)
        {
            this.codigo = codigo;
            this.grupo = grupo;
            this.ciudad = ciudad;
            this.departamento = departamento;
        }
    }
}
