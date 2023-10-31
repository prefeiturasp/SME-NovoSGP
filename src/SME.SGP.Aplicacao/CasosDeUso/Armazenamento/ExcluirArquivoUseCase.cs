using MediatR;
using System;
using System.IO;
using System.Threading.Tasks;
using SME.SGP.Infra;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;

namespace SME.SGP.Aplicacao
{
    public class ExcluirArquivoUseCase : AbstractUseCase, IExcluirArquivoUseCase
    {
        public ExcluirArquivoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(Guid param)
        {
            var entidadeArquivo = await mediator.Send(new ObterArquivoPorCodigoQuery(param));
            if (entidadeArquivo.EhNulo())
              throw new NegocioException(MensagemNegocioComuns.ARQUIVO_INF0RMADO_NAO_ENCONTRADO);

            await mediator.Send(new ExcluirArquivoRepositorioPorIdCommand(entidadeArquivo.Id));
            
            var extencao = Path.GetExtension(entidadeArquivo.Nome);

            var filtro = new FiltroExcluirArquivoArmazenamentoDto {ArquivoNome = param.ToString() + extencao};
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RemoverArquivoArmazenamento, filtro, Guid.NewGuid(), null));
            
            return true;
        }
    }
}
