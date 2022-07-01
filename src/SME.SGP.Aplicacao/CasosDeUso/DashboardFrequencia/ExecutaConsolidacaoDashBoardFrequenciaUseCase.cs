using MediatR;
using Newtonsoft.Json;
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

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.RotaConsolidacaoDiariaDashBoardFrequencia, mensagemParaPublicar, codigoCorrelacao, null));

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.RotaConsolidacaoSemanalDashBoardFrequencia, mensagemParaPublicar, codigoCorrelacao, null));

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.RotaConsolidacaoMensalDashBoardFrequencia, mensagemParaPublicar, codigoCorrelacao, null));

            return true; 
        }
    }
}
