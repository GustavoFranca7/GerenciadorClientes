using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Text;

// Camada Core / Data: tudo que conversa diretamente com o Oracle.
namespace GerenciadorClientes.Core.Data
{
    public static class Database
    {
        // Classe estática para gerenciar a conexão com o banco de dados Oracle.
        // Propriedade para armazenar a string de conexão (privada para proteção)
        private static string ConnectionString =>
            "User Id=PROJETO_CLIENTES;Password=123456;Data Source=localhost:1521/XEPDB1;";

        // Método que retorna uma nova conexão pronta para uso
        public static OracleConnection GetConnection()
        {
            return new OracleConnection(ConnectionString);
        }
    }
}
