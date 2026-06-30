using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
<<<<<<< HEAD
using Prog1Projeto;
=======
using MySql.Data.MySqlClient;
>>>>>>> aa389bb542821a46c08c9b070e1ca48b28929089

namespace Prog1Projeto
{
    public class Livro
    {
        public int ID_livro { get; set; }
        public string titulo_livro { get; set; }
        public string autor_livro { get; set; }

        public Livro(int ID_livro, string titulo_livro, string autor_livro)
        {
            this.ID_livro = ID_livro;
            this.titulo_livro = titulo_livro;
            this.autor_livro = autor_livro;
        }
    }

    public class GerenciamentoLivro
    {
        private static List<Livro> Lista_Livro = new List<Livro>();
        private static LivroDAO LivroDAO = new LivroDAO();

        private int proximoID = 1;

        public bool Adicionar_livro(string titulo, string autor)
        {
            if (string.IsNullOrWhiteSpace(titulo) || string.IsNullOrWhiteSpace(autor))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Cadastro nulo, tente novamente.");
                return false;
            }

            LivroDAO.Inserir(titulo, autor);
            return true;
        }

        public bool ListarLivros(out List<Livro> livros)
        {
            livros = LivroDAO.ListarLivros();

            if (livros.Count == 0 || livros == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  Nenhum livro cadastrado no banco de dados.\n");
                Console.ResetColor();
                return false;
            }

            return true;
        }

        public bool Buscar_livro(string pesquisa_titulo, string pesquisa_autor, out List<Livro> resultado_busca)
        {
            resultado_busca = LivroDAO.buscar_livro(pesquisa_titulo, pesquisa_autor);

            if (resultado_busca.Count == 0 || resultado_busca == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  Nenhum livro com esses dados encontrado no banco de dados.\n");
                Console.ResetColor();
                return false;
            }

            return true;
        }

        public bool Alterar_livro(int id, string novo_titulo, string novo_autor)
        {
            var livros = LivroDAO.ListarTodos();
            Livro livro = livros.Find(l => l.ID_livro == id);

            if (livro == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n  Livro com ID( {id} ) não encontrado ou não existente");
                Console.ResetColor();
                return false;
            }
            if (string.IsNullOrWhiteSpace(novo_titulo)) novo_titulo = livro.titulo_livro;
            if (string.IsNullOrWhiteSpace(novo_autor)) novo_autor = livro.autor_livro;

            return LivroDAO.Atualizar(id, novo_titulo, novo_autor);
        }

