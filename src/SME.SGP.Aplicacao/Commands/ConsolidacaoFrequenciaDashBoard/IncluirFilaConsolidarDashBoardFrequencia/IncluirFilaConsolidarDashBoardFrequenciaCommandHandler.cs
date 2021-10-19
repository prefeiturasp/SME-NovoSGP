using MediatR;
using Sentry;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaConsolidarDashBoardFrequenciaCommandHandler : IRequestHandler<IncluirFilaConsolidarDashBoardFrequenciaCommand, bool>
    {
        private readonly IMediator mediator;

        public IncluirFilaConsolidarDashBoardFrequenciaCommandHandler(IMediator mediator)
        {
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

            try
            {
                var publicar = await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaConsolidacaoDashBoardFrequencia, filtro, Guid.NewGuid(), null));
                if (!publicar)
                {
                    var mensagem = $"Não foi possível inserir a turma: {request.TurmaId} na fila de consolidação de frequência.";
                    SentrySdk.CaptureMessage(mensagem);
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }

            return true;
        }
    }
}
