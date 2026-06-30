using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Prog1Projeto
{
    public class EmprestimoDAO
    {
        private static Livro MapearLivro(MySqlDataReader leitor)
        {
            return new Livro(
                leitor.GetInt32("livroId"),
                leitor.GetString("titulo"),
                leitor.GetString("autor")
            );
        }

        private static Emprestimo MapearEmprestimo(MySqlDataReader leitor)
        {
            DateTime? data_devolucao = leitor.IsDBNull(leitor.GetOrdinal("data_devolucao"))
                ? null
                : leitor.GetDateTime("data_devolucao");

            return new Emprestimo(
                leitor.GetInt32("id"),
                MapearLivro(leitor),
                leitor.GetString("nome"),
                leitor.GetDateTime("data_emprestimo"),
                leitor.GetDateTime("data_prevista").ToString("yyyy-MM-dd"),
                data_devolucao
            );
        }

        public static bool InserirEmprestimo(int idLivro, string nomeCliente, string dataPrevista)
        {
            using (MySqlConnection conexao = Conexaobd.fazerconexao())
            {
                conexao.Open();

                string sql = @"INSERT INTO emprestimos (usuarioId, livroId, data_emprestimo, data_prevista, data_devolucao)
                              VALUES (@usuarioId, @livroId, @dataEmprestimo, @dataPrevista, NULL)";

                using (MySqlCommand comando = new MySqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@usuarioId", 1);
                    comando.Parameters.AddWithValue("@livroId", idLivro);
                    comando.Parameters.AddWithValue("@dataEmprestimo", DateTime.Now.ToString("yyyy-MM-dd"));
                    comando.Parameters.AddWithValue("@dataPrevista", dataPrevista);
                    return comando.ExecuteNonQuery() > 0;
                }
            }
        }

        public static List<Emprestimo> ListarEmprestimos()
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

        public static bool AtualizarDevolucao(int ID_emprestimo)
        {
            using (MySqlConnection conexao = Conexaobd.fazerconexao())
            {
                conexao.Open();

                string sql = "UPDATE emprestimos SET data_devolucao = @dataDevolucao WHERE id = @id";

                using (MySqlCommand comando = new MySqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@id", ID_emprestimo);
                    comando.Parameters.AddWithValue("@dataDevolucao", DateTime.Now.ToString("yyyy-MM-dd"));
                    return comando.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}