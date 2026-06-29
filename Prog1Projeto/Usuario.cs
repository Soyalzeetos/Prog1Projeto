using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Prog1Projeto
{

    public abstract class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }

        public abstract DateTime CalcularPrazoDevolucao(DateTime dataEmprestimo);
    }
}
