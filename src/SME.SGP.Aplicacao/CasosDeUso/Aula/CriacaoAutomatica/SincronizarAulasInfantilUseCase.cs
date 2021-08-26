using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

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

        public async Task<bool> Executar(long codigoTurma)
        {
            return await mediator
                .Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaSincronizarAulasInfatil, codigoTurma, Guid.NewGuid(), null));
        }
    }
}
