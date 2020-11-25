using System;
using System.Collections.Generic;
using System.Text;

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

        public long Id { get; set; }

        public long PendenciaId { get; set; }
        public Pendencia Pendencia { get; set; }

        public long ComponenteCurricularId { get; set; }
        public ComponenteCurricular ComponenteCurricular { get; set; }

        public long TurmaId { get; set; }
        public Turma Turma { get; set; }

        public long? PeriodoEscolarId { get; set; }
        public PeriodoEscolar PeriodoEscolar { get; set; }

        public string ProfessorRf { get; set; }
    }
}
