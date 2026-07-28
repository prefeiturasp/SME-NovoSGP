using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class PendenciaProfessor
    {
        public PendenciaProfessor() { }
        public PendenciaProfessor(long pendenciaId, long turmaId, long componenteCurricularId, string professorRf, long? periodoEscolarId)
        {
            PendenciaId = pendenciaId;
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            PeriodoEscolarId = periodoEscolarId;
            ProfessorRf = professorRf;
        }
        [Key]
        public long Id { get; set; }

        public long PendenciaId { get; set; }
        [Computed]
        public Pendencia Pendencia { get; set; }

        public long ComponenteCurricularId { get; set; }
        [Computed]
        public ComponenteCurricular ComponenteCurricular { get; set; }

        public long TurmaId { get; set; }
        [Computed]
        public Turma Turma { get; set; }

        public long? PeriodoEscolarId { get; set; }
        [Computed]
        public PeriodoEscolar PeriodoEscolar { get; set; }

        public string ProfessorRf { get; set; }
    }
}
