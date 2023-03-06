using System;

namespace SME.SGP.Infra
{
    public class PendenciaProfessorComponenteCurricularDto
    {
        public long PendenciaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public string ProfessorRf { get; set; }
        public string CodigoTurma { get; set; }
        public PendenciaProfessorComponenteCurricularDto() { }
    }
}
