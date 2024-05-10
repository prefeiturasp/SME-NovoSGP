using MediatR;
using SME.SGP.Dominio;
using System;
using System.IO;
using System.Threading.Tasks;
using SME.SGP.Infra;

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

            if (entidadeArquivo.EhNulo())
                throw new NegocioException("O arquivo informado não foi encontrado");

            await mediator.Send(new ExcluirReferenciaArquivoDocumentoPorArquivoIdCommand(param.DocumentoId, entidadeArquivo.Id));
            await mediator.Send(new ExcluirArquivoRepositorioPorIdCommand(entidadeArquivo.Id));
            
            var extencao = Path.GetExtension(entidadeArquivo.Nome);

            var filtro = new FiltroExcluirArquivoArmazenamentoDto {ArquivoNome = entidadeArquivo.Codigo.ToString() + extencao};
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RemoverArquivoArmazenamento, filtro, Guid.NewGuid(), null));
            
            return true;
        }
    }
}
