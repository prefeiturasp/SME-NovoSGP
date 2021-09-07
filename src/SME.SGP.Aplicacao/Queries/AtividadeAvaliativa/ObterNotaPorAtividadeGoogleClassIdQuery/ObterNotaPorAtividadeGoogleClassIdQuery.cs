using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterNotaPorAtividadeGoogleClassIdQuery : IRequest<NotaConceito>
    {
        public long AtividadeId { get; set; }
        public string CodigoAluno { get; set; }

        public ObterNotaPorAtividadeGoogleClassIdQuery(long atividadeId,string codigoAluno)
        {
            AtividadeId = atividadeId;
            CodigoAluno = codigoAluno;
        }
    }
}