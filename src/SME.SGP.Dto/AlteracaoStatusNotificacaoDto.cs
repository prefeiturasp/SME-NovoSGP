namespace SME.SGP.Dto
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