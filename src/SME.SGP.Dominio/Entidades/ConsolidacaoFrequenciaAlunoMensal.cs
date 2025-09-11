using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class ConsolidacaoFrequenciaAlunoMensal
    {
        public long Id { get; set; }
        public long TurmaId { get; set; }
        public string AlunoCodigo { get; set; }
        public int Mes { get; set; }
        public double Percentual { get; set; }
        public int QuantidadeAulas { get; set; }
        public int QuantidadeAusencias { get; set; }
        public int QuantidadeCompensacoes { get; set; }
    }
}
