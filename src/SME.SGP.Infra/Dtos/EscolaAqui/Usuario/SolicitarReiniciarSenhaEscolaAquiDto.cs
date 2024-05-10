namespace SME.SGP.Infra.Dtos.EscolaAqui
{
    public class SolicitarReiniciarSenhaEscolaAquiDto
    {
        public string Cpf { get; set; }

        public SolicitarReiniciarSenhaEscolaAquiDto(string cpf)
        {
            Cpf = cpf;
        }
    }

}
