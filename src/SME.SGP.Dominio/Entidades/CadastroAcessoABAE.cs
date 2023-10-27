using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dominio
{
    public class CadastroAcessoABAE : EntidadeBase
    {
        public string Nome { get; set; }
        public long UeId { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public bool Situacao { get; set; }
        public string Cep { get; set; }
        public string Endereco { get; set; }
        public int Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public bool Excluido { get; set; }
        public string UeCodigo { get; set; }
        public string DreCodigo { get; set; }

        public void ExcluirLogicamente()
        {
            Excluido = true;
        }
    }
}