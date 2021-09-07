using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterNotaPorAtividadeGoogleClassIdQuery : IRequest<NotaConceito>
    {
        public long AtividadeId { get; set; }
        public long CodigoAluno { get; set; }

        public ObterNotaPorAtividadeGoogleClassIdQuery(long atividadeId,long codigoAluno)
        {
            AtividadeId = atividadeId;
            CodigoAluno = codigoAluno;
        }
    }
}