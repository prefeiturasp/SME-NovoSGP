using MediatR;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao
{
    public class SincronizarAulasInfantilUseCase : ISincronizarAulasInfantilUseCase
    {
        private readonly IMediator mediator;

        public SincronizarAulasInfantilUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public void Executar()
        {
            mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaSincronizarAulasInfatil, null, Guid.NewGuid(), null));
        }
    }
}
