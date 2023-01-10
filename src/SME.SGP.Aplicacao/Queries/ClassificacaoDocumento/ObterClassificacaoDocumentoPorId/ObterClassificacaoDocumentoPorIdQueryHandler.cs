using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterClassificacaoDocumentoPorIdQueryHandler : IRequestHandler<ObterClassificacaoDocumentoPorIdQuery, ClassificacaoDocumento>
    {
        private readonly IRepositorioClassificacaoDocumento repositorioClassificacaoDocumento;

        public ObterClassificacaoDocumentoPorIdQueryHandler(
            IRepositorioClassificacaoDocumento repositorioClassificacaoDocumento)
        {
            this.repositorioClassificacaoDocumento = repositorioClassificacaoDocumento ??
                                                     throw new ArgumentNullException(
                                                         nameof(repositorioClassificacaoDocumento));
        }

        public async Task<ClassificacaoDocumento> Handle(ObterClassificacaoDocumentoPorIdQuery request,
            CancellationToken cancellationToken)
        {
            return await repositorioClassificacaoDocumento.ObterPorIdAsync(request.ClassificacaoId);
        }
    }
}