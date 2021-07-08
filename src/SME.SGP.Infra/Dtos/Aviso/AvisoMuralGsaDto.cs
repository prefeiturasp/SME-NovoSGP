using System;

namespace SME.SGP.Infra
{
    public class AvisoMuralGsaDto
    {
        public long AvisoClassroomId { get; set; }
        public string TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public string UsuarioRf { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public string Mensagem { get; set; }
    }
}
