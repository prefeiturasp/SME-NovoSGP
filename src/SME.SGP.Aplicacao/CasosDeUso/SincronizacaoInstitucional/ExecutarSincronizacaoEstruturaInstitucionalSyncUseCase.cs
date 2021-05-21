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
            var codigoCorrelacao = Guid.NewGuid();

            SentrySdk.AddBreadcrumb($"Mensagem ExecutarSincronizacaoEstruturaInstitucionalSyncUseCase", "Rabbit - ExecutarSincronizacaoEstruturaInstitucionalSyncUseCase");
                        
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.SincronizaEstruturaInstitucionalDreSync, string.Empty, codigoCorrelacao, null));

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.SincronizaEstruturaInstitucionalTipoEscolaSync, string.Empty, codigoCorrelacao, null));

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.SincronizaEstruturaInstitucionalCicloSync, string.Empty, codigoCorrelacao, null));
        }
    }
}
