using MediatR;
using SME.SGP.Dominio;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirDocumentoUseCase : AbstractUseCase, IExcluirDocumentoUseCase
    {
        public ExcluirDocumentoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(long documentoId)
        {
            var entidadeDocumento = await mediator.Send(new ObterDocumentoPorIdQuery(documentoId));

            if (entidadeDocumento == null)
                throw new NegocioException("O documento informado não foi encontrado");

            if(entidadeDocumento.ArquivoId != null)
            {
                var entidadeArquivo = await mediator.Send(new ObterArquivoPorIdQuery(entidadeDocumento.ArquivoId.GetValueOrDefault()));

                if (entidadeArquivo == null)
                    throw new NegocioException("O arquivo relacionado não foi encontrado");

                await mediator.Send(new ExcluirReferenciaArquivoDocumentoPorArquivoIdCommand(documentoId, entidadeArquivo.Id));
                await mediator.Send(new ExcluirArquivoRepositorioPorIdCommand(entidadeArquivo.Id));
                await mediator.Send(new ExcluirArquivoFisicoCommand(entidadeArquivo.Codigo, entidadeArquivo.Tipo, entidadeArquivo.Nome));
            }

            await mediator.Send(new ExcluirDocumentoPorIdCommand(documentoId));

            return true;
        }
    }
}
