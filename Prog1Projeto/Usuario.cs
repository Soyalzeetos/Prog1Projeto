using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Prog1Projeto
{
    internal class Usuario
    {
        public class Cliente
        {
            public int Id { get; set; }
            public string Nome { get; set; }
            public string Email { get; set; }
            public void PreencheCliente(MySqlDataReader resultado)
            {
                this.Id = Convert.ToInt32(resultado[0]);
                this.Nome = resultado[1].ToString();
                this.Email = resultado[2].ToString();
            }
            public void Mostrar()
            {
                Console.WriteLine(this.Id + "\t" + this.Nome + "\t" + this.Email);
            }
            // Métodos Incluir, Buscar, Apagar e Alterar serão implementados abaixo
        }
    }
}
