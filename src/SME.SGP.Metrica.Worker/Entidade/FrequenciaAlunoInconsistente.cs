namespace SME.SGP.Metrica.Worker.Entidade
{
    public class FrequenciaAlunoInconsistente
    {
        public string TurmaCodigo { get; set; }
        public long ComponenteCurricularId { get; set; }
        public string AlunoCodigo { get; set; }
        public long PeriodoEscolarId { get; set; }
        public int TotalAulas { get; set; }
        public int TotalAusencias { get; set; }
        public int TotalPresencas { get; set; }
        public int TotalRemotos { get; set; }
        public int TotalAulasCalculado { get; set; }
        public int TotalAusenciasCalculado { get; set; }
        public int TotalPresencasCalculado { get; set; }
        public int TotalRemotosCalculado { get; set; }
    }
}
