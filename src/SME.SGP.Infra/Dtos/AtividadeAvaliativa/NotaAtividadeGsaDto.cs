using System;
using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class NotaAtividadeGsaDto
    {
        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public long AtividadeGoogleClassroomId { get; set; }
        public StatusGSA StatusGsa { get; set; }
        public double Nota { get; set; }
        public DateTime DataEntregaAvaliacao { get; set; }
        public string CodigoAluno { get; set; }
        public string Registro { get; set; }

        public NotaAtividadeGsaDto(long turmaId, long componenteCurricularId, long atividadeGoogleClassroomId,
            StatusGSA statusGsa, double nota, DateTime dataEntregaAvaliacao, string codigoAluno, string registro)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            AtividadeGoogleClassroomId = atividadeGoogleClassroomId;
            StatusGsa = statusGsa;
            Nota = nota;
            DataEntregaAvaliacao = dataEntregaAvaliacao;
            CodigoAluno = codigoAluno;
            Registro = registro;
        }

        public NotaAtividadeGsaDto()
        {
        }
    }
}