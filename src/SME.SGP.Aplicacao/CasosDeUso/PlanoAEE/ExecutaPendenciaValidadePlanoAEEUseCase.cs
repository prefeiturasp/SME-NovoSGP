using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaPendenciaValidadePlanoAEEUseCase : AbstractUseCase, IExecutaPendenciaValidadePlanoAEEUseCase
    {
        public ExecutaPendenciaValidadePlanoAEEUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task Executar()
        {
            SentrySdk.AddBreadcrumb($"Mensagem ExecutaPendenciaValidadoPlanoAEEUseCase", "Rabbit - ExecutaPendenciaValidadoPlanoAEEUseCase");

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.GerarPendenciaValidadePlanoAEE, null, Guid.NewGuid(), null));
        }
    }
}
