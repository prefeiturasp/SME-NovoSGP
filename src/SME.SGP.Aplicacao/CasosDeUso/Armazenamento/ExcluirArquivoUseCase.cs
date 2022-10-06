using MediatR;
using System;
using System.IO;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ExcluirArquivoUseCase : AbstractUseCase, IExcluirArquivoUseCase
    {
        public ExcluirArquivoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(Guid codigoArquivo)
        {
            var entidadeArquivo = await mediator.Send(new ObterArquivoPorCodigoQuery(codigoArquivo));
            await mediator.Send(new ExcluirArquivoRepositorioPorIdCommand(entidadeArquivo.Id));
            
            var extencao = Path.GetExtension(entidadeArquivo.Nome);

            var filtro = new FiltroExcluirArquivoArmazenamentoDto {ArquivoNome = codigoArquivo.ToString() + extencao};
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RemoverArquivoArmazenamento, filtro, Guid.NewGuid(), null));
            
            return true;
        }
    }
}
