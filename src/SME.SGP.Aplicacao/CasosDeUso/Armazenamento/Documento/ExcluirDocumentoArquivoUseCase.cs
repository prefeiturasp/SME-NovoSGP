using MediatR;
using SME.SGP.Dominio;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirDocumentoArquivoUseCase : AbstractUseCase, IExcluirDocumentoArquivoUseCase
    {
        public ExcluirDocumentoArquivoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar((long DocumentoId, Guid CodigoArquivo) param)
        {
            var entidadeArquivo = await mediator.Send(new ObterArquivoPorCodigoQuery(param.CodigoArquivo));

            if (entidadeArquivo == null)
                throw new NegocioException("O arquivo informado não foi encontrado");

            await mediator.Send(new ExcluirReferenciaArquivoDocumentoPorArquivoIdCommand(param.DocumentoId, entidadeArquivo.Id));
            await mediator.Send(new ExcluirArquivoRepositorioPorIdCommand(entidadeArquivo.Id));
            await mediator.Send(new ExcluirArquivoFisicoCommand(entidadeArquivo.Codigo, entidadeArquivo.Tipo, entidadeArquivo.Nome));

            return true;
        }
    }
}
