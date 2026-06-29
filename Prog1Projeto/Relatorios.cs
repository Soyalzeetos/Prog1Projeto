using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Prog1Projeto
{
    internal class Relatorios
    {
        public void ComprovanteEmprestimo(Usuario usuario, string livro, DateTime dataEmprestimo)
        {
            string caminhoArquivo = "$Comprovante_Emprestimo{usuario.Nome}.txt";
            try
            {
                using (StreamWriter writer = new StreamWriter(caminhoArquivo))
                {
                    writer.WriteLine("Comprovante de Empréstimo");
                    writer.WriteLine("------------------------");
                    writer.WriteLine($"Usuário: {usuario.Nome}");
                    writer.WriteLine($"Livro: {livro}");
                    writer.WriteLine($"Data do Empréstimo: {dataEmprestimo.ToShortDateString()}");
                    writer.WriteLine($"Prazo de Devolução: {usuario.CalcularPrazoDevolucao(dataEmprestimo).ToShortDateString()}");
                }
                Console.WriteLine($"Comprovante de empréstimo gerado com sucesso em '{caminhoArquivo}'.");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao gerar o comprovante de empréstimo: {ex.Message}");
            }
        }
    }
}
