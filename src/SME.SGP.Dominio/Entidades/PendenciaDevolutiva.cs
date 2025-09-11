using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class PendenciaDevolutiva
    {
        public long Id { get; set; }
        public long PedenciaId { get; set; }
        public Pendencia Pendencia { get; set; }
        public long ComponenteCurricularId { get; set; }
        public ComponenteCurricular ComponenteCurricular { get; set; }
        public long TurmaId { get; set; }
        public Turma Turma { get; set; }
    }
}
