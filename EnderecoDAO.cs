using System;
using Oracle.ManagedDataAccess.Client;

namespace GerenciadorClientes
{
    public class EnderecoDAO
    {
        public Endereco BuscarPorClienteId(int clienteId)
        {
            string query = @"SELECT ID, CLIENTE_ID, LOGRADOURO, NUMERO, CIDADE, ESTADO, CEP 
                            FROM ENDERECOS 
                            WHERE CLIENTE_ID = :clienteId";

            using (OracleConnection conexao = Database.GetConnection())
            {
                conexao.Open();
                using (OracleCommand cmd = new OracleCommand(query, conexao))
                {
                    cmd.Parameters.Add(new OracleParameter("clienteId", clienteId));

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Endereco
                            {
                                Id = reader.GetInt32(0),
                                ClienteId = reader.GetInt32(1),
                                Logradouro = reader.GetString(2),
                                Numero = reader.GetString(3),
                                Cidade = reader.GetString(4),
                                Estado = reader.GetString(5),
                                Cep = reader.GetString(6)
                            };
                        }
                    }
                }
            }

            return null; // Retorna null caso o cliente não tenha endereço cadastrado
        }
    }
}