using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Prog1Projeto
{
    public class LivroDAO
    {
        private static Livro MapearLivro(MySqlDataReader leitor)
        {
            return new Livro(
                leitor.GetInt32("id"),
                leitor.GetString("titulo"),
                leitor.GetString("autor")
            );
        }

        public static void Inserir(string titulo, string autor)
        {
            using (MySqlConnection conexao = Conexaobd.fazerconexao())
            {
                conexao.Open();

                string sql = "INSERT INTO livros (titulo, autor, ano, disponivel) VALUES (@titulo, @autor, 0, TRUE)";

                using (MySqlCommand comando = new MySqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@titulo", titulo);
                    comando.Parameters.AddWithValue("@autor", autor);
                    comando.ExecuteNonQuery();
                }
            }
        }

        public static List<Livro> ListarLivros()
        {
            List<Livro> livros = new List<Livro>();

            using (MySqlConnection conexao = Conexaobd.fazerconexao())
            {
                conexao.Open();

                string sql = "SELECT id, titulo, autor FROM livros";

                using (MySqlCommand comando = new MySqlCommand(sql, conexao))
                using (MySqlDataReader leitor = comando.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        livros.Add(MapearLivro(leitor));
                    }
                }
            }

            return livros;
        }

        public static List<Livro> ListarTodos()
        {
            return ListarLivros();
        }

        public static List<Livro> buscar_livro(string pesquisa_titulo, string pesquisa_autor)
        {
            List<Livro> livros = new List<Livro>();

            using (MySqlConnection conexao = Conexaobd.fazerconexao())
            {
                conexao.Open();

                string sql = "SELECT id, titulo, autor FROM livros WHERE 1 = 1";

                if (!string.IsNullOrWhiteSpace(pesquisa_titulo))
                {
                    sql += " AND titulo LIKE @titulo";
                }

                if (!string.IsNullOrWhiteSpace(pesquisa_autor))
                {
                    sql += " AND autor LIKE @autor";
                }

                using (MySqlCommand comando = new MySqlCommand(sql, conexao))
                {
                    if (!string.IsNullOrWhiteSpace(pesquisa_titulo))
                    {
                        comando.Parameters.AddWithValue("@titulo", "%" + pesquisa_titulo + "%");
                    }

                    if (!string.IsNullOrWhiteSpace(pesquisa_autor))
                    {
                        comando.Parameters.AddWithValue("@autor", "%" + pesquisa_autor + "%");
                    }

                    using (MySqlDataReader leitor = comando.ExecuteReader())
                    {
                        while (leitor.Read())
                        {
                            livros.Add(MapearLivro(leitor));
                        }
                    }
                }
            }

            return livros;
        }

        public static bool Atualizar(int id, string novo_titulo, string novo_autor)
        {
            using (MySqlConnection conexao = Conexaobd.fazerconexao())
            {
                conexao.Open();

                string sql = "UPDATE livros SET titulo = @titulo, autor = @autor WHERE id = @id";

                using (MySqlCommand comando = new MySqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@id", id);
                    comando.Parameters.AddWithValue("@titulo", novo_titulo);
                    comando.Parameters.AddWithValue("@autor", novo_autor);

                    return comando.ExecuteNonQuery() > 0;
                }
            }
        }

        public static bool Excluir(int id)
        {
            using (MySqlConnection conexao = Conexaobd.fazerconexao())
            {
                conexao.Open();

                string sql = "DELETE FROM livros WHERE id = @id";

                using (MySqlCommand comando = new MySqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@id", id);
                    return comando.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}