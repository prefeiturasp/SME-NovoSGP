namespace SME.SGP.Infra
{
    public class FiltroComponentesFechamentoConsolidadoDto
    {
        public FiltroComponentesFechamentoConsolidadoDto(long turmaId, int bimestre)
        {
            TurmaId = turmaId;
            Bimestre = bimestre;
        }

        public long TurmaId { get; set; }
        public int Bimestre { get; set; }
    }
}
