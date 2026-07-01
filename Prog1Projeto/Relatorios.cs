using System;
using System.Collections.Generic;

namespace Prog1Projeto
{
    internal class Relatorios
    {
        private readonly RelatorioDAO relatorioDAO = new RelatorioDAO();

        public void ComprovanteEmprestimo(Usuario usuario, string livro, DateTime dataEmprestimo)
        {
            relatorioDAO.GerarComprovanteEmprestimo(usuario.Nome, livro, dataEmprestimo, usuario.CalcularPrazoDevolucao(dataEmprestimo));
        }

        public void GerarRelatorioEmprestimos(List<Emprestimo> emprestimos)
        {
            relatorioDAO.GerarRelatorioEmprestimos(emprestimos);
        }
    }
}
