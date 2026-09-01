using GerenciadorClientes;

class Program
{
    private static ClienteView _clienteView = new ClienteView();

    static void Main(string[] args)
    {
        bool executar = true;

        while (executar)
        {
            Console.Clear();
            Console.WriteLine("=========================================");
            Console.WriteLine("        GERENCIADOR DE CLIENTES          ");
            Console.WriteLine("=========================================");
            Console.WriteLine("1 - Listar Clientes");
            Console.WriteLine("2 - Cadastrar Novo Cliente");
            Console.WriteLine("3 - Atualizar Cliente/Endereço");
            Console.WriteLine("0 - Sair");
            Console.WriteLine("=========================================");
            Console.Write("Escolha uma opção: ");

            string opcao = Console.ReadLine()?.Trim();

            switch (opcao)
            {
                case "1":
                    _clienteView.ExibirListaClientes();
                    break;
                case "2":
                    _clienteView.ExecutarCadastrarCliente();
                    break;
                case "3":
                    _clienteView.MenuEditar(); // Chamada do novo menu de edição
                    break;
                case "0":
                    executar = false;
                    Console.WriteLine("\nSaindo do sistema...");
                    break;
                default:
                    Console.WriteLine("\nOpção inválida! Pressione qualquer tecla para continuar...");
                    Console.ReadKey();
                    break;
            }
        }
    }
}