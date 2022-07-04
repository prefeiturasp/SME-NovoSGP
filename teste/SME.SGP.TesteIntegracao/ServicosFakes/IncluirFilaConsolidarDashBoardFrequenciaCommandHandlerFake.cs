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
    public class IncluirFilaConsolidarDashBoardFrequenciaCommandHandlerFake : IRequestHandler<IncluirFilaConsolidarDashBoardFrequenciaCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IConsolidacaoDashBoardFrequenciaPorDataETipoUseCase consolidacaoDashBoardFrequenciaPorDataETipoUseCase;
        

        public IncluirFilaConsolidarDashBoardFrequenciaCommandHandlerFake(IMediator mediator, IConsolidacaoDashBoardFrequenciaPorDataETipoUseCase consolidacaoDashBoardFrequenciaPorDataETipoUseCase)
        {
            this.consolidacaoDashBoardFrequenciaPorDataETipoUseCase = consolidacaoDashBoardFrequenciaPorDataETipoUseCase ?? throw new ArgumentNullException(nameof(consolidacaoDashBoardFrequenciaPorDataETipoUseCase));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(IncluirFilaConsolidarDashBoardFrequenciaCommand request, CancellationToken cancellationToken)
        {
            var filtro = new FiltroConsolidadoDashBoardFrequenciaDto()
            {

                TurmaId = request.TurmaId,
                DataAula = request.DataAula,
                TipoPeriodo = request.TipoPeriodo
            };

            await consolidacaoDashBoardFrequenciaPorDataETipoUseCase.Executar(new MensagemRabbit(JsonSerializer.Serialize(filtro)));

            return true;
        }
    }
}
