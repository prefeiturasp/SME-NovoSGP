using System;
using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class NotaAtividadeGsaDto
    {
        public string TurmaId { get; set; }
        public string ComponenteCurricularId { get; set; }
        public long AtividadeGoogleClassroomId { get; set; }
        public StatusGSA StatusGsa { get; set; }
        public double Nota { get; set; }
        public DateTime DataAvaliacao { get; set; }

        public NotaAtividadeGsaDto(string turmaId, string componenteCurricularId, long atividadeGoogleClassroomId,
            StatusGSA statusGsa, double nota, DateTime dataAvaliacao)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            AtividadeGoogleClassroomId = atividadeGoogleClassroomId;
            StatusGsa = statusGsa;
            Nota = nota;
            DataAvaliacao = dataAvaliacao;
        }

        public NotaAtividadeGsaDto()
        {
        }
    }
}