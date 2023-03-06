using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ObterDocumentosArquivosPorDocumentoIdQueryHandler : IRequestHandler<ObterDocumentosArquivosPorDocumentoIdQuery, IEnumerable<DocumentoArquivoDto>>
    {
        private readonly IRepositorioDocumentoArquivo repositorioDocumentoArquivo;

        public ObterDocumentosArquivosPorDocumentoIdQueryHandler(
            IRepositorioDocumentoArquivo repositorioDocumentoArquivo)
        {
            this.repositorioDocumentoArquivo = repositorioDocumentoArquivo ??
                                               throw new ArgumentNullException(nameof(repositorioDocumentoArquivo));
        }

        public async Task<IEnumerable<DocumentoArquivoDto>> Handle(ObterDocumentosArquivosPorDocumentoIdQuery request,
            CancellationToken cancellationToken)
        {
            return await repositorioDocumentoArquivo.ObterDocumentosArquivosPorDocumentoIdAsync(request.DocumentoId);
        }
    }
}