using System;

namespace SME.SGP.Infra.Dtos.ImportarArquivo
{
    public class RetornoDto
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public long Id { get; set; }

        public static RetornoDto RetornarSucesso(string mensagem, long id)
        {
            return new RetornoDto
            {
                Sucesso = true,
                Mensagem = mensagem,
                Id = id
            };
        }

        public static RetornoDto RetornarSucesso(string mensagem)
        {
            return new RetornoDto
            {
                Sucesso = true,
                Mensagem = mensagem
            };
        }
    }
}
