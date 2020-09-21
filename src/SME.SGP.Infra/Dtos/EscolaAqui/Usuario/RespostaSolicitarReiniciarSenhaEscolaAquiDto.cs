namespace SME.SGP.Infra.Dtos.EscolaAqui
{
    public class RespostaSolicitarReiniciarSenhaEscolaAquiDto
    {
        public string Mensagem { get; set; }

        public RespostaSolicitarReiniciarSenhaEscolaAquiDto(string mensagem)
        {
            Mensagem = mensagem;
        }
    }

}
