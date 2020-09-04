namespace SME.SGP.Infra.Dtos.EscolaAqui
{
    public class RespostaSolicitarReiniciarSenhaDto
    {
        public string Mensagem { get; set; }

        public RespostaSolicitarReiniciarSenhaDto(string mensagem)
        {
            Mensagem = mensagem;
        }
    }

}
