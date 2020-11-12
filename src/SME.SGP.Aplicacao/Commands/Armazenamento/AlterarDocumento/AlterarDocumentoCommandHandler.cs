using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarDocumentoCommandHandler : AbstractUseCase, IRequestHandler<AlterarDocumentoCommand, AuditoriaDto>
    {
        private readonly IRepositorioDocumento repositorioDocumento;

        public AlterarDocumentoCommandHandler(IMediator mediator, IRepositorioDocumento repositorioDocumento) : base(mediator)
        {
            this.repositorioDocumento = repositorioDocumento ?? throw new ArgumentNullException(nameof(repositorioDocumento));
        }

        public async Task<AuditoriaDto> Handle(AlterarDocumentoCommand request, CancellationToken cancellationToken)
        {
            var documento = await mediator.Send(new ObterDocumentoPorIdQuery(request.DocumentoId));
            if (documento == null)
                throw new NegocioException("Documento informado não existe");

            var arquivo = await mediator.Send(new ObterArquivoPorCodigoQuery(request.CodigoArquivo));
            if (arquivo == null)
                throw new NegocioException($"O código de arquivo informado não foi encontrado!");

            documento.ArquivoId = arquivo.Id;

            await repositorioDocumento.SalvarAsync(documento);

            return (AuditoriaDto)documento;
        }
    }
}
