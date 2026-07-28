using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class PendenciaDevolutiva
    {
        [Key]
        public long Id { get; set; }
        public long PedenciaId { get; set; }
        [Computed]
        public Pendencia Pendencia { get; set; }
        public long ComponenteCurricularId { get; set; }
        [Computed]
        public ComponenteCurricular ComponenteCurricular { get; set; }
        public long TurmaId { get; set; }
        [Computed]
        public Turma Turma { get; set; }
    }
}
