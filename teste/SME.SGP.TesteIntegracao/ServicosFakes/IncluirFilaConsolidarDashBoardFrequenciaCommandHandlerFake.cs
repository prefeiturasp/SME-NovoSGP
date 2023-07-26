using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao
{
    public class IncluirFilaConsolidarDashBoardFrequenciaCommandHandlerFake : IRequestHandler<IncluirFilaConsolidacaoDiariaDashBoardFrequenciaCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IExecutaConsolidacaoDiariaDashBoardFrequenciaPorTurmaUseCase executaConsolidacaoDiariaDashBoardFrequenciaPorTurmaUseCase;
        

        public IncluirFilaConsolidarDashBoardFrequenciaCommandHandlerFake(IMediator mediator, IExecutaConsolidacaoDiariaDashBoardFrequenciaPorTurmaUseCase executaConsolidacaoDiariaDashBoardFrequenciaPorTurmaUseCase)
        {
            this.executaConsolidacaoDiariaDashBoardFrequenciaPorTurmaUseCase = executaConsolidacaoDiariaDashBoardFrequenciaPorTurmaUseCase ?? throw new ArgumentNullException(nameof(executaConsolidacaoDiariaDashBoardFrequenciaPorTurmaUseCase));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(IncluirFilaConsolidacaoDiariaDashBoardFrequenciaCommand request, CancellationToken cancellationToken)
        {
            var filtro = new ConsolidacaoPorTurmaDashBoardFrequencia()
            {
                Mes = request.DataAula.Month,
                AnoLetivo = request.DataAula.Year,
                TurmaId = request.TurmaId,
                DataAula = request.DataAula
            };

            await executaConsolidacaoDiariaDashBoardFrequenciaPorTurmaUseCase.Executar(new MensagemRabbit(JsonSerializer.Serialize(filtro)));

            return true;
        }
    }
}
