using System;

namespace SME.SGP.Infra
{
    public class AtividadeGsaDto
    {
        public long AtividadeClassroomId { get; set; }
        public string TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public string UsuarioRf { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public string Email { get; set; }
    }
}
