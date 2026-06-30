using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prog1Projeto
{
    public class Emprestimo
    {
        public int ID_emprestimo { get; set; }
        public Livro livro_emprestado { get; set; }
        public string nome_cliente { get; set; }
        public DateTime data_emprestimo { get; set; }
        public string data_devolucao_prevista { get; set; }
        public DateTime? data_devolucao { get; set; }

        public Emprestimo(int ID_emprestimo, Livro livro_emprestado, string nome_cliente, string data_devolucao_prevista)
        {
            this.ID_emprestimo = ID_emprestimo;
            this.livro_emprestado = livro_emprestado;
            this.nome_cliente = nome_cliente;
            this.data_emprestimo = DateTime.Now;
            this.data_devolucao_prevista = data_devolucao_prevista;
            this.data_devolucao = null;
        }

        public Emprestimo(int ID_emprestimo, Livro livro_emprestado, string nome_cliente, DateTime data_emprestimo, string data_devolucao_prevista, DateTime? data_devolucao)
        {
            this.ID_emprestimo = ID_emprestimo;
            this.livro_emprestado = livro_emprestado;
            this.nome_cliente = nome_cliente;
            this.data_emprestimo = data_emprestimo;
            this.data_devolucao_prevista = data_devolucao_prevista;
            this.data_devolucao = data_devolucao;
        }
    }

    public class GerenciamentoEmprestimo
    {
        private static List<Emprestimo> Lista_Emprestimo = new List<Emprestimo>();
        private static EmprestimoDAO EmprestimoDAO = new EmprestimoDAO();

        public bool Realizar_emprestimo(Livro livro_emprestado, string nome_cliente, string data_devolucao_prevista)
        {
            if (livro_emprestado == null) return false;

            return EmprestimoDAO.Inserir(livro_emprestado.ID_livro, nome_cliente, data_devolucao_prevista);
        }

        public bool Mostrar_emprestimo(out List<Emprestimo> emprestimos)
        {
            emprestimos = EmprestimoDAO.ListarTodos();

            if (emprestimos == null || emprestimos.Count == 0)
            {
                return false;
            }
            return true;
        }

        public bool Realizar_devolucao(int ID_emprestimo)
        {
            var emprestimos = EmprestimoDAO.ListarTodos();
            Emprestimo emprestimo = emprestimos.Find(e => e.ID_emprestimo == ID_emprestimo && e.data_devolucao == null);

            if (emprestimo == null) return false;

            return EmprestimoDAO.AtualizarDevolucao(ID_emprestimo);
        }

        public bool Mostrar_emprestimo_aberto(out List<Emprestimo> abertos)
        {
            var todos = EmprestimoDAO.ListarTodos();
            abertos = todos.Where(e => e.data_devolucao == null).ToList();

            if (abertos.Count == 0) return false;
            return true;
        }
    }

    public class InteracoesEmprestimo
    {
        private GerenciamentoEmprestimo gerenciador = new GerenciamentoEmprestimo();
        private GerenciamentoLivro gerenciador_livro = new GerenciamentoLivro();
        private InteracoesLivro interacao_livro = new InteracoesLivro();
        private static EmprestimoDAO EmprestimoDAO = new EmprestimoDAO();

        public InteracoesEmprestimo() { }

        public void MenuEmprestimo()
        {
            interacao_livro.MenuListar();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(new string('=', 50));
            Console.WriteLine("                    Emprestimo");
            Console.WriteLine(new string('=', 50));
            Console.ResetColor();

            Console.WriteLine("\n  Id do livro emprestado:");
            Console.Write("  ");
            int id_livro_emprestado = int.Parse(Console.ReadLine());

            Livro livro = null;
            if (gerenciador_livro.ListarLivros(out List<Livro> livros))
            {
                livro = livros.Find(l => l.ID_livro == id_livro_emprestado);
            }

            if (livro != null)
            {
                Console.WriteLine("\n  Nome do cliente:");
                Console.Write("  ");
                string nome_cliente = Console.ReadLine();
                Console.WriteLine("\n  Data prevista de devolucao:");
                Console.Write("  ");
                string data_devolucao_prevista = Console.ReadLine();

                gerenciador.Realizar_emprestimo(livro, nome_cliente, data_devolucao_prevista);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n  Cadastro de emprestimo feito com sucesso.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("  Nenhum livro cadastrado.\n");
                Console.ResetColor();
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(new string('-', 50));
            Console.ResetColor();
            interacao_livro.AguardarTecla();
        }

        public void MenuDevolucao()
        {
            MenuListarAbertosInterno();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(new string('=', 50));
            Console.WriteLine("                    Devolucao");
            Console.WriteLine(new string('=', 50));
            Console.ResetColor();

            Console.WriteLine("\n  Id do emprestimo que será devolvido:");
            Console.Write("  ");
            int id_emprestimo = int.Parse(Console.ReadLine());

            bool sucesso = gerenciador.Realizar_devolucao(id_emprestimo);

            if (sucesso)
            {
                var emp = EmprestimoDAO.ListarTodos().Find(e => e.ID_emprestimo == id_emprestimo);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n  Devolucao feita com sucesso!");
                Console.WriteLine($"  Livro \"{emp.livro_emprestado.titulo_livro}\" devolvido em: {emp.data_devolucao.Value.ToString("dd/MM/yyyy HH:mm")}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n  Nenhum emprestimo ativo encontrado com o ID ( {id_emprestimo} ).\n");
                Console.ResetColor();
            }

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(new string('-', 50));
            Console.ResetColor();
            interacao_livro.AguardarTecla();
        }

        public void MenuListarAbertos()
        {
            MenuListarAbertosInterno();
            interacao_livro.AguardarTecla();
        }

        private void MenuListarAbertosInterno()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(new string('=', 50));
            Console.WriteLine("               Emprestimos em Aberto");
            Console.WriteLine(new string('=', 50));
            Console.ResetColor();
            Console.WriteLine();

            if (!gerenciador.Mostrar_emprestimo_aberto(out List<Emprestimo> abertos))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  Nenhum emprestimo em aberto no momento.\n");
                Console.ResetColor();
            }
            else
            {
                foreach (var emp in abertos)
                {
                    Console.WriteLine($"  ID Empréstimo: {emp.ID_emprestimo}");
                    Console.WriteLine($"  Livro: {emp.livro_emprestado.titulo_livro} (ID Livro: {emp.livro_emprestado.ID_livro})");
                    Console.WriteLine($"  Cliente: {emp.nome_cliente}");
                    Console.WriteLine($"  Data do Empréstimo: {emp.data_emprestimo.ToString("dd/MM/yyyy")}");
                    Console.WriteLine($"  Previsão de Devolução: {emp.data_devolucao_prevista}");
                    Console.WriteLine(new string('-', 40));
                }
            }

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(new string('-', 50));
            Console.ResetColor();
        }

        public void MenuHistorico()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(new string('=', 50));
            Console.WriteLine("              Historico de Emprestimos");
            Console.WriteLine(new string('=', 50));
            Console.ResetColor();
            Console.WriteLine();

            if (!gerenciador.Mostrar_emprestimo(out List<Emprestimo> todos))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  Nenhum registro de emprestimo encontrado.\n");
                Console.ResetColor();
            }
            else
            {
                foreach (var emp in todos)
                {
                    string status = emp.data_devolucao.HasValue
                        ? $"Devolvido em: {emp.data_devolucao.Value.ToString("dd/MM/yyyy HH:mm")}"
                        : "STATUS: EM ABERTO";

                    Console.WriteLine($"  ID Empréstimo: {emp.ID_emprestimo} | Livro: {emp.livro_emprestado.titulo_livro}");
                    Console.WriteLine($"  Cliente: {emp.nome_cliente} | {status}");
                    Console.WriteLine(new string('-', 50));
                }
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(new string('-', 50));
            Console.ResetColor();
            interacao_livro.AguardarTecla();
        }
    }
}
