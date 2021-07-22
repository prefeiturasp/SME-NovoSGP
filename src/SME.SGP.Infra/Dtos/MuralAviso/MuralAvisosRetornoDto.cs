using System;

namespace SME.SGP.Infra
{
    public class MuralAvisosRetornoDto
    {
        public DateTime DataPublicacao { get; set; }
        public string Mensagem { get; set; }
        public string Email { get; set; }

        public MuralAvisosRetornoDto(DateTime dataPublicacao, string mensagem, string email)
        {
            DataPublicacao = dataPublicacao;
            Mensagem = mensagem;
            Email = email;
        }
    }
}