using System;

namespace SME.SGP.Infra
{
    public class RegistroIndividualDto
    {
        public long Id { get; set; }

        public long TurmaId { get; set; }
        public TurmaDto Turma { get; set; }

        public long AlunoCodigo { get; set; }

        public long ComponenteCurricularId { get; set; }

        public DateTime Data { get; set; }
        public string Registro { get; set; }

        public bool Excluido { get; set; }
        public bool Migrado { get; set; }

        public AuditoriaDto Auditoria { get; set; }
    }
}
