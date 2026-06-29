using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Prog1Projeto
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Iniciando o teste do Gerador de Relatórios...\n");

            Aluno alunoTeste = new Aluno();
            alunoTeste.Id = 1;
            alunoTeste.Nome = "Pablo";
            alunoTeste.Email = "pablo@teste.com";
            string nomeDoLivro = "Programação Orientada a Objetos em C#";
            DateTime dataDeHoje = DateTime.Now;
            Relatorios gerador = new Relatorios();
            gerador.ComprovanteEmprestimo(alunoTeste, nomeDoLivro, dataDeHoje);

            Console.WriteLine("\nTeste concluído! Pressione qualquer tecla para sair...");
            Console.ReadKey();
        }
    }
}
