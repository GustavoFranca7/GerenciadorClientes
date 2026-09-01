using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;

namespace GerenciadorClientes
{
    // Classe responsável por realizar operações de persistência de dados (CRUD) para a entidade Cliente utilizando o Oracle.
    public class ClienteDAO
    {
        // Insere um novo cliente no banco de dados. O ID é gerado
        // pela sequência SEQ_CLIENTES.NEXTVAL definida no Oracle.
        public bool Cadastrar(Cliente cliente)
        {
            // Query serve para inserir um novo registro na tabela CLIENTES, utilizando parâmetros nomeados  
            string query = @"INSERT INTO CLIENTES (ID, NOME, CPF, EMAIL, TELEFONE) 
                            VALUES (SEQ_CLIENTES.NEXTVAL, :nome, :cpf, :email, :telefone)";

            // Abre uma conexão com o banco utilizando a fábrica Database.GetConnection()
            using (OracleConnection conexao = Database.GetConnection())
            {
                conexao.Open();

                // Prepara o comando com parâmetros nomeados para evitar SQL injection
                using (OracleCommand cmd = new OracleCommand(query, conexao))
                {
                    cmd.Parameters.Add(new OracleParameter("nome", cliente.Nome));
                    cmd.Parameters.Add(new OracleParameter("cpf", cliente.Cpf));
                    cmd.Parameters.Add(new OracleParameter("email", cliente.Email));
                    cmd.Parameters.Add(new OracleParameter("telefone", cliente.Telefone));

                    // Executa a inserção e retorna true se pelo menos uma linha foi afetada
                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    return linhasAfetadas > 0;
                }
            }
        }

        //Recupera todos os clientes do banco, mapeando cada registro para
        // uma instância de Cliente e retornando-os em uma lista ordenada por ID.
        public List<Cliente> ListarTodos()
        {
            List<Cliente> lista = new List<Cliente>();
            string query = "SELECT ID, NOME, CPF, EMAIL, TELEFONE, DATA_CADASTRO FROM CLIENTES ORDER BY ID";

            // Abre conexão com o banco
            using (OracleConnection conexao = Database.GetConnection())
            {
                conexao.Open();

                using (OracleCommand cmd = new OracleCommand(query, conexao))
                {
                    // Executa o reader para percorrer os resultados
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Mapeia as colunas do SELECT para as propriedades do objeto Cliente
                            Cliente c = new Cliente
                            {
                                Id = reader.GetInt32(0),
                                Nome = reader.GetString(1),
                                Cpf = reader.GetString(2),
                                Email = reader.GetString(3),
                                // Trata valores nulos na coluna TELEFONE
                                Telefone = reader.IsDBNull(4) ? "Não informado" : reader.GetString(4),
                                DataCadastro = reader.GetDateTime(5)
                            };

                            lista.Add(c);
                        }
                    }
                }
            }

            return lista;
        }
        public bool CadastrarComEndereco(Cliente cliente)
        {
            string sqlCliente = @"INSERT INTO CLIENTES (ID, NOME, CPF, EMAIL, TELEFONE) 
                                 VALUES (SEQ_CLIENTES.NEXTVAL, :nome, :cpf, :email, :telefone) 
                                 RETURNING ID INTO :id";

            string sqlEndereco = @"INSERT INTO ENDERECOS (ID, CLIENTE_ID, LOGRADOURO, NUMERO, CIDADE, ESTADO, CEP) 
                                  VALUES (SEQ_ENDERECOS.NEXTVAL, :clienteId, :logradouro, :numero, :cidade, :estado, :cep)";

            using (OracleConnection conexao = Database.GetConnection())
            {
                conexao.Open();
                // Inicia uma transação no banco
                using (OracleTransaction transacao = conexao.BeginTransaction())
                {
                    try
                    {
                        int idClienteGerado;

                        // 1. Inserir Cliente e resgatar o ID gerado
                        using (OracleCommand cmdCliente = new OracleCommand(sqlCliente, conexao))
                        {
                            cmdCliente.Transaction = transacao;
                            cmdCliente.Parameters.Add(new OracleParameter("nome", cliente.Nome));
                            cmdCliente.Parameters.Add(new OracleParameter("cpf", cliente.Cpf));
                            cmdCliente.Parameters.Add(new OracleParameter("email", cliente.Email));
                            cmdCliente.Parameters.Add(new OracleParameter("telefone", cliente.Telefone));

                            // Configura parâmetro de SAÍDA (OUT) para capturar o ID
                            OracleParameter pId = new OracleParameter("id", OracleDbType.Int32)
                            {
                                Direction = System.Data.ParameterDirection.Output
                            };
                            cmdCliente.Parameters.Add(pId);

                            cmdCliente.ExecuteNonQuery();

                            // Recupera o valor que o Oracle gerou na Sequence
                            idClienteGerado = Convert.ToInt32(pId.Value.ToString());
                        }

                        // 2. Inserir Endereço usando o idClienteGerado
                        using (OracleCommand cmdEndereco = new OracleCommand(sqlEndereco, conexao))
                        {
                            cmdEndereco.Transaction = transacao;
                            cmdEndereco.Parameters.Add(new OracleParameter("clienteId", idClienteGerado));
                            cmdEndereco.Parameters.Add(new OracleParameter("logradouro", cliente.Endereco.Logradouro));
                            cmdEndereco.Parameters.Add(new OracleParameter("numero", cliente.Endereco.Numero));
                            cmdEndereco.Parameters.Add(new OracleParameter("cidade", cliente.Endereco.Cidade));
                            cmdEndereco.Parameters.Add(new OracleParameter("estado", cliente.Endereco.Estado));
                            cmdEndereco.Parameters.Add(new OracleParameter("cep", cliente.Endereco.Cep));

                            cmdEndereco.ExecuteNonQuery();
                        }

                        // Confirma ambas as gravações no banco de dados
                        transacao.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        // Desfaz tudo se houver qualquer erro no meio do caminho
                        transacao.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
