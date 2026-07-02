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
        public string Tipo { get; set; }

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
        
        public bool AdicionarUsuario(string nome, string email, int tipo)
        {
            if (string.IsNullOrEmpty(nome) || string.IsNullOrWhiteSpace(email))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Cadastro nulo, tente novamente.");
                return false;
            }

            if (tipo == 1)
            {
                usuarioDAO.InserirUsuario(nome, email, "Professor");
                return true;
            }
            else if (tipo == 2)
            {
                usuarioDAO.InserirUsuario(nome, email, "Aluno");
                return true;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Tipo inválido. Use 1 para Professor ou 2 para Aluno.");
            return false;
        }
        public bool ListarUsuarios(out List<Usuario> usuarios)
        {
            usuarios = usuarioDAO.ListarUsuarios();

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
            resultado_busca = usuarioDAO.BuscarUsuario(pesquisa_nome, pesquisa_email);

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
            Usuario usuario = usuarios.Find(l => l.Id == id);

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
            Usuario usuario = usuarios.Find(l => l.Id == id);

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

    public class InteracoesUsuario
    {
        private GerenciamentoUsuario gerenciador = new GerenciamentoUsuario();

        public void AguardarTecla()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("  Aperte qualquer tecla para continuar:");
            Console.ResetColor();
            Console.Write("  ");
            Console.ReadKey();
        }

        public void MenuAlterar()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(new string('=', 50));
            Console.WriteLine("             Alterar dados de Usuarios");
            Console.WriteLine(new string('=', 50));
            Console.ResetColor();

            Console.WriteLine("\n  Digite o ID do usuario para alterar seus dados:");
            Console.Write("  ");
            int ID_alterar = int.Parse(Console.ReadLine());

            Usuario usuario = new UsuarioDAO().ListarTodos().Find(u => u.Id == ID_alterar);

            if (usuario != null)
            {
                Console.WriteLine($"\n  Usuario encontrado: {usuario.Nome} | Email: {usuario.Email}");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(new string('-', 50));
                Console.ResetColor();

                Console.WriteLine("\n  Novo Nome:");
                Console.Write("  ");
                string novo_nome = Console.ReadLine();
                Console.WriteLine("\n  Novo Email:");
                Console.Write("  ");
                string novo_email = Console.ReadLine();

                bool sucesso = gerenciador.Alterar_usuario(ID_alterar, novo_nome, novo_email);
                Console.WriteLine();

                Console.ForegroundColor = sucesso ? ConsoleColor.Green : ConsoleColor.Red;
                Console.WriteLine(sucesso ? "  Alteracao feita com sucesso" : "  Alteracao mal sucedida");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(new string('-', 50));
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n  Usuario não existente ou ID errado.");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(new string('-', 50));
                Console.ResetColor();
            }
            AguardarTecla();
        }

        public void MenuExcluir()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(new string('=', 50));
            Console.WriteLine("                Excluir Usuarios");
            Console.WriteLine(new string('=', 50));
            Console.ResetColor();

            Console.WriteLine("\n  Digite o ID de um usuario para o excluir:");
            Console.Write("  ");
            int ID_excluir = int.Parse(Console.ReadLine());

            // Corrigido: usar uma instância de UsuarioDAO (método não é estático)
            Usuario usuario = new UsuarioDAO().ListarUsuarios().Find(u => u.Id == ID_excluir);

            if (usuario != null)
            {
                Console.WriteLine($"\n  Usuario encontrado: {usuario.Nome} | Email: {usuario.Email}");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine(new string('-', 50));
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n  Deseja excluir esse usuario (s/n):");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  (ATENCAO:Esse passo é irreversivel)");
                Console.ResetColor();
                Console.Write("  ");
                char confirmacao = char.ToLower(Console.ReadKey().KeyChar);
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Green;
                if (confirmacao == 's')
                {
                    gerenciador.Excluir_usuario(ID_excluir);
                    Console.WriteLine("\nUsuario excluido com sucesso");
                }
                else if (confirmacao == 'n')
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nExclusão cancelada");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nOpcao Invalida");
                }
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine(new string('-', 50));
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n  Usuario com ID( {ID_excluir} ) não encontrado ou não existente");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine(new string('-', 50));
                Console.ResetColor();
            }
            AguardarTecla();
        }
    }
}
