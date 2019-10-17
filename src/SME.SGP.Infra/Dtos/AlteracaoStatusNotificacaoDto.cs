namespace SME.SGP.Infra
{
    public class AlteracaoStatusNotificacaoDto
    {
        public AlteracaoStatusNotificacaoDto(string mensagem, bool sucesso)
        {
            Mensagem = mensagem;
            Sucesso = sucesso;
        }

        public string Mensagem { get; set; }
        public bool Sucesso { get; set; }
    }
}