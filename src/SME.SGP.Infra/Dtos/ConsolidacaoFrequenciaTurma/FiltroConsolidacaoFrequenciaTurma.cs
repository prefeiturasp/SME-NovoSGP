using System;

namespace SME.SGP.Infra
{
    public class FiltroConsolidacaoFrequenciaTurma
    {
        public FiltroConsolidacaoFrequenciaTurma() { }
        public FiltroConsolidacaoFrequenciaTurma(long turmaId, string turmaCodigo, double percentualFrequenciaMinimo, DateTime data)
        {
            TurmaId = turmaId;
            TurmaCodigo = turmaCodigo;
            PercentualFrequenciaMinimo = percentualFrequenciaMinimo;
            Data = data;
        }

        public DateTime Data { get; set; }
        public long TurmaId { get; set; }
        public string TurmaCodigo { get; set; }
        public double PercentualFrequenciaMinimo { get; set; }
    }
}
