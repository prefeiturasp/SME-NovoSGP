using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ValidarTipoDocumentoDaClassificacaoQueryHandler : IRequestHandler<ValidarTipoDocumentoDaClassificacaoQuery, bool>
    {
        private readonly IRepositorioClassificacaoDocumento repositorioClassificacaoDocumento;

        public ValidarTipoDocumentoDaClassificacaoQueryHandler(IRepositorioClassificacaoDocumento repositorioClassificacaoDocumento)
        {
            this.repositorioClassificacaoDocumento = repositorioClassificacaoDocumento ?? throw new ArgumentNullException(nameof(repositorioClassificacaoDocumento));
        }

        public async Task<bool> Handle(ValidarTipoDocumentoDaClassificacaoQuery request, CancellationToken cancellationToken)
            => await repositorioClassificacaoDocumento.ValidarTipoDocumento(request.ClassificacaoDocumentoId, (int)request.TipoDocumento);
    }
}
