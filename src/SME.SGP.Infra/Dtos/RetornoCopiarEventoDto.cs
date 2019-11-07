namespace SME.SGP.Infra
{
    public class RetornoCopiarEventoDto
    {
        public RetornoCopiarEventoDto(string mensagem, bool sucesso = false)
        {
            Mensagem = mensagem;
            Sucesso = sucesso;
        }

        public string Mensagem { get; set; }
        public bool Sucesso { get; set; }
    }
}