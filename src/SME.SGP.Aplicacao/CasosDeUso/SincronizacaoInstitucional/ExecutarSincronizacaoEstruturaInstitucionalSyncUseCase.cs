using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class ExecutarSincronizacaoEstruturaInstitucionalSyncUseCase : AbstractUseCase, IExecutarSincronizacaoInstitucionalSyncUseCase
    {
        public ExecutarSincronizacaoEstruturaInstitucionalSyncUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task Executar()
        {
            SentrySdk.AddBreadcrumb($"Mensagem ExecutarSincronizacaoEstruturaInstitucionalSyncUseCase", "Rabbit - ExecutarSincronizacaoEstruturaInstitucionalSyncUseCase");

            //TODO: Fazer um Sync de Dre específico;
            var codigosDre = await mediator.Send(new ObterCodigosDresQuery());

            foreach (var codigoDre in codigosDre)
            {
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbit.SincronizaEstruturaInstitucionalDreTratar, codigoDre, Guid.NewGuid(), null, fila: RotasRabbit.SincronizaEstruturaInstitucionalDreTratar));
            }

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbit.SincronizaEstruturaInstitucionalTipoEscolaSync, string.Empty, Guid.NewGuid(), null, fila: RotasRabbit.SincronizaEstruturaInstitucionalTipoEscolaSync));

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbit.SincronizaEstruturaInstitucionalCicloSync, string.Empty, Guid.NewGuid(), null, fila: RotasRabbit.SincronizaEstruturaInstitucionalCicloSync));
        }
    }
}
