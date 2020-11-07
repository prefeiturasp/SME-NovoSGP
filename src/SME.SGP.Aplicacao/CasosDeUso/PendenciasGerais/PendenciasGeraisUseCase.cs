using MediatR;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao
{
    public class PendenciasGeraisUseCase : IPendenciasGeraisUseCase
    {
        private readonly IMediator mediator;

        public PendenciasGeraisUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public void Executar()
        {
            mediator.Send(new PublicarFilaSgpCommand(RotasRabbit.RotaExecutaVerificacaoPendenciasGerais, null, Guid.NewGuid(), null));
        }
    }
}
