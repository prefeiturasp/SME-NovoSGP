using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterClassificacaoDocumentoPorIdQuery : IRequest<ClassificacaoDocumento>
    {
        public ObterClassificacaoDocumentoPorIdQuery(long classificacaoId)
        {
            ClassificacaoId = classificacaoId;
        }

        public long ClassificacaoId { get; }
    }
}