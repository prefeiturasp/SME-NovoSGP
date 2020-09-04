namespace SME.SGP.Infra.Dtos.EscolaAqui
{
    public class SolicitarReiniciarSenhaDto
    {
        public string Cpf { get; set; }

        public SolicitarReiniciarSenhaDto(string cpf)
        {
            Cpf = cpf;
        }
    }

}
