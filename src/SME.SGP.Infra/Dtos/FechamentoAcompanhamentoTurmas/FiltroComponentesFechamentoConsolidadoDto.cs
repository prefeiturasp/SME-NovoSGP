namespace SME.SGP.Infra
{
    public class FiltroComponentesFechamentoConsolidadoDto
    {
        public FiltroComponentesFechamentoConsolidadoDto(long turmaId, int bimestre, int situacaoFechamento)
        {
            TurmaId = turmaId;
            Bimestre = bimestre;
            SituacaoFechamento = situacaoFechamento;
        }

        public long TurmaId { get; set; }
        public int Bimestre { get; set; }
        public int SituacaoFechamento { get; set; }
    }
}
