namespace SME.SGP.Infra
{
    public class FiltroCodigoTurmaInfantilPorAnoDto
    {
        public FiltroCodigoTurmaInfantilPorAnoDto(int anoAtual)
        {
            AnoAtual = anoAtual;
        }

        public int AnoAtual { get; set; }
    }
}
