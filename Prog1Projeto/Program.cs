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
                    case 1: MenuAbaLivros(); break;
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
