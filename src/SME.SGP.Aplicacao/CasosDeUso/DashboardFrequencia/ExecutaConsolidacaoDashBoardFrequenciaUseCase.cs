using MediatR;
using Newtonsoft.Json;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaConsolidacaoDashBoardFrequenciaUseCase : AbstractUseCase, IExecutaConsolidacaoDashBoardFrequenciaUseCase
    {
        public ExecutaConsolidacaoDashBoardFrequenciaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(FiltroConsolicacaoGeralDashBoardFrequenciaDto filtro)
        {
            var mensagemParaPublicar = JsonConvert.SerializeObject(filtro);

            var codigoCorrelacao = Guid.NewGuid();

            SentrySdk.AddBreadcrumb($"Mensagem ExecutaConsolidacaoDashBoardFrequenciaUseCase", "Rabbit - ExecutaConsolidacaoDashBoardFrequenciaUseCase");

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaConsolidacaoDiariaDashBoardFrequencia, mensagemParaPublicar, codigoCorrelacao, null));

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaConsolidacaoSemanalDashBoardFrequencia, mensagemParaPublicar, codigoCorrelacao, null));

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaConsolidacaoMensalDashBoardFrequencia, mensagemParaPublicar, codigoCorrelacao, null));

            return true; 
        }
    }
}
