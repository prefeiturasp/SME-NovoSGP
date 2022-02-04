namespace SME.SGP.Infra
{
    public class TurmaComComponenteDto
    {
        public int Id { get; set; }
        public string NomeTurma { get; set; }
        public long TurmaCodigo { get; set; }
        public long ComponenteCurricularCodigo { get; set; }
        public string Turno { get; set; }
        public bool PendenciaFrequencia { get; set; }
        public bool PendenciaPlanoAula { get; set; }
        public bool PendenciaAvaliacoes { get; set; }
        public bool PendenciaDiarioBordo { get; set; }
        public bool PendenciaFechamento { get; set; }
        public bool PeriodoFechamentoIniciado { get; set; }

    }
}
