using MediatR;
using System;
using System.Threading.Tasks;

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
            await mediator.Send(new ExcluirArquivoFisicoCommand(codigoArquivo, entidadeArquivo.Tipo, entidadeArquivo.Nome));

            return true;
        }
    }
}
