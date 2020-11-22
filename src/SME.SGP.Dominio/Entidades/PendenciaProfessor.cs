using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class PendenciaProfessor
    {
        public long Id { get; set; }

        public long PendenciaId { get; set; }
        public Pendencia Pendencia { get; set; }

        public long ComponenteCurricularId { get; set; }
        public ComponenteCurricular ComponenteCurricular { get; set; }

        public long TurmaId { get; set; }
        public Turma Turma { get; set; }

        public string ProfessorRf { get; set; }
    }
}
