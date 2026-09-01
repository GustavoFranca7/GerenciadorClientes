using System;
using System.Collections.Generic;
using System.Text;

namespace GerenciadorClientes
{
    public class Endereco
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Cep { get; set; }

        public Endereco() { }

        public Endereco(string logradouro, string numero, string cidade, string estado, string cep)
        {
            Logradouro = logradouro;
            Numero = numero;
            Cidade = cidade;
            Estado = estado;
            Cep = cep;
        }
    }
}