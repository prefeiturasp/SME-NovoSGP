using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PublicarPendenciaAusenciaRegistroIndividualUseCase : AbstractUseCase, IPublicarPendenciaAusenciaRegistroIndividualUseCase
    {
        public PublicarPendenciaAusenciaRegistroIndividualUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task Executar()
        {
            SentrySdk.AddBreadcrumb($"Mensagem {nameof(PublicarPendenciaAusenciaRegistroIndividualUseCase)}", $"Rabbit - {nameof(PublicarPendenciaAusenciaRegistroIndividualUseCase)}");
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaPendenciaAusenciaRegistroIndividual, null, Guid.NewGuid(), null));
        }
    }
}