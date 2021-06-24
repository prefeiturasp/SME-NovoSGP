using MediatR;
using Sentry;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class ExecutarSincronizacaoMediaRegistrosIndividuaisSyncUseCase : AbstractUseCase, IExecutarSincronizacaoMediaRegistrosIndividuaisSyncUseCase
    {
        public ExecutarSincronizacaoMediaRegistrosIndividuaisSyncUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task Executar()
        {
            SentrySdk.AddBreadcrumb($"Mensagem ExecutarSincronizacaoMediaRegistrosIndividuaisSyncUseCase", "Rabbit - ExecutarSincronizacaoMediaRegistrosIndividuaisSyncUseCase");

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarMediaRegistrosIndividuaisTurma, string.Empty, Guid.NewGuid(), null));
        }
    }
}
