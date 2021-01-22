using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirReferenciaArquivoDocumentoPorArquivoIdCommandHandler : IRequestHandler<ExcluirReferenciaArquivoDocumentoPorArquivoIdCommand, bool>
    {
        private readonly IRepositorioDocumento repositorioDocumento;

        public ExcluirReferenciaArquivoDocumentoPorArquivoIdCommandHandler(IRepositorioDocumento repositorioDocumento)
        {
            this.repositorioDocumento = repositorioDocumento ?? throw new ArgumentNullException(nameof(repositorioDocumento));
        }

        public async Task<bool> Handle(ExcluirReferenciaArquivoDocumentoPorArquivoIdCommand request, CancellationToken cancellationToken)
            => await repositorioDocumento.RemoverReferenciaArquivo(request.DocumentoId, request.ArquivoId);
    }
}
