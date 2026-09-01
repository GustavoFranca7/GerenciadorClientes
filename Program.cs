using System;

namespace GerenciadorClientes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Instanciamos a classe de visão/interface
            ClienteView clienteView = new ClienteView();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("\x1b[38;2;255;165;0mEste texto é Laranja RGB puro!\x1b[0m");
                Console.WriteLine("========================================");
                Console.WriteLine("     SISTEMA DE GESTÃO DE CLIENTES      ");
                Console.WriteLine("========================================");
                Console.WriteLine("1 - Listar Clientes");
                Console.WriteLine("2 - Adicionar Novo Cliente");
                Console.WriteLine("0 - Sair do Sistema");
                Console.WriteLine("========================================");
                Console.Write("Escolha uma opção: ");

                string opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        clienteView.ExibirListaClientes();
                        break;

                    case "2":
                        clienteView.ExecutarCadastrarCliente();
                        break;

                    case "0":
                        Console.WriteLine("\nEncerrando o programa... Até logo!");
                        return;

                    default:
                        Console.WriteLine("\nOpção inválida! Pressione qualquer tecla...");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}