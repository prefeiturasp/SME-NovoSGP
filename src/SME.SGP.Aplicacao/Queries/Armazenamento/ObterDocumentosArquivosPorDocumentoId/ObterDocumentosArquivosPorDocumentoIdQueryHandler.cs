using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao;

public class ObterDocumentosArquivosPorDocumentoIdQueryHandler : IRequestHandler<ObterDocumentosArquivosPorDocumentoIdQuery, IEnumerable<DocumentoArquivo>>
{
    private readonly IRepositorioDocumentoArquivo repositorioDocumentoArquivo;

    public ObterDocumentosArquivosPorDocumentoIdQueryHandler(IRepositorioDocumentoArquivo repositorioDocumentoArquivo)
    {
        this.repositorioDocumentoArquivo = repositorioDocumentoArquivo ?? throw new ArgumentNullException(nameof(repositorioDocumentoArquivo));
    }

    public async Task<IEnumerable<DocumentoArquivo>> Handle(ObterDocumentosArquivosPorDocumentoIdQuery request, CancellationToken cancellationToken)
    {
        return await repositorioDocumentoArquivo.ObterDocumentosArquivosPorDocumentoIdAsync(request.DocumentoId);
    }
}