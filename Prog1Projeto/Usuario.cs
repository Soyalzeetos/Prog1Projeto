using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Prog1Projeto
{

    public abstract class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }

        public abstract DateTime CalcularPrazoDevolucao(DateTime dataEmprestimo);

        public Usuario(int id, string nome, string email)
        {
            this.Id = id;
            this. Nome = nome;
            this.Email = email;
        }
    }

    public class GerenciamentoUsuario
    {
        private static List<Usuario> usuarios = new List<Usuario>();
        private static UsuarioDAO usuarioDAO = new UsuarioDAO();

        private int proximoId = 1;
        
        public bool AdicionarUsuario(string nome, string email)
        {
            if (string.IsNullOrEmpty(nome) || string.IsNullOrWhiteSpace(email))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Cadastro nulo, tente novamente.");
                return false;
            }
            usuarioDAO.InserirUsuario(nome, email);
            return true;
        }
        public bool ListarUsuarios(out List<Usuario> usuarios)
        {
            usuarios = usuarioDAO.ListarLivros();

            if (usuarios.Count == 0 || usuarios == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  Nenhum usuario cadastrado no banco de dados.\n");
                Console.ResetColor();
                return false;
            }

            return true;
        }

        public bool Buscar_usuario(string pesquisa_nome, string pesquisa_email, out List<Usuario> resultado_busca)
        {
            resultado_busca = usuarioDAO.buscar_livro(pesquisa_nome, pesquisa_email);

            if (resultado_busca.Count == 0 || resultado_busca == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  Nenhum usuario com esses dados encontrado no banco de dados.\n");
                Console.ResetColor();
                return false;
            }

            return true;
        }

        public bool Alterar_usuario(int id, string novo_nome, string novo_email)
        {
            var usuarios = usuarioDAO.ListarTodos();
            Usuario usuario = usuarios.Find(l => l.ID_livro == id);

            if (usuario == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n  Usuario com ID( {id} ) não encontrado ou não existente");
                Console.ResetColor();
                return false;
            }
            if (string.IsNullOrWhiteSpace(novo_nome)) novo_nome = usuario.Nome;
            if (string.IsNullOrWhiteSpace(novo_email)) novo_email = usuario.Email;

            return LivroDAO.Atualizar(id, novo_nome, novo_email);
        }
        public bool Excluir_usuario(int id)
        {
            var usuarios = usuarioDAO.ListarTodos();
            Usuario usuario = usuarios.Find(l => l.ID_livro == id);

            if (usuario == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n  Usuario com ID( {id} ) não encontrado ou não existente");
                Console.ResetColor();
                return false;
            }

            return usuarioDAO.Excluir(id);
        }
    }
}
