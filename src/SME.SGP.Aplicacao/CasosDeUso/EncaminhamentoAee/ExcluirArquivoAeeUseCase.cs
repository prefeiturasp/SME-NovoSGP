using MediatR;
using SME.SGP.Dominio;
using System;
using System.IO;
using System.Threading.Tasks;
using SME.SGP.Infra;

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

            if (entidadeArquivo.EhNulo())
                throw new NegocioException("O arquivo informado não foi encontrado");

            await mediator.Send(new ExcluirReferenciaArquivoAeePorArquivoIdCommand(entidadeArquivo.Id));
            await mediator.Send(new ExcluirArquivoRepositorioPorIdCommand(entidadeArquivo.Id));
            
            var extencao = Path.GetExtension(entidadeArquivo.Nome);

            var filtro = new FiltroExcluirArquivoArmazenamentoDto {ArquivoNome = entidadeArquivo.Codigo.ToString() + extencao};
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RemoverArquivoArmazenamento, filtro, Guid.NewGuid(), null));
            
            return true;
        }
    }
}
