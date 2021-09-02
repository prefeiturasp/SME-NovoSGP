using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasPorGoogleClassroomIdTurmaIdComponentCurricularId : IRequest<NotaConceito>
    {
        public long AtividadeGoogleClassroomId { get; set; }
        public string TurmaId { get; set; }
        public string componenteCurricularId { get; set; }

        public ObterNotasPorGoogleClassroomIdTurmaIdComponentCurricularId(long atividadeGoogleClassroomId, string turmaId, string componenteCurricularId)
        {
            AtividadeGoogleClassroomId = atividadeGoogleClassroomId;
            TurmaId = turmaId;
            this.componenteCurricularId = componenteCurricularId;
            this.componenteCurricularId = componenteCurricularId;
        }
    }
}