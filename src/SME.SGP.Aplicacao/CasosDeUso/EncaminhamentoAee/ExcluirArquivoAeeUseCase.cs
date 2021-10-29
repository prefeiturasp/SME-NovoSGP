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

        public async Task<bool> Executar(Guid arquivoCodigo)
        {
            var entidadeArquivo = await mediator.Send(new ObterArquivoPorCodigoQuery(arquivoCodigo));

            if (entidadeArquivo == null)
                throw new NegocioException("O arquivo informado não foi encontrado");

            await mediator.Send(new ExcluirReferenciaArquivoAeePorArquivoIdCommand(entidadeArquivo.Id));
            await mediator.Send(new ExcluirArquivoRepositorioPorIdCommand(entidadeArquivo.Id));
            await mediator.Send(new ExcluirArquivoFisicoCommand(entidadeArquivo.Codigo, entidadeArquivo.Tipo, entidadeArquivo.Nome));

            return true;
        }
    }
}
