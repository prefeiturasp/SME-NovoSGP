using MediatR;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaVerificacaoPendenciasGeraisUseCase : IExecutaVerificacaoPendenciasGeraisUseCase
    {
        private readonly IMediator mediator;

        public ExecutaVerificacaoPendenciasGeraisUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Executar()
        {
            await mediator.Send(new VerificaPendenciaCalendarioUeCommand());
            await mediator.Send(new VerificaPendenciaParametroEventoCommand());
        }
    }
}
