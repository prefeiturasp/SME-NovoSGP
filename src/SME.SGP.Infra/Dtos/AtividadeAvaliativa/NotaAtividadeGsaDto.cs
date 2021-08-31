using System;

namespace SME.SGP.Infra
{
    public class NotaAtividadeGsaDto
    {
        public string TurmaId { get; set; }
        public string ComponenteCurricularId { get; set; }
        public long AtividadeGoogleClassroomId { get; set; }
        public double Nota { get; set; }
        public DateTime DataAvaliacao { get; set; }

        public NotaAtividadeGsaDto(string turmaId, string componenteCurricularId, long atividadeGoogleClassroomId, double nota, DateTime dataAvaliacao)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            AtividadeGoogleClassroomId = atividadeGoogleClassroomId;
            Nota = nota;
            DataAvaliacao = dataAvaliacao;
        }

        public NotaAtividadeGsaDto()
        {
        }
    }
}