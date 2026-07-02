using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Prog1Projeto;

namespace Prog1Projeto
{
    internal class Program
    {
        static InteracoesLivro interacao_livro = new InteracoesLivro();
        static InteracoesEmprestimo interacao_emprestimo = new InteracoesEmprestimo();
        static InteracoesUsuario interacao_usuario = new InteracoesUsuario();
        static GerenciamentoUsuario gerenciador_usuario = new GerenciamentoUsuario();

        static void Main(string[] args)
        {
            int opcao = -1;

            while (opcao != 0)
            {
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(new string('=', 50));
                Console.WriteLine("              SISTEMA BIBLIOTECA");
                Console.WriteLine(new string('=', 50));

                Console.ResetColor();
                Console.WriteLine();

                Console.WriteLine("  1 - Usuarios");
                Console.WriteLine("  2 - Livros");
                Console.WriteLine("  3 - Emprestimos");
                Console.WriteLine("  0 - Sair");

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(new string('-', 50));
                Console.ResetColor();

                Console.Write(" Digite a opção desejada: ");
                opcao = int.Parse(Console.ReadLine());

                switch (opcao)
                {
                    case 1: MenuAbaUsuario(); break;
                    case 2: MenuAbaLivros(); break;
                    case 3: MenuAbaEmprestimo(); break;
                    case 0:
                        Console.WriteLine("\n  Saindo do sistema...");
                        break;
                    default:
                        Console.WriteLine("\n  Opção inválida! Digite um número do menu.");
                        interacao_livro.AguardarTecla();
                        break;
                }
            }
        }

        static void MenuAbaUsuario()
        {
            int opcaoAba = -1;
            while (opcaoAba != 0)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(new string('=', 50));
                Console.WriteLine("                    USUARIOS");
                Console.WriteLine(new string('=', 50));
                Console.ResetColor();
                Console.WriteLine();

                Console.WriteLine("  [1] - Cadastrar Novo Usuário");
                Console.WriteLine("  [2] - Listar Todos os Usuários");
                Console.WriteLine("  [3] - Buscar Usuário");
                Console.WriteLine("  [4] - Alterar Dados de um Usuário");
                Console.WriteLine("  [5] - Remover Usuário");
                Console.WriteLine("  [0] - Voltar");

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(new string('-', 50));
                Console.ResetColor();
                Console.Write(" Digite a opção desejada: ");

                int.TryParse(Console.ReadLine(), out opcaoAba);

                switch (opcaoAba)
                {
                    case 1:
                        Console.WriteLine("\n  Nome:");
                        Console.Write("  ");
                        string nome = Console.ReadLine();
                        Console.WriteLine("\n  Email:");
                        Console.Write("  ");
                        string email = Console.ReadLine();
                        Console.WriteLine("\n  Tipo (1 = Professor, 2 = Aluno):");
                        Console.Write("  ");
                        int.TryParse(Console.ReadLine(), out int tipo);
                        bool adicionou = gerenciador_usuario.AdicionarUsuario(nome, email, tipo);
                        Console.ForegroundColor = adicionou ? ConsoleColor.Green : ConsoleColor.Red;
                        Console.WriteLine(adicionou ? "\n  Cadastro de usuario feito com sucesso." : "\n  Falha ao cadastrar usuario.");
                        Console.ResetColor();
                        interacao_usuario.AguardarTecla();
                        break;

                    case 2:
                        if (gerenciador_usuario.ListarUsuarios(out List<Usuario> lista))
                        {
                            foreach (var u in lista)
                            {
                                Console.WriteLine($"  ID: {u.Id} | Nome: {u.Nome} | Email: {u.Email} | Email: {u}");
                            }
                        }
                        interacao_usuario.AguardarTecla();
                        break;

                    case 3:
                        Console.WriteLine("\n  Nome (ou deixe vazio):");
                        Console.Write("  ");
                        string pesquisaNome = Console.ReadLine();
                        Console.WriteLine("\n  Email (ou deixe vazio):");
                        Console.Write("  ");
                        string pesquisaEmail = Console.ReadLine();
                        if (gerenciador_usuario.Buscar_usuario(pesquisaNome, pesquisaEmail, out List<Usuario> resultados))
                        {
                            foreach (var u in resultados)
                            {
                                Console.WriteLine($"  ID: {u.Id} | Nome: {u.Nome} | Email: {u.Email}");
                            }
                        }
                        interacao_usuario.AguardarTecla();
                        break;

                    case 4:
                        Console.WriteLine("\n  Digite o ID do usuario para alterar seus dados:");
                        Console.Write("  ");
                        if (int.TryParse(Console.ReadLine(), out int idAlterar))
                        {
                            Console.WriteLine("\n  Novo Nome (ou enter para manter):");
                            Console.Write("  ");
                            string novoNome = Console.ReadLine();
                            Console.WriteLine("\n  Novo Email (ou enter para manter):");
                            Console.Write("  ");
                            string novoEmail = Console.ReadLine();
                            bool alterou = gerenciador_usuario.Alterar_usuario(idAlterar, novoNome, novoEmail);
                            Console.ForegroundColor = alterou ? ConsoleColor.Green : ConsoleColor.Red;
                            Console.WriteLine(alterou ? "\n  Alteracao feita com sucesso" : "\n  Alteracao mal sucedida");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\n  ID inválido.");
                            Console.ResetColor();
                        }
                        interacao_usuario.AguardarTecla();
                        break;

                    case 5:
                        Console.WriteLine("\n  Digite o ID de um usuario para o excluir:");
                        Console.Write("  ");
                        if (int.TryParse(Console.ReadLine(), out int idExcluir))
                        {
                            Console.Write("  Confirma exclusão? (s/n): ");
                            char confirm = char.ToLower(Console.ReadKey().KeyChar);
                            Console.WriteLine();
                            if (confirm == 's')
                            {
                                bool excluiu = gerenciador_usuario.Excluir_usuario(idExcluir);
                                Console.ForegroundColor = excluiu ? ConsoleColor.Green : ConsoleColor.Red;
                                Console.WriteLine(excluiu ? "\n  Usuario excluido com sucesso" : "\n  Falha ao excluir usuario");
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\n  Exclusão cancelada");
                                Console.ResetColor();
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\n  ID inválido.");
                            Console.ResetColor();
                        }
                        interacao_usuario.AguardarTecla();
                        break;

                    case 0:
                        break;

                    default:
                        Console.WriteLine("\n  Opção inválida!");
                        interacao_usuario.AguardarTecla();
                        break;
                }
            }
        }

