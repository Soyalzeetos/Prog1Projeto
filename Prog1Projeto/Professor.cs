using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prog1Projeto
{
    public class Professor : Usuario
    {
        public Professor(int id, string nome, string email) : base(id, nome, email)
        {
        }
        public Professor() : base(0, "", "")
        {
        }
        public override DateTime CalcularPrazoDevolucao(DateTime dataEmprestimo)
        {
            return dataEmprestimo.AddDays(15);
        }
    }
}
