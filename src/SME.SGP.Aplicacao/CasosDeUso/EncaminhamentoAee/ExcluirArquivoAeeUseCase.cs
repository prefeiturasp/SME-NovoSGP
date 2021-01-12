using MediatR;
using SME.SGP.Dominio;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirArquivoAeeUseCase : AbstractUseCase, IExcluirArquivoAeeUseCase
    {
        public ExcluirArquivoAeeUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(long arquivoId)
        {
            var entidadeArquivo = await mediator.Send(new ObterArquivoPorIdQuery(arquivoId));

            if (entidadeArquivo == null)
                throw new NegocioException("O arquivo informado não foi encontrado");

            await mediator.Send(new ExcluirReferenciaArquivoAeePorArquivoIdCommand(arquivoId));
            await mediator.Send(new ExcluirArquivoRepositorioPorIdCommand(arquivoId));
            await mediator.Send(new ExcluirArquivoFisicoCommand(entidadeArquivo.Codigo, entidadeArquivo.Tipo, entidadeArquivo.Nome));

            return true;
        }
    }
}
