using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes.MensagensNegocio;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaConsolidacaoDiariaDashBoardFrequenciaCommandHandler : IRequestHandler<IncluirFilaConsolidacaoDiariaDashBoardFrequenciaCommand, bool>
    {
        private readonly IMediator mediator;

        public IncluirFilaConsolidacaoDiariaDashBoardFrequenciaCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(IncluirFilaConsolidacaoDiariaDashBoardFrequenciaCommand request, CancellationToken cancellationToken)
        {
            var filtro = new ConsolidacaoPorTurmaDashBoardFrequencia()
            {
                TurmaId = request.TurmaId,
                Mes = request.DataAula.Month,
                AnoLetivo = request.DataAula.Year,
                DataAula = request.DataAula,
            };            

            return await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.RotaConsolidacaoDiariaDashBoardFrequenciaPorTurma, filtro, Guid.NewGuid(), null));
        }
    }
}
