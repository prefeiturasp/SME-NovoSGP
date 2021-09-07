using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterNotaPorAtividadeGoogleClassIdQuery : IRequest<NotaConceito>
    {
        public long AtividadeGoogleClassroomId { get; set; }
        public string TurmaId { get; set; }
        public string ComponenteCurricularId { get; set; }
        public long CodigoAluno { get; set; }

        public ObterNotaPorAtividadeGoogleClassIdQuery(long atividadeGoogleClassroomId,long codigoAluno)
        {
            AtividadeGoogleClassroomId = atividadeGoogleClassroomId;
            CodigoAluno = codigoAluno;
        }
    }
}