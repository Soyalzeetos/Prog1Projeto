using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MySql.Data.MySqlClient;

namespace Prog1Projeto
{
    public class RelatorioDAO
    {
        public List<Emprestimo> ListarEmprestimos()
        {
            List<Emprestimo> emprestimos = new List<Emprestimo>();

            using (MySqlConnection conexao = Conexaobd.fazerconexao())
            {
                conexao.Open();

                string sql = @"SELECT e.id, e.livroId, e.data_emprestimo, e.data_prevista, e.data_devolucao,
                                      u.nome, l.titulo, l.autor
                               FROM emprestimos e
                               JOIN usuarios u ON e.usuarioId = u.id
                               JOIN livros l ON e.livroId = l.id";

                using (MySqlCommand comando = new MySqlCommand(sql, conexao))
                using (MySqlDataReader leitor = comando.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        emprestimos.Add(MapearEmprestimo(leitor));
                    }
                }
            }

            return emprestimos;
        }

        public void GerarRelatorioEmprestimos()
        {
            var emprestimos = ListarEmprestimos();
            string nomeArquivo = $"Relatorio_Emprestimos_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            string caminhoArquivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nomeArquivo);

            try
            {
                using (StreamWriter writer = new StreamWriter(caminhoArquivo))
                {
                    writer.WriteLine("Relatório de Empréstimos");
                    writer.WriteLine("------------------------");

                    if (emprestimos == null || emprestimos.Count == 0)
                    {
                        writer.WriteLine("Nenhum empréstimo registrado.");
                    }
                    else
                    {
                        foreach (var emprestimo in emprestimos)
                        {
                            string status = emprestimo.data_devolucao.HasValue
                                ? $"Devolvido em {emprestimo.data_devolucao.Value:dd/MM/yyyy}"
                                : "Em aberto";

                            writer.WriteLine($"ID: {emprestimo.ID_emprestimo}");
                            writer.WriteLine($"Cliente: {emprestimo.nome_cliente}");
                            writer.WriteLine($"Livro: {emprestimo.livro_emprestado.titulo_livro}");
                            writer.WriteLine($"Data do empréstimo: {emprestimo.data_emprestimo:dd/MM/yyyy}");
                            writer.WriteLine($"Previsão de devolução: {emprestimo.data_devolucao_prevista}");
                            writer.WriteLine($"Status: {status}");
                            writer.WriteLine("------------------------");
                        }
                    }
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Relatório gerado com sucesso em '{caminhoArquivo}'.");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Erro ao gerar o relatório: {ex.Message}");
                Console.ResetColor();
            }
        }

        public void GerarComprovanteEmprestimo(string nomeUsuario, string tituloLivro, DateTime dataEmprestimo, DateTime prazoDevolucao)
        {
            string nomeArquivo = $"Comprovante_Emprestimo_{SanitizarNome(nomeUsuario)}.txt";
            string caminhoArquivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nomeArquivo);

            try
            {
                using (StreamWriter writer = new StreamWriter(caminhoArquivo))
                {
                    writer.WriteLine("Comprovante de Empréstimo");
                    writer.WriteLine("------------------------");
                    writer.WriteLine($"Usuário: {nomeUsuario}");
                    writer.WriteLine($"Livro: {tituloLivro}");
                    writer.WriteLine($"Data do Empréstimo: {dataEmprestimo:dd/MM/yyyy}");
                    writer.WriteLine($"Prazo de Devolução: {prazoDevolucao:dd/MM/yyyy}");
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Comprovante de empréstimo gerado em '{caminhoArquivo}'.");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Erro ao gerar o comprovante de empréstimo: {ex.Message}");
                Console.ResetColor();
            }
        }

        private Livro MapearLivro(MySqlDataReader leitor)
        {
            return new Livro(
                leitor.GetInt32("livroId"),
                leitor.GetString("titulo"),
                leitor.GetString("autor")
            );
        }

        private Emprestimo MapearEmprestimo(MySqlDataReader leitor)
        {
            DateTime? dataDevolucao = leitor.IsDBNull(leitor.GetOrdinal("data_devolucao"))
                ? (DateTime?)null
                : leitor.GetDateTime("data_devolucao");

            return new Emprestimo(
                leitor.GetInt32("id"),
                MapearLivro(leitor),
                leitor.GetString("nome"),
                leitor.GetDateTime("data_emprestimo"),
                leitor.GetDateTime("data_prevista").ToString("yyyy-MM-dd"),
                dataDevolucao
            );
        }

        private string SanitizarNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                return "usuario";
            }

            return string.Concat(nome.Where(char.IsLetterOrDigit).Select(c => char.ToLowerInvariant(c)));
        }
    }
}
