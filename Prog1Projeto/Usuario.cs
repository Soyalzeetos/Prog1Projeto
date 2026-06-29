using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Prog1Projeto
{

    public abstract class Pessoa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }

        public abstract DateTime CalcularPrazoDevolucao(DateTime dataEmprestimo);
    }

    public class Aluno : Pessoa
    {
        public override DateTime CalcularPrazoDevolucao(DateTime dataEmprestimo)
        {
            return dataEmprestimo.AddDays(7);
        }
    }

    public class Professor : Pessoa
    {
        public override DateTime CalcularPrazoDevolucao(DateTime dataEmprestimo)
        {
            return dataEmprestimo.AddDays(15);
        }
    }
}
