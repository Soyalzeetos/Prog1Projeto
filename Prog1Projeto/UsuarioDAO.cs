using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Prog1Projeto
{
    public class UsuarioDAO
    {
        private static Usuario MapearUsuario(MySqlDataReader leitor)
        {
            int id = leitor.GetInt32("id");
            string nome = leitor.GetString("nome");
            string email = leitor.GetString("email");
            string tipo = leitor.GetString("tipo");

            if (tipo == "Professor")
            {
                return new Professor(id, nome, email);
            }

            return new Aluno(id, nome, email);
        }

        public void InserirUsuario(string nome, string email)
        {
            using (MySqlConnection conexao = Conexaobd.fazerconexao())
            {
                conexao.Open();

                string sql = "INSERT INTO usuarios (nome, email, tipo) VALUES (@nome, @email, @tipo)";

                using (MySqlCommand comando = new MySqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@nome", nome);
                    comando.Parameters.AddWithValue("@email", email);
                    comando.Parameters.AddWithValue("@tipo", "Aluno");
                    comando.ExecuteNonQuery();
                }
            }
        }

        public List<Usuario> ListarUsuarios()
        {
            List<Usuario> usuarios = new List<Usuario>();

            using (MySqlConnection conexao = Conexaobd.fazerconexao())
            {
                conexao.Open();

                string sql = "SELECT id, nome, email, tipo FROM usuarios";

                using (MySqlCommand comando = new MySqlCommand(sql, conexao))
                using (MySqlDataReader leitor = comando.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        usuarios.Add(MapearUsuario(leitor));
                    }
                }
            }

            return usuarios;
        }

        public List<Usuario> ListarTodos()
        {
            return ListarUsuarios();
        }

        public List<Usuario> BuscarUsuario(string pesquisa_nome, string pesquisa_email)
        {
            List<Usuario> usuarios = new List<Usuario>();

            using (MySqlConnection conexao = Conexaobd.fazerconexao())
            {
                conexao.Open();

                string sql = "SELECT id, nome, email, tipo FROM usuarios WHERE 1 = 1";

                if (!string.IsNullOrWhiteSpace(pesquisa_nome))
                {
                    sql += " AND nome LIKE @nome";
                }

                if (!string.IsNullOrWhiteSpace(pesquisa_email))
                {
                    sql += " AND email LIKE @email";
                }

                using (MySqlCommand comando = new MySqlCommand(sql, conexao))
                {
                    if (!string.IsNullOrWhiteSpace(pesquisa_nome))
                    {
                        comando.Parameters.AddWithValue("@nome", "%" + pesquisa_nome + "%");
                    }

                    if (!string.IsNullOrWhiteSpace(pesquisa_email))
                    {
                        comando.Parameters.AddWithValue("@email", "%" + pesquisa_email + "%");
                    }

                    using (MySqlDataReader leitor = comando.ExecuteReader())
                    {
                        while (leitor.Read())
                        {
                            usuarios.Add(MapearUsuario(leitor));
                        }
                    }
                }
            }

            return usuarios;
        }

        public bool Atualizar(int id, string novo_nome, string novo_email)
        {
            using (MySqlConnection conexao = Conexaobd.fazerconexao())
            {
                conexao.Open();

                string sql = "UPDATE usuarios SET nome = @nome, email = @email WHERE id = @id";

                using (MySqlCommand comando = new MySqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@id", id);
                    comando.Parameters.AddWithValue("@nome", novo_nome);
                    comando.Parameters.AddWithValue("@email", novo_email);
                    return comando.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Excluir(int id)
        {
            using (MySqlConnection conexao = Conexaobd.fazerconexao())
            {
                conexao.Open();

                string sql = "DELETE FROM usuarios WHERE id = @id";

                using (MySqlCommand comando = new MySqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@id", id);
                    return comando.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
