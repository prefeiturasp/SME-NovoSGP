namespace SME.SGP.Infra
{
    public class FiltroConsolidacaoFrequenciaTurma
    {
        public FiltroConsolidacaoFrequenciaTurma() { }
        public FiltroConsolidacaoFrequenciaTurma(long turmaId, string turmaCodigo, double percentualFrequenciaMinimo)
        {
            TurmaId = turmaId;
            TurmaCodigo = turmaCodigo;
            PercentualFrequenciaMinimo = percentualFrequenciaMinimo;
        }

        public long TurmaId { get; set; }
        public string TurmaCodigo { get; set; }
        public double PercentualFrequenciaMinimo { get; set; }
    }
}
