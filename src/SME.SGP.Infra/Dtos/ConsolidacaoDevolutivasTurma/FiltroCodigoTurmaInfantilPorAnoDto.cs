namespace SME.SGP.Infra
{
    public class FiltroCodigoTurmaInfantilPorAnoDto
    {
        public FiltroCodigoTurmaInfantilPorAnoDto(int anoAtual,long ueId)
        {
            AnoAtual = anoAtual;
            UeId = ueId;
        }

        public int AnoAtual { get; set; }
        public long UeId { get; set; }
    }
}