        static void MenuAbaLivros()
        {
            int opcaoAba = -1;
            while (opcaoAba != 0)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(new string('=', 50));
                Console.WriteLine("                    LIVROS");
                Console.WriteLine(new string('=', 50));
                Console.ResetColor();
                Console.WriteLine();

                Console.WriteLine("  [1] - Cadastrar Novo Livro");
                Console.WriteLine("  [2] - Listar Todos os Livros");
                Console.WriteLine("  [3] - Buscar Livro");
                Console.WriteLine("  [4] - Alterar Dados de um Livro");
                Console.WriteLine("  [5] - Remover Livro");
                Console.WriteLine("  [0] - Voltar");

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(new string('-', 50));
                Console.ResetColor();
                Console.Write(" Digite a opção desejada: ");

                opcaoAba = int.Parse(Console.ReadLine());

                switch (opcaoAba)
                {
                    case 1: interacao_livro.MenuCadastrar(); break;
                    case 2: interacao_livro.MenuListar(); break;
                    case 3: interacao_livro.MenuBuscar(); break;
                    case 4: interacao_livro.MenuAlterar(); break;
                    case 5: interacao_livro.MenuExcluir(); break;
                    case 0: break;
                    default:
                        Console.WriteLine("\n  Opção inválida!");
                        interacao_livro.AguardarTecla();
                        break;
                }

            }
        }

        static void MenuAbaEmprestimo()
        {
            int opcaoAba = -1;
            while (opcaoAba != 0)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(new string('=', 50));
                Console.WriteLine("                    Emprestimo");
                Console.WriteLine(new string('=', 50));
                Console.ResetColor();
                Console.WriteLine();

                Console.WriteLine("  [1] - Realizar Emprestimo");
                Console.WriteLine("  [2] - Registrar Devolucao");
                Console.WriteLine("  [3] - Listar Emprestimos Abertos");
                Console.WriteLine("  [4] - Histórico de Empréstimos");
                Console.WriteLine("  [0] - Voltar");

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(new string('-', 50));
                Console.ResetColor();
                Console.Write(" Digite a opção desejada: ");

                opcaoAba = int.Parse(Console.ReadLine());

                switch (opcaoAba)
                {
                    case 1: interacao_emprestimo.MenuEmprestimo(); break;
                    case 2: interacao_emprestimo.MenuDevolucao(); break;
                    case 3: interacao_emprestimo.MenuListarAbertos(); break;
                    case 4: interacao_emprestimo.MenuHistorico(); break;
                    case 0: break;
                    default:
                        Console.WriteLine("\n  Opção inválida!");
                        interacao_livro.AguardarTecla();
                        break;
                }

            }
        }
    }
}
