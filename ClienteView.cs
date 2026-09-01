using System;
using System.Collections.Generic;

namespace GerenciadorClientes
{
    // Classe responsável pela interface de console para operações relacionadas a Cliente.
    // Contém métodos para exibir a lista de clientes e cadastrar um novo cliente,
    // delegando a persistência para ClienteDAO.
    public class ClienteView
    {   
        private ClienteDAO _clienteDao = new ClienteDAO();// Instância do DAO que comunica com o banco (Oracle) para CRUD de clientes.
        private EnderecoView _enderecoView = new EnderecoView(); // Instância da View de Endereço
        private EnderecoDAO _enderecoDao = new EnderecoDAO(); // Instância do DAO de Endereço

        // Método: ExibirListaClientes
        // Descrição: Exibe no console todos os clientes cadastrados.
        public void ExibirListaClientes()
        {
            Console.Clear();
            Console.WriteLine("=========================================================================================================");
            Console.WriteLine("                                      LISTA DE CLIENTES CADASTRADOS                                      ");
            Console.WriteLine("=========================================================================================================");

            // Cabeçalho ajustado
            Console.WriteLine($"| {"ID",-4} | {"NOME",-15} | {"CPF",-14} | {"EMAIL",-28} | {"TELEFONE",-15} | {"CADASTRO",-10} |");
            Console.WriteLine("---------------------------------------------------------------------------------------------------------");

            List<Cliente> clientes = _clienteDao.ListarTodos();

            if (clientes.Count == 0)
            {
                Console.WriteLine("| Nenhum cliente encontrado no banco de dados.                                                           |");
            }
            else
            {
                foreach (var c in clientes)
                {
                    string nome = c.Nome.Length > 15 ? c.Nome.Substring(0, 12) + "..." : c.Nome;
                    string email = c.Email.Length > 28 ? c.Email.Substring(0, 25) + "..." : c.Email;
                    string dataFormatada = c.DataCadastro.ToString("dd/MM/yyyy");

                    // Correção: o ,-10 fica DENTRO das chaves da dataFormatada
                    Console.WriteLine($"| {c.Id,-4} | {nome,-15} | {c.Cpf,-14} | {email,-28} | {c.Telefone,-15} | {dataFormatada,-10} |");
                }
            }

            Console.WriteLine("=========================================================================================================");

            Console.Write("Deseja verificar o endereço de algum cliente? (S/N): ");
            string resposta = Console.ReadLine()?.Trim().ToUpper();

            if (resposta == "S")
            {
                bool pesquisarNovamente = true;

                while (pesquisarNovamente)
                {
                    Console.Write("\nDigite o ID do cliente: ");

                    // Validação de entrada para garantir que o ID seja um número inteiro
                    if (int.TryParse(Console.ReadLine(), out int idCliente))
                    {
                        Endereco endereco = _enderecoDao.BuscarPorClienteId(idCliente);

                        if (endereco != null)
                        {
                            _enderecoView.ExibirEnderecoDoCliente(idCliente);
                        }
                        else
                        {
                            Console.WriteLine($"\n[AVISO] Nenhum endereço localizado para o ID {idCliente}.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\n[ERRO] Por favor, digite um número de ID válido.");
                    }

                    // Pergunta centralizada no final de cada tentativa
                    Console.Write("\nDeseja pesquisar outro ID? (S/N): ");
                    string opcao = Console.ReadLine()?.Trim().ToUpper();

                    if (opcao != "S")
                    {
                        pesquisarNovamente = false;
                    }
                }
            }

            PausarERetornar();
        }

        // Método: ExecutarCadastrarCliente
        // Descrição: Executa o fluxo de cadastro de um novo cliente via console.
        public void ExecutarCadastrarCliente()
        {
            Console.Clear();
            Console.WriteLine("--- ADICIONAR NOVO CLIENTE E ENDEREÇO ---\n");

            // DADOS DO CLIENTE
            Console.Write("Nome: ");
            string nome = Console.ReadLine();

            Console.Write("CPF (apenas números): ");
            string cpf = Console.ReadLine();

            Console.Write("E-mail: ");
            string email = Console.ReadLine();

            Console.Write("Telefone: ");
            string telefone = Console.ReadLine();

            // DADOS DO ENDEREÇO
            Console.WriteLine("\n--- DADOS DE ENDEREÇO ---");
            Console.Write("Logradouro (Rua/Av): ");
            string logradouro = Console.ReadLine();

            Console.Write("Número: ");
            string numero = Console.ReadLine();

            Console.Write("Cidade: ");
            string cidade = Console.ReadLine();

            Console.Write("Estado (UF - ex: SC): ");
            string estado = Console.ReadLine();

            Console.Write("CEP: ");
            string cep = Console.ReadLine();

            // Montagem dos Objetos
            Cliente novoCliente = new Cliente(nome, cpf, email, telefone);
            novoCliente.Endereco = new Endereco(logradouro, numero, cidade, estado, cep);

            try
            {
                bool cadastrado = _clienteDao.CadastrarComEndereco(novoCliente);

                if (cadastrado)
                {
                    Console.WriteLine("\n[SUCESSO] Cliente e Endereço cadastrados juntos no Oracle!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[ERRO] Falha ao cadastrar: {ex.Message}");
            }

            PausarERetornar();
        }

        // Método auxiliar: PausarERetornar
        // Descrição: Exibe uma mensagem e aguarda o usuário pressionar uma tecla.
        private void PausarERetornar()
        {
            Console.WriteLine("\nPressione qualquer tecla para retornar ao menu principal...");
            Console.ReadKey();
        }
    }
}
