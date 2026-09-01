using System;

namespace GerenciadorClientes
{
    public class EnderecoView
    {
        private EnderecoDAO _enderecoDao = new EnderecoDAO();

        public void ExibirEnderecoDoCliente(int clienteId)
        {
            Endereco endereco = _enderecoDao.BuscarPorClienteId(clienteId);

            Console.WriteLine("\n--- ENDEREÇO DO CLIENTE ---");
            if (endereco != null)
            {
                Console.WriteLine($"Logradouro: {endereco.Logradouro}, Nº {endereco.Numero}");
                Console.WriteLine($"Cidade/UF:  {endereco.Cidade}/{endereco.Estado}");
                Console.WriteLine($"CEP:        {endereco.Cep}");
            }
            else
            {
                Console.WriteLine("[AVISO] Nenhum endereço localizado para este ID de cliente.");
            }
        }
    }
}