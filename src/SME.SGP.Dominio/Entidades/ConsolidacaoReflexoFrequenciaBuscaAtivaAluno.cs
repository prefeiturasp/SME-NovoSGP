using System;
using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class ConsolidacaoReflexoFrequenciaBuscaAtivaAluno : EntidadeBase
    {
        public string TurmaCodigo { get; set; }
        public string UeCodigo { get; set; }
        public int AnoLetivo { get; set; }
        public Modalidade Modalidade { get; set; }
        public string AlunoCodigo  { get; set; }
        public string AlunoNome { get; set; }
        public DateTime DataBuscaAtiva { get; set; }
        public int Mes { get; set; }
        public double PercFrequenciaAntesAcao { get; set; }
        public double PercFrequenciaAposAcao { get; set; }
    }
}
