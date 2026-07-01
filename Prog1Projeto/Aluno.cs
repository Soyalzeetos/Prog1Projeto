using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prog1Projeto
{
    public class Aluno : Usuario
    {
        public override DateTime CalcularPrazoDevolucao(DateTime dataEmprestimo)
        {
            return dataEmprestimo.AddDays(7);
        }
        
    }
}
