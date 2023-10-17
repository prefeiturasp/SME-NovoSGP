using System;
using System.IO;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ExcluirArquivoItineranciaUseCase :AbstractUseCase, IExcluirArquivoItineranciaUseCase
    {
        public ExcluirArquivoItineranciaUseCase(IMediator mediator) : base(mediator)
        {
            
        }
        public async Task<bool> Executar(Guid arquivoCodigo)
        {
            var entidadeArquivo = await mediator.Send(new ObterArquivoPorCodigoQuery(arquivoCodigo));
            if (entidadeArquivo.EhNulo())
                throw new NegocioException(MensagemNegocioComuns.ARQUIVO_INF0RMADO_NAO_ENCONTRADO);

            await mediator.Send(new ExcluirItineranciaQuestaoPorArquivoCommand(entidadeArquivo.Id));
            await mediator.Send(new ExcluirArquivoRepositorioPorIdCommand(entidadeArquivo.Id));
            
            var extencao = Path.GetExtension(entidadeArquivo.Nome);
            
            var filtro = new FiltroExcluirArquivoArmazenamentoDto {ArquivoNome = entidadeArquivo.Codigo.ToString() + extencao};
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RemoverArquivoArmazenamento, filtro, Guid.NewGuid(), null));
            return true;
        }
    }
}