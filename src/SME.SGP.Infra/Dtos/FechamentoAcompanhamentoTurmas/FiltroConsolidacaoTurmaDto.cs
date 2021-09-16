namespace SME.SGP.Infra
{
    public class FiltroConsolidacaoTurmaDto
    {
        public FiltroConsolidacaoTurmaDto(string turmaCodigo, int? bimestre)
        {
            TurmaCodigo = turmaCodigo;
            Bimestre = bimestre;
        }

        public string TurmaCodigo { get; }
        public int? Bimestre { get; }
    }
}
