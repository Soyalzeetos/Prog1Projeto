using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prog1Projeto
{
    public class Aluno : Usuario
    {
        public Aluno(int id, string nome, string email) : base(id, nome, email)
        {
        }
        public Aluno() : base(0, "", "")
        {
        }
        public override DateTime CalcularPrazoDevolucao(DateTime dataEmprestimo)
        {
            return dataEmprestimo.AddDays(7);
        }
        
    }
}
