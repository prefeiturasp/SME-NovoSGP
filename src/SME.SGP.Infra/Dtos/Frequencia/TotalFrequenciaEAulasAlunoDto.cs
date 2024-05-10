namespace SME.SGP.Infra
{
    public class TotalFrequenciaEAulasAlunoDto
    {
        public int TotalPresencas { get; set; }
        public int TotalAusencias { get; set; }
        public int TotalRemotos { get; set; }
        public string AlunoCodigo { get; set; }
        public string ComponenteCurricularId { get; set; }
        public int TotalAulas { get => TotalPresencas + TotalAusencias + TotalRemotos; }
    }
}
