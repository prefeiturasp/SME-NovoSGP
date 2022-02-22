namespace SME.SGP.Infra
{
    public class DadosCriacaoAulasAutomaticasCarregamentoDto
    {
        public DadosCriacaoAulasAutomaticasCarregamentoDto()
        {
            Pagina = 1;
        }
        public int Pagina { get; set; }
        public string CodigoTurma { get; set; }
    }
}
