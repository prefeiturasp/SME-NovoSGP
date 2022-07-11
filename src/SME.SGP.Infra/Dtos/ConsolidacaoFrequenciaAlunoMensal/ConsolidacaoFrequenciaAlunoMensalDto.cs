namespace SME.SGP.Infra
{
    public class ConsolidacaoFrequenciaAlunoMensalDto
    {
        public long Id { get; set; }
        public long TurmaId { get; set; }
        public string AlunoCodigo { get; set; }
        public int Mes { get; set; }
        public decimal Percentual { get; set; }
        public int QuantidadeAulas { get; set; }
        public int QuantidadeAusencias { get; set; }
        public int QuantidadeCompensacoes { get; set; }
    }
}
