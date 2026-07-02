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
                leitor.GetString("autor"),
                leitor.GetInt32("ano"),
                leitor.GetBoolean("disponivel")
            );
        }

        private static DateTime? ReadNullableDate(MySqlDataReader leitor, string columnName)
        {
            int ord = leitor.GetOrdinal(columnName);
            if (leitor.IsDBNull(ord)) return null;

            object val = leitor.GetValue(ord);

            if (val is DateTime dt) return dt;

            if (val is string s)
            {
                if (string.IsNullOrWhiteSpace(s) || s.StartsWith("0000-00-00")) return null;
                DateTime parsed;
                if (DateTime.TryParse(s, out parsed)) return parsed;
                return null;
            }
            try
            {
                return Convert.ToDateTime(val);
            }
            catch
            {
                return null;
            }
        }

        private static DateTime ReadDateSafe(MySqlDataReader leitor, string columnName, DateTime fallback)
        {
            var maybe = ReadNullableDate(leitor, columnName);
            return maybe ?? fallback;
        }

        private static Emprestimo MapearEmprestimo(MySqlDataReader leitor)
        {
            DateTime? data_devolucao = ReadNullableDate(leitor, "data_devolucao");

            DateTime data_emprestimo = ReadDateSafe(leitor, "data_emprestimo", DateTime.MinValue);

            var data_prevista_dt = ReadNullableDate(leitor, "data_prevista");
            string data_prevista_str = data_prevista_dt?.ToString("yyyy-MM-dd") ?? null;

            return new Emprestimo(
                leitor.GetInt32("id"),
                MapearLivro(leitor),
                leitor.GetString("nome"),
                data_emprestimo,
                data_prevista_str,
                data_devolucao
            );
        }

        public static bool InserirEmprestimo(int idLivro, int usuarioId)
        {
            Usuario usuario = new UsuarioDAO().ObterPorId(usuarioId);
            if (usuario == null)
            {
                return false;
            }

            using (MySqlConnection conexao = Conexaobd.fazerconexao())
            {
                conexao.Open();

                if (!LivroEstaDisponivel(conexao, idLivro))
                {
                    return false;
                }

                DateTime dataEmprestimo = DateTime.Now;
                DateTime dataPrevista = usuario.CalcularPrazoDevolucao(dataEmprestimo);

                using (MySqlTransaction transacao = conexao.BeginTransaction())
                {
                    try
                    {
                        string insertSql = @"INSERT INTO emprestimos (usuarioId, livroId, data_emprestimo, data_prevista, data_devolucao)
                                             VALUES (@usuarioId, @livroId, @dataEmprestimo, @dataPrevista, NULL)";

                        using (MySqlCommand comando = new MySqlCommand(insertSql, conexao, transacao))
                        {
                            comando.Parameters.AddWithValue("@usuarioId", usuarioId);
                            comando.Parameters.AddWithValue("@livroId", idLivro);
                            comando.Parameters.AddWithValue("@dataEmprestimo", dataEmprestimo.ToString("yyyy-MM-dd"));
                            comando.Parameters.AddWithValue("@dataPrevista", dataPrevista.ToString("yyyy-MM-dd"));
                            comando.ExecuteNonQuery();
                        }

                        string updateSql = "UPDATE livros SET disponivel = FALSE WHERE id = @id";
                        using (MySqlCommand comandoUpdate = new MySqlCommand(updateSql, conexao, transacao))
                        {
                            comandoUpdate.Parameters.AddWithValue("@id", idLivro);
                            comandoUpdate.ExecuteNonQuery();
                        }

                        transacao.Commit();
                        return true;
                    }
                    catch
                    {
                        transacao.Rollback();
                        return false;
                    }
                }
            }
        }


        public static bool LivroEstaDisponivel(int idLivro)
        {
            using (MySqlConnection conexao = Conexaobd.fazerconexao())
            {
                conexao.Open();
                return LivroEstaDisponivel(conexao, idLivro);
            }
        }

        public static bool LivroEstaDisponivel(MySqlConnection conexao, int idLivro)
        {
            string sql = "SELECT disponivel FROM livros WHERE id = @id LIMIT 1";

            using (MySqlCommand comando = new MySqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("@id", idLivro);

                object resultado = comando.ExecuteScalar();
                if (resultado == null || resultado == DBNull.Value)
                {
                    return false;
                }

                return Convert.ToBoolean(resultado);
            }
        }

        public static List<Emprestimo> ListarEmprestimos()
        {
            List<Emprestimo> emprestimos = new List<Emprestimo>();

            using (MySqlConnection conexao = Conexaobd.fazerconexao())
            {
                conexao.Open();

                string sql = @"SELECT e.id, e.livroId, e.data_emprestimo, e.data_prevista, e.data_devolucao,
                                      u.nome, l.titulo, l.autor, l.ano, l.disponivel
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

        public static Emprestimo ObterPorId(int ID_emprestimo)
        {
            using (MySqlConnection conexao = Conexaobd.fazerconexao())
            {
                conexao.Open();

                string sql = @"SELECT e.id, e.livroId, e.data_emprestimo, e.data_prevista, e.data_devolucao,
                                      u.nome, l.titulo, l.autor, l.ano, l.disponivel
                               FROM emprestimos e
                               JOIN usuarios u ON e.usuarioId = u.id
                               JOIN livros l ON e.livroId = l.id
                               WHERE e.id = @id";

                using (MySqlCommand comando = new MySqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@id", ID_emprestimo);

                    using (MySqlDataReader leitor = comando.ExecuteReader())
                    {
                        if (leitor.Read())
                        {
                            return MapearEmprestimo(leitor);
                        }
                    }
                }
            }

            return null;
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