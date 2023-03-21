using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDocumentoPorIdCompletoQueryHandler : IRequestHandler<ObterDocumentoPorIdCompletoQuery, ObterDocumentoResumidoDto>
    {
        private readonly IRepositorioDocumento repositorioDocumento;

        public ObterDocumentoPorIdCompletoQueryHandler(IRepositorioDocumento repositorioDocumento)
        {
            this.repositorioDocumento = repositorioDocumento ?? throw new ArgumentNullException(nameof(repositorioDocumento));
        }

        public async Task<ObterDocumentoResumidoDto> Handle(ObterDocumentoPorIdCompletoQuery request, CancellationToken cancellationToken) 
            => await repositorioDocumento.ObterPorIdCompleto(request.DocumentoId);
    }
}
