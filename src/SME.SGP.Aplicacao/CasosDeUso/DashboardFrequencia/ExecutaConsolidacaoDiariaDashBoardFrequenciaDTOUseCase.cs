using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaConsolidacaoDiariaDashBoardFrequenciaDTOUseCase : AbstractUseCase, IExecutaConsolidacaoDiariaDashBoardFrequenciaDTOUseCase
    {
        public ExecutaConsolidacaoDiariaDashBoardFrequenciaDTOUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(FiltroConsolicacaoDiariaDashBoardFrequenciaDTO filtroConsolicacao)
        {
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.RotaConsolidacaoDiariaDashBoardFrequencia, filtroConsolicacao, Guid.NewGuid(), null));

            return true;
        }
    }
}
