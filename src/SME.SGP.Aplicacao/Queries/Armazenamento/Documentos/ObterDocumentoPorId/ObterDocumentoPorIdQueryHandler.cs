using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDocumentoPorIdQueryHandler : IRequestHandler<ObterDocumentoPorIdQuery, Documento>
    {
        private readonly IRepositorioDocumento repositorioDocumento;

        public ObterDocumentoPorIdQueryHandler(IRepositorioDocumento repositorioDocumento)
        {
            this.repositorioDocumento = repositorioDocumento ?? throw new ArgumentNullException(nameof(repositorioDocumento));
        }

        public async Task<Documento> Handle(ObterDocumentoPorIdQuery request, CancellationToken cancellationToken) 
            => await repositorioDocumento.ObterPorIdAsync(request.DocumentoId);
    }
}
