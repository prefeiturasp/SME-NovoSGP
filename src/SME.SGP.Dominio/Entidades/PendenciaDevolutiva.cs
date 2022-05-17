namespace SME.SGP.Dominio
{
    public class PendenciaDevolutiva : EntidadeBase
    {
        public long PedenciaId { get; set; }
        public Pendencia Pendencia { get; set; }
        public long ComponenteCurricularId { get; set; }
        public ComponenteCurricular ComponenteCurricular { get; set; }
        public long TurmaId { get; set; }
        public Turma Turma { get; set; }
    }
}