        public bool Excluir_livro(int id)
        {
            var livros = LivroDAO.ListarTodos();
            Livro livro = livros.Find(l => l.ID_livro == id);

            if (livro == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n  Livro com ID( {id} ) não encontrado ou não existente");
                Console.ResetColor();
                return false;
            }

            return LivroDAO.Excluir(id);
        }
    }

    public class InteracoesLivro
    {
        private GerenciamentoLivro gerenciador = new GerenciamentoLivro();

        public void AguardarTecla()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("  Aperte qualquer tecla para continuar:");
            Console.ResetColor();
            Console.Write("  ");
            Console.ReadKey();
        }

        public void MenuCadastrar()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(new string('=', 50));
            Console.WriteLine("                    Cadastro");
            Console.WriteLine(new string('=', 50));
            Console.ResetColor();

            Console.WriteLine("\n  Titulo do livro:");
            Console.Write("  ");
            string titulo_livro = Console.ReadLine();

            Console.WriteLine("\n  Autor do livro:");
            Console.Write("  ");
            string autor_livro = Console.ReadLine();

            bool sucesso = gerenciador.Adicionar_livro(titulo_livro, autor_livro);

            if (sucesso)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n  Cadastro de livro feito com sucesso.");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(new string('-', 50));
                Console.ResetColor();
                AguardarTecla();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(new string('-', 50));
            Console.ResetColor();
            AguardarTecla();
        }

        public void MenuListar()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(new string('=', 50));
            Console.WriteLine("                 Lista de Livros");
            Console.WriteLine(new string('=', 50));
            Console.ResetColor();
            Console.WriteLine();

            if (gerenciador.ListarLivros(out List<Livro> listaDeLivros))
            {
                foreach (var livro in listaDeLivros)
                {
                    Console.WriteLine($"  ID: {livro.ID_livro} | Titulo: {livro.titulo_livro} | Autor: {livro.autor_livro}");
                }

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\n" + new string('-', 50));
                Console.ResetColor();
            }
            AguardarTecla();
        }

        public void MenuBuscar()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(new string('=', 50));
            Console.WriteLine("                 Busca de Livros");
            Console.WriteLine(new string('=', 50));
            Console.ResetColor();

            Console.WriteLine("\n  Titulo:");
            Console.Write("  ");
            string pesquisa_titulo = Console.ReadLine();
            Console.WriteLine("\n  Autor:");
            Console.Write("  ");
            string pesquisa_autor = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("\n" + new string('=', 50));
            Console.WriteLine("               Resultado da busca");
            Console.WriteLine(new string('=', 50));
            Console.ResetColor();

            bool sucesso = gerenciador.Buscar_livro(pesquisa_titulo, pesquisa_autor, out List<Livro> busca);

            if (sucesso)
            {
                foreach (var livro in busca)
                {
                    Console.WriteLine($"  ID: {livro.ID_livro} | Titulo: {livro.titulo_livro} | Autor: {livro.autor_livro}");
                }
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("\n" + new string('-', 50));
                Console.ResetColor();
            }
            AguardarTecla();
        }

        public void MenuAlterar()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(new string('=', 50));
            Console.WriteLine("             Alterar dados de Livros");
            Console.WriteLine(new string('=', 50));
            Console.ResetColor();

            Console.WriteLine("\n  Digite o ID do livro para alterar seus dados:");
            Console.Write("  ");
            int ID_alterar = int.Parse(Console.ReadLine());

            Livro livro = LivroDAO.ListarTodos().Find(l => l.ID_livro == ID_alterar);

            if (livro != null)
            {
                Console.WriteLine($"\n  Livro encontrado: {livro.titulo_livro} | Autor: {livro.autor_livro}");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(new string('-', 50));
                Console.ResetColor();

                Console.WriteLine("\n  Novo Tiulo:");
                Console.Write("  ");
                string novo_titulo = Console.ReadLine();
                Console.WriteLine("\n  Novo Autor:");
                Console.Write("  ");
                string novo_autor = Console.ReadLine();

                bool sucesso = gerenciador.Alterar_livro(ID_alterar, novo_titulo, novo_autor);
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
                Console.WriteLine("\n  Livro não existente ou ID errado.");
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
            Console.WriteLine("                Excluir Livros");
            Console.WriteLine(new string('=', 50));
            Console.ResetColor();

            Console.WriteLine("\n  Digite o ID de um livro para o excluir:");
            Console.Write("  ");
            int ID_excluir = int.Parse(Console.ReadLine());

            Livro livro = LivroDAO.ListarTodos().Find(l => l.ID_livro == ID_excluir);

            if (livro != null)
            {
                Console.WriteLine($"\n  Livro encontrado: {livro.titulo_livro} | Autor: {livro.autor_livro}");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine(new string('-', 50));
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n  Deseja excluir esse livro (s/n):");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  (ATENCAO:Esse passo é irreversivel)");
                Console.ResetColor();
                Console.Write("  ");
                char confirmacao = char.ToLower(Console.ReadKey().KeyChar);
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Green;
                if (confirmacao == 's')
                {
                    gerenciador.Excluir_livro(ID_excluir);
                    Console.WriteLine("\nLivro excluido com sucesso");
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
                Console.WriteLine($"\n  Livro com ID( {ID_excluir} ) não encontrado ou não existente");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine(new string('-', 50));
                Console.ResetColor();
            }
            AguardarTecla();
        }
    }
}
