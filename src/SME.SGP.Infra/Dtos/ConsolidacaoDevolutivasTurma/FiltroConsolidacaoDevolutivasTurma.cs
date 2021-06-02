namespace SME.SGP.Infra
{
    public class FiltroConsolidacaoDevolutivasTurma
    {
        public FiltroConsolidacaoDevolutivasTurma() { }
        public FiltroConsolidacaoDevolutivasTurma(long turmaId, string turmaCodigo)
        {
            TurmaId = turmaId;
            TurmaCodigo = turmaCodigo;
        }

        public long TurmaId { get; set; }
        public string TurmaCodigo { get; set; }
    }
}
