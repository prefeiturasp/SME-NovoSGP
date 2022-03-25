namespace SME.SGP.Infra
{
    public class FiltroConsolidacaoConselhoClasseAlunoPorAnoDto
    {
        public FiltroConsolidacaoConselhoClasseAlunoPorAnoDto(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }

        public int AnoLetivo { get; set;  }
    }
}
