using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirArquivoPAPUseCase : AbstractUseCase, IExcluirArquivoPAPUseCase
    {
        public ExcluirArquivoPAPUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(Guid arquivoCodigo)
        {
            var entidadeArquivo = await mediator.Send(new ObterArquivoPorCodigoQuery(arquivoCodigo));

            if (entidadeArquivo == null)
                throw new NegocioException(MensagemNegocioComuns.ARQUIVO_INF0RMADO_NAO_ENCONTRADO);

            await mediator.Send(new ExcluirReferenciaArquivoPAPPorArquivoIdCommand(entidadeArquivo.Id));
            await mediator.Send(new ExcluirArquivoRepositorioPorIdCommand(entidadeArquivo.Id));

            var extencao = Path.GetExtension(entidadeArquivo.Nome);

            var filtro = new FiltroExcluirArquivoArmazenamentoDto { ArquivoNome = entidadeArquivo.Codigo.ToString() + extencao };
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RemoverArquivoArmazenamento, filtro, Guid.NewGuid(), null));

            return true;
        }
    }
}
