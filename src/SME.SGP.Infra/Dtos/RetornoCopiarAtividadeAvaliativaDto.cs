namespace SME.SGP.Infra
{
    public class RetornoCopiarAtividadeAvaliativaDto
    {
        public RetornoCopiarAtividadeAvaliativaDto(string mensagem, bool sucesso = false)
        {
            Mensagem = mensagem;
            Sucesso = sucesso;
        }

        public string Mensagem { get; set; }
        public bool Sucesso { get; set; }
    }
}
